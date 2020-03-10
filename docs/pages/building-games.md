# Building games with AltUnityTester

If changes are made inside a test, rebuilding the application is not necessary. A rebuild is needed only if changes are made inside the Unity project. For more information on writing tests, consult [Writing and running tests](writing-running-tests.md) page.

### 1.AltUnityTester GUI:

1. Open AltUnityTester from UnityEditor->Window->AltUnityTester
2. Select on what platform do you want to build the game
3. Press “Build Only” or “Build & Run” button



### 2.Custom Build

If you have a custom build from your game, you can just add to your method the following two methods:

``` c#
AltUnityBuilder.AddAltUnityTesterInScritpingDefineSymbolsGroup(BuildTargetGroup.Android);//Target group for which you are building
AltUnityBuilder.InsertAltUnityInScene(FirstSceneOfTheGame);
```


### 3.Manual

To build a game for the purpose of using AltUnityTester you will need the following:

* Add “ALTUNITYTESTER” in BuildSetting->Player Settings-> Other Settings->Scripting Define Symbols
* Add “AltUnityRunnerPrefab” in the first scene of your project. The prefab is found in “AltUnityTester->Prefabs” folder.
* Build the game in development mode


### 4.Command line

To build a game from command line we simply created a script with a method that sets all the projects settings needed.
```c#
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
            "Assets/AltUnityTester-BindingsAndExamples(can_be_deleted)/Scenes/Scene 1 AltUnityDriverTestScene.unity",
            "Assets/AltUnityTester-BindingsAndExamples(can_be_deleted)/Scenes/Scene 2 Draggable Panel.unity",
            "Assets/AltUnityTester-BindingsAndExamples(can_be_deleted)/Scenes/Scene 3 Drag And Drop.unity",
            "Assets/AltUnityTester-BindingsAndExamples(can_be_deleted)/Scenes/Scene 4 No Cameras.unity",
            "Assets/AltUnityTester-BindingsAndExamples(can_be_deleted)/Scenes/Scene 5 Keyboard Input.unity"
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
            // EditorApplication.Exit(0);
        } catch (Exception exception) {
            Debug.LogException(exception);
            // EditorApplication.Exit(1);
        }

    }
``` 
This is our method to build for Android that we call with the following command:
```bash
/Applications/Unity/Unity.app/Contents/MacOS/Unity -projectPath $CI_PROJECT_DIR -executeMethod BuildAltUnityTester.AndroidBuildFromCommandLine -logFile buildAndroid.log -quit
```
More info about commands you can find [here](https://docs.unity3d.com/Manual/CommandLineArguments.html)

**The two important lines in the method that add AltUnityTester in the build are the same as mentioned in [custom build](#custom-build)**


