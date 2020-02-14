using System.Linq;

public delegate void TestRunDelegate(string name);


public class AltUnityTestRunner
{


    public enum TestRunMode { RunAllTest, RunSelectedTest, RunFailedTest }

    private static System.Threading.Thread thread;
    //This are for progressBar when are runned
    private static float progress;
    private static float total;
    private static string _testName;

    public static TestRunDelegate CallRunDelegate = new TestRunDelegate(ShowProgresBar);


    public static void RunTests(TestRunMode testMode)
    {
        UnityEngine.Debug.Log("Started running test");
        System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        System.Reflection.Assembly assembly = assemblies.FirstOrDefault(assemblyName => assemblyName.GetName().Name.Equals("Assembly-CSharp-Editor"));

        var filters = AddTestToBeRun(testMode);
        NUnit.Framework.Interfaces.ITestListener listener = new AltUnityTestRunListener(CallRunDelegate);
        var testAssemblyRunner = new NUnit.Framework.Api.NUnitTestAssemblyRunner(new NUnit.Framework.Api.DefaultTestAssemblyBuilder());

        testAssemblyRunner.Load(assembly, new System.Collections.Generic.Dictionary<string, object>());
        progress = 0;
        total = filters.Filters.Count;
        System.Threading.Thread runTestThread = new System.Threading.Thread(() =>
        {
            var result = testAssemblyRunner.Run(listener, filters);
            SetTestStatus(result);
            AltUnityTesterEditor.isTestRunResultAvailable = true;
            AltUnityTesterEditor.selectedTest = -1;
        });

        runTestThread.Start();
        if (AltUnityTesterEditor.EditorConfiguration.platform != AltUnityPlatform.Editor)
        {
            float previousProgres = progress - 1;
            while (runTestThread.IsAlive)
            {
                if (previousProgres == progress) continue;
                UnityEditor.EditorUtility.DisplayProgressBar(progress == total ? "This may take a few seconds" : _testName,
                    progress + "/" + total, progress / total);
                previousProgres = progress;
            }
        }

        runTestThread.Join();
        if (AltUnityTesterEditor.EditorConfiguration.platform != AltUnityPlatform.Editor)
        {
            AltUnityTesterEditor.needsRepaiting = true;
            UnityEditor.EditorUtility.ClearProgressBar();
        }
    }



    private static void ShowProgresBar(string name)
    {
        progress++;
        _testName = name;
    }

    private void SetTestStatus(System.Collections.Generic.List<NUnit.Framework.Interfaces.ITestResult> results)
    {
        bool passed = true;
        int numberOfTestPassed = 0;
        int numberOfTestFailed = 0;
        double totalTime = 0;
        foreach (var test in AltUnityTesterEditor.EditorConfiguration.MyTests)
        {
            int counter = 0;
            // int testPassed = 0;
            int testPassedCounter = 0;
            int testFailedCounter = 0;
            foreach (var result in results)
            {
                if (test.Type == typeof(NUnit.Framework.Internal.TestAssembly))
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

                if (test.Type == typeof(NUnit.Framework.Internal.TestFixture))
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

                if (test.Type == typeof(NUnit.Framework.Internal.TestMethod))
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

            if (test.Type != typeof(NUnit.Framework.Internal.TestMethod))
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
        var listOfTests = AltUnityTesterEditor.EditorConfiguration.MyTests;
        var serializeTests = Newtonsoft.Json.JsonConvert.SerializeObject(listOfTests, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
        UnityEditor.EditorPrefs.SetString("tests", serializeTests);

        AltUnityTesterEditor.reportTestPassed = numberOfTestPassed;
        AltUnityTesterEditor.reportTestFailed = numberOfTestFailed;
        AltUnityTesterEditor.isTestRunResultAvailable = true;
        AltUnityTesterEditor.selectedTest = -1;
        AltUnityTesterEditor.timeTestRan = totalTime;
        if (passed)
        {
            UnityEngine.Debug.Log("All test passed");
        }
        else
            UnityEngine.Debug.Log("Test failed");
    }

    private static NUnit.Framework.Internal.Filters.OrFilter AddTestToBeRun(TestRunMode testMode)
    {
        NUnit.Framework.Internal.Filters.OrFilter filter = new NUnit.Framework.Internal.Filters.OrFilter();
        switch (testMode)
        {
            case TestRunMode.RunAllTest:
                foreach (var test in AltUnityTesterEditor.EditorConfiguration.MyTests)
                    if (!test.IsSuite)
                        filter.Add(new NUnit.Framework.Internal.Filters.FullNameFilter(test.TestName));
                break;
            case TestRunMode.RunSelectedTest:
                foreach (var test in AltUnityTesterEditor.EditorConfiguration.MyTests)
                    if (test.Selected && !test.IsSuite)
                        filter.Add(new NUnit.Framework.Internal.Filters.FullNameFilter(test.TestName));
                break;
            case TestRunMode.RunFailedTest:
                foreach (var test in AltUnityTesterEditor.EditorConfiguration.MyTests)
                    if (test.Status == -1 && !test.IsSuite)
                        filter.Add(new NUnit.Framework.Internal.Filters.FullNameFilter(test.TestName));
                break;
        }

        return filter;
    }

    static int SetTestStatus(NUnit.Framework.Interfaces.ITestResult test)
    {

        if (!test.Test.IsSuite)
        {
            var status = 0;
            if (test.PassCount == 1)
            {
                status = 1;
                AltUnityTesterEditor.reportTestPassed++;
            }
            else if (test.FailCount == 1)
            {
                status = -1;
                AltUnityTesterEditor.reportTestFailed++;
            }
            AltUnityTesterEditor.timeTestRan += test.Duration;
            int index = AltUnityTesterEditor.EditorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName));
            AltUnityTesterEditor.EditorConfiguration.MyTests[index].Status = status;
            AltUnityTesterEditor.EditorConfiguration.MyTests[index].TestDuration = test.Duration;
            AltUnityTesterEditor.EditorConfiguration.MyTests[index].TestStackTrace = test.StackTrace;
            AltUnityTesterEditor.EditorConfiguration.MyTests[index].TestResultMessage = test.Message;
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
            AltUnityTesterEditor.EditorConfiguration.MyTests[AltUnityTesterEditor.EditorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName))].Status = 0;
            return 0;
        }

        if (failCount > 0)
        {
            AltUnityTesterEditor.EditorConfiguration.MyTests[AltUnityTesterEditor.EditorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName))].Status = -1;
            return -1;

        }
        AltUnityTesterEditor.EditorConfiguration.MyTests[AltUnityTesterEditor.EditorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName))].Status = 1;
        return 1;
    }

    public static void SetUpListTest()
    {
        var myTests = new System.Collections.Generic.List<AltUnityMyTest>();
        System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

        const string engineTestRunnerAssemblyName = "UnityEngine.TestRunner";
        const string editorTestRunnerAssemblyName = "UnityEditor.TestRunner";
        const string editorAssemblyName = "Assembly-CSharp-Editor";
        foreach (var assembly in assemblies)
        {
            /*
             * Skips test runner assemblies and assemblies that do not contain references to test assemblies
             */
            bool isEditorAssembly = assembly.GetName().Name.Equals(editorAssemblyName);
            if (!isEditorAssembly)
            {
                bool isEngineTestRunnerAssembly = assembly.GetName().Name.Contains(engineTestRunnerAssemblyName);
                bool isEditorTestRunnerAssembly = assembly.GetName().Name.Contains(editorTestRunnerAssemblyName);
                bool isTestAssembly = assembly.GetReferencedAssemblies().FirstOrDefault(
                            reference => reference.Name.Contains(engineTestRunnerAssemblyName)
                                         || reference.Name.Contains(editorTestRunnerAssemblyName)) == null;
                if (isEngineTestRunnerAssembly ||
                    isEditorTestRunnerAssembly ||
                    isTestAssembly)
                {
                    continue;
                }
            }

            var testSuite = (NUnit.Framework.Internal.TestSuite)new NUnit.Framework.Api.DefaultTestAssemblyBuilder().Build(assembly, new System.Collections.Generic.Dictionary<string, object>());
            addTestSuiteToMyTest(testSuite, myTests);
        }

        AltUnityTesterEditor.EditorConfiguration.MyTests = myTests;
    }

    private static void addTestSuiteToMyTest(NUnit.Framework.Interfaces.ITest testSuite, System.Collections.Generic.List<AltUnityMyTest> newMyTests)
    {
        string path = null;

        if (testSuite.GetType() == typeof(NUnit.Framework.Internal.TestMethod))
        {
            var fullName = testSuite.FullName;
            var className = fullName.Split('.')[0];
            var assets = UnityEditor.AssetDatabase.FindAssets(className);
            if (assets.Length != 0)
            {
                path = UnityEditor.AssetDatabase.GUIDToAssetPath(assets[0]);
            }
        }
        var index = AltUnityTesterEditor.EditorConfiguration.MyTests.FirstOrDefault(a => a.TestName.Equals(testSuite.FullName));
        if (index == null)
        {
            if (testSuite.Parent == null)
            {
                newMyTests.Add(new AltUnityMyTest(false, testSuite.FullName, 0, testSuite.IsSuite, testSuite.GetType(),
                    "", testSuite.TestCaseCount, false, null, null, 0, path));
            }
            else
            {
                newMyTests.Add(new AltUnityMyTest(false, testSuite.FullName, 0, testSuite.IsSuite, testSuite.GetType(),
                    testSuite.Parent.FullName, testSuite.TestCaseCount, false, null, null, 0, path));
            }

        }
        else
        {
            newMyTests.Add(new AltUnityMyTest(index.Selected, index.TestName, index.Status, index.IsSuite, testSuite.GetType(),
                index.ParentName, testSuite.TestCaseCount, index.FoldOut, index.TestResultMessage, index.TestStackTrace, index.TestDuration, path));
        }
        foreach (var test in testSuite.Tests)
        {
            addTestSuiteToMyTest(test, newMyTests);
        }
    }


#if UNITY_EDITOR_OSX
    static void RunAllTestsIOS() {
        try {

            AltUnityTesterEditor.InitEditorConfiguration();
            UnityEngine.Debug.Log("Started running test");
            System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            System.Reflection.Assembly assembly = assemblies.FirstOrDefault(assemblyName => assemblyName.GetName().Name.Equals("Assembly-CSharp-Editor"));

            var testSuite2 = (NUnit.Framework.Internal.TestSuite)new NUnit.Framework.Api.DefaultTestAssemblyBuilder().Build(assembly, new System.Collections.Generic.Dictionary<string, object>());

            NUnit.Framework.Internal.Filters.OrFilter filter = new NUnit.Framework.Internal.Filters.OrFilter();
            foreach (var test in testSuite2.Tests)
                foreach (var t in test.Tests) {
                    UnityEngine.Debug.Log(t.FullName);
                    filter.Add(new NUnit.Framework.Internal.Filters.FullNameFilter(t.FullName));
                }

            NUnit.Framework.Interfaces.ITestListener listener = new AltUnityTestRunListener(null);
            var testAssemblyRunner = new NUnit.Framework.Api.NUnitTestAssemblyRunner(new NUnit.Framework.Api.DefaultTestAssemblyBuilder());
            testAssemblyRunner.Load(assembly, new System.Collections.Generic.Dictionary<string, object>());
            var result = testAssemblyRunner.Run(listener, filter);
            if (result.FailCount > 0) {
                UnityEditor.EditorApplication.Exit(1);
            }
        } catch (System.Exception e) {
            UnityEngine.Debug.LogError(e);
            UnityEditor.EditorApplication.Exit(1);
        }

    }
#endif



    static void RunAllTestsAndroid()
    {
        try
        {
            AltUnityTesterEditor.InitEditorConfiguration();
            UnityEngine.Debug.Log("Started running test");
            System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            System.Reflection.Assembly assembly = assemblies.FirstOrDefault(assemblyName => assemblyName.GetName().Name.Equals("Assembly-CSharp-Editor"));

            var testSuite2 =
                (NUnit.Framework.Internal.TestSuite)new NUnit.Framework.Api.DefaultTestAssemblyBuilder().Build(assembly, new System.Collections.Generic.Dictionary<string, object>());

            NUnit.Framework.Internal.Filters.OrFilter filter = new NUnit.Framework.Internal.Filters.OrFilter();
            foreach (var test in testSuite2.Tests)
                foreach (var t in test.Tests)
                {
                    UnityEngine.Debug.Log(t.FullName);
                    filter.Add(new NUnit.Framework.Internal.Filters.FullNameFilter(t.FullName));
                }

            NUnit.Framework.Interfaces.ITestListener listener = new AltUnityTestRunListener(null);
            var testAssemblyRunner = new NUnit.Framework.Api.NUnitTestAssemblyRunner(new NUnit.Framework.Api.DefaultTestAssemblyBuilder());

            testAssemblyRunner.Load(assembly, new System.Collections.Generic.Dictionary<string, object>());


            var result = testAssemblyRunner.Run(listener, filter);
            if (result.FailCount > 0)
            {
                UnityEditor.EditorApplication.Exit(1);
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError(e);
            UnityEditor.EditorApplication.Exit(1);
        }
    }


}
