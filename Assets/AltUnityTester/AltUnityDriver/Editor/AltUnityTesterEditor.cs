using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Filters;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Button = UnityEngine.UI.Button;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public delegate void TestRunDelegate(string name);


public class AltUnityTesterEditor : EditorWindow
{
   

    private Button _android;
    Object _obj;


    private static EditorConfiguration _editorConfiguration;
    public static AltUnityTesterEditor _window;

    public static TestSuite _testSuite;

    public TestRunDelegate CallRunDelegate = new TestRunDelegate(ShowProgresBar);
    // public TestRunDelegate CallRunDelegateCommandline = new TestRunDelegate();

    private static Texture2D passIcon;
    private static Texture2D failIcon;
    private static int selectedTest = -1;
    private static Color defaultColor;
    private static Color greenColor = new Color(0.0f, 0.5f, 0.2f, 1f);
    private static Color redColor = new Color(0.7f, 0.15f, 0.15f, 1f);
    private static Color selectedTestColor = new Color(1f, 1f, 1f, 1f);
    private static int idIproxyProcess;
    private static bool iProxyOn = false;

    public static string PreviousScenePath;
    public static Scene SceneWithAltUnityRunner;
    public static string SceneWithAltUnityRunnerPath;
    public static Object AltUnityRunner;
    public static bool built = false;
    public static bool runnedInEditor = false;
    public static Scene copyScene;

    private static Thread thread;

    //This are for progressBar when are runned
    private static float progress;
    private static float total;
    private static string _testName;

    Vector2 _scrollPosition;
    private Vector2 _scrollPositonTestResult;

    //private static List<MyDevices> _myDeviceses=new List<MyDevices>();

    private bool _foldOutScenes = true;
    private bool _foldOutBuildSettings = true;
    private bool _foldOutIosSettings = true;

    //TestResult after running a test
    private static bool isTestRunResultAvailable = false;
    private static int reportTestPassed;
    private static int reportTestFailed;
    private static double timeTestRunned;

   

    private enum TestRunMode { RunAllTest, RunSelectedTest, RunFailedTest }

    void RunTests(TestRunMode testMode)
    {
        Debug.Log("Started running test");
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly assembly = assemblies.FirstOrDefault(assemblyName => assemblyName.GetName().Name.Equals("Assembly-CSharp-Editor"));

        var filters = AddTestToBeRun(testMode);
        if (_editorConfiguration.platform != Platform.Editor)
        {
#if UNITY_EDITOR_OSX
        RemoveForwardAndroid();
        if (_editorConfiguration.platform==Platform.iOS)
        {
            thread = new Thread(ThreadForwardIos);
            thread.Start();
            while (!iProxyOn)
            {
                Thread.Sleep(250);
            }
        }
        else
#endif
#if UNITY_EDITOR_WIN
            RemoveForwardAndroid();
#endif
            ForwardAndroid();
        }

        ITestListener listener = new TestRunListener(CallRunDelegate);
        var testAssemblyRunner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());

        testAssemblyRunner.Load(assembly, new Dictionary<string, object>());
        progress = 0;
        total = filters.Filters.Count;
        Thread runTestThread = new Thread(() =>
        {
            var result = testAssemblyRunner.Run(listener, filters);
            if (_editorConfiguration.platform != Platform.Editor)
            {
#if UNITY_EDITOR_OSX
            if (_editorConfiguration.platform==Platform.iOS)
            {
                KillIProxy(idIproxyProcess);
                thread.Join();
            }
            else
#endif
                RemoveForwardAndroid();
            }

            SetTestStatus(result);
            isTestRunResultAvailable = true;
            selectedTest = -1;
        });

        runTestThread.Start();
        if (_editorConfiguration.platform != Platform.Editor)
        {
            float previousProgres = progress - 1;
            while (runTestThread.IsAlive)
            {
                if (previousProgres == progress) continue;
                EditorUtility.DisplayProgressBar(progress == total ? "This may take a few seconds" : _testName,
                    progress + "/" + total, progress / total);
                previousProgres = progress;
            }
        }

        runTestThread.Join();
        if (_editorConfiguration.platform != Platform.Editor)
        {
            Repaint();
            EditorUtility.ClearProgressBar();
        }
    }

    private static void ShowProgresBar(string name)
    {
        progress++;
        _testName = name;
    }

    private void SetTestStatus(List<ITestResult> results)
    {
        bool passed = true;
        int numberOfTestPassed = 0;
        int numberOfTestFailed = 0;
        double totalTime = 0;
        foreach (var test in _editorConfiguration.MyTests)
        {
            int counter = 0;
            // int testPassed = 0;
            int testPassedCounter = 0;
            int testFailedCounter = 0;
            foreach (var result in results)
            {
                if (test.Type == typeof(TestAssembly))
                {
                    counter++;
                    var enumerator = result.Children.GetEnumerator();
                    enumerator.MoveNext();
                    if (enumerator.Current != null)
                    {
                        var enumerator2 = enumerator.Current.Children.GetEnumerator();
                        enumerator2.MoveNext();
                        if (enumerator2.Current != null && enumerator2.Current.FailCount > 0)
                        {

                            testFailedCounter++;
                        }
                        else if (enumerator2.Current != null && enumerator2.Current.PassCount > 0)
                        {
                            testPassedCounter++;
                        }

                        enumerator2.Dispose();
                    }

                    enumerator.Dispose();

                }

                if (test.Type == typeof(TestFixture))
                {
                    var enumerator = result.Children.GetEnumerator();
                    enumerator.MoveNext();
                    if (enumerator.Current != null && enumerator.Current.FullName.Equals(test.TestName))
                    {
                        counter++;
                        var enumerator2 = enumerator.Current.Children.GetEnumerator();
                        enumerator2.MoveNext();
                        if (enumerator2.Current != null && enumerator2.Current.FailCount > 0)
                        {
                            testFailedCounter++;

                        }
                        else if (enumerator2.Current != null && enumerator2.Current.PassCount > 0)
                        {
                            testPassedCounter++;

                        }
                        enumerator2.Dispose();
                    }
                    enumerator.Dispose();
                }

                if (test.Type == typeof(TestMethod))
                {
                    var enumerator = result.Children.GetEnumerator();
                    enumerator.MoveNext();
                    if (enumerator.Current != null)
                    {
                        var enumerator2 = enumerator.Current.Children.GetEnumerator();
                        enumerator2.MoveNext();
                        if (enumerator2.Current != null && enumerator2.Current.FullName.Equals(test.TestName))
                        {
                            if (enumerator2.Current.FailCount > 0)
                            {
                                test.Status = -1;
                                test.TestResultMessage = enumerator2.Current.Message + " \n\n\n StackTrace:  " + enumerator2.Current.StackTrace;
                                passed = false;
                                numberOfTestFailed++;

                            }
                            else if (enumerator2.Current.PassCount > 0)
                            {
                                test.Status = 1;
                                test.TestResultMessage = "Passed in " + enumerator2.Current.Duration;
                                numberOfTestPassed++;
                            }

                            totalTime += (enumerator2.Current.EndTime - enumerator2.Current.StartTime).TotalSeconds;
                        }
                        enumerator2.Dispose();
                    }

                    enumerator.Dispose();
                }


            }

            if (test.Type != typeof(TestMethod))
            {
                if (test.TestCaseCount == counter)
                {
                    if (testFailedCounter == 0 && testPassedCounter == counter)
                    {
                        test.Status = 1;
                        test.TestResultMessage = "All method passed ";
                    }
                    else
                    {
                        test.Status = -1;
                        passed = false;
                        test.TestResultMessage = "There are methods that failed";
                    }
                }
            }
        }
        var listOfTests = _editorConfiguration.MyTests;
        var serializeTests = JsonConvert.SerializeObject(listOfTests, Formatting.Indented, new JsonSerializerSettings {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        EditorPrefs.SetString("tests", serializeTests);

        reportTestPassed = numberOfTestPassed;
        reportTestFailed = numberOfTestFailed;
        isTestRunResultAvailable = true;
        selectedTest = -1;
        timeTestRunned = totalTime;
        if (passed)
        {
            Debug.Log("All test passed");
        }
        else
            Debug.Log("Test failed");
    }

    private static OrFilter AddTestToBeRun(TestRunMode testMode)
    {
        OrFilter filter = new OrFilter();
        switch (testMode)
        {
            case TestRunMode.RunAllTest:
                foreach (var test in _editorConfiguration.MyTests)
                    if (!test.IsSuite)
                        filter.Add(new FullNameFilter(test.TestName));
                break;
            case TestRunMode.RunSelectedTest:
                foreach (var test in _editorConfiguration.MyTests)
                    if (test.Selected && !test.IsSuite)
                        filter.Add(new FullNameFilter(test.TestName));
                break;
            case TestRunMode.RunFailedTest:
                foreach (var test in _editorConfiguration.MyTests)
                    if (test.Status == -1 && !test.IsSuite)
                        filter.Add(new FullNameFilter(test.TestName));
                break;
        }

        return filter;
    }

    static int SetTestStatus(ITestResult test)
    {

        if (!test.Test.IsSuite)
        {
            var status = 0;
            string message = "";
            if (test.PassCount == 1)
            {
                status = 1;
                message = "Passed in " + test.Duration;
                reportTestPassed++;

            }
            else if (test.FailCount == 1)
            {
                status = -1;
                message = test.Message;
                reportTestFailed++;
            }

            timeTestRunned += test.Duration;
            int index = _editorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName));
            _editorConfiguration.MyTests[index].Status = status;
            _editorConfiguration.MyTests[index].TestResultMessage = message;

            return status;
        }

        var failCount = 0;
        var notExecutedCount = 0;
        var passCount = 0;
        foreach (var testChild in test.Children)
        {
            var status = SetTestStatus(testChild);
            if (status == 0)
                notExecutedCount++;
            else if (status == -1)
            {
                failCount++;
                
            }
            else
            {
                passCount++;
            }
        }

        if (test.Test.TestCaseCount != passCount + failCount + notExecutedCount)
        {
            _editorConfiguration.MyTests[_editorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName))].Status = 0;
            return 0;
        }

        if (failCount > 0)
        {
            _editorConfiguration.MyTests[_editorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName))].Status = -1;
            return -1;

        }
        _editorConfiguration.MyTests[_editorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName))].Status = 1;
        return 1;
    }

    //[Serializable]
    //internal class MyDevices
    //{
    //    private bool _selected;
    //    private string _deviceName;
    //    private string _uid;

    //    public MyDevices(bool selected, string deviceName, string uid)
    //    {
    //        _selected = selected;
    //        _deviceName = deviceName;
    //        _uid = uid;
    //    }

    //    public bool Selected
    //    {
    //        get { return _selected; }
    //        set { _selected = value; }
    //    }

    //    public string DeviceName
    //    {
    //        get { return _deviceName; }
    //        set { _deviceName = value; }
    //    }

    //    public string Uid
    //    {
    //        get { return _uid; }
    //        set { _uid = value; }
    //    }
    //}


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
        if (_editorConfiguration == null)
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
        SetUpListTest();


    }

    private void GetListOfSceneFromEditor()
    {
        List<MyScenes> newSceneses =new List<MyScenes>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            newSceneses.Add(new MyScenes(scene.enabled,scene.path,0));
        }

        _editorConfiguration.Scenes = newSceneses;
    }


    public static void InitEditorConfiguration()
    {
        if (AssetDatabase.FindAssets("idProject").Length == 0)
        {
            _editorConfiguration = ScriptableObject.CreateInstance<EditorConfiguration>();
            AssetDatabase.CreateAsset(_editorConfiguration, "Assets/AltUnityTester/AltUnityDriver/Editor/idProject.asset");
            AssetDatabase.SaveAssets();

        }
        else
        {
            _editorConfiguration = AssetDatabase.LoadAssetAtPath<EditorConfiguration>(
                AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("idProject")[0]));
        }
        EditorUtility.SetDirty(_editorConfiguration);

    }


    void OnInspectorUpdate()
    {
        Repaint();
    }

    private void OnGUI()
    {
        if (isTestRunResultAvailable)
            isTestRunResultAvailable = !EditorUtility.DisplayDialog("Test raport",
                " Total test:" + (reportTestFailed + reportTestPassed) + Environment.NewLine + " Test passed:" +
                reportTestPassed + Environment.NewLine + " Test failed:" + reportTestFailed + Environment.NewLine +
                " Duration:" + timeTestRunned+" seconds", "Ok");
        if (Application.isPlaying && !runnedInEditor)
        {
            runnedInEditor = true;
        }

        if (!Application.isPlaying && runnedInEditor)
        {
            AfterExitPlayMode();

        }

        var screenWidth = EditorGUIUtility.currentViewWidth;

        EditorGUILayout.BeginHorizontal();
        var leftSide = (screenWidth / 3) * 2;
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, true, true, GUILayout.Width(leftSide));

        DisplayTestGui(_editorConfiguration.MyTests);
        EditorGUILayout.Separator();
        DisplayBuildSettings();


        EditorGUILayout.EndScrollView();
        var rightSide = (screenWidth / 3);
        EditorGUILayout.BeginVertical(GUILayout.Width(rightSide));
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Platform", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        _editorConfiguration.platform =(Platform) GUILayout.SelectionGrid((int)_editorConfiguration.platform,Enum.GetNames(typeof(Platform)) , Enum.GetNames(typeof(Platform)).Length, EditorStyles.radioButton);
   
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Test", EditorStyles.boldLabel);

        if (GUILayout.Button("RunAllTest"))
        {
            if (_editorConfiguration.platform == Platform.Editor)
            {
                Thread testThread = new Thread(() => RunTests(TestRunMode.RunAllTest));
                testThread.Start();
            }
            else
            {

                RunTests(TestRunMode.RunAllTest);
            }
        }
        if (GUILayout.Button("RunSelectedTest"))
        {
            if (_editorConfiguration.platform == Platform.Editor)
            {
                Thread testThread = new Thread(() => RunTests(TestRunMode.RunSelectedTest));
                testThread.Start();
            }
            else
            {

                RunTests(TestRunMode.RunSelectedTest);
            }
        }
        if (GUILayout.Button("RunFailedTest"))
        {
            if (_editorConfiguration.platform == Platform.Editor)
            {
                Thread testThread = new Thread(() => RunTests(TestRunMode.RunFailedTest));
                testThread.Start();
            }
            else
            {

                RunTests(TestRunMode.RunFailedTest);
            }
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        if (built)
        {
            var found = false;

            Scene scene = EditorSceneManager.OpenScene(GetFirstSceneWhichWillBeBuilt());
            if (scene.path.Equals(GetFirstSceneWhichWillBeBuilt()))
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
                    built = false;
            }

        }

        EditorGUILayout.LabelField("Build", EditorStyles.boldLabel);
        if (GUILayout.Button("Build&Run Android"))
        {
            AndroidDefault();

        }
#if UNITY_EDITOR_OSX
        if (GUILayout.Button("Build&Run IOS"))
        {
            IosDefault();

        }
#else
        EditorGUI.BeginDisabledGroup(true);
        GUILayout.Button("Build&Run IOS");
        EditorGUI.EndDisabledGroup();
#endif
        EditorGUILayout.LabelField("Play", EditorStyles.boldLabel);
        if (GUILayout.Button("Run in Editor"))
        {
            RunInEditor();

        }

        EditorGUILayout.LabelField("", GUILayout.ExpandHeight(true));
        //Status test

        _scrollPositonTestResult = EditorGUILayout.BeginScrollView(_scrollPositonTestResult, GUI.skin.textArea);
        if (selectedTest != -1)
        {
            EditorGUILayout.LabelField("Test Result for:  " + _editorConfiguration.MyTests[selectedTest].TestName, EditorStyles.boldLabel);
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Message:");
            if (_editorConfiguration.MyTests[selectedTest].TestResultMessage == null)
                GUILayout.TextArea("No informartion about this test available.\nPlease rerun the test.",
                    GUILayout.MaxHeight(75));
            else
            {
                string text = _editorConfiguration.MyTests[selectedTest].TestResultMessage;
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

    private void AfterExitPlayMode()
    {
        var activeScene = EditorSceneManager.GetActiveScene();
        var altUnityRunner = activeScene.GetRootGameObjects()
            .FirstOrDefault(gameObject => gameObject.name.Equals("AltUnityRunnerPrefab"));
        if (altUnityRunner != null)
        {
            DestroyImmediate(altUnityRunner);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
        }

        RemoveAltUnityTesterFromScriptingDefineSymbols(EditorUserBuildSettings.selectedBuildTargetGroup);

        runnedInEditor = false;
    }

    private static void RemoveAltUnityTesterFromScriptingDefineSymbols(BuildTargetGroup targetGroup)
    {
        try
        {
            string altunitytesterdefine = "ALTUNITYTESTER";
            var scriptingDefineSymbolsForGroup =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
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
                if(newScriptingDefineSymbolsForGroup.Length!=0)
                    newScriptingDefineSymbolsForGroup.Remove(newScriptingDefineSymbolsForGroup.Length - 1);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup,
                    newScriptingDefineSymbolsForGroup);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Some Error Happened +" + e.Message);
            Debug.LogError("Stack trace "+e.StackTrace);
        }
    }

    private void RunInEditor()
    {
        InsertAltUnityInTheFirstScene();
        AddAltUnityTesterInScritpingDefineSymbolsGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

        EditorApplication.isPlaying = true;

    }

    private static void AddAltUnityTesterInScritpingDefineSymbolsGroup(BuildTargetGroup targetGroup)
    {
        var scriptingDefineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        if (!scriptingDefineSymbolsForGroup.Contains("ALTUNITYTESTER")) 
            scriptingDefineSymbolsForGroup += ";ALTUNITYTESTER";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, scriptingDefineSymbolsForGroup);
    }

    //private void TestDEvice()
    //{
    //    Process process = new Process();
    //    ProcessStartInfo startInfo = new ProcessStartInfo
    //    {
    //        WindowStyle = ProcessWindowStyle.Hidden,

    //        UseShellExecute = false,

    //        RedirectStandardOutput = true,

    //        FileName = "adb.exe",

    //        Arguments = "devices -l"
    //    };
    //    process.StartInfo = startInfo;
    //    process.Start();
    //    var str = process.StandardOutput.ReadLine();
    //    while (process.StandardOutput.Peek()!=13)
    //    {
    //        str = process.StandardOutput.ReadLine();
    //        var pieces = str.Split(' ');
    //        var devicename = pieces[6].Split(':');
    //        _myDeviceses.Add(new MyDevices(false,devicename[1],pieces[0]));
    //        Debug.Log(process.StandardOutput.Peek());
    //    }
    //    process.WaitForExit();
    //}

    private void DisplayBuildSettings()
    {
        _foldOutBuildSettings = EditorGUILayout.Foldout(_foldOutBuildSettings, "Build Settings");
        if (_foldOutBuildSettings)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
            _editorConfiguration.OutputPathName = EditorGUILayout.TextField("Output path", _editorConfiguration.OutputPathName);
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
           
            _editorConfiguration.appendToName =
                EditorGUILayout.Toggle(_editorConfiguration.appendToName);
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
                _editorConfiguration.AdbPath = EditorGUILayout.TextField("Adb Path: ", _editorConfiguration.AdbPath);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));
                _editorConfiguration.IProxyPath = EditorGUILayout.TextField("Iproxy Path: ", _editorConfiguration.IProxyPath);
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
            if (_editorConfiguration.Scenes.Count != 0)
            {
                GUILayout.BeginVertical(GUI.skin.textField);
                MyScenes sceneToBeRemoved = null;
                int counter = 0;
                foreach (var scene in _editorConfiguration.Scenes)
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


                    if (_editorConfiguration.Scenes.IndexOf(scene) != 0 && _editorConfiguration.Scenes.Count > 1)
                    {

                        if (GUILayout.Button("^", GUILayout.MaxWidth(30)))
                        {
                            SceneMove(scene, true);
                            EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();
                        }
                    }

                    if (_editorConfiguration.Scenes.IndexOf(scene) != _editorConfiguration.Scenes.Count - 1 && _editorConfiguration.Scenes.Count > 1)
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
                if (_editorConfiguration.Scenes.All(n => n.Path != path))
                {
                    _editorConfiguration.Scenes.Add(new MyScenes(false, path, 0));
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
                _editorConfiguration.Scenes = new List<MyScenes>();
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
        foreach (var scene in _editorConfiguration.Scenes)
        {
            if (scene.ToBeBuilt)
            {
                copyMySceneses.Add(scene);
            }
        }

        _editorConfiguration.Scenes = copyMySceneses;
        EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();
    }

    private void DeselectAllScenes()
    {
        foreach (var scene in _editorConfiguration.Scenes)
        {
            scene.ToBeBuilt = false;
        }
        EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();

    }

    private static void SelectAllScenes()
    {
        foreach (var scene in _editorConfiguration.Scenes)
        {
            scene.ToBeBuilt = true;
        }
        EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();


    }

    private void SetUpListTest()
    {
        var myTests = new List<MyTest>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly assembly = assemblies.FirstOrDefault(assemblyName => assemblyName.GetName().Name.Equals("Assembly-CSharp-Editor"));
        var testSuite2 = (TestSuite)new DefaultTestAssemblyBuilder().Build(assembly, new Dictionary<string, object>());
        addTestSuiteToMyTest(testSuite2, myTests);
        _editorConfiguration.MyTests = myTests;
    }
    private void addTestSuiteToMyTest(ITest testSuite, List<MyTest> newMyTests)
    {
        var index = _editorConfiguration.MyTests.FirstOrDefault(a => a.TestName.Equals(testSuite.FullName));
        if (index == null)
        {
            if (testSuite.Parent == null)
            {
                newMyTests.Add(new MyTest(false, testSuite.FullName, 0, testSuite.IsSuite, testSuite.GetType(),
                    "", testSuite.TestCaseCount, false, null));
            }
            else
            {
                newMyTests.Add(new MyTest(false, testSuite.FullName, 0, testSuite.IsSuite, testSuite.GetType(),
                    testSuite.Parent.FullName, testSuite.TestCaseCount, false, null));
            }

        }
        else
        {
            newMyTests.Add(new MyTest(index.Selected, index.TestName, index.Status, index.IsSuite, testSuite.GetType(),
                index.ParentName, testSuite.TestCaseCount, index.FoldOut, index.TestResultMessage));
        }
        foreach (var test in testSuite.Tests)
        {
            addTestSuiteToMyTest(test, newMyTests);
        }
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
                gsAlterQuest.normal.background = MakeTex(20,20, selectedTestColor);
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
    /// <summary>
    /// Checks every child if a testsuite is checked
    /// Unchecks every parent of a test if it is unchecked 
    /// </summary>
    /// <param name="test"> test or testsuite that is unchecked or checked</param>
    private void ChangeSelectionChildsAndParent(MyTest test)
    {
        if (test.Selected)
        {
            if (test.Type == typeof(TestAssembly))
            {
                foreach (var test2 in _editorConfiguration.MyTests)
                {
                    test2.Selected = true;
                }
            }
            else
            {
                if (test.IsSuite)
                {
                    var index = _editorConfiguration.MyTests.IndexOf(test);
                    for (int i = index + 1; i <= index + test.TestCaseCount; i++)
                    {
                        _editorConfiguration.MyTests[i].Selected = true;
                    }
                }
            }
        }
        else
        {
            if (test.Type == typeof(TestAssembly))
            {
                foreach (var test2 in _editorConfiguration.MyTests)
                {
                    test2.Selected = false;
                }
            }
            else
            {
                var dummy = test;
                if (test.Type == typeof(TestFixture))
                {
                    var index = _editorConfiguration.MyTests.IndexOf(test);
                    for (int i = index + 1; i <= index + test.TestCaseCount; i++)
                    {
                        _editorConfiguration.MyTests[i].Selected = false;
                    }
                }
                while (dummy.ParentName != null)
                {
                    dummy = _editorConfiguration.MyTests.FirstOrDefault(a => a.TestName.Equals(dummy.ParentName));
                    if (dummy != null)
                        dummy.Selected = false;
                    else
                        return;
                }
            }
        }

    }
    /// <summary>
    /// Move the scene up or down in the list by one
    /// </summary>
    /// <param name="scene">Scene that will be moved</param>
    /// <param name="up">If true the scene will be moved up else it will be moved down</param>
    private static void SceneMove(MyScenes scene, bool up)
    {
        int index = _editorConfiguration.Scenes.IndexOf(scene);
        if (up)
        {
            Swap(index, index - 1);
        }
        else
        {
            Swap(index, index + 1);
        }
    }
    /// <summary>
    /// Swap 2 scenes from the list
    /// </summary>
    /// <param name="index1">Index of the first scene</param>
    /// <param name="index2">Index of the second scene</param>
    public static void Swap(int index1, int index2)
    {
        MyScenes backUp = _editorConfiguration.Scenes[index1];
        _editorConfiguration.Scenes[index1] = _editorConfiguration.Scenes[index2];
        _editorConfiguration.Scenes[index2] = backUp;
    }
    /// <summary>
    /// Add all scene that are found in the project
    /// </summary>
    private static void AddAllScenes()
    {
        var scenesToBeAddedGuid = AssetDatabase.FindAssets("t:SceneAsset");
        _editorConfiguration.Scenes = new List<MyScenes>();
        foreach (var sceneGuid in scenesToBeAddedGuid)
        {
            var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
            _editorConfiguration.Scenes.Add(new MyScenes(false, scenePath, 0));

        }

        EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();

    }

    private static EditorBuildSettingsScene[] PathFromTheSceneInCurrentList()
    {
        List<EditorBuildSettingsScene> listofPath = new List<EditorBuildSettingsScene>();
        foreach (var scene in _editorConfiguration.Scenes)
        {
            listofPath.Add(new EditorBuildSettingsScene(scene.Path, scene.ToBeBuilt));
        }

        return listofPath.ToArray();
    }

    /// <summary>
    /// Remove a scene from the list of scenes
    /// </summary>
    /// <param name="scene">The scene that will be removed</param>
    private void RemoveScene(MyScenes scene)
    {

        _editorConfiguration.Scenes.Remove(scene);
        EditorBuildSettings.scenes = PathFromTheSceneInCurrentList();

    }
//#if UNITY_EDITOR_OSX
    

    private static void IosDefault()
    {
        try{
        Debug.Log("Starting IOS build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
        InitBuildSetup(BuildTargetGroup.iOS);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = _editorConfiguration.OutputPathName;
        buildPlayerOptions.scenes = GetSceneForBuild();

        buildPlayerOptions.target = BuildTarget.iOS;
        buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AutoRunPlayer;

        var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
#if UNITY_2017
            if (results.Equals(""))
            {
                Debug.Log("No Build Errors");

            }
            else
                Debug.LogError("Build Error!");

#else
      if (results.summary.totalErrors == 0)
        {
            Debug.Log("No Build Errors");

        }
        else
            Debug.LogError("Build Error!");
        
#endif

            Debug.Log("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);

        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            built = true;
            ResetBuildSetup(BuildTargetGroup.iOS);
        }

    }
    private static void IosBuildFromCommandLine()
    {
        try
        {
        InitEditorConfiguration();
        InitBuildSetup(BuildTargetGroup.iOS);
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
        buildPlayerOptions.scenes = GetSceneForBuild();

        buildPlayerOptions.target = BuildTarget.iOS;

        buildPlayerOptions.options = BuildOptions.Development;

        var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
        built = true;
        RemoveAltUnityTesterFromScriptingDefineSymbols(BuildTargetGroup.iOS);
       
#if UNITY_2017
            if (results.Equals(""))
            {
                Debug.Log("No Build Errors");

            }
            else
            Debug.LogError("Build Error!");
            EditorApplication.Exit(1);

#else
        if (results.summary.totalErrors == 0)
        {
            Debug.Log("No Build Errors");

        }
        else
        {
            Debug.LogError("Build Error!");
            EditorApplication.Exit(1);
        }
        
#endif
            Debug.Log("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
            // EditorApplication.Exit(0);

         }
        catch (Exception exception)
        {
            Debug.Log(exception);
            EditorApplication.Exit(1);
        }


    }
//#endif



    public static void InitBuildSetup(BuildTargetGroup buildTargetGroup)
    {

        if (_editorConfiguration.appendToName)
        {
            PlayerSettings.productName = PlayerSettings.productName + "Test";
            string bundleIdentifier = PlayerSettings.GetApplicationIdentifier(buildTargetGroup) + "Test";
            PlayerSettings.SetApplicationIdentifier(buildTargetGroup,bundleIdentifier);
        }
        AddAltUnityTesterInScritpingDefineSymbolsGroup(buildTargetGroup);
    }

    private static void ResetBuildSetup(BuildTargetGroup buildTargetGroup)
    {

        if (_editorConfiguration.appendToName)
        {
            PlayerSettings.productName = PlayerSettings.productName.Remove(PlayerSettings.productName.Length - 5);
            string bundleIdentifier = PlayerSettings.GetApplicationIdentifier(buildTargetGroup).Remove(PlayerSettings.GetApplicationIdentifier(buildTargetGroup).Length - 5);
            PlayerSettings.SetApplicationIdentifier(buildTargetGroup, bundleIdentifier);
        }
        
        RemoveAltUnityTesterFromScriptingDefineSymbols(buildTargetGroup);
    }

    static void AndroidDefault()
    {
        try
        {
            InitEditorConfiguration();
            InitBuildSetup(BuildTargetGroup.Android);
            
            
            Debug.Log("Starting Android build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.locationPathName = _editorConfiguration.OutputPathName+".apk";
            buildPlayerOptions.scenes = GetSceneForBuild();

            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AutoRunPlayer;
            var results = BuildPipeline.BuildPlayer(buildPlayerOptions);

           

#if UNITY_2017
            if (results.Equals(""))
            {
                Debug.Log("No Build Errors");

            }
            else
                Debug.LogError("Build Error!");

#else
      if (results.summary.totalErrors == 0)
            {
                Debug.Log("No Build Errors");

            }
            else
                Debug.LogError("Build Error! " + results.steps + "\n Result: " + results.summary.result +
                               "\n Stripping info: " + results.strippingInfo);
        
#endif

            Debug.Log("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            built = true;
            ResetBuildSetup(BuildTargetGroup.Android);
        }
        
    }
    static void AndroidBuildFromCommandLine()
    {
        try
        {
        InitEditorConfiguration();
        InitBuildSetup(BuildTargetGroup.Android);
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
//        buildPlayerOptions.locationPathName = _editorConfiguration.OutPutFileNameAndroidDefault();
        buildPlayerOptions.scenes = GetSceneForBuild();

         buildPlayerOptions.locationPathName = "sampleGame.apk";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AutoRunPlayer;
        var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
        built = true;
        ResetBuildSetup(BuildTargetGroup.Android);

#if UNITY_2017
            if (results.Equals(""))
            {
                Debug.Log("No Build Errors");

            }
            else
                Debug.LogError("Build Error!");
            EditorApplication.Exit(1);

#else
       if (results.summary.totalErrors == 0)
        {
            Debug.Log("No Build Errors");

        }
        else
        {
            Debug.LogError("Build Error! " + results.steps + "\n Result: " + results.summary.result + "\n Stripping info: " + results.strippingInfo);
            EditorApplication.Exit(1);
        }
        
#endif

            Debug.Log("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
        EditorApplication.Exit(0);
        }
        catch (Exception exception)
        {
            Debug.Log(exception);
            EditorApplication.Exit(1);
        }

    }

    private static string[] GetSceneForBuild()
    {
        if (_editorConfiguration.Scenes.Count == 0)
        {
            AddAllScenes();
            SelectAllScenes();
        }
        List<String> sceneList = new List<string>();
        foreach (var scene in _editorConfiguration.Scenes)
        {
            if (scene.ToBeBuilt)
            {
                sceneList.Add(scene.Path);
            }
        }

        InsertAltUnityInTheFirstScene();


        return sceneList.ToArray();
    }

    public static void InsertAltUnityInTheFirstScene(String mainScene)
    {
        Debug.Log("Adding AltUnityRunnerPrefab into the [" + mainScene + "] scene.");
        var altUnityRunner =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("AltUnityRunnerPrefab")[0]));
        PreviousScenePath = SceneManager.GetActiveScene().path;
        SceneWithAltUnityRunner = EditorSceneManager.OpenScene(mainScene);
        AltUnityRunner = PrefabUtility.InstantiatePrefab(altUnityRunner);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        Debug.Log("Scene successfully modified.");
    }

    private static void InsertAltUnityInTheFirstScene()
    {
        var altUnityRunner =
            AssetDatabase.LoadAssetAtPath<GameObject>(
                AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("AltUnityRunnerPrefab")[0]));



        PreviousScenePath = SceneManager.GetActiveScene().path;
        SceneWithAltUnityRunner = EditorSceneManager.OpenScene(GetFirstSceneWhichWillBeBuilt());

        AltUnityRunner = PrefabUtility.InstantiatePrefab(altUnityRunner);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();

        try
        {
            EditorSceneManager.OpenScene(PreviousScenePath);
        }
        catch
        {
            Debug.Log("No scene was loaded yet.");
        }
    }

    private static string GetFirstSceneWhichWillBeBuilt()
    {
        foreach (var scene in _editorConfiguration.Scenes)
        {
            if (scene.ToBeBuilt)
            {
                return scene.Path;
            }
        }

        return "";
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }
#if UNITY_EDITOR_OSX
    private static void ThreadForwardIos()
    {

        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo {
            WindowStyle = ProcessWindowStyle.Normal,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = _editorConfiguration.IProxyPath,
            Arguments = "13000 13000"
        };
        process.StartInfo = startInfo;
        process.Start();
        idIproxyProcess = process.Id;
        iProxyOn = true;
        process.WaitForExit();
    }
    private static void KillIProxy(int id)
    {


        var chosenOne = Process.GetProcessesByName("iproxy");
        chosenOne[0].Kill();
        chosenOne[0].WaitForExit();
    }
#endif

    private static void ForwardAndroid()
    {
        string adbFileName;
#if UNITY_EDITOR_WIN
        adbFileName = "adb.exe";
#elif UNITY_EDITOR_OSX
        adbFileName = _editorConfiguration.AdbPath;
#endif

        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo {
            WindowStyle = ProcessWindowStyle.Hidden,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = adbFileName,
            Arguments = "forward tcp:13000 tcp:13000 "
        };
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();

    }
    private static void RemoveForwardAndroid()
    {
        string adbFileName;
#if UNITY_EDITOR_WIN
        adbFileName = "adb.exe";
#elif UNITY_EDITOR_OSX
        adbFileName = _editorConfiguration.AdbPath;
#endif
        var process = new Process();
        var startInfo = new ProcessStartInfo {
            WindowStyle = ProcessWindowStyle.Normal,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = adbFileName,
            Arguments = "forward --remove-all"
        };
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();
    }


    [MenuItem("Assets/Create/AltUnityTest", false, 80)]
    public static void CreateAltUnityTest()
    {

        var templatePath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("DefaultTestExample")[0]);

        string folderPath = GetPath();
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
    public static bool CreateAltUnityTestValid()
    {
        return (GetPath() + "/").Contains("/Editor/");
    }

    [MenuItem("Window/CreateAltUnityTesterPackage")]
    public static void CreateAltUnityTesterPackage() {
        Debug.Log("AltUnityTester - Unity Package creation started...");
        string packageName="AltUnityTester.unitypackage";
        string assetPathNames = "Assets/AltUnityTester";
        AssetDatabase.ExportPackage(assetPathNames, packageName, ExportPackageOptions.Recurse);
        Debug.Log("AltUnityTester - Unity Package done.");
    }

    private static string GetPath()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (Path.GetExtension(path) != "")
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
        EditorSceneManager.OpenScene(PreviousScenePath);


    }

    static void RunAllTestsAndroid()
    {
        try
        {
            InitEditorConfiguration();
            Debug.Log("Started running test");
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly assembly = assemblies.FirstOrDefault(assemblyName => assemblyName.GetName().Name.Equals("Assembly-CSharp-Editor"));

            var testSuite2 =
                (TestSuite) new DefaultTestAssemblyBuilder().Build(assembly, new Dictionary<string, object>());

            OrFilter filter = new OrFilter();
            foreach (var test in testSuite2.Tests)
            foreach (var t in test.Tests)
            {
                Debug.Log(t.FullName);
                filter.Add(new FullNameFilter(t.FullName));
            }


            RemoveForwardAndroid();
#if UNITY_EDITOR_OSX
        if(idIproxyProcess!=0)    
            KillIProxy(idIproxyProcess);
#endif
            ForwardAndroid();

            ITestListener listener = new TestRunListener(null);
            var testAssemblyRunner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());

            testAssemblyRunner.Load(assembly, new Dictionary<string, object>());


            var result = testAssemblyRunner.Run(listener, filter);

            RemoveForwardAndroid();
            if (result.FailCount > 0)
            {
                EditorApplication.Exit(1);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            EditorApplication.Exit(1);
        }
    }

#if UNITY_EDITOR_OSX
    static void RunAllTestsIOS()
    {
        try { 

        InitEditorConfiguration();
        Debug.Log("Started running test");
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly assembly = assemblies.FirstOrDefault(assemblyName => assemblyName.GetName().Name.Equals("Assembly-CSharp-Editor"));

        var testSuite2 = (TestSuite)new DefaultTestAssemblyBuilder().Build(assembly, new Dictionary<string, object>());

        OrFilter filter = new OrFilter();
        foreach (var test in testSuite2.Tests)
            foreach (var t in test.Tests)
            {
                Debug.Log(t.FullName);
                filter.Add(new FullNameFilter(t.FullName));
            }
        RemoveForwardAndroid();
        thread = new Thread(ThreadForwardIos);
        thread.Start();
        while (!iProxyOn)
        {
            Thread.Sleep(250);
        }



        ITestListener listener = new TestRunListener(null);
        var testAssemblyRunner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());

        testAssemblyRunner.Load(assembly, new Dictionary<string, object>());


        var result = testAssemblyRunner.Run(listener, filter);


        KillIProxy(idIproxyProcess);
        thread.Join();


        if (result.FailCount > 0)
        {
            EditorApplication.Exit(1);
        }
     }
        catch (Exception e)
        {
            Debug.LogError(e);
            EditorApplication.Exit(1);
        }

    }
#endif


}
