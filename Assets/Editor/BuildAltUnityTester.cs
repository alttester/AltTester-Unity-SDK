using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildAltUnityTester {


    [MenuItem("Build/Android")]
    static void AndroidBuildFromCommandLine() {
        try {
            string versionNumber = DateTime.Now.ToString("yyMMddHHss");

            PlayerSettings.companyName = "Altom";
            PlayerSettings.productName = "sampleGame";
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "fi.altom.altunitytester");
            PlayerSettings.bundleVersion = versionNumber;
            PlayerSettings.Android.bundleVersionCode = int.Parse(versionNumber);
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
#if UNITY_2018_1_OR_NEWER
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
#endif

            Debug.Log("Starting Android build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new string[]
            {
            "Assets/AltUnityTesterExamples/Scenes/Scene 1 AltUnityDriverTestScene.unity",
            "Assets/AltUnityTesterExamples/Scenes/Scene 2 Draggable Panel.unity",
            "Assets/AltUnityTesterExamples/Scenes/Scene 3 Drag And Drop.unity",
            "Assets/AltUnityTesterExamples/Scenes/Scene 4 No Cameras.unity",
            "Assets/AltUnityTesterExamples/Scenes/Scene 5 Keyboard Input.unity",
            "Assets/AltUnityTesterExamples/Scenes/Scene6.unity"
            
            };

            buildPlayerOptions.locationPathName = "sampleGame.apk";
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AutoRunPlayer;
            
			AltUnityBuilder.AddAltUnityTesterInScritpingDefineSymbolsGroup(BuildTargetGroup.Android);
			AltUnityBuilder.InsertAltUnityInScene(buildPlayerOptions.scenes[0]);
            
			var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
            AltUnityBuilder.RemoveAltUnityTesterFromScriptingDefineSymbols(BuildTargetGroup.Android);


#if UNITY_2017
            if (results.Equals(""))
            {
                Debug.Log("No Build Errors");

            }
            else
                Debug.LogError("Build Error!");
            EditorApplication.Exit(1);

#else
            if (results.summary.totalErrors == 0) {
                Debug.Log("No Build Errors");

            } else {
                Debug.LogError("Build Error! " + results.steps + "\n Result: " + results.summary.result + "\n Stripping info: " + results.strippingInfo);
                EditorApplication.Exit(1);
            }

#endif

            Debug.Log("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
            EditorApplication.Exit(0);
        } catch (Exception exception) {
            Debug.LogException(exception);
            EditorApplication.Exit(1);
        }

    }
    
    [MenuItem("Build/iOS")]
    private static void IosBuildFromCommandLine() {
        try {
            string versionNumber = DateTime.Now.ToString("yyMMddHHss");
            PlayerSettings.companyName = "Altom";
            PlayerSettings.productName = "sampleGame";
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "fi.altom.altunitytester");
            PlayerSettings.bundleVersion = versionNumber;
            PlayerSettings.iOS.appleEnableAutomaticSigning = true;
            PlayerSettings.iOS.appleDeveloperTeamID = "59ESG8ELF5";
            Debug.Log("Starting IOS build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.locationPathName = "sampleGame";
           buildPlayerOptions.scenes = new string[]
            {
            "Assets/AltUnityTesterExamples/Scenes/Scene 1 AltUnityDriverTestScene.unity",
            "Assets/AltUnityTesterExamples/Scenes/Scene 2 Draggable Panel.unity",
            "Assets/AltUnityTesterExamples/Scenes/Scene 3 Drag And Drop.unity",
            "Assets/AltUnityTesterExamples/Scenes/Scene 4 No Cameras.unity",
            "Assets/AltUnityTesterExamples/Scenes/Scene 5 Keyboard Input.unity",
            "Assets/AltUnityTesterExamples/Scenes/Scene6.unity"
            };

            buildPlayerOptions.target = BuildTarget.iOS;
            buildPlayerOptions.options = BuildOptions.Development;

			AltUnityBuilder.AddAltUnityTesterInScritpingDefineSymbolsGroup(BuildTargetGroup.iOS);
			AltUnityBuilder.InsertAltUnityInScene(buildPlayerOptions.scenes[0]);
            
            var results = BuildPipeline.BuildPlayer(buildPlayerOptions);

#if UNITY_2017
            if (results.Equals(""))
            {
                Debug.Log("No Build Errors");

            }
            else
            Debug.LogError("Build Error!");
            EditorApplication.Exit(1);

#else
            if (results.summary.totalErrors == 0) {
                Debug.Log("No Build Errors");

            } else {
                Debug.LogError("Build Error!");
                EditorApplication.Exit(1);
            }

#endif
            Debug.Log("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
            // EditorApplication.Exit(0);

        } catch (Exception exception) {
            Debug.Log(exception);
            EditorApplication.Exit(1);
        }


    }
}
