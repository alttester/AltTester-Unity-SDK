using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;


public static class PlatformName
{	
	public const string Android = "Android";
	public const string iOS = "iOS";
}


public class AltUnityBuilder {

    public static bool built = false;


    public static string PreviousScenePath;
    public static Scene SceneWithAltUnityRunner;
    public static string SceneWithAltUnityRunnerPath;
    public static Object AltUnityRunner;
    public static Scene copyScene;

    public static void InitBuildSetup(BuildTargetGroup buildTargetGroup) {

        if (AltUnityTesterEditor.EditorConfiguration.appendToName) {
            PlayerSettings.productName = PlayerSettings.productName + "Test";
            string bundleIdentifier = PlayerSettings.GetApplicationIdentifier(buildTargetGroup) + "Test";
            PlayerSettings.SetApplicationIdentifier(buildTargetGroup, bundleIdentifier);
        }
        AddAltUnityTesterInScritpingDefineSymbolsGroup(buildTargetGroup);
    }

    private static void ResetBuildSetup(BuildTargetGroup buildTargetGroup) {

        if (AltUnityTesterEditor.EditorConfiguration.appendToName) {
            PlayerSettings.productName = PlayerSettings.productName.Remove(PlayerSettings.productName.Length - 5);
            string bundleIdentifier = PlayerSettings.GetApplicationIdentifier(buildTargetGroup).Remove(PlayerSettings.GetApplicationIdentifier(buildTargetGroup).Length - 5);
            PlayerSettings.SetApplicationIdentifier(buildTargetGroup, bundleIdentifier);
        }

        RemoveAltUnityTesterFromScriptingDefineSymbols(buildTargetGroup);
    }

    public static void BuildAndroidFromUI(bool autoRun = false) {
        try {
            AltUnityTesterEditor.InitEditorConfiguration();
            InitBuildSetup(BuildTargetGroup.Android);
            Debug.Log("Starting Android build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.locationPathName = AltUnityTesterEditor.EditorConfiguration.OutputPathName + ".apk";
            buildPlayerOptions.scenes = GetScenesForBuild();

            buildPlayerOptions.target = BuildTarget.Android;
            if (autoRun) {
                buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AutoRunPlayer;
            } else {
                buildPlayerOptions.options = BuildOptions.Development;
            }
            var results = BuildPipeline.BuildPlayer(buildPlayerOptions);



#if UNITY_2017
            if (results.Equals(""))
            {
                Debug.Log("No Build Errors");

            }
            else
                Debug.LogError("Build Error!");

#else
            if (results.summary.totalErrors == 0) {
                Debug.Log("No Build Errors");

            } else
                Debug.LogError("Build Error! " + results.steps + "\n Result: " + results.summary.result +
                               "\n Stripping info: " + results.strippingInfo);

#endif

            Debug.Log("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
        } catch (Exception e) {
            Debug.LogError(e);
        } finally {
            built = true;
            ResetBuildSetup(BuildTargetGroup.Android);
        }

    }
    

    public static void RemoveAltUnityTesterFromScriptingDefineSymbols(BuildTargetGroup targetGroup) {
        try {
            string altunitytesterdefine = "ALTUNITYTESTER";
            var scriptingDefineSymbolsForGroup =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            string newScriptingDefineSymbolsForGroup = "";
            if (scriptingDefineSymbolsForGroup.Contains(altunitytesterdefine)) {
                var split = scriptingDefineSymbolsForGroup.Split(';');
                foreach (var define in split) {
                    if (define != altunitytesterdefine) {
                        newScriptingDefineSymbolsForGroup += define + ";";
                    }
                }
                if (newScriptingDefineSymbolsForGroup.Length != 0)
                    newScriptingDefineSymbolsForGroup.Remove(newScriptingDefineSymbolsForGroup.Length - 1);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup,
                    newScriptingDefineSymbolsForGroup);
            }
        } catch (Exception e) {
            Debug.LogError("Some Error Happened +" + e.Message);
            Debug.LogError("Stack trace " + e.StackTrace);
        }
    }


    public static void AddAltUnityTesterInScritpingDefineSymbolsGroup(BuildTargetGroup targetGroup) {
        var scriptingDefineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        if (!scriptingDefineSymbolsForGroup.Contains("ALTUNITYTESTER"))
            scriptingDefineSymbolsForGroup += ";ALTUNITYTESTER";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, scriptingDefineSymbolsForGroup);
    }

    private static string[] GetScenesForBuild() {
        if (AltUnityTesterEditor.EditorConfiguration.Scenes.Count == 0) {
            AltUnityTesterEditor.AddAllScenes();
            AltUnityTesterEditor.SelectAllScenes();
        }
        List<String> sceneList = new List<string>();
        foreach (var scene in AltUnityTesterEditor.EditorConfiguration.Scenes) {
            if (scene.ToBeBuilt) {
                sceneList.Add(scene.Path);
            }
        }

        InsertAltUnityInTheFirstScene();


        return sceneList.ToArray();
    }

    public static void InsertAltUnityInScene(String scene) {
        Debug.Log("Adding AltUnityRunnerPrefab into the [" + scene + "] scene.");
        var altUnityRunner = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("AltUnityRunnerPrefab")[0]));
        SceneWithAltUnityRunner = EditorSceneManager.OpenScene(scene);
        AltUnityRunner = PrefabUtility.InstantiatePrefab(altUnityRunner);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        Debug.Log("Scene successfully modified.");
    }

    public static void InsertAltUnityInTheFirstScene() {
        var altUnityRunner =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("AltUnityRunnerPrefab")[0]));



        PreviousScenePath = SceneManager.GetActiveScene().path;
        SceneWithAltUnityRunner = EditorSceneManager.OpenScene(GetFirstSceneWhichWillBeBuilt());

        AltUnityRunner = PrefabUtility.InstantiatePrefab(altUnityRunner);
        AltUnityRunner altUnityRunnerComponent = ((GameObject)AltUnityRunner).GetComponent<AltUnityRunner>();
        altUnityRunnerComponent.SocketPortNumber = AltUnityTesterEditor.EditorConfiguration.serverPort;
        altUnityRunnerComponent.requestEndingString = AltUnityTesterEditor.EditorConfiguration.requestEnding;
        altUnityRunnerComponent.requestSeparatorString = AltUnityTesterEditor.EditorConfiguration.requestSeparator;
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();

        try {
            EditorSceneManager.OpenScene(PreviousScenePath);
        } catch {
            Debug.Log("No scene was loaded yet.");
        }
    }

    public static string GetFirstSceneWhichWillBeBuilt() {
        foreach (var scene in AltUnityTesterEditor.EditorConfiguration.Scenes) {
            if (scene.ToBeBuilt) {
                return scene.Path;
            }
        }

        return "";
    }

    //#if UNITY_EDITOR_OSX


    public static void BuildiOSFromUI(bool autoRun) {
        try {
            Debug.Log("Starting IOS build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
            InitBuildSetup(BuildTargetGroup.iOS);
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.locationPathName = AltUnityTesterEditor.EditorConfiguration.OutputPathName;
            buildPlayerOptions.scenes = GetScenesForBuild();

            buildPlayerOptions.target = BuildTarget.iOS;
            if (autoRun) {
                buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AutoRunPlayer;
            } else {
                buildPlayerOptions.options = BuildOptions.Development;
            }

            var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
#if UNITY_2017
            if (results.Equals(""))
            {
                Debug.Log("No Build Errors");

            }
            else
                Debug.LogError("Build Error!");

#else
            if (results.summary.totalErrors == 0) {
                Debug.Log("No Build Errors");

            } else
                Debug.LogError("Build Error!");

#endif

            Debug.Log("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);

        } catch (Exception e) {
            Debug.LogError(e);
        } finally {
            built = true;
            ResetBuildSetup(BuildTargetGroup.iOS);
        }

    }

}
