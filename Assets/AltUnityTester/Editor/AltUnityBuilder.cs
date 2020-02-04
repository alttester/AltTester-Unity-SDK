


public static class PlatformName
{
    public const string Android = "Android";
    public const string iOS = "iOS";
    public const string Standalone = "Standalone";
}


public class AltUnityBuilder
{

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
        if(buildTargetGroup==UnityEditor.BuildTargetGroup.Standalone)
            CreateJsonFileForInputMappingOfAxis();
    }

    private static void ResetBuildSetup(UnityEditor.BuildTargetGroup buildTargetGroup)
    {

        if (AltUnityTesterEditor.EditorConfiguration.appendToName)
        {
            UnityEditor.PlayerSettings.productName = UnityEditor.PlayerSettings.productName.Remove(UnityEditor.PlayerSettings.productName.Length - 5);
            string bundleIdentifier = UnityEditor.PlayerSettings.GetApplicationIdentifier(buildTargetGroup).Remove(UnityEditor.PlayerSettings.GetApplicationIdentifier(buildTargetGroup).Length - 5);
            UnityEditor.PlayerSettings.SetApplicationIdentifier(buildTargetGroup, bundleIdentifier);
        }

        RemoveAltUnityTesterFromScriptingDefineSymbols(buildTargetGroup);
    }

    public static void BuildAndroidFromUI(bool autoRun = false)
    {
        try
        {
            AltUnityTesterEditor.InitEditorConfiguration();
            InitBuildSetup(UnityEditor.BuildTargetGroup.Android);
            UnityEngine.Debug.Log("Starting Android build..." + UnityEditor.PlayerSettings.productName + " : " + UnityEditor.PlayerSettings.bundleVersion);
            UnityEditor.BuildPlayerOptions buildPlayerOptions = new UnityEditor.BuildPlayerOptions();
            if (AltUnityTesterEditor.EditorConfiguration.OutputPathName.Split('.').Length == 2)
                buildPlayerOptions.locationPathName = AltUnityTesterEditor.EditorConfiguration.OutputPathName;
            else
                buildPlayerOptions.locationPathName = AltUnityTesterEditor.EditorConfiguration.OutputPathName + ".apk";

            buildPlayerOptions.scenes = GetScenesForBuild();

            buildPlayerOptions.target = UnityEditor.BuildTarget.Android;
            BuildGame(autoRun, buildPlayerOptions);
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError(e);
        }
        finally
        {
            built = true;
            ResetBuildSetup(UnityEditor.BuildTargetGroup.Android);
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

            string ouputPath = AltUnityTesterEditor.EditorConfiguration.OutputPathName;
            switch (buildTarget)
            {
                case UnityEditor.BuildTarget.StandaloneOSX:
                    if (AltUnityTesterEditor.EditorConfiguration.OutputPathName.Split('.').Length == 1)
                        ouputPath += ".app";
                    break;
                case UnityEditor.BuildTarget.StandaloneWindows:
                    if (AltUnityTesterEditor.EditorConfiguration.OutputPathName.Split('/').Length == 1)
                        ouputPath += "/" + ouputPath;
                    if (AltUnityTesterEditor.EditorConfiguration.OutputPathName.Split('.').Length == 1)
                        ouputPath += ".exe";
                    break;
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    if (AltUnityTesterEditor.EditorConfiguration.OutputPathName.Split('/').Length == 1)
                        ouputPath += "/" + ouputPath;
                    if (AltUnityTesterEditor.EditorConfiguration.OutputPathName.Split('.').Length == 1)
                        ouputPath += ".exe";
                    break;
                case UnityEditor.BuildTarget.StandaloneLinux64:
                    if (AltUnityTesterEditor.EditorConfiguration.OutputPathName.Split('/').Length == 1)
                        ouputPath += "/" + ouputPath;
                    if (AltUnityTesterEditor.EditorConfiguration.OutputPathName.Split('.').Length == 1)
                        ouputPath += ".x86_64";
                    break;
            }
            buildPlayerOptions.locationPathName = ouputPath;


            buildPlayerOptions.scenes = GetScenesForBuild();

            buildPlayerOptions.target = buildTarget;
            BuildGame(autoRun, buildPlayerOptions);
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError(e);
        }
        finally
        {
            built = true;
            ResetBuildSetup(UnityEditor.BuildTargetGroup.Standalone);
        }

    }

    private static void BuildGame(bool autoRun, UnityEditor.BuildPlayerOptions buildPlayerOptions)
    {
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
        if(!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources/AltUnityTester"))
        {
            UnityEditor.AssetDatabase.CreateFolder("Assets","Resources");
            UnityEditor.AssetDatabase.CreateFolder("Assets/Resources","AltUnityTester");
        }
        System.IO.File.WriteAllText(filePath, dataAsJson);
        

    }

    private static string[] GetScenesForBuild()
    {
        if (AltUnityTesterEditor.EditorConfiguration.Scenes.Count == 0)
        {
            AltUnityTesterEditor.AddAllScenes();
            AltUnityTesterEditor.SelectAllScenes();
        }
        System.Collections.Generic.List<string> sceneList = new System.Collections.Generic.List<string>();
        foreach (var scene in AltUnityTesterEditor.EditorConfiguration.Scenes) {
            if (scene.ToBeBuilt) {
                sceneList.Add(scene.Path);
            }
        }

        InsertAltUnityInTheFirstScene();


        return sceneList.ToArray();
    }

    public static void InsertAltUnityInScene(string scene)
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
        }
        else
        {
            component.ShowInputs = AltUnityTesterEditor.EditorConfiguration.inputVisualizer;
            component.showPopUp = AltUnityTesterEditor.EditorConfiguration.showPopUp;
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


    public static void BuildiOSFromUI(bool autoRun) {
        try {
            UnityEngine.Debug.Log("Starting IOS build..." + UnityEditor.PlayerSettings.productName + " : " + UnityEditor.PlayerSettings.bundleVersion);
            InitBuildSetup(UnityEditor.BuildTargetGroup.iOS);
            UnityEditor.BuildPlayerOptions buildPlayerOptions = new UnityEditor.BuildPlayerOptions();
            buildPlayerOptions.locationPathName = AltUnityTesterEditor.EditorConfiguration.OutputPathName;
            buildPlayerOptions.scenes = GetScenesForBuild();

            buildPlayerOptions.target = UnityEditor.BuildTarget.iOS;
            BuildGame(autoRun, buildPlayerOptions);

        } catch (System.Exception e) {
            UnityEngine.Debug.LogError(e);
        } finally {
            built = true;
            ResetBuildSetup(UnityEditor.BuildTargetGroup.iOS);
        }

    }
#endif


    public enum InputType
    {
        KeyOrMouseButton,
        MouseMovement,
        JoystickAxis,
    };

}
