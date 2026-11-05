/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using AltTester.AltTesterUnitySDK.Commands;
using AltTester.AltTesterUnitySDK.Editor.Logging;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace AltTester.AltTesterUnitySDK.Editor
{
    /// <summary>
    /// Removes the AltTester prefab that AltBuilder saved into scenes, without needing the AltTester Editor window.
    /// Only the prefab instances AltBuilder inserted are removed, so a prefab placed by hand stays.
    /// The cleanup runs once the editor is idle after a build, so a build script that makes several builds keeps
    /// the prefab until it returns, and when the editor starts, for builds that failed or ran in batch mode.
    /// Nothing is removed while "Keep AltTester® prefab in scene" is on.
    /// </summary>
    [InitializeOnLoad]
    internal class AltPrefabCleanup : IPostprocessBuildWithReport
    {
        // In Library/ so it survives editor restarts but is never committed. One "scenePath\tGlobalObjectId" per line.
        private const string trackedPrefabsFile = "Library/AltTester/PrefabInsertedScenes.txt";
        private const string startupCleanupDoneKey = "AltTester.PrefabCleanupStartupDone";
        // SessionState survives domain reloads, which drop a queued delayCall, e.g. when a build restores the defines.
        private const string cleanupPendingKey = "AltTester.PrefabCleanupPending";

        private static readonly NLog.Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();

        private struct TrackedPrefab
        {
            public string ScenePath;
            public string ObjectId;
        }

        public int callbackOrder => 0;

        static AltPrefabCleanup()
        {
            EditorApplication.playModeStateChanged += onPlayModeStateChanged;
            if (!SessionState.GetBool(startupCleanupDoneKey, false))
            {
                SessionState.SetBool(startupCleanupDoneKey, true);
                SessionState.SetBool(cleanupPendingKey, true);
            }
            // Only a cleanup still owed by the editor start or a build runs here, so a domain reload after a script
            // change doesn't remove a prefab a build script has just inserted.
            if (SessionState.GetBool(cleanupPendingKey, false))
                queueCleanup();
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            QueueCleanup();
        }

        /// <summary>
        /// Removes the prefabs AltBuilder inserted once the editor is idle. Call it after a build that may have
        /// failed, since a failed build doesn't reach OnPostprocessBuild.
        /// </summary>
        internal static void QueueCleanup()
        {
            SessionState.SetBool(cleanupPendingKey, true);
            queueCleanup();
        }

        /// <summary>
        /// Records that AltBuilder saved <paramref name="altRunner"/> into <paramref name="scenePath"/>, so it is
        /// removed again later. Call it after the scene is saved, so the object has its final id.
        /// </summary>
        internal static void Track(string scenePath, UnityEngine.GameObject altRunner)
        {
            if (string.IsNullOrEmpty(scenePath) || altRunner == null)
                return;
            var trackedPrefabs = readTrackedPrefabs();
            trackedPrefabs.Add(new TrackedPrefab { ScenePath = scenePath, ObjectId = GlobalObjectId.GetGlobalObjectIdSlow(altRunner).ToString() });
            writeTrackedPrefabs(trackedPrefabs);
        }

        /// <summary>
        /// Stops tracking <paramref name="altRunner"/>. Call it before destroying a prefab outside this class, so its
        /// record doesn't stay behind.
        /// </summary>
        internal static void Untrack(UnityEngine.GameObject altRunner)
        {
            var objectId = GlobalObjectId.GetGlobalObjectIdSlow(altRunner).ToString();
            var trackedPrefabs = readTrackedPrefabs();
            if (trackedPrefabs.RemoveAll(prefab => prefab.ObjectId == objectId) > 0)
                writeTrackedPrefabs(trackedPrefabs);
        }

        /// <summary>
        /// Removes the prefab from <paramref name="scenePath"/> now, also one placed by hand, and stops tracking it.
        /// </summary>
        /// <returns>False when the scene has unsaved changes or can't be saved, so its file still has the prefab.</returns>
        internal static bool RemoveFromScene(string scenePath)
        {
            if (!removePrefabs(scenePath, null))
                return false;
            var trackedPrefabs = readTrackedPrefabs();
            if (trackedPrefabs.RemoveAll(prefab => prefab.ScenePath == scenePath) > 0)
                writeTrackedPrefabs(trackedPrefabs);
            return true;
        }

        private static void onPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode && SessionState.GetBool(cleanupPendingKey, false))
                queueCleanup();
        }

        private static void queueCleanup()
        {
            EditorApplication.delayCall -= removeInsertedPrefabs;
            EditorApplication.delayCall += removeInsertedPrefabs;
        }

        private static void removeInsertedPrefabs()
        {
            // Scenes can't be opened or saved now, or Play in Editor still needs the prefab it inserted. The cleanup
            // stays pending and runs again after the domain reload, when play mode ends or after the build.
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || BuildPipeline.isBuildingPlayer || AltTesterEditorWindow.IsPlayInEditorPending)
                return;
            SessionState.SetBool(cleanupPendingKey, false);

            var trackedPrefabs = readTrackedPrefabs();
            if (trackedPrefabs.Count == 0 || keepPrefabInScene())
                return;

            foreach (var scene in trackedPrefabs.GroupBy(prefab => prefab.ScenePath).ToList())
            {
                try
                {
                    // A scene with unsaved changes stays tracked; the next build or editor start retries it.
                    if (removePrefabs(scene.Key, new HashSet<string>(scene.Select(prefab => prefab.ObjectId))))
                        trackedPrefabs.RemoveAll(prefab => prefab.ScenePath == scene.Key);
                }
                catch (System.Exception e)
                {
                    logger.Error($"Could not remove the AltTester® prefab from {scene.Key}: {e.Message}");
                }
            }
            writeTrackedPrefabs(trackedPrefabs);
        }

        /// <param name="objectIds">The GlobalObjectIds of the instances to remove, or null for every AltTester prefab.</param>
        /// <returns>True when the scene file no longer contains those prefabs.</returns>
        private static bool removePrefabs(string scenePath, HashSet<string> objectIds)
        {
            if (!File.Exists(scenePath))
                return true;

            var scene = SceneManager.GetSceneByPath(scenePath);
            // A scene can be in the Hierarchy but unloaded; it is loaded here and unloaded again afterwards.
            var inHierarchy = scene.IsValid();
            var openedHere = !inHierarchy || !scene.isLoaded;
            // Saving would also save the user's unsaved changes, so leave the scene alone and retry later.
            if (!openedHere && scene.isDirty)
                return false;
            if (openedHere)
                scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

            // A prefab the user deleted and replaced has a different id, so the replacement stays.
            var prefabs = findAltTesterPrefabs(scene)
                .Where(prefab => objectIds == null || objectIds.Contains(GlobalObjectId.GetGlobalObjectIdSlow(prefab).ToString()))
                .ToList();
            var removed = true;
            if (prefabs.Count > 0)
            {
                foreach (var prefab in prefabs)
                    UnityEngine.Object.DestroyImmediate(prefab);
                EditorSceneManager.MarkSceneDirty(scene);
                removed = EditorSceneManager.SaveScene(scene);
                if (removed)
                    logger.Info($"Removed the AltTester® prefab from {scenePath}.");
            }

            if (openedHere)
                EditorSceneManager.CloseScene(scene, !inHierarchy);
            return removed;
        }

        private static List<UnityEngine.GameObject> findAltTesterPrefabs(Scene scene)
        {
            return scene.GetRootGameObjects().Where(gameObject => gameObject.GetComponent<AltRunner>() != null).ToList();
        }

        private static bool keepPrefabInScene()
        {
            // Load the settings without AltTesterEditorWindow.InitEditorConfiguration, which creates them if missing.
            var configuration = AltTesterEditorWindow.EditorConfiguration;
            if (configuration == null)
            {
                var guids = AssetDatabase.FindAssets("t:AltEditorConfiguration");
                if (guids.Length > 0)
                    configuration = AssetDatabase.LoadAssetAtPath<AltEditorConfiguration>(AssetDatabase.GUIDToAssetPath(guids[0]));
            }
            return configuration != null && configuration.KeepAltTesterPrefabInScene;
        }

        private static List<TrackedPrefab> readTrackedPrefabs()
        {
            if (!File.Exists(trackedPrefabsFile))
                return new List<TrackedPrefab>();
            return File.ReadAllLines(trackedPrefabsFile)
                .Select(line => line.Split('\t'))
                .Where(parts => parts.Length == 2)
                .Select(parts => new TrackedPrefab { ScenePath = parts[0], ObjectId = parts[1] })
                .ToList();
        }

        private static void writeTrackedPrefabs(List<TrackedPrefab> trackedPrefabs)
        {
            if (trackedPrefabs.Count == 0)
            {
                if (File.Exists(trackedPrefabsFile))
                    File.Delete(trackedPrefabsFile);
                return;
            }
            Directory.CreateDirectory(Path.GetDirectoryName(trackedPrefabsFile));
            File.WriteAllLines(trackedPrefabsFile, trackedPrefabs.Select(prefab => prefab.ScenePath + "\t" + prefab.ObjectId).Distinct());
        }
    }
}
