/*
    Copyright(C) 2024 Altom Consulting

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
using AltTester.AltTesterUnitySDK.Commands;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Editor.Logging;
#if UNITY_2021_3_OR_NEWER && ADDRESSABLES
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
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
                UnityEditor.PlayerSettings.SetApplicationIdentifier(buildTargetGroup, $"{UnityEditor.PlayerSettings.GetApplicationIdentifier(buildTargetGroup)}Test");
            }
            AddAltTesterInScriptingDefineSymbolsGroup(buildTargetGroup);
            if (buildTargetGroup == UnityEditor.BuildTargetGroup.Standalone)
                CreateJsonFileForInputMappingOfAxis();

        }
        public static void BuildGameFromUI(UnityEditor.BuildTarget buildTarget, UnityEditor.BuildTargetGroup buildTargetGroup, bool autoRun = false)
        {
            try
            {
#if UNITY_2021_3_OR_NEWER && ADDRESSABLES
                AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
                AddressableAssetSettings.PlayerBuildOption currentValue = AddressableAssetSettings.PlayerBuildOption.PreferencesValue;
                if (settings != null)
                {
                    currentValue = settings.BuildAddressablesWithPlayerBuild;
                    if (!(settings.BuildAddressablesWithPlayerBuild == AddressableAssetSettings.PlayerBuildOption.DoNotBuildWithPlayer))
                    {
                        AddressableAssetSettings.CleanPlayerContent();
                        AddressableAssetSettings.BuildPlayerContent(out _);
                    }
                    settings.BuildAddressablesWithPlayerBuild = AddressableAssetSettings.PlayerBuildOption.DoNotBuildWithPlayer;
                }
#endif
                InitBuildSetup(buildTargetGroup);
                logger.Debug($"Starting {buildTarget} build...{UnityEditor.PlayerSettings.productName}:{UnityEditor.PlayerSettings.bundleVersion}");

                var buildPlayerOptions = new UnityEditor.BuildPlayerOptions
                {
                    locationPathName = getOutputPath(buildTarget),
                    scenes = getScenesForBuild(),
                    target = buildTarget,
                    targetGroup = buildTargetGroup
                };

                buildGame(autoRun, buildPlayerOptions);
#if UNITY_2021_3_OR_NEWER && ADDRESSABLES
                if (settings != null)
                {
                    settings.BuildAddressablesWithPlayerBuild = currentValue;
                }
#endif
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
            RemoveScriptingDefineSymbol(ALTTESTERDEFINE, targetGroup);
        }
        public static void RemoveScriptingDefineSymbol(string symbol, UnityEditor.BuildTargetGroup targetGroup)
        {
            if (AltTesterEditorWindow.EditorConfiguration != null && AltTesterEditorWindow.EditorConfiguration.KeepAUTSymbolDefined)
                return;
            try
            {
                var scriptingDefineSymbolsForGroup =
                    UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
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
                    UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup,
                        newScriptingDefineSymbolsForGroup);
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

            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            if (!scriptingDefineSymbolsForGroup.Contains(symbol))
            {
                scriptingDefineSymbolsForGroup += ";" + symbol;
            }
            UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, scriptingDefineSymbolsForGroup);
        }

        [System.Obsolete("Use AddAltTesterInScriptingDefineSymbolsGroup instead.")]
        public static void AddAltTesterInScritpingDefineSymbolsGroup(UnityEditor.BuildTargetGroup targetGroup)
        {
            AddAltTesterInScriptingDefineSymbolsGroup(targetGroup);
        }

        public static bool CheckAltTesterIsDefineAsAScriptingSymbol(UnityEditor.BuildTargetGroup targetGroup)
        {
            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
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

        public static void InsertAltInScene(string scene, AltInstrumentationSettings instrumentationSettings)
        {
            logger.Debug("Adding AltTesterPrefab into the [" + scene + "] scene.");
            var altRunner = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets(PREFABNAME)[0]));

            SceneWithAltRunner = EditorSceneManager.OpenScene(scene);
            AltRunner = UnityEditor.PrefabUtility.InstantiatePrefab(altRunner);
            var altRunnerComponent = ((GameObject)AltRunner).GetComponent<AltRunner>();
            altRunnerComponent.InstrumentationSettings = instrumentationSettings;

            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
            logger.Info("AltTesterPrefab successfully modified into the [" + scene + "] scene.");
        }

        public static void InsertAltInTheActiveScene(AltInstrumentationSettings instrumentationSettings)
        {
            var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
            InsertAltInScene(activeScene, instrumentationSettings);
        }

        public static void InsertAltInTheFirstScene(AltInstrumentationSettings instrumentationSettings)
        {
            var altRunner = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets(PREFABNAME)[0]));

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
                string bundleIdentifier = UnityEditor.PlayerSettings.GetApplicationIdentifier(buildTargetGroup).Remove(UnityEditor.PlayerSettings.GetApplicationIdentifier(buildTargetGroup).Length - 5);
                UnityEditor.PlayerSettings.SetApplicationIdentifier(buildTargetGroup, bundleIdentifier);
            }

            RemoveAltTesterFromScriptingDefineSymbols(buildTargetGroup);
        }

        private static void buildGame(bool autoRun, UnityEditor.BuildPlayerOptions buildPlayerOptions)
        {
            UnityEditor.PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            UnityEditor.PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
            UnityEditor.PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.None);

#if ENABLE_INPUT_SYSTEM
            modifyTestAssembliesToOnlyWorkInEditor();
            buildPlayerOptions.options = UnityEditor.BuildOptions.Development | (autoRun ? UnityEditor.BuildOptions.AutoRunPlayer : UnityEditor.BuildOptions.ShowBuiltPlayer) | UnityEditor.BuildOptions.IncludeTestAssemblies;
#else
            buildPlayerOptions.options = UnityEditor.BuildOptions.Development | (autoRun ? UnityEditor.BuildOptions.AutoRunPlayer : UnityEditor.BuildOptions.ShowBuiltPlayer); 
#endif


            var results = UnityEditor.BuildPipeline.BuildPlayer(buildPlayerOptions);


#if UNITY_2017
            if (results.Equals(""))
            {
                logger.Info("Build path: " + buildPlayerOptions.locationPathName);
                logger.Info("Build " + UnityEditor.PlayerSettings.productName + ":" + UnityEditor.PlayerSettings.bundleVersion + " Succeeded");
            }
            else
            {
                logger.Error("Build Error!");
            }

#else
            if (results.summary.totalErrors == 0 || results.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                logger.Info($"Build path: {buildPlayerOptions.locationPathName}");
                logger.Info($"Build {UnityEditor.PlayerSettings.productName}:{UnityEditor.PlayerSettings.bundleVersion} Succeeded");
            }
            else
            {
                logger.Error($"Build Error! {results.steps}\n Result: {results.summary.result}\n Stripping info: {results.strippingInfo}");
            }
#endif

        }

        private static void modifyTestAssembliesToOnlyWorkInEditor()
        {
            var assemblies = CompilationPipeline.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.flags == AssemblyFlags.None)
                {

                    var path = CompilationPipeline.GetAssemblyDefinitionFilePathFromAssemblyName(assembly.name);
                    if (path == null)
                        continue;
                    StreamReader reader = new StreamReader(path);
                    string input = reader.ReadToEnd();
                    reader.Close();
                    if (input.Contains("UNITY_INCLUDE_TESTS"))
                    {
                        using (StreamWriter writer = new StreamWriter(path, false))
                        {
                            string output = input.Replace("\"includePlatforms\": [],",
                            "\"includePlatforms\": [\"Editor\"],");
                            writer.Write(output);
                            writer.Close();
                        }
                    }
                }
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

            InsertAltInTheFirstScene(AltTesterEditorWindow.EditorConfiguration.GetInstrumentationSettings());

            return sceneList.ToArray();
        }
    }
}
