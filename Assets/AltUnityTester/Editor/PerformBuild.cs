using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


class PerformBuild {
    private static string bundleIdentifierDefault = "fi.altom.altunitytester";
    private static string productName = "sampleGame";
    private static string companyName = "Altom";
    private static string outputFolder = "";
    private static string outputFilenameAndroidDefault = outputFolder + PerformBuild.productName + ".apk";
    private static int buildNumber = 0;

	private static void Init() {
		string versionNumber = DateTime.Now.ToString("yyMMddHHss");
		
		PlayerSettings.productName = productName;
		PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, bundleIdentifierDefault);
		PlayerSettings.bundleVersion = versionNumber;
		PlayerSettings.Android.bundleVersionCode = int.Parse(versionNumber);
		PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
		PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
		PlayerSettings.companyName = companyName;
	}
	
	
	[MenuItem("Build/Target/Android")]
	static void AndroidDefault() {
		Debug.Log("Starting Android build..." +  PerformBuild.productName + " : " + PlayerSettings.bundleVersion);
        Init();
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = outputFilenameAndroidDefault;
        buildPlayerOptions.scenes = new [] {"Assets/AltUnityTester/ExamplesAndTests (can be deleted)/SampleScene/AltUnityDriverTestScene.unity"};
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.Development;
        string results = BuildPipeline.BuildPlayer(buildPlayerOptions).summary.result.ToString();
        if (results.Contains("Succeeded"))
            Debug.Log("No Build Errors");
        else
            Debug.LogError("Build Error! " + results);
		Debug.Log("Finished. " +  PerformBuild.productName + " : " + PlayerSettings.bundleVersion);
    }
 }
