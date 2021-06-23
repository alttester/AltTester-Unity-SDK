using System.Collections.Generic;
using Altom.AltUnityDriver;
using Altom.Editor.Logging;
using Assets.AltUnityTester.AltUnityServer.AltSocket;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Altom.Editor
{
    public static class PlatformName
    {
        public const string Android = "Android";
        public const string iOS = "iOS";
        public const string Standalone = "Standalone";
    }


    public class AltUnityBuilder
    {
        private static readonly NLog.Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();
        public enum InputType
        {
            KeyOrMouseButton,
            MouseMovement,
            JoystickAxis,
        };

        public static bool built = false;
        public static string PreviousScenePath;
        public static UnityEngine.SceneManagement.Scene SceneWithAltUnityRunner;
        public static UnityEngine.Object AltUnityRunner;

        public static void InitBuildSetup(UnityEditor.BuildTargetGroup buildTargetGroup)
        {
            AltUnityTesterEditor.InitEditorConfiguration();

            if (AltUnityTesterEditor.EditorConfiguration.appendToName)
            {
                UnityEditor.PlayerSettings.productName = UnityEditor.PlayerSettings.productName + "Test";
                string bundleIdentifier = UnityEditor.PlayerSettings.GetApplicationIdentifier(buildTargetGroup) + "Test";
                UnityEditor.PlayerSettings.SetApplicationIdentifier(buildTargetGroup, bundleIdentifier);
            }
            AddAltUnityTesterInScritpingDefineSymbolsGroup(buildTargetGroup);
            if (buildTargetGroup == UnityEditor.BuildTargetGroup.Standalone)
                CreateJsonFileForInputMappingOfAxis();

        }

        public static void BuildAndroidFromUI(bool autoRun = false)
        {
            try
            {
                InitBuildSetup(UnityEditor.BuildTargetGroup.Android);
                logger.Debug("Starting Android build..." + UnityEditor.PlayerSettings.productName + " : " + UnityEditor.PlayerSettings.bundleVersion);

                var buildPlayerOptions = new UnityEditor.BuildPlayerOptions
                {
                    locationPathName = getOutputPath(UnityEditor.BuildTarget.Android),
                    scenes = getScenesForBuild(),
                    target = UnityEditor.BuildTarget.Android
                };

                buildGame(autoRun, buildPlayerOptions);
            }
            catch (System.Exception e)
            {
                logger.Error(e);
            }
            finally
            {
                built = true;
                resetBuildSetup(UnityEditor.BuildTargetGroup.Android);
            }

        }

        public static void BuildStandaloneFromUI(UnityEditor.BuildTarget buildTarget, bool autoRun = false)
        {
            try
            {
                InitBuildSetup(UnityEditor.BuildTargetGroup.Standalone);
                logger.Debug("Starting Standalone build..." + UnityEditor.PlayerSettings.productName + " : " + UnityEditor.PlayerSettings.bundleVersion);

                UnityEditor.BuildPlayerOptions buildPlayerOptions = new UnityEditor.BuildPlayerOptions
                {
                    locationPathName = getOutputPath(buildTarget),
                    scenes = getScenesForBuild(),
                    target = buildTarget
                };

                buildGame(autoRun, buildPlayerOptions);
            }
            catch (System.Exception e)
            {
                logger.Error(e);
            }
            finally
            {
                built = true;
                resetBuildSetup(UnityEditor.BuildTargetGroup.Standalone);
            }

        }

        public static void RemoveAltUnityTesterFromScriptingDefineSymbols(UnityEditor.BuildTargetGroup targetGroup)
        {
            try
            {
                string altunitytesterdefine = "ALTUNITYTESTER";
                var scriptingDefineSymbolsForGroup =
                    UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
                string newScriptingDefineSymbolsForGroup = "";
                if (scriptingDefineSymbolsForGroup.Contains(altunitytesterdefine))
                {
                    var split = scriptingDefineSymbolsForGroup.Split(';');
                    foreach (var define in split)
                    {
                        if (define != altunitytesterdefine)
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

        public static void AddAltUnityTesterInScritpingDefineSymbolsGroup(UnityEditor.BuildTargetGroup targetGroup)
        {
            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            if (!scriptingDefineSymbolsForGroup.Contains("ALTUNITYTESTER"))
                scriptingDefineSymbolsForGroup += ";ALTUNITYTESTER";
            UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, scriptingDefineSymbolsForGroup);
        }

        public static void CreateJsonFileForInputMappingOfAxis()
        {
            string gameDataProjectFilePath = "/Resources/AltUnityTester/AltUnityTesterInputAxisData.json";
            var inputManager = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            var obj = new UnityEditor.SerializedObject(inputManager);

            UnityEditor.SerializedProperty axisArray = obj.FindProperty("m_Axes");

            if (axisArray.arraySize == 0)
                logger.Info("No Axes");
            var axisList = new List<AltUnityAxis>();
            for (int i = 0; i < axisArray.arraySize; ++i)
            {
                var axis = axisArray.GetArrayElementAtIndex(i);

                var name = axis.FindPropertyRelative("m_Name").stringValue;
                var inputType = (InputType)axis.FindPropertyRelative("type").intValue;
                var negativeButton = axis.FindPropertyRelative("negativeButton").stringValue;
                var positiveButton = axis.FindPropertyRelative("positiveButton").stringValue;
                var altPositiveButton = axis.FindPropertyRelative("altPositiveButton").stringValue;
                var altNegativeButton = axis.FindPropertyRelative("altNegativeButton").stringValue;
                axisList.Add(new AltUnityAxis(name, negativeButton, positiveButton, altPositiveButton, altNegativeButton));
            }

            string dataAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(axisList);
            string filePath = Application.dataPath + gameDataProjectFilePath;
            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources/AltUnityTester"))
            {
                if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
                }
                UnityEditor.AssetDatabase.CreateFolder("Assets/Resources", "AltUnityTester");
            }
            System.IO.File.WriteAllText(filePath, dataAsJson);
        }

        public static void InsertAltUnityInScene(string scene, int port = 13000)
        {
            logger.Debug("Adding AltUnityRunnerPrefab into the [" + scene + "] scene.");
            var altUnityRunner = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("AltUnityRunnerPrefab")[0]));

            SceneWithAltUnityRunner = EditorSceneManager.OpenScene(scene);
            AltUnityRunner = UnityEditor.PrefabUtility.InstantiatePrefab(altUnityRunner);
            var component = ((GameObject)AltUnityRunner).GetComponent<AltUnityRunner>();
            if (AltUnityTesterEditor.EditorConfiguration == null)
            {
                component.ShowInputs = false;
                component.showPopUp = true;
                component.SocketPortNumber = port;
            }
            else
            {
                initSetings(component);
            }

            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
            logger.Info("AltUnityRunnerPrefab successfully modified into the [" + scene + "] scene.");
        }

        public static void InsertAltUnityInTheActiveScene()
        {
            var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
            InsertAltUnityInScene(activeScene);
        }

        public static void InsertAltUnityInTheFirstScene()
        {
            var altUnityRunner =
                UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(
                    UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("AltUnityRunnerPrefab")[0]));
            altUnityRunner.GetComponent<AltUnityRunner>().ShowInputs = AltUnityTesterEditor.EditorConfiguration.InputVisualizer;

            PreviousScenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
            SceneWithAltUnityRunner = EditorSceneManager.OpenScene(GetFirstSceneWhichWillBeBuilt());

            AltUnityRunner = UnityEditor.PrefabUtility.InstantiatePrefab(altUnityRunner);
            AltUnityRunner altUnityRunnerComponent = ((UnityEngine.GameObject)AltUnityRunner).GetComponent<AltUnityRunner>();
            initSetings(altUnityRunnerComponent);


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
            foreach (var scene in AltUnityTesterEditor.EditorConfiguration.Scenes)
            {
                if (scene.ToBeBuilt)
                {
                    return scene.Path;
                }
            }

            return "";
        }

#if UNITY_EDITOR_OSX


        public static void BuildiOSFromUI(bool autoRun)
        {
            try
            {
                InitBuildSetup(UnityEditor.BuildTargetGroup.iOS);
                logger.Debug("Starting IOS build..." + UnityEditor.PlayerSettings.productName + " : " + UnityEditor.PlayerSettings.bundleVersion);
                UnityEditor.BuildPlayerOptions buildPlayerOptions = new UnityEditor.BuildPlayerOptions();
                buildPlayerOptions.locationPathName = getOutputPath(UnityEditor.BuildTarget.iOS);
                buildPlayerOptions.scenes = getScenesForBuild();

                buildPlayerOptions.target = UnityEditor.BuildTarget.iOS;
                buildGame(autoRun, buildPlayerOptions);

            }
            catch (System.Exception e)
            {
                logger.Error(e);
            }
            finally
            {
                built = true;
                resetBuildSetup(UnityEditor.BuildTargetGroup.iOS);
            }

        }
#endif

        private static void initSetings(AltUnityRunner altUnityRunnerComponent)
        {
            altUnityRunnerComponent.SocketPortNumber = AltUnityTesterEditor.EditorConfiguration.ServerPort;
            altUnityRunnerComponent.requestEndingString = AltUnityTesterEditor.EditorConfiguration.RequestEnding;
            altUnityRunnerComponent.requestSeparatorString = AltUnityTesterEditor.EditorConfiguration.RequestSeparator;
            altUnityRunnerComponent.ShowInputs = AltUnityTesterEditor.EditorConfiguration.InputVisualizer;
            altUnityRunnerComponent.showPopUp = AltUnityTesterEditor.EditorConfiguration.ShowPopUp;
            altUnityRunnerComponent.MaxLogLength = string.IsNullOrEmpty(AltUnityTesterEditor.EditorConfiguration.MaxLogLength) ? 0 : int.Parse(AltUnityTesterEditor.EditorConfiguration.MaxLogLength);
        }

        private static string getOutputPath(UnityEditor.BuildTarget target)
        {
            var outputPath = AltUnityTesterEditor.EditorConfiguration.BuildLocationPath;

            outputPath = string.IsNullOrEmpty(outputPath) ? UnityEditor.PlayerSettings.productName : outputPath + System.IO.Path.DirectorySeparatorChar.ToString() + UnityEditor.PlayerSettings.productName;

            switch (target)
            {
                case UnityEditor.BuildTarget.Android:
                    if (!outputPath.EndsWith(".apk"))
                    {
                        outputPath = outputPath + ".apk";
                    }

                    break;
                case UnityEditor.BuildTarget.StandaloneOSX:
                    if (!outputPath.EndsWith(".app"))
                    {
                        outputPath = outputPath + ".app";
                    }
                    break;
                case UnityEditor.BuildTarget.StandaloneWindows:
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    if (!outputPath.EndsWith(".exe"))
                    {
                        outputPath = outputPath + ".exe";
                    }
                    break;
                case UnityEditor.BuildTarget.StandaloneLinux64:
                    if (!outputPath.EndsWith(".x86_64"))
                    {
                        outputPath = outputPath + ".x86_64";
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
            if (AltUnityTesterEditor.EditorConfiguration.appendToName)
            {
                UnityEditor.PlayerSettings.productName = UnityEditor.PlayerSettings.productName.Remove(UnityEditor.PlayerSettings.productName.Length - 5);
                string bundleIdentifier = UnityEditor.PlayerSettings.GetApplicationIdentifier(buildTargetGroup).Remove(UnityEditor.PlayerSettings.GetApplicationIdentifier(buildTargetGroup).Length - 5);
                UnityEditor.PlayerSettings.SetApplicationIdentifier(buildTargetGroup, bundleIdentifier);
            }

            RemoveAltUnityTesterFromScriptingDefineSymbols(buildTargetGroup);
        }

        private static void buildGame(bool autoRun, UnityEditor.BuildPlayerOptions buildPlayerOptions)
        {
            UnityEditor.PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            UnityEditor.PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
            UnityEditor.PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.None);

            if (autoRun)
            {
                buildPlayerOptions.options = UnityEditor.BuildOptions.Development | UnityEditor.BuildOptions.AutoRunPlayer;
            }
            else
            {
                buildPlayerOptions.options = UnityEditor.BuildOptions.Development | UnityEditor.BuildOptions.ShowBuiltPlayer;
            }
            var results = UnityEditor.BuildPipeline.BuildPlayer(buildPlayerOptions);

#if UNITY_2017
            if (results.Equals(""))
            {
                logger.Info("No Build Errors");
            }
            else
            {
                logger.Error("Build Error!");
            }

#else
            if (results.summary.totalErrors == 0 || results.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                logger.Info("No Build Errors");
            }
            else
            {
                logger.Error("Build Error! " + results.steps + "\n Result: " + results.summary.result +
                           "\n Stripping info: " + results.strippingInfo);
            }
#endif

            logger.Info("Finished. " + UnityEditor.PlayerSettings.productName + " : " + UnityEditor.PlayerSettings.bundleVersion);
        }

        private static string[] getScenesForBuild()
        {
            if (AltUnityTesterEditor.EditorConfiguration.Scenes.Count == 0)
            {
                AltUnityTesterEditor.AddAllScenes();
                AltUnityTesterEditor.SelectAllScenes();
            }
            var sceneList = new List<string>();
            foreach (var scene in AltUnityTesterEditor.EditorConfiguration.Scenes)
            {
                if (scene.ToBeBuilt)
                {
                    sceneList.Add(scene.Path);
                }
            }

            InsertAltUnityInTheFirstScene();

            return sceneList.ToArray();
        }
    }
}