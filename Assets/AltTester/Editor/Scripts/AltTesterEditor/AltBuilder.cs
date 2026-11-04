/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterUnitySDK.Commands;
using AltTester.AltTesterUnitySDK.Editor.Logging;
using AltTesterTools;
using UnityEditor;
#if UNITY_6000_0_OR_NEWER
using UnityEditor.Build;
#endif
using UnityEditor.Compilation;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Editor
{
    public static class PlatformName
    {
        public const string Android = "Android";
        public const string iOS = "iOS";
        public const string Standalone = "Standalone";
    }


    public class AltBuilder
    {
        private const string ALTTESTERDEFINE = "ALTTESTER";
        private const string PREFABNAME = "AltTesterPrefab";
        private const string PREFABNAMEWITHEXTENSION = "AltTesterPrefab.prefab";
        private const string WEBGLDEFINE = "UNITY_WEBGL";

        private static readonly NLog.Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();

        public static bool Built = false;
        public static string PreviousScenePath;
        public static UnityEngine.SceneManagement.Scene SceneWithAltRunner;
        public static UnityEngine.Object AltRunner;

        public static void InitBuildSetup(UnityEditor.BuildTargetGroup buildTargetGroup)
        {
            AltTesterEditorWindow.InitEditorConfiguration();

            if (AltTesterEditorWindow.EditorConfiguration.appendToName)
            {
                UnityEditor.PlayerSettings.productName += "Test";
#if UNITY_6000_0_OR_NEWER
                UnityEditor.PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup), $"{UnityEditor.PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup))}Test");
#else
                UnityEditor.PlayerSettings.SetApplicationIdentifier(buildTargetGroup, $"{UnityEditor.PlayerSettings.GetApplicationIdentifier(buildTargetGroup)}Test");
#endif
            }
            AddAltTesterInScriptingDefineSymbolsGroup(buildTargetGroup);
        }

        public static void BuildGameFromUI(UnityEditor.BuildTarget buildTarget, UnityEditor.BuildTargetGroup buildTargetGroup, bool autoRun = false)
        {
            // Everything the build changes in Player Settings is restored afterwards, also when the build fails,
            // so the next build made outside the AltTester Editor doesn't inherit it.
            var playerSettings = new PlayerSettingsSnapshot(buildTargetGroup);
            try
            {
                InitBuildSetup(buildTargetGroup);
                logger.Debug($"Starting {buildTarget} build...{UnityEditor.PlayerSettings.productName}:{UnityEditor.PlayerSettings.bundleVersion}");

                var buildPlayerOptions = new UnityEditor.BuildPlayerOptions
                {
                    locationPathName = getOutputPath(buildTarget),
                    scenes = getScenesForBuild(),
                    target = buildTarget,
                    targetGroup = buildTargetGroup
                };
#if NINTENDO_ENABLED && ALTTESTER_NONGPL
                if (buildTargetGroup == UnityEditor.BuildTargetGroup.Switch)
                {

                    UnityEditor.EditorUserBuildSettings.explicitNullChecks = true;
                    UnityEditor.EditorUserBuildSettings.explicitArrayBoundsChecks = true;
                    UnityEditor.EditorUserBuildSettings.explicitDivideByZeroChecks = true;
                    UnityEditor.PlayerSettings.SetManagedStrippingLevel(UnityEditor.BuildTargetGroup.Switch, UnityEditor.ManagedStrippingLevel.Minimal);
                }
#endif

                buildGame(autoRun, buildPlayerOptions);
            }
            catch (System.Exception e)
            {
                logger.Error(e);
            }
            finally
            {
                Built = true;
                AltInputAxesProcessor.DeleteAxesFile();
                resetBuildSetup(buildTargetGroup, playerSettings);
                AltPrefabCleanup.QueueCleanup();
            }
        }
        public static void RemoveAltTesterFromScriptingDefineSymbols(UnityEditor.BuildTargetGroup targetGroup)
        {
            if (AltTesterEditorWindow.EditorConfiguration != null && AltTesterEditorWindow.EditorConfiguration.KeepAUTSymbolDefined)
                return;
            RemoveScriptingDefineSymbol(ALTTESTERDEFINE, targetGroup);
        }
        /// <summary>
        /// Adds ALTTESTER to every build target: the Player Settings of each platform, Dedicated Server included,
        /// and on Unity 6 each build profile's scripting defines and Player Settings override.
        /// </summary>
        public static void AddAltTesterToAllScriptingDefineSymbols()
        {
            AltScriptingDefineSymbols.AddEverywhere(ALTTESTERDEFINE);
        }

        /// <summary>
        /// Removes ALTTESTER from every build target: the Player Settings of each platform, Dedicated Server included,
        /// and on Unity 6 each build profile's scripting defines and Player Settings override.
        /// </summary>
        public static void RemoveAltTesterFromAllScriptingDefineSymbols()
        {
            AltScriptingDefineSymbols.RemoveEverywhere(ALTTESTERDEFINE);
        }

        public static void RemoveScriptingDefineSymbol(string symbol, UnityEditor.BuildTargetGroup targetGroup)
        {
            try
            {
                var defines = AltBuildInstrumentation.SplitDefines(AltBuildInstrumentation.GetPlayerSettingsDefines(targetGroup));
                if (defines.Contains(symbol))
                {
                    setScriptingDefineSymbols(targetGroup, string.Join(";", defines.Where(define => define != symbol)));
                }
            }
            catch (System.Exception e)
            {
                logger.Error("Some Error Happened +" + e.Message);
                logger.Error("Stack trace " + e.StackTrace);
            }
        }

        public static void AddAltTesterInScriptingDefineSymbolsGroup(UnityEditor.BuildTargetGroup targetGroup)
        {
            AddScriptingDefineSymbol(ALTTESTERDEFINE, targetGroup);
        }
        public static void AddScriptingDefineSymbol(string symbol, UnityEditor.BuildTargetGroup targetGroup)
        {
            var defines = AltBuildInstrumentation.SplitDefines(AltBuildInstrumentation.GetPlayerSettingsDefines(targetGroup));
            if (!defines.Contains(symbol))
            {
                setScriptingDefineSymbols(targetGroup, string.Join(";", defines.Append(symbol)));
            }
        }

        private static void setScriptingDefineSymbols(UnityEditor.BuildTargetGroup targetGroup, string defines)
        {
#if UNITY_6000_0_OR_NEWER
            UnityEditor.PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(targetGroup), defines);
#else
            UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
#endif
        }

        [System.Obsolete("Use AddAltTesterInScriptingDefineSymbolsGroup instead.")]
        public static void AddAltTesterInScritpingDefineSymbolsGroup(UnityEditor.BuildTargetGroup targetGroup)
        {
            AddAltTesterInScriptingDefineSymbolsGroup(targetGroup);
        }

        public static bool CheckAltTesterIsDefineAsAScriptingSymbol(UnityEditor.BuildTargetGroup targetGroup)
        {
            return AltBuildInstrumentation.SplitDefines(AltBuildInstrumentation.GetPlayerSettingsDefines(targetGroup)).Contains(ALTTESTERDEFINE);
        }

        [System.Obsolete("The Input Manager axes are added to instrumented builds automatically.")]
        public static void CreateJsonFileForInputMappingOfAxis()
        {
            logger.Warn("CreateJsonFileForInputMappingOfAxis no longer creates a file: the Input Manager axes are added to instrumented builds automatically.");
        }

        /// <summary>
        /// The Input Manager axes as JSON, as AltInput reads them.
        /// </summary>
        public static string GetInputAxesData()
        {
            var inputManager = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            var obj = new UnityEditor.SerializedObject(inputManager);

            UnityEditor.SerializedProperty axisArray = obj.FindProperty("m_Axes");

            if (axisArray.arraySize == 0)
                logger.Info("No Axes");
            var axisList = new List<AltAxis>();
            for (int i = 0; i < axisArray.arraySize; ++i)
            {
                var axis = axisArray.GetArrayElementAtIndex(i);

                var name = axis.FindPropertyRelative("m_Name").stringValue;
                var inputType = (InputType)axis.FindPropertyRelative("type").intValue;
                var negativeButton = axis.FindPropertyRelative("negativeButton").stringValue;
                var positiveButton = axis.FindPropertyRelative("positiveButton").stringValue;
                var altPositiveButton = axis.FindPropertyRelative("altPositiveButton").stringValue;
                var altNegativeButton = axis.FindPropertyRelative("altNegativeButton").stringValue;
                var axisDirection = axis.FindPropertyRelative("axis").intValue;
                axisList.Add(new AltAxis(name, inputType, negativeButton, positiveButton, altPositiveButton, altNegativeButton, axisDirection));
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(axisList);
        }
        [System.Obsolete("Use InsertAltTesterInScene instead.")]
        public static void InsertAltInScene(string scene, AltInstrumentationSettings instrumentationSettings)
        {
            InsertAltTesterInScene(scene, instrumentationSettings);
        }
        public static void InsertAltTesterInScene(string scene, AltInstrumentationSettings instrumentationSettings)
        {

            var AltPrefab = GetAltTesterPrefab();
            SceneWithAltRunner = EditorSceneManager.OpenScene(scene);
            insertAltTesterInSceneWithAltRunner(AltPrefab, instrumentationSettings);
        }

        /// <summary>
        /// Adds the AltTester® prefab to SceneWithAltRunner and saves the scene. When the scene already has one, e.g.
        /// placed by hand or by "Keep AltTester® prefab in scene", only its settings are updated, so the build has a
        /// single AltRunner. That prefab isn't tracked, so it stays in the scene after the build.
        /// </summary>
        private static void insertAltTesterInSceneWithAltRunner(GameObject altTesterPrefab, AltInstrumentationSettings instrumentationSettings)
        {
            var existingAltRunner = SceneWithAltRunner.GetRootGameObjects().FirstOrDefault(gameObject => gameObject.GetComponent<AltRunner>() != null);
            AltRunner = existingAltRunner != null ? existingAltRunner : UnityEditor.PrefabUtility.InstantiatePrefab(altTesterPrefab);
            var altRunnerComponent = ((GameObject)AltRunner).GetComponent<AltRunner>();
            altRunnerComponent.InstrumentationSettings = instrumentationSettings;
            // Without this, a change made from code to a prefab instance loaded from the scene isn't saved.
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(altRunnerComponent);

            EditorSceneManager.MarkSceneDirty(SceneWithAltRunner);
            EditorSceneManager.SaveOpenScenes();
            if (existingAltRunner == null)
                AltPrefabCleanup.Track(SceneWithAltRunner.path, (GameObject)AltRunner);
        }

        /// <summary>
        /// Removes the AltTester® prefab from <paramref name="scene"/> and saves the scene. Call it once a build made
        /// with InsertAltTesterInScene is done, so builds made without AltTester® don't contain the prefab.
        /// </summary>
        public static void RemoveAltTesterFromScene(string scene)
        {
            if (!AltPrefabCleanup.RemoveFromScene(scene))
                logger.Warn($"The AltTester® prefab was not removed from {scene} because the scene has unsaved changes or could not be saved. Save or discard the changes and remove it again.");
        }

        public static GameObject CreateAltTesterPrefab()
        {
            string locationToSavePrefab = GetAltTesterPrefabLocation();
            Directory.CreateDirectory("Assets/Prefabs/AltTester");
            CreateAltPrefab.CreateAltPrefabForUnity6(locationToSavePrefab);
            return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(locationToSavePrefab);

        }

        public static string GetAltTesterPrefabLocation()
        {
            return $"Assets/Prefabs/AltTester/{PREFABNAMEWITHEXTENSION}";
        }
        public static GameObject GetAltTesterPrefab()
        {
            var AltTesterPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(GetAltTesterPrefabLocation());
            if (AltTesterPrefab == null)
                return CreateAltTesterPrefab();
            // A prefab generated with another setup, e.g. another Active Input Handling, can reference scripts that
            // aren't compiled now. AltTesterImportErrorChecker removes it in delayCall, which never runs with
            // -batchmode -quit, so check it here too. Regenerating it in place keeps its GUID, and the scenes using it.
            if (HasMissingScripts(AltTesterPrefab))
            {
                logger.Info($"{GetAltTesterPrefabLocation()} has missing scripts, so it is generated again.");
                return CreateAltTesterPrefab();
            }
            return AltTesterPrefab;
        }

        internal static bool HasMissingScripts(GameObject prefab)
        {
            return prefab.GetComponentsInChildren<Component>(true).Any(component => component == null);
        }
        [System.Obsolete("Use InsertAltTesterInTheActiveScene instead.")]
        public static void InsertAltInTheActiveScene(AltInstrumentationSettings instrumentationSettings)
        {
            InsertAltTesterInTheActiveScene(instrumentationSettings);
        }
        public static void InsertAltTesterInTheActiveScene(AltInstrumentationSettings instrumentationSettings)
        {
            var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
            if (string.IsNullOrEmpty(activeScene))
            {
                logger.Warn("The active scene is not saved, so the AltTester® prefab can't be added to it. Save the scene first.");
                return;
            }
            InsertAltTesterInScene(activeScene, instrumentationSettings);
        }
        [System.Obsolete("Use InsertAltTesterInTheFirstScene instead.")]
        public static void InsertAltInTheFirstScene(AltInstrumentationSettings instrumentationSettings)
        {
            InsertAltTesterInTheFirstScene(instrumentationSettings);
        }
        public static void InsertAltTesterInTheFirstScene(AltInstrumentationSettings instrumentationSettings)
        {

            var altRunner = GetAltTesterPrefab();
            PreviousScenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
            SceneWithAltRunner = EditorSceneManager.OpenScene(GetFirstSceneWhichWillBeBuilt());
            insertAltTesterInSceneWithAltRunner(altRunner, instrumentationSettings);
            try
            {
                EditorSceneManager.OpenScene(PreviousScenePath);
            }
            catch
            {
                logger.Info("No scene was loaded yet.");
            }
        }

        public static string GetFirstSceneWhichWillBeBuilt()
        {
            foreach (var scene in AltTesterEditorWindow.EditorConfiguration.Scenes)
            {
                if (scene.ToBeBuilt)
                {
                    return scene.Path;
                }
            }

            return "";
        }

        private static string getOutputPath(UnityEditor.BuildTarget target)
        {
            var outputPath = AltTesterEditorWindow.EditorConfiguration.BuildLocationPath;

            outputPath = string.IsNullOrEmpty(outputPath) ? "build" : outputPath;
            outputPath = System.IO.Path.Combine(outputPath, UnityEditor.PlayerSettings.productName);

            switch (target)
            {
                case UnityEditor.BuildTarget.Android:
                    if (!outputPath.EndsWith(".apk"))
                    {
                        outputPath += ".apk";
                    }

                    break;
                case UnityEditor.BuildTarget.StandaloneOSX:
                    if (!outputPath.EndsWith(".app"))
                    {
                        outputPath += ".app";
                    }
                    break;
                case UnityEditor.BuildTarget.StandaloneWindows:
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    if (!outputPath.EndsWith(".exe"))
                    {
                        outputPath += ".exe";
                    }
                    break;
                case UnityEditor.BuildTarget.StandaloneLinux64:
                    if (!outputPath.EndsWith(".x86_64"))
                    {
                        outputPath += ".x86_64";
                    }
                    break;
#if NINTENDO_ENABLED && ALTTESTER_NONGPL
                case UnityEditor.BuildTarget.Switch:
                    if (!outputPath.EndsWith(".nspd"))
                    {
                        outputPath += ".nspd";
                    }
                    break;
#endif

                case UnityEditor.BuildTarget.iOS:
                default:
                    break;

            }

            return outputPath;
        }

        private static void resetBuildSetup(UnityEditor.BuildTargetGroup buildTargetGroup, PlayerSettingsSnapshot playerSettings)
        {
            bool keepAltTesterDefined = AltTesterEditorWindow.EditorConfiguration != null && AltTesterEditorWindow.EditorConfiguration.KeepAUTSymbolDefined;
            playerSettings.Restore(keepAltTesterDefined ? ALTTESTERDEFINE : null);

            if (CheckAltTesterIsDefineAsAScriptingSymbol(buildTargetGroup))
            {
                logger.Warn($"{ALTTESTERDEFINE} is still defined for {buildTargetGroup}, so builds made outside the AltTester® Editor are instrumented too. " +
                    "Turn off \"Keep ALTTESTER symbol defined\" in the AltTester® Editor, or remove the symbol from the Player Settings, before making a production build.");
            }
        }

        /// <summary>
        /// The Player Settings an AltTester Editor build changes, saved before the build so they can be restored after it.
        /// </summary>
        private class PlayerSettingsSnapshot
        {
            private readonly UnityEditor.BuildTargetGroup buildTargetGroup;
            private readonly string productName;
            private readonly string applicationIdentifier;
            private readonly string scriptingDefines;
            private readonly Dictionary<LogType, StackTraceLogType> stackTraceLogTypes = new Dictionary<LogType, StackTraceLogType>();
#if NINTENDO_ENABLED && ALTTESTER_NONGPL
            private readonly UnityEditor.ManagedStrippingLevel switchStrippingLevel;
#endif

            public PlayerSettingsSnapshot(UnityEditor.BuildTargetGroup buildTargetGroup)
            {
                this.buildTargetGroup = buildTargetGroup;
                productName = UnityEditor.PlayerSettings.productName;
                applicationIdentifier = getApplicationIdentifier(buildTargetGroup);
                scriptingDefines = AltBuildInstrumentation.GetPlayerSettingsDefines(buildTargetGroup);
                foreach (LogType logType in System.Enum.GetValues(typeof(LogType)))
                {
                    stackTraceLogTypes[logType] = UnityEditor.PlayerSettings.GetStackTraceLogType(logType);
                }
#if NINTENDO_ENABLED && ALTTESTER_NONGPL
                switchStrippingLevel = UnityEditor.PlayerSettings.GetManagedStrippingLevel(UnityEditor.BuildTargetGroup.Switch);
#endif
            }

            /// <param name="keepDefined">A symbol to leave defined even if it wasn't defined before the build.</param>
            public void Restore(string keepDefined)
            {
                if (UnityEditor.PlayerSettings.productName != productName)
                    UnityEditor.PlayerSettings.productName = productName;
                if (getApplicationIdentifier(buildTargetGroup) != applicationIdentifier)
                    setApplicationIdentifier(buildTargetGroup, applicationIdentifier);
                foreach (var stackTraceLogType in stackTraceLogTypes)
                {
                    UnityEditor.PlayerSettings.SetStackTraceLogType(stackTraceLogType.Key, stackTraceLogType.Value);
                }
#if NINTENDO_ENABLED && ALTTESTER_NONGPL
                UnityEditor.PlayerSettings.SetManagedStrippingLevel(UnityEditor.BuildTargetGroup.Switch, switchStrippingLevel);
#endif

                var defines = AltBuildInstrumentation.SplitDefines(scriptingDefines);
                if (keepDefined != null && !defines.Contains(keepDefined))
                    defines = defines.Append(keepDefined).ToArray();
                var restoredDefines = string.Join(";", defines);
                // Setting the defines recompiles the scripts, so only do it when they changed.
                if (string.Join(";", AltBuildInstrumentation.SplitDefines(AltBuildInstrumentation.GetPlayerSettingsDefines(buildTargetGroup))) != restoredDefines)
                    setScriptingDefineSymbols(buildTargetGroup, restoredDefines);
            }

            private static string getApplicationIdentifier(UnityEditor.BuildTargetGroup buildTargetGroup)
            {
#if UNITY_6000_0_OR_NEWER
                return UnityEditor.PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup));
#else
                return UnityEditor.PlayerSettings.GetApplicationIdentifier(buildTargetGroup);
#endif
            }

            private static void setApplicationIdentifier(UnityEditor.BuildTargetGroup buildTargetGroup, string identifier)
            {
#if UNITY_6000_0_OR_NEWER
                UnityEditor.PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup), identifier);
#else
                UnityEditor.PlayerSettings.SetApplicationIdentifier(buildTargetGroup, identifier);
#endif
            }
        }

        private static void buildGame(bool autoRun, UnityEditor.BuildPlayerOptions buildPlayerOptions)
        {
            UnityEditor.PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            UnityEditor.PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
            UnityEditor.PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.None);


            buildPlayerOptions.options = UnityEditor.BuildOptions.Development | (autoRun ? UnityEditor.BuildOptions.AutoRunPlayer : UnityEditor.BuildOptions.ShowBuiltPlayer);


            var results = UnityEditor.BuildPipeline.BuildPlayer(buildPlayerOptions);



            if (results.summary.totalErrors == 0 || results.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                logger.Info($"Build path: {buildPlayerOptions.locationPathName}");
                logger.Info($"Build {UnityEditor.PlayerSettings.productName}:{UnityEditor.PlayerSettings.bundleVersion} Succeeded");
            }
            else
            {
                logger.Error($"Build Error! {results.steps}\n Result: {results.summary.result}\n Stripping info: {results.strippingInfo}");
            }


        }



        private static string[] getScenesForBuild()
        {
            if (AltTesterEditorWindow.EditorConfiguration.Scenes.Count == 0)
            {
                AltTesterEditorWindow.AddAllScenes();
                AltTesterEditorWindow.SelectAllScenes();
            }
            var sceneList = new List<string>();
            foreach (var scene in AltTesterEditorWindow.EditorConfiguration.Scenes)
            {
                if (scene.ToBeBuilt)
                {
                    sceneList.Add(scene.Path);
                }
            }

            InsertAltTesterInTheFirstScene(AltTesterEditorWindow.EditorConfiguration.GetInstrumentationSettings());

            return sceneList.ToArray();
        }
    }
}
