/*
    Copyright(C) 2026 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
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
            CreateJsonFileForInputMappingOfAxis();

        }
        public static void BuildGameFromUI(UnityEditor.BuildTarget buildTarget, UnityEditor.BuildTargetGroup buildTargetGroup, bool autoRun = false)
        {
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
                resetBuildSetup(buildTargetGroup);
            }

        }
        public static void RemoveAltTesterFromScriptingDefineSymbols(UnityEditor.BuildTargetGroup targetGroup)
        {
            if (AltTesterEditorWindow.EditorConfiguration != null && AltTesterEditorWindow.EditorConfiguration.KeepAUTSymbolDefined)
                return;
            RemoveScriptingDefineSymbol(ALTTESTERDEFINE, targetGroup);
        }
        public static void RemoveScriptingDefineSymbol(string symbol, UnityEditor.BuildTargetGroup targetGroup)
        {
            try
            {
#if UNITY_6000_0_OR_NEWER
                var scriptingDefineSymbolsForGroup =
                    UnityEditor.PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(targetGroup));
#else
                var scriptingDefineSymbolsForGroup =
                UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
#endif
                string newScriptingDefineSymbolsForGroup = "";
                if (scriptingDefineSymbolsForGroup.Contains(symbol))
                {
                    var split = scriptingDefineSymbolsForGroup.Split(';');
                    foreach (var define in split)
                    {
                        if (define != symbol)
                        {
                            newScriptingDefineSymbolsForGroup += define + ";";
                        }
                    }
                    if (newScriptingDefineSymbolsForGroup.Length != 0)
                        newScriptingDefineSymbolsForGroup.Remove(newScriptingDefineSymbolsForGroup.Length - 1);
#if UNITY_6000_0_OR_NEWER
                    UnityEditor.PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(targetGroup),
                                           newScriptingDefineSymbolsForGroup);
#else
                    UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup,
                        newScriptingDefineSymbolsForGroup);
#endif

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
#if UNITY_6000_0_OR_NEWER
            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(targetGroup));
#else
            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
#endif
            if (!scriptingDefineSymbolsForGroup.Contains(symbol))
            {
                scriptingDefineSymbolsForGroup += ";" + symbol;
            }
#if UNITY_6000_0_OR_NEWER
            UnityEditor.PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(targetGroup), scriptingDefineSymbolsForGroup);

#else
            UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, scriptingDefineSymbolsForGroup);

#endif
        }

        [System.Obsolete("Use AddAltTesterInScriptingDefineSymbolsGroup instead.")]
        public static void AddAltTesterInScritpingDefineSymbolsGroup(UnityEditor.BuildTargetGroup targetGroup)
        {
            AddAltTesterInScriptingDefineSymbolsGroup(targetGroup);
        }

        public static bool CheckAltTesterIsDefineAsAScriptingSymbol(UnityEditor.BuildTargetGroup targetGroup)
        {
#if UNITY_6000_0_OR_NEWER
            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(targetGroup));
#else
            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
#endif
            return scriptingDefineSymbolsForGroup.Contains(ALTTESTERDEFINE);
        }

        public static void CreateJsonFileForInputMappingOfAxis()
        {
            string gameDataProjectFilePath = "/Resources/AltTester/AltTesterInputAxisData.json";
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

            string dataAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(axisList);
            string filePath = Application.dataPath + gameDataProjectFilePath;
            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources/AltTester"))
            {
                if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
                }
                UnityEditor.AssetDatabase.CreateFolder("Assets/Resources", "AltTester");
            }
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(dataAsJson);
            }
            UnityEditor.AssetDatabase.Refresh();
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
            AltRunner = UnityEditor.PrefabUtility.InstantiatePrefab(AltPrefab);
            var altRunnerComponent = ((GameObject)AltRunner).GetComponent<AltRunner>();
            altRunnerComponent.InstrumentationSettings = instrumentationSettings;

            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
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
            return AltTesterPrefab;
        }
        [System.Obsolete("Use InsertAltTesterInTheActiveScene instead.")]
        public static void InsertAltInTheActiveScene(AltInstrumentationSettings instrumentationSettings)
        {
            InsertAltTesterInTheActiveScene(instrumentationSettings);
        }
        public static void InsertAltTesterInTheActiveScene(AltInstrumentationSettings instrumentationSettings)
        {
            var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().Where(gameObject => gameObject.name.Equals(PREFABNAME)).ToList().Count > 0)
            {
                logger.Info("AltTester® prefab is already in the active scene.");
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

            AltRunner = UnityEditor.PrefabUtility.InstantiatePrefab(altRunner);
            AltRunner altRunnerComponent = ((GameObject)AltRunner).GetComponent<AltRunner>();
            altRunnerComponent.InstrumentationSettings = instrumentationSettings;
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
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

        private static void resetBuildSetup(UnityEditor.BuildTargetGroup buildTargetGroup)
        {
            if (AltTesterEditorWindow.EditorConfiguration.appendToName)
            {
                UnityEditor.PlayerSettings.productName = UnityEditor.PlayerSettings.productName.Remove(UnityEditor.PlayerSettings.productName.Length - 5);
#if UNITY_6000_0_OR_NEWER
                string bundleIdentifier = UnityEditor.PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup)).Remove(UnityEditor.PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup)).Length - 5);
                UnityEditor.PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup), bundleIdentifier);
#else
                string bundleIdentifier = UnityEditor.PlayerSettings.GetApplicationIdentifier(buildTargetGroup).Remove(UnityEditor.PlayerSettings.GetApplicationIdentifier(buildTargetGroup).Length - 5);
                UnityEditor.PlayerSettings.SetApplicationIdentifier(buildTargetGroup, bundleIdentifier);
#endif

            }

            RemoveAltTesterFromScriptingDefineSymbols(buildTargetGroup);
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
