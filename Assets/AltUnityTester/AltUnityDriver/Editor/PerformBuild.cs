using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


class PerformBuild
{
    private static string bundleIdentifierDefault = "fi.altom.altunitytester";
    private static string productName = "sampleGame";
    private static string companyName = "Altom";
    private static string outputFolder = "";
    private static string outputFilenameAndroidDefault = outputFolder + PerformBuild.productName + ".apk";
    private static int buildNumber = 0;

    private static void Init()
    {
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
    static void AndroidDefault()
    {
        Debug.Log("Starting Android build..." + PerformBuild.productName + " : " + PlayerSettings.bundleVersion);
        Init();
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = outputFilenameAndroidDefault;
        buildPlayerOptions.scenes = new[] {"Assets/ExamplesAndTests (can be deleted)/Scenes/Scene 1 AltUnityDriverTestScene.unity",
                                            "Assets/ExamplesAndTests (can be deleted)/Scenes/Scene 2 Draggable Panel.unity",
                                            "Assets/ExamplesAndTests (can be deleted)/Scenes/Scene 3 Drag And Drop.unity",
                                            "Assets/ExamplesAndTests (can be deleted)/Scenes/Scene 4 GameWithMobileSingleStickControls.unity",
                                            "Assets/ExamplesAndTests (can be deleted)/Scenes/Scene 5 GameWithCarTiltControls.unity"};
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.Development|BuildOptions.AutoRunPlayer;
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "INPUT;MOBILE_INPUT;ALTUNITYTESTER");
        var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (results.summary.totalErrors == 0)
            Debug.Log("No Build Errors");
        else
        {
            var ErrorMessageString = "Steps:\n";
            foreach (var steps in results.steps)
            {

                ErrorMessageString += steps.name + " ";
                foreach (var message in steps.messages)
                {
                    ErrorMessageString = "\n" + message.type + " -> " + message.content;
                }

                ErrorMessageString += "\n" + steps.duration + "\n";

            }

            ErrorMessageString += "\n\n";

            Debug.LogError("Build Error! " + ErrorMessageString + "\n Result: " + results.summary.result + "\n Stripping info: " + results.strippingInfo);
        }
        Debug.Log("Finished. " + PerformBuild.productName + " : " + PlayerSettings.bundleVersion);
    }
}
