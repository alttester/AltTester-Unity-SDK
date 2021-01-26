using Altom.AltUnityDriver;

public static class PlatformName
{
    public const string Android = "Android";
    public const string iOS = "iOS";
    public const string Standalone = "Standalone";
}


public class AltUnityBuilder
{
    public enum InputType
    {
        KeyOrMouseButton,
        MouseMovement,
        JoystickAxis,
    };

    public static bool built = false;
    public static string PreviousScenePath;
    public static UnityEngine.SceneManagement.Scene SceneWithAltUnityRunner;
    public static string SceneWithAltUnityRunnerPath;
    public static UnityEngine.Object AltUnityRunner;
    public static UnityEngine.SceneManagement.Scene copyScene;

    public static void InitBuildSetup(UnityEditor.BuildTargetGroup buildTargetGroup)
    {

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
            AltUnityTesterEditor.InitEditorConfiguration();
            InitBuildSetup(UnityEditor.BuildTargetGroup.Android);
            UnityEngine.Debug.Log("Starting Android build..." + UnityEditor.PlayerSettings.productName + " : " + UnityEditor.PlayerSettings.bundleVersion);

            UnityEditor.BuildPlayerOptions buildPlayerOptions = new UnityEditor.BuildPlayerOptions();
            buildPlayerOptions.locationPathName = getOutputPath(UnityEditor.BuildTarget.Android);
            buildPlayerOptions.scenes = getScenesForBuild();
            buildPlayerOptions.target = UnityEditor.BuildTarget.Android;

            buildGame(autoRun, buildPlayerOptions);
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError(e);
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
            AltUnityTesterEditor.InitEditorConfiguration();
            InitBuildSetup(UnityEditor.BuildTargetGroup.Standalone);
            UnityEngine.Debug.Log("Starting Standalone build..." + UnityEditor.PlayerSettings.productName + " : " + UnityEditor.PlayerSettings.bundleVersion);

            UnityEditor.BuildPlayerOptions buildPlayerOptions = new UnityEditor.BuildPlayerOptions();
            buildPlayerOptions.locationPathName = getOutputPath(buildTarget);
            buildPlayerOptions.scenes = getScenesForBuild();
            buildPlayerOptions.target = buildTarget;

            buildGame(autoRun, buildPlayerOptions);
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError(e);
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
            UnityEngine.Debug.LogError("Some Error Happened +" + e.Message);
            UnityEngine.Debug.LogError("Stack trace " + e.StackTrace);
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
        UnityEditor.SerializedObject obj = new UnityEditor.SerializedObject(inputManager);

        UnityEditor.SerializedProperty axisArray = obj.FindProperty("m_Axes");

        if (axisArray.arraySize == 0)
            UnityEngine.Debug.Log("No Axes");
        System.Collections.Generic.List<AltUnityAxis> axisList = new System.Collections.Generic.List<AltUnityAxis>();
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
        string filePath = UnityEngine.Application.dataPath + gameDataProjectFilePath;
        if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources/AltUnityTester"))
        {
            UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
            UnityEditor.AssetDatabase.CreateFolder("Assets/Resources", "AltUnityTester");
        }
        System.IO.File.WriteAllText(filePath, dataAsJson);


    }

    public static void InsertAltUnityInScene(string scene, int port = 13000)
    {
        UnityEngine.Debug.Log("Adding AltUnityRunnerPrefab into the [" + scene + "] scene.");
        var altUnityRunner = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("AltUnityRunnerPrefab")[0]));

        SceneWithAltUnityRunner = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scene);
        AltUnityRunner = UnityEditor.PrefabUtility.InstantiatePrefab(altUnityRunner);
        var component = ((UnityEngine.GameObject)AltUnityRunner).GetComponent<AltUnityRunner>();
        if (AltUnityTesterEditor.EditorConfiguration == null)
        {
            component.ShowInputs = false;
            component.showPopUp = true;
            component.SocketPortNumber = port;
        }
        else
        {
            component.ShowInputs = AltUnityTesterEditor.EditorConfiguration.inputVisualizer;
            component.showPopUp = AltUnityTesterEditor.EditorConfiguration.showPopUp;
            component.SocketPortNumber = AltUnityTesterEditor.EditorConfiguration.serverPort;
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
        UnityEngine.Debug.Log("Scene successfully modified.");
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
        altUnityRunner.GetComponent<AltUnityRunner>().ShowInputs = AltUnityTesterEditor.EditorConfiguration.inputVisualizer;

        PreviousScenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
        SceneWithAltUnityRunner = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(GetFirstSceneWhichWillBeBuilt());

        AltUnityRunner = UnityEditor.PrefabUtility.InstantiatePrefab(altUnityRunner);
        AltUnityRunner altUnityRunnerComponent = ((UnityEngine.GameObject)AltUnityRunner).GetComponent<AltUnityRunner>();
        altUnityRunnerComponent.SocketPortNumber = AltUnityTesterEditor.EditorConfiguration.serverPort;
        altUnityRunnerComponent.requestEndingString = AltUnityTesterEditor.EditorConfiguration.requestEnding;
        altUnityRunnerComponent.requestSeparatorString = AltUnityTesterEditor.EditorConfiguration.requestSeparator;
        altUnityRunnerComponent.ShowInputs = AltUnityTesterEditor.EditorConfiguration.inputVisualizer;
        altUnityRunnerComponent.showPopUp = AltUnityTesterEditor.EditorConfiguration.showPopUp;
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();

        try
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(PreviousScenePath);
        }
        catch
        {
            UnityEngine.Debug.Log("No scene was loaded yet.");
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
            UnityEngine.Debug.Log("Starting IOS build..." + UnityEditor.PlayerSettings.productName + " : " + UnityEditor.PlayerSettings.bundleVersion);
            InitBuildSetup(UnityEditor.BuildTargetGroup.iOS);
            UnityEditor.BuildPlayerOptions buildPlayerOptions = new UnityEditor.BuildPlayerOptions();
            buildPlayerOptions.locationPathName = AltUnityTesterEditor.EditorConfiguration.OutputPathName;
            buildPlayerOptions.scenes = getScenesForBuild();

            buildPlayerOptions.target = UnityEditor.BuildTarget.iOS;
            buildGame(autoRun, buildPlayerOptions);

        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError(e);
        }
        finally
        {
            built = true;
            resetBuildSetup(UnityEditor.BuildTargetGroup.iOS);
        }

    }
#endif

    private static string getOutputPath(UnityEditor.BuildTarget target)
    {
        var outputPath = AltUnityTesterEditor.EditorConfiguration.OutputPathName;

        if (string.IsNullOrEmpty(outputPath))
            outputPath = UnityEditor.PlayerSettings.productName;

        if (outputPath.EndsWith("/") || outputPath.EndsWith("\\"))
            outputPath = outputPath + UnityEditor.PlayerSettings.productName;

        if (outputPath.Split('/').Length == 1 && outputPath.Split('\\').Length == 1)
            outputPath += System.IO.Path.DirectorySeparatorChar.ToString() + outputPath;

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
        UnityEditor.PlayerSettings.SetStackTraceLogType(UnityEngine.LogType.Log, UnityEngine.StackTraceLogType.None);
        UnityEditor.PlayerSettings.SetStackTraceLogType(UnityEngine.LogType.Warning, UnityEngine.StackTraceLogType.None);
        UnityEditor.PlayerSettings.SetStackTraceLogType(UnityEngine.LogType.Assert, UnityEngine.StackTraceLogType.None);

        if (autoRun)
        {
            buildPlayerOptions.options = UnityEditor.BuildOptions.Development | UnityEditor.BuildOptions.AutoRunPlayer;
        }
        else
        {
            buildPlayerOptions.options = UnityEditor.BuildOptions.Development;
        }
        var results = UnityEditor.BuildPipeline.BuildPlayer(buildPlayerOptions);

#if UNITY_2017
            if (results.Equals(""))
            {
                UnityEngine.Debug.Log("No Build Errors");

            }
            else
                UnityEngine.Debug.LogError("Build Error!");

#else
        if (results.summary.totalErrors == 0)
        {
            UnityEngine.Debug.Log("No Build Errors");

        }
        else
            UnityEngine.Debug.LogError("Build Error! " + results.steps + "\n Result: " + results.summary.result +
                           "\n Stripping info: " + results.strippingInfo);

#endif

        UnityEngine.Debug.Log("Finished. " + UnityEditor.PlayerSettings.productName + " : " + UnityEditor.PlayerSettings.bundleVersion);
    }

    private static string[] getScenesForBuild()
    {
        if (AltUnityTesterEditor.EditorConfiguration.Scenes.Count == 0)
        {
            AltUnityTesterEditor.AddAllScenes();
            AltUnityTesterEditor.SelectAllScenes();
        }
        System.Collections.Generic.List<string> sceneList = new System.Collections.Generic.List<string>();
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
