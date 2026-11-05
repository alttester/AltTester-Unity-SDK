/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Linq;
using AltTester.AltTesterUnitySDK.Editor.Logging;
using AltTester.AltTesterUnitySDK.InputModule;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

namespace AltTester.AltTesterUnitySDK.Editor
{
    /// <summary>
    /// Gives AltInput the Input Manager axes: in builds they are written into the AltInput components of each built
    /// scene, so they ship only with the AltTester prefab; in Play Mode they are set directly. For a prefab
    /// instantiated at runtime, instrumented builds also get the axes as a Resources file, removed after the build.
    /// </summary>
    [InitializeOnLoad]
    internal class AltInputAxesProcessor : IProcessSceneWithReport, IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        private const string resourcesFolder = "Assets/Resources";
        private const string axesFolder = resourcesFolder + "/AltTester";
        private const string axesFile = axesFolder + "/AltTesterInputAxisData.json";
        // Kept in SessionState, since the build can trigger a domain reload before the file is removed.
        private const string resourcesFolderCreatedKey = "AltTester.InputAxes.ResourcesFolderCreated";

        private static readonly NLog.Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();

        public int callbackOrder => 0;

        private static bool resourcesFolderCreated
        {
            get => SessionState.GetBool(resourcesFolderCreatedKey, false);
            set => SessionState.SetBool(resourcesFolderCreatedKey, value);
        }

        static AltInputAxesProcessor()
        {
            EditorApplication.playModeStateChanged += onPlayModeStateChanged;
            // After the domain reload that comes with entering Play Mode.
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                setPlayModeAxesData();
            // Left over by older SDK versions, which kept it in every build, or by an instrumented build that failed.
            EditorApplication.delayCall += () =>
            {
                if (DeleteAxesFile())
                    logger.Info($"Deleted {axesFile}: the Input Manager axes are added to instrumented builds only.");
            };
            // OnPostprocessBuild isn't called for a failed build, and delayCall never runs with -batchmode -quit.
            EditorApplication.quitting += () => DeleteAxesFile();
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            // Also clears a file left by a failed instrumented build, so it can't end up in a production build.
            DeleteAxesFile();
            if (!AltBuildInstrumentation.IsInstrumentedBuild(report.summary.platform))
                return;

            resourcesFolderCreated = !AssetDatabase.IsValidFolder(resourcesFolder);
            System.IO.Directory.CreateDirectory(axesFolder);
            System.IO.File.WriteAllText(axesFile, AltBuilder.GetInputAxesData());
            AssetDatabase.ImportAsset(axesFile, ImportAssetOptions.ForceSynchronousImport);
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            DeleteAxesFile();
        }

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            var altInputs = scene.GetRootGameObjects()
                .SelectMany(gameObject => gameObject.GetComponentsInChildren<AltInput>(true))
                .ToList();
            if (altInputs.Count == 0)
                return;

            var axesData = AltBuilder.GetInputAxesData();
            foreach (var altInput in altInputs)
            {
                var serializedAltInput = new SerializedObject(altInput);
                serializedAltInput.FindProperty("inputAxesData").stringValue = axesData;
                serializedAltInput.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static void onPlayModeStateChanged(PlayModeStateChange state)
        {
            // Covers entering Play Mode without a domain reload.
            if (state == PlayModeStateChange.ExitingEditMode)
                setPlayModeAxesData();
        }

        private static void setPlayModeAxesData()
        {
            try
            {
                AltInput.InputAxesData = AltBuilder.GetInputAxesData();
            }
            catch (System.Exception e)
            {
                logger.Error($"Could not read the Input Manager axes: {e.Message}");
            }
        }

        /// <summary>
        /// Removes the axes file written for an instrumented build, and the Resources folders if the build created
        /// them. Called after the build, also when it fails, since OnPostprocessBuild only runs for a successful one.
        /// </summary>
        internal static bool DeleteAxesFile()
        {
            if (!System.IO.File.Exists(axesFile))
                return false;
            AssetDatabase.DeleteAsset(axesFile);
            deleteFolderIfEmpty(axesFolder);
            if (resourcesFolderCreated)
                deleteFolderIfEmpty(resourcesFolder);
            resourcesFolderCreated = false;
            return true;
        }

        private static void deleteFolderIfEmpty(string folder)
        {
            if (AssetDatabase.IsValidFolder(folder) && AssetDatabase.FindAssets("", new[] { folder }).Length == 0)
                AssetDatabase.DeleteAsset(folder);
        }
    }
}
