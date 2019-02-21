using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;


public class AltUnityTesterEditor : EditorWindow
{
   

    private Button _android;
    Object _obj;

    public static bool needsRepaiting = false;

    public static EditorConfiguration EditorConfiguration;
    public static AltUnityTesterEditor _window;

    public static TestSuite _testSuite;

    // public TestRunDelegate CallRunDelegateCommandline = new TestRunDelegate();

    private static Texture2D passIcon;
    private static Texture2D failIcon;
    public static int selectedTest = -1;
    private static Color defaultColor;
    private static Color greenColor = new Color(0.0f, 0.5f, 0.2f, 1f);
    private static Color redColor = new Color(0.7f, 0.15f, 0.15f, 1f);
    private static Color selectedTestColor = new Color(1f, 1f, 1f, 1f);



    Vector2 _scrollPosition;
    private Vector2 _scrollPositonTestResult;


    private bool _foldOutScenes = true;
    private bool _foldOutBuildSettings = true;
    private bool _foldOutIosSettings = true;
    private bool _foldOutAltUnityServerSettings = true;

    //TestResult after running a test
    public static bool isTestRunResultAvailable = false;
    public static int reportTestPassed;
    public static int reportTestFailed;
    public static double timeTestRan;

    public static List<MyDevices> devices=new List<MyDevices>();
    public static Dictionary<string,int> iosForwards=new Dictionary<string, int>();

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/AltUnityTester")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        _window = (AltUnityTesterEditor)GetWindow(typeof(AltUnityTesterEditor));
        _window.Show();

    }


    private void OnFocus()
    {
    

        if (EditorConfiguration == null)
        {
            InitEditorConfiguration();
        }

        if (failIcon == null)
        {
            var findIcon = AssetDatabase.FindAssets("16px-indicator-fail");
            failIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(findIcon[0]));
        }
        if (passIcon == null)
        {
            var findIcon = AssetDatabase.FindAssets("16px-indicator-pass");
            passIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(findIcon[0]));

        }


        GetListOfSceneFromEditor();
        AltUnityTestRunner.SetUpListTest();


    }

    private void GetListOfSceneFromEditor()
    {
        List<MyScenes> newSceneses =new List<MyScenes>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            newSceneses.Add(new MyScenes(scene.enabled,scene.path,0));
        }

        EditorConfiguration.Scenes = newSceneses;
    }


    public static void InitEditorConfiguration()
    {
        if (AssetDatabase.FindAssets("AltUnityTesterEditorSettings").Length == 0)
        {
            var altUnityEditorFolderPath=AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("AltUnityTesterEditor")[0]);
            altUnityEditorFolderPath=altUnityEditorFolderPath.Substring(0,altUnityEditorFolderPath.Length-24);
            Debug.Log(altUnityEditorFolderPath);
            EditorConfiguration = ScriptableObject.CreateInstance<EditorConfiguration>();
            AssetDatabase.CreateAsset(EditorConfiguration, altUnityEditorFolderPath+"/AltUnityTesterEditorSettings.asset");
            AssetDatabase.SaveAssets();

        }
        else
        {
            EditorConfiguration = AssetDatabase.LoadAssetAtPath<EditorConfiguration>(
                AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("AltUnityTesterEditorSettings")[0]));
        }
        EditorUtility.SetDirty(EditorConfiguration);

    }


    void OnInspectorUpdate()
    {
        Repaint();
    }

    private void OnGUI()
    {

        if (needsRepaiting)
        {
            needsRepaiting = false;
            Repaint();
        }

        if (isTestRunResultAvailable)
        {
            isTestRunResultAvailable = !EditorUtility.DisplayDialog("Test Report",
                  " Total tests:" + (reportTestFailed + reportTestPassed) + Environment.NewLine + " Tests passed:" +
                  reportTestPassed + Environment.NewLine + " Tests failed:" + reportTestFailed + Environment.NewLine +
                  " Duration:" + timeTestRan + " seconds", "Ok");
            reportTestFailed = 0;
            reportTestPassed = 0;
            timeTestRan = 0;
        }
        if (Application.isPlaying && !EditorConfiguration.ranInEditor)
        {
            EditorConfiguration.ranInEditor = true;
        }

        if (!Application.isPlaying && EditorConfiguration.ranInEditor)
        {
            AfterExitPlayMode();

        }

        DrawGUI();

    }

    private void DrawGUI()
    {
        var screenWidth = EditorGUIUtility.currentViewWidth;
        //----------------------Left Panel------------
        EditorGUILayout.BeginHorizontal();
        var leftSide = (screenWidth / 3) * 2;
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, true, true, GUILayout.Width(leftSide));

        DisplayTestGui(EditorConfiguration.MyTests);

        EditorGUILayout.Separator();

        DisplayBuildSettings();

        EditorGUILayout.Separator();

        DisplayAltUnityServerSettings();

        EditorGUILayout.Separator();

        DisplayPortForwarding();


        EditorGUILayout.EndScrollView();

        //-------------------Right Panel--------------
        var rightSide = (screenWidth / 3);
        EditorGUILayout.BeginVertical(GUILayout.Width(rightSide));

        EditorGUILayout.LabelField("Platform", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorConfiguration.platform = (Platform)GUILayout.SelectionGrid((int)EditorConfiguration.platform, Enum.GetNames(typeof(Platform)), Enum.GetNames(typeof(Platform)).Length, EditorStyles.radioButton);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Tests", EditorStyles.boldLabel);

        if (GUILayout.Button("Run All Tests"))
        {
            if (EditorConfiguration.platform == Platform.Editor)
            {
                Thread testThread = new Thread(() => AltUnityTestRunner.RunTests(AltUnityTestRunner.TestRunMode.RunAllTest));
                testThread.Start();
            }
            else
            {

                AltUnityTestRunner.RunTests(AltUnityTestRunner.TestRunMode.RunAllTest);
            }
        }
        if (GUILayout.Button("Run Selected Tests"))
        {
            if (EditorConfiguration.platform == Platform.Editor)
            {
                Thread testThread = new Thread(() => AltUnityTestRunner.RunTests(AltUnityTestRunner.TestRunMode.RunSelectedTest));
                testThread.Start();
            }
            else
            {

                AltUnityTestRunner.RunTests(AltUnityTestRunner.TestRunMode.RunSelectedTest);
            }
        }
        if (GUILayout.Button("Run Failed Tests"))
        {
            if (EditorConfiguration.platform == Platform.Editor)
            {
                Thread testThread = new Thread(() => AltUnityTestRunner.RunTests(AltUnityTestRunner.TestRunMode.RunFailedTest));
                testThread.Start();
            }
            else
            {

                AltUnityTestRunner.RunTests(AltUnityTestRunner.TestRunMode.RunFailedTest);
            }
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        if (AltUnityBuilder.built)
        {
            var found = false;

            Scene scene = EditorSceneManager.OpenScene(AltUnityBuilder.GetFirstSceneWhichWillBeBuilt());
            if (scene.path.Equals(AltUnityBuilder.GetFirstSceneWhichWillBeBuilt()))
            {
                if (scene.GetRootGameObjects()
                    .Any(gameObject => gameObject.name.Equals("AltUnityRunnerPrefab")))
                {
                    SceneManager.SetActiveScene(scene);
                    var altunityRunner = scene.GetRootGameObjects()
                        .First(a => a.name.Equals("AltUnityRunnerPrefab"));
                    DestroyAltUnityRunner(altunityRunner);
                    found = true;
                }

                if (found == false)
                    AltUnityBuilder.built = false;
            }

        }

        EditorGUILayout.LabelField("Build", EditorStyles.boldLabel);
        if (EditorConfiguration.platform != Platform.Editor)
        {
            if (GUILayout.Button("Build Only"))
            {
                if (EditorConfiguration.platform == Platform.Android)
                {
                    AltUnityBuilder.BuildAndroidFromUI(autoRun: false);
                }
#if UNITY_EDITOR_OSX
                else if (EditorConfiguration.platform == Platform.iOS) {
                    AltUnityBuilder.BuildiOSFromUI(autoRun: false);
                }
#endif
                else
                {
                    RunInEditor();
                }
            }
        }
        else
        {
            EditorGUI.BeginDisabledGroup(true);
            GUILayout.Button("Build Only");
            EditorGUI.EndDisabledGroup();
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Run", EditorStyles.boldLabel);
        if (GUILayout.Button("Play in Editor"))
        {
            EditorConfiguration.platform = Platform.Editor;
            RunInEditor();
        }

        if (EditorConfiguration.platform != Platform.Editor)
        {
            if (GUILayout.Button("Build & Run on Device"))
            {
                if (EditorConfiguration.platform == Platform.Android)
                {
                    AltUnityBuilder.BuildAndroidFromUI(autoRun: true);
                }
#if UNITY_EDITOR_OSX
                else if (EditorConfiguration.platform == Platform.iOS) {
                    AltUnityBuilder.BuildiOSFromUI(autoRun: true);
                }
#endif
            }
        }
        else
        {
            EditorGUI.BeginDisabledGroup(true);
            GUILayout.Button("Build & Run on Device");
            EditorGUI.EndDisabledGroup();
        }

        EditorGUILayout.LabelField("", GUILayout.ExpandHeight(true));
        //Status test

        _scrollPositonTestResult = EditorGUILayout.BeginScrollView(_scrollPositonTestResult, GUI.skin.textArea);
        if (selectedTest != -1)
        {
            EditorGUILayout.LabelField("Test Result for:  " + EditorConfiguration.MyTests[selectedTest].TestName, EditorStyles.boldLabel);
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Message:");
            if (EditorConfiguration.MyTests[selectedTest].TestResultMessage == null)
                GUILayout.TextArea("No informartion about this test available.\nPlease rerun the test.",
                    GUILayout.MaxHeight(75));
            else
            {
                string text = EditorConfiguration.MyTests[selectedTest].TestResultMessage;
                int lineContor = 1;
                int textLength = (int)rightSide / 7;
                if (text.Length > textLength)
                {
                    var splited = text.Split(' ');
                    text = "";
                    foreach (var word in splited)
                    {
                        text = text + " " + word;
                        if (text.Length > textLength * lineContor)
                        {
                            lineContor++;
                            text = text + "\n";
                        }
                    }
                }

                EditorGUILayout.TextArea(text);
            }
        }
        else
        {
            EditorGUILayout.LabelField("No test selected");
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void DisplayPortForwarding()
    {
        _foldOutScenes = EditorGUILayout.Foldout(_foldOutScenes, "PortForwading");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
        EditorGUILayout.BeginVertical();
        if (_foldOutScenes)
        {
            GUILayout.BeginVertical(GUI.skin.textField);
            GUILayout.BeginHorizontal();
            GUILayout.Label("DeviceId", EditorStyles.boldLabel, GUILayout.MinWidth(50));
            GUILayout.Label("Local Port", EditorStyles.boldLabel, GUILayout.MinWidth(50), GUILayout.MaxWidth(100));
            GUILayout.Label("Remote Port", EditorStyles.boldLabel, GUILayout.MinWidth(50), GUILayout.MaxWidth(100));
            if (GUILayout.Button("Refresh", GUILayout.MinWidth(50), GUILayout.MaxWidth(100)))
            {
                RefreshDeviceList();
            }
            GUILayout.EndHorizontal();

            if (devices.Count != 0)
            {
                foreach (var device in devices)
                {
                    if (device.Active)
                    {
                        var styleActive = new GUIStyle(GUI.skin.textField);
                        styleActive.normal.background = MakeTexture(20, 20, greenColor);
                        
                        GUILayout.BeginHorizontal(styleActive);
                        GUILayout.Label(device.DeviceId, GUILayout.MinWidth(50));
                        GUILayout.Label(device.LocalPort.ToString(), GUILayout.MinWidth(50), GUILayout.MaxWidth(100));
                        GUILayout.Label(device.RemotePort.ToString(), GUILayout.MinWidth(50), GUILayout.MaxWidth(100));
                        if(GUILayout.Button("Stop", GUILayout.MinWidth(50), GUILayout.MaxWidth(100)))
                        {
                            if (device.Platform == Platform.Android)
                            {
                                AltUnityPortHandler.RemoveForwardAndroid(device.LocalPort);
                            }
#if UNITY_EDITOR_OSX
                            else
                            {
                                int id;
                                if(iosForwards.TryGetValue(device.DeviceId,out id)){
                                    AltUnityPortHandler.KillIProxy(id);
                                    iosForwards.Remove(device.DeviceId);
                                }

                            }
#endif

                            device.Active = false;
                            RefreshDeviceList();

                        }
                    }
                    else
                    {
                        var style = GUI.skin.textField;
                        GUILayout.BeginHorizontal(style);
                        GUILayout.Label(device.DeviceId, GUILayout.MinWidth(50));
                        device.LocalPort=EditorGUILayout.IntField(device.LocalPort, GUILayout.MinWidth(50), GUILayout.MaxWidth(100));
                        device.RemotePort=EditorGUILayout.IntField(device.RemotePort, GUILayout.MinWidth(50), GUILayout.MaxWidth(100));
                        if(GUILayout.Button("Start", GUILayout.MinWidth(50), GUILayout.MaxWidth(100)))
                        {
                            if (device.Platform == Platform.Android)
                            {
                                var response=AltUnityPortHandler.ForwardAndroid(device.DeviceId,device.LocalPort,device.RemotePort);
                                if(!response.Equals("Ok")){
                                    Debug.LogError(response);
                                }
                            }
#if UNITY_EDITOR_OSX
                            else
                            {
                                var response=AltUnityPortHandler.ForwardIos(device.DeviceId,device.LocalPort,device.RemotePort);
                                if(response.StartsWith("Ok")){
                                    var processID=int.Parse(response.Split(' ')[1]);
                                    iosForwards.Add(device.DeviceId,processID);
                                    device.Active=true;
                                }else{
                                    Debug.LogError(response);
                                }
                                
                            }

#endif
                            RefreshDeviceList();
                        }

                    }                  

                    GUILayout.EndHorizontal();

                }

                
            }
            else
            {
                EditorGUILayout.TextArea("No devices connected. Click \"refresh\" button to search for devices");
            }
            GUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void RefreshDeviceList()
    {
        List<MyDevices> adbDevices = AltUnityPortHandler.GetDevicesAndroid();
        List<MyDevices> androidForwardedDevices = AltUnityPortHandler.GetForwardedDevicesAndroid();
        foreach(var adbDevice in adbDevices)
        {
            var deviceForwarded = androidForwardedDevices.FirstOrDefault(device => device.DeviceId.Equals(adbDevice.DeviceId));
            if (deviceForwarded != null)
            {
                adbDevice.LocalPort = deviceForwarded.LocalPort;
                adbDevice.RemotePort = deviceForwarded.RemotePort;
                adbDevice.Active = deviceForwarded.Active;
            }
        }
        foreach(var device in devices)
        {
            var existingDevice = adbDevices.FirstOrDefault(d => d.DeviceId.Equals(device.DeviceId));
            if (existingDevice != null && device.Active==false && existingDevice.Active==false)
            {
                existingDevice.LocalPort = device.LocalPort;
                existingDevice.RemotePort = device.RemotePort;
            }
        }
 #if UNITY_EDITOR_OSX
        List<MyDevices> iOSDEvices=AltUnityPortHandler.GetConnectediOSDevices();
        foreach(var iOSDEvice in iOSDEvices){
            var iOSForwardedDevice=devices.FirstOrDefault(a=>a.DeviceId.Equals(iOSDEvice.DeviceId));
            if(iOSForwardedDevice!=null){
                iOSDEvice.LocalPort=iOSForwardedDevice.LocalPort;
                iOSDEvice.RemotePort=iOSForwardedDevice.RemotePort;
                iOSDEvice.Active=iOSForwardedDevice.Active;
            }
        }
#endif
        

        devices = adbDevices;
#if UNITY_EDITOR_OSX
        devices.AddRange(iOSDEvices);
#endif
    }

    private void DisplayAltUnityServerSettings()
    {
        _foldOutAltUnityServerSettings = EditorGUILayout.Foldout(_foldOutAltUnityServerSettings, "AltUnityServer Settings");
        if (_foldOutAltUnityServerSettings)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
            EditorConfiguration.requestSeparator = EditorGUILayout.TextField("Request separator", EditorConfiguration.requestSeparator);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
            EditorConfiguration.requestEnding = EditorGUILayout.TextField("Request ending", EditorConfiguration.requestEnding);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
            EditorConfiguration.serverPort = EditorGUILayout.IntField("Server port", EditorConfiguration.serverPort);
            EditorGUILayout.EndHorizontal();
        }
    }

    private void AfterExitPlayMode() {
        RemoveAltUnityRunnerPrefab();
        AltUnityBuilder.RemoveAltUnityTesterFromScriptingDefineSymbols(EditorUserBuildSettings.selectedBuildTargetGroup);
        EditorConfiguration.ranInEditor = false;
    }

    private static void RemoveAltUnityRunnerPrefab() {
        var activeScene = EditorSceneManager.GetActiveScene();
        var altUnityRunners = activeScene.GetRootGameObjects()
            .Where(gameObject => gameObject.name.Equals("AltUnityRunnerPrefab"));
        foreach (var altUnityRunner in altUnityRunners) {
            DestroyImmediate(altUnityRunner);

        }
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
    }


    private void RunInEditor()
    {
        AltUnityBuilder.InsertAltUnityInTheFirstScene();
        AltUnityBuilder.AddAltUnityTesterInScritpingDefineSymbolsGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

        EditorApplication.isPlaying = true;

    }

    private void DisplayBuildSettings()
    {
        _foldOutBuildSettings = EditorGUILayout.Foldout(_foldOutBuildSettings, "Build Settings");
        if (_foldOutBuildSettings)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
            EditorConfiguration.OutputPathName = EditorGUILayout.TextField("Output path", EditorConfiguration.OutputPathName);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
            var androidBundleIdentifier = EditorGUILayout.TextField("Android Bundle Identifier",
                PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android));
            if (androidBundleIdentifier != PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android))
            {
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, androidBundleIdentifier);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
            var iOSBundleIdentifier = EditorGUILayout.TextField("iOS Bundle Identifier",
                PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS));
            if (iOSBundleIdentifier != PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS))
            {
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, iOSBundleIdentifier);
            }
            //            BundleIdentifier= EditorGUILayout.TextField("Android Bundle Identifier", BundleIdentifier);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
            PlayerSettings.companyName = EditorGUILayout.TextField("Company Name", PlayerSettings.companyName);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
            PlayerSettings.productName = EditorGUILayout.TextField("Product Name", PlayerSettings.productName);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
            EditorGUILayout.LabelField("Append \"Test\" to product name for AltUnityTester builds");
           
            EditorConfiguration.appendToName =
                EditorGUILayout.Toggle(EditorConfiguration.appendToName);
            EditorGUILayout.EndHorizontal();
            
#if UNITY_EDITOR_OSX
            _foldOutIosSettings = EditorGUILayout.Foldout(_foldOutIosSettings, "IOS Settings");
            if (_foldOutIosSettings)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
                PlayerSettings.iOS.appleDeveloperTeamID = EditorGUILayout.TextField("Signing Team Id: ", PlayerSettings.iOS.appleDeveloperTeamID);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
                PlayerSettings.iOS.appleEnableAutomaticSigning = EditorGUILayout.Toggle("Automatically Sign: ", PlayerSettings.iOS.appleEnableAutomaticSigning );
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
                EditorConfiguration.AdbPath = EditorGUILayout.TextField("Adb Path: ", EditorConfiguration.AdbPath);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
                EditorConfiguration.IProxyPath = EditorGUILayout.TextField("Iproxy Path: ", EditorConfiguration.IProxyPath);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
                EditorConfiguration.XcrunPath = EditorGUILayout.TextField("Xcrun Path: ", EditorConfiguration.XcrunPath);
                EditorGUILayout.EndHorizontal();
            }
#endif


            DisplayScenes();
        }
    }

    private void DisplayScenes()
    {
        _foldOutScenes = EditorGUILayout.Foldout(_foldOutScenes, "SceneManager");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
        EditorGUILayout.BeginVertical();
        if (_foldOutScenes)
        {
            if (EditorConfiguration.Scenes.Count != 0)
            {
                GUILayout.BeginVertical(GUI.skin.textField);
                MyScenes sceneToBeRemoved = null;
                int counter = 0;
                foreach (var scene in EditorConfiguration.Scenes)
                {
                    GUILayout.BeginHorizontal(GUI.skin.textArea);
                    var valToggle = EditorGUILayout.Toggle(scene.ToBeBuilt, GUILayout.MaxWidth(10));
                    if (valToggle != scene.ToBeBuilt)
                    {
                        scene.ToBeBuilt = valToggle;
                        EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();
                    }
                    EditorGUILayout.LabelField(scene.Path);
                    string value;
                    if (scene.ToBeBuilt)
                    {
                        scene.BuildScene = counter;
                        counter++;
                        value = scene.BuildScene.ToString();
                    }
                    else
                    {
                        value = "";
                    }

                    EditorGUILayout.LabelField(value, GUILayout.MaxWidth(30));


                    if (EditorConfiguration.Scenes.IndexOf(scene) != 0 && EditorConfiguration.Scenes.Count > 1)
                    {

                        if (GUILayout.Button("^", GUILayout.MaxWidth(30)))
                        {
                            SceneMove(scene, true);
                            EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();
                        }
                    }

                    if (EditorConfiguration.Scenes.IndexOf(scene) != EditorConfiguration.Scenes.Count - 1 && EditorConfiguration.Scenes.Count > 1)
                        if (GUILayout.Button("v", GUILayout.MaxWidth(30)))
                        {
                            SceneMove(scene, false);
                            EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();
                        }


                    if (GUILayout.Button("X", GUILayout.MaxWidth(30)))
                    {
                        sceneToBeRemoved = scene;
                    }

                    GUILayout.EndHorizontal();

                }


                if (sceneToBeRemoved != null)
                {
                    RemoveScene(sceneToBeRemoved);
                }

                GUILayout.EndVertical();
            }

            GUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Add scene: ", GUILayout.MaxWidth(80));
            _obj = EditorGUILayout.ObjectField(_obj, typeof(SceneAsset), true);

            if (_obj != null)
            {
                var path = AssetDatabase.GetAssetPath(_obj);
                if (EditorConfiguration.Scenes.All(n => n.Path != path))
                {
                    EditorConfiguration.Scenes.Add(new MyScenes(false, path, 0));
                    _obj = new Object();
                }

                _obj = null;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add all scenes", EditorStyles.miniButtonLeft))
            {
                AddAllScenes();
            }

            if (GUILayout.Button("Select all scenes", EditorStyles.miniButtonMid))
            {
                SelectAllScenes();
            }
            if (GUILayout.Button("Deselect all scenes", EditorStyles.miniButtonMid))
            {
                DeselectAllScenes();
            }
            if (GUILayout.Button("Remove not selected scenes", EditorStyles.miniButtonMid))
            {
                RemoveNotSelectedScenes();
            }
            if (GUILayout.Button("Remove all scenes", EditorStyles.miniButtonRight))
            {
                EditorConfiguration.Scenes = new List<MyScenes>();
                EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

    }

    private void RemoveNotSelectedScenes()
    {
        List<MyScenes> copyMySceneses = new List<MyScenes>();
        foreach (var scene in EditorConfiguration.Scenes)
        {
            if (scene.ToBeBuilt)
            {
                copyMySceneses.Add(scene);
            }
        }

        EditorConfiguration.Scenes = copyMySceneses;
        EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();
    }

    private void DeselectAllScenes()
    {
        foreach (var scene in EditorConfiguration.Scenes)
        {
            scene.ToBeBuilt = false;
        }
        EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();

    }

    public static void SelectAllScenes()
    {
        foreach (var scene in EditorConfiguration.Scenes)
        {
            scene.ToBeBuilt = true;
        }
        EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();


    }


    private void DisplayTestGui(List<MyTest> tests)
    {
        EditorGUILayout.LabelField("Test", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical(GUI.skin.textArea);

        int foldOutCounter = 0;

        foreach (var test in tests)
        {
            if (foldOutCounter > 0)
            {
                foldOutCounter--;
                continue;
            }

            if (tests.IndexOf(test) == selectedTest)
            {
                GUIStyle gsAlterQuest = new GUIStyle();
                gsAlterQuest.normal.background = MakeTexture(20,20, selectedTestColor);
                EditorGUILayout.BeginHorizontal(gsAlterQuest);

            }
            else
            {
                EditorGUILayout.BeginHorizontal();
            }

            if (test.Type == typeof(TestFixture))
            {
                EditorGUILayout.LabelField("    ", GUILayout.Width(30));
            }
            else if (test.Type == typeof(TestMethod))
            {
                EditorGUILayout.LabelField("    ", GUILayout.Width(60));
            }

            var valueChanged = EditorGUILayout.Toggle(test.Selected, GUILayout.Width(10));
            if (valueChanged != test.Selected)
            {
                test.Selected = valueChanged;
                ChangeSelectionChildsAndParent(test);
            }

            if (test.Status == 0)
            {
                var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft };
                EditorGUILayout.LabelField(test.TestName, style);
            }
            else
            {
                Color color = redColor;
                Texture2D icon = failIcon;
                if (test.Status == 1)
                {
                    color = greenColor;
                    icon = passIcon;
                }
                GUILayout.Label(icon, GUILayout.Width(30));
                GUIStyle guiStyle = new GUIStyle {normal = {textColor = color}};

                EditorGUILayout.LabelField(test.TestName, guiStyle);
            }

            if (test.Type != typeof(TestMethod))
            {
                test.FoldOut = EditorGUILayout.Foldout(test.FoldOut,"");
                if (!test.FoldOut)
                {
                    if (test.Type == typeof(TestAssembly))
                    {
                        foldOutCounter = tests.Count - 1;
                    }
                    else
                    {
                        foldOutCounter = test.TestCaseCount;
                    }
                }
            }

            if (!test.IsSuite) { 
            if (GUILayout.Button("Info", GUILayout.Width(50)))
            {
                selectedTest = tests.IndexOf(test);
            }
            }
            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.EndVertical();
    }
    
    private void ChangeSelectionChildsAndParent(MyTest test)
    {
        if (test.Selected)
        {
            if (test.Type == typeof(TestAssembly))
            {
                foreach (var test2 in EditorConfiguration.MyTests)
                {
                    test2.Selected = true;
                }
            }
            else
            {
                if (test.IsSuite)
                {
                    var index = EditorConfiguration.MyTests.IndexOf(test);
                    for (int i = index + 1; i <= index + test.TestCaseCount; i++)
                    {
                        EditorConfiguration.MyTests[i].Selected = true;
                    }
                }
            }
        }
        else
        {
            if (test.Type == typeof(TestAssembly))
            {
                foreach (var test2 in EditorConfiguration.MyTests)
                {
                    test2.Selected = false;
                }
            }
            else
            {
                var dummy = test;
                if (test.Type == typeof(TestFixture))
                {
                    var index = EditorConfiguration.MyTests.IndexOf(test);
                    for (int i = index + 1; i <= index + test.TestCaseCount; i++)
                    {
                        EditorConfiguration.MyTests[i].Selected = false;
                    }
                }
                while (dummy.ParentName != null)
                {
                    dummy = EditorConfiguration.MyTests.FirstOrDefault(a => a.TestName.Equals(dummy.ParentName));
                    if (dummy != null)
                        dummy.Selected = false;
                    else
                        return;
                }
            }
        }

    }

    private static void SceneMove(MyScenes scene, bool up)
    {
        int index = EditorConfiguration.Scenes.IndexOf(scene);
        if (up)
        {
            Swap(index, index - 1);
        }
        else
        {
            Swap(index, index + 1);
        }
    }


    public static void Swap(int index1, int index2)
    {
        MyScenes backUp = EditorConfiguration.Scenes[index1];
        EditorConfiguration.Scenes[index1] = EditorConfiguration.Scenes[index2];
        EditorConfiguration.Scenes[index2] = backUp;
    }


    public static void AddAllScenes()
    {
        var scenesToBeAddedGuid = AssetDatabase.FindAssets("t:SceneAsset");
        EditorConfiguration.Scenes = new List<MyScenes>();
        foreach (var sceneGuid in scenesToBeAddedGuid)
        {
            var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
            EditorConfiguration.Scenes.Add(new MyScenes(false, scenePath, 0));

        }

        EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();

    }

    private static EditorBuildSettingsScene[] PathFromTheSceneInCurrentList()
    {
        List<EditorBuildSettingsScene> listofPath = new List<EditorBuildSettingsScene>();
        foreach (var scene in EditorConfiguration.Scenes)
        {
            listofPath.Add(new EditorBuildSettingsScene(scene.Path, scene.ToBeBuilt));
        }

        return listofPath.ToArray();
    }

    private void RemoveScene(MyScenes scene)
    {

        EditorConfiguration.Scenes.Remove(scene);
        EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();

    }

    private Texture2D MakeTexture(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }



    [MenuItem("Assets/Create/AltUnityTest", false, 80)]
    public static void CreateAltUnityTest()
    {

        var templatePath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("DefaultTestExample")[0]);

        string folderPath = GetPathForSelectedItem();
        MethodInfo method = typeof(ProjectWindowUtil).GetMethod("CreateScriptAsset", BindingFlags.Static | BindingFlags.NonPublic);
        if (method == null)
            return;
        string newFilePath = Path.Combine(folderPath, "NewAltUnityTest.cs");
        method.Invoke((object)null, new object[2]
        {
            (object) templatePath,
            (object) newFilePath
        });

    }

    [MenuItem("Assets/Create/AltUnityTest", true, 80)]
    public static bool CanCreateAltUnityTest()
    {
        return (GetPathForSelectedItem() + "/").Contains("/Editor/");
    }

    [MenuItem("Window/CreateAltUnityTesterPackage")]
    public static void CreateAltUnityTesterPackage() {
        Debug.Log("AltUnityTester - Unity Package creation started...");
        string packageName="AltUnityTester.unitypackage";
        string assetPathNames = "Assets/AltUnityTester";
        AssetDatabase.ExportPackage(assetPathNames, packageName, ExportPackageOptions.Recurse);
        Debug.Log("AltUnityTester - Unity Package done.");
    }

    private static string GetPathForSelectedItem()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (Path.GetExtension(path) != "") //checks if current item is a folder or a file 
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }
        return path;
    }

    private static void DestroyAltUnityRunner(Object altUnityRunner)
    {

        DestroyImmediate(altUnityRunner);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(AltUnityBuilder.PreviousScenePath);
    }

}
