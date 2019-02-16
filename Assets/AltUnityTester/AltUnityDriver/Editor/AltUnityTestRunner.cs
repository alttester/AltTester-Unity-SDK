using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Filters;
using UnityEditor;
using UnityEngine;

public delegate void TestRunDelegate(string name);


public class AltUnityTestRunner {


    public enum TestRunMode { RunAllTest, RunSelectedTest, RunFailedTest }

    private static Thread thread;
    //This are for progressBar when are runned
    private static float progress;
    private static float total;
    private static string _testName;

    public static TestRunDelegate CallRunDelegate = new TestRunDelegate(ShowProgresBar);


    public static void RunTests(TestRunMode testMode) {
        Debug.Log("Started running test");
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly assembly = assemblies.FirstOrDefault(assemblyName => assemblyName.GetName().Name.Equals("Assembly-CSharp-Editor"));

        var filters = AddTestToBeRun(testMode);
        ITestListener listener = new TestRunListener(CallRunDelegate);
        var testAssemblyRunner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());

        testAssemblyRunner.Load(assembly, new Dictionary<string, object>());
        progress = 0;
        total = filters.Filters.Count;
        Thread runTestThread = new Thread(() => {
            var result = testAssemblyRunner.Run(listener, filters);
            SetTestStatus(result);
            AltUnityTesterEditor.isTestRunResultAvailable = true;
            AltUnityTesterEditor.selectedTest = -1;
        });

        runTestThread.Start();
        if (AltUnityTesterEditor.EditorConfiguration.platform != Platform.Editor) {
            float previousProgres = progress - 1;
            while (runTestThread.IsAlive) {
                if (previousProgres == progress) continue;
                EditorUtility.DisplayProgressBar(progress == total ? "This may take a few seconds" : _testName,
                    progress + "/" + total, progress / total);
                previousProgres = progress;
            }
        }

        runTestThread.Join();
        if (AltUnityTesterEditor.EditorConfiguration.platform != Platform.Editor) {
            AltUnityTesterEditor.needsRepaiting = true;
            EditorUtility.ClearProgressBar();
        }
    }



    private static void ShowProgresBar(string name) {
        progress++;
        _testName = name;
    }

    private void SetTestStatus(List<ITestResult> results) {
        bool passed = true;
        int numberOfTestPassed = 0;
        int numberOfTestFailed = 0;
        double totalTime = 0;
        foreach (var test in AltUnityTesterEditor.EditorConfiguration.MyTests) {
            int counter = 0;
            // int testPassed = 0;
            int testPassedCounter = 0;
            int testFailedCounter = 0;
            foreach (var result in results) {
                if (test.Type == typeof(TestAssembly)) {
                    counter++;
                    var enumerator = result.Children.GetEnumerator();
                    enumerator.MoveNext();
                    if (enumerator.Current != null) {
                        var enumerator2 = enumerator.Current.Children.GetEnumerator();
                        enumerator2.MoveNext();
                        if (enumerator2.Current != null && enumerator2.Current.FailCount > 0) {

                            testFailedCounter++;
                        } else if (enumerator2.Current != null && enumerator2.Current.PassCount > 0) {
                            testPassedCounter++;
                        }

                        enumerator2.Dispose();
                    }

                    enumerator.Dispose();

                }

                if (test.Type == typeof(TestFixture)) {
                    var enumerator = result.Children.GetEnumerator();
                    enumerator.MoveNext();
                    if (enumerator.Current != null && enumerator.Current.FullName.Equals(test.TestName)) {
                        counter++;
                        var enumerator2 = enumerator.Current.Children.GetEnumerator();
                        enumerator2.MoveNext();
                        if (enumerator2.Current != null && enumerator2.Current.FailCount > 0) {
                            testFailedCounter++;

                        } else if (enumerator2.Current != null && enumerator2.Current.PassCount > 0) {
                            testPassedCounter++;

                        }
                        enumerator2.Dispose();
                    }
                    enumerator.Dispose();
                }

                if (test.Type == typeof(TestMethod)) {
                    var enumerator = result.Children.GetEnumerator();
                    enumerator.MoveNext();
                    if (enumerator.Current != null) {
                        var enumerator2 = enumerator.Current.Children.GetEnumerator();
                        enumerator2.MoveNext();
                        if (enumerator2.Current != null && enumerator2.Current.FullName.Equals(test.TestName)) {
                            if (enumerator2.Current.FailCount > 0) {
                                test.Status = -1;
                                test.TestResultMessage = enumerator2.Current.Message + " \n\n\n StackTrace:  " + enumerator2.Current.StackTrace;
                                passed = false;
                                numberOfTestFailed++;

                            } else if (enumerator2.Current.PassCount > 0) {
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

            if (test.Type != typeof(TestMethod)) {
                if (test.TestCaseCount == counter) {
                    if (testFailedCounter == 0 && testPassedCounter == counter) {
                        test.Status = 1;
                        test.TestResultMessage = "All method passed ";
                    } else {
                        test.Status = -1;
                        passed = false;
                        test.TestResultMessage = "There are methods that failed";
                    }
                }
            }
        }
        var listOfTests = AltUnityTesterEditor.EditorConfiguration.MyTests;
        var serializeTests = JsonConvert.SerializeObject(listOfTests, Formatting.Indented, new JsonSerializerSettings {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        EditorPrefs.SetString("tests", serializeTests);

        AltUnityTesterEditor.reportTestPassed = numberOfTestPassed;
        AltUnityTesterEditor.reportTestFailed = numberOfTestFailed;
        AltUnityTesterEditor.isTestRunResultAvailable = true;
        AltUnityTesterEditor.selectedTest = -1;
        AltUnityTesterEditor.timeTestRan = totalTime;
        if (passed) {
            Debug.Log("All test passed");
        } else
            Debug.Log("Test failed");
    }

    private static OrFilter AddTestToBeRun(TestRunMode testMode) {
        OrFilter filter = new OrFilter();
        switch (testMode) {
            case TestRunMode.RunAllTest:
                foreach (var test in AltUnityTesterEditor.EditorConfiguration.MyTests)
                    if (!test.IsSuite)
                        filter.Add(new FullNameFilter(test.TestName));
                break;
            case TestRunMode.RunSelectedTest:
                foreach (var test in AltUnityTesterEditor.EditorConfiguration.MyTests)
                    if (test.Selected && !test.IsSuite)
                        filter.Add(new FullNameFilter(test.TestName));
                break;
            case TestRunMode.RunFailedTest:
                foreach (var test in AltUnityTesterEditor.EditorConfiguration.MyTests)
                    if (test.Status == -1 && !test.IsSuite)
                        filter.Add(new FullNameFilter(test.TestName));
                break;
        }

        return filter;
    }

    static int SetTestStatus(ITestResult test) {

        if (!test.Test.IsSuite) {
            var status = 0;
            string message = "";
            if (test.PassCount == 1) {
                status = 1;
                message = "Passed in " + test.Duration;
                AltUnityTesterEditor.reportTestPassed++;

            } else if (test.FailCount == 1) {
                status = -1;
                message = test.Message;
                AltUnityTesterEditor.reportTestFailed++;
            }

            AltUnityTesterEditor.timeTestRan += test.Duration;
            int index = AltUnityTesterEditor.EditorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName));
            AltUnityTesterEditor.EditorConfiguration.MyTests[index].Status = status;
            AltUnityTesterEditor.EditorConfiguration.MyTests[index].TestResultMessage = message;

            return status;
        }

        var failCount = 0;
        var notExecutedCount = 0;
        var passCount = 0;
        foreach (var testChild in test.Children) {
            var status = SetTestStatus(testChild);
            if (status == 0)
                notExecutedCount++;
            else if (status == -1) {
                failCount++;

            } else {
                passCount++;
            }
        }

        if (test.Test.TestCaseCount != passCount + failCount + notExecutedCount) {
            AltUnityTesterEditor.EditorConfiguration.MyTests[AltUnityTesterEditor.EditorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName))].Status = 0;
            return 0;
        }

        if (failCount > 0) {
            AltUnityTesterEditor.EditorConfiguration.MyTests[AltUnityTesterEditor.EditorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName))].Status = -1;
            return -1;

        }
        AltUnityTesterEditor.EditorConfiguration.MyTests[AltUnityTesterEditor.EditorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName))].Status = 1;
        return 1;
    }

    public static void SetUpListTest() {
        var myTests = new List<MyTest>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly assembly = assemblies.FirstOrDefault(assemblyName => assemblyName.GetName().Name.Equals("Assembly-CSharp-Editor"));
        var testSuite2 = (TestSuite)new DefaultTestAssemblyBuilder().Build(assembly, new Dictionary<string, object>());
        addTestSuiteToMyTest(testSuite2, myTests);
        AltUnityTesterEditor.EditorConfiguration.MyTests = myTests;
    }

    private static void addTestSuiteToMyTest(ITest testSuite, List<MyTest> newMyTests) {
        var index = AltUnityTesterEditor.EditorConfiguration.MyTests.FirstOrDefault(a => a.TestName.Equals(testSuite.FullName));
        if (index == null) {
            if (testSuite.Parent == null) {
                newMyTests.Add(new MyTest(false, testSuite.FullName, 0, testSuite.IsSuite, testSuite.GetType(),
                    "", testSuite.TestCaseCount, false, null));
            } else {
                newMyTests.Add(new MyTest(false, testSuite.FullName, 0, testSuite.IsSuite, testSuite.GetType(),
                    testSuite.Parent.FullName, testSuite.TestCaseCount, false, null));
            }

        } else {
            newMyTests.Add(new MyTest(index.Selected, index.TestName, index.Status, index.IsSuite, testSuite.GetType(),
                index.ParentName, testSuite.TestCaseCount, index.FoldOut, index.TestResultMessage));
        }
        foreach (var test in testSuite.Tests) {
            addTestSuiteToMyTest(test, newMyTests);
        }
    }


#if UNITY_EDITOR_OSX
    static void RunAllTestsIOS() {
        try {

            AltUnityTesterEditor.InitEditorConfiguration();
            Debug.Log("Started running test");
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly assembly = assemblies.FirstOrDefault(assemblyName => assemblyName.GetName().Name.Equals("Assembly-CSharp-Editor"));

            var testSuite2 = (TestSuite)new DefaultTestAssemblyBuilder().Build(assembly, new Dictionary<string, object>());

            OrFilter filter = new OrFilter();
            foreach (var test in testSuite2.Tests)
                foreach (var t in test.Tests) {
                    Debug.Log(t.FullName);
                    filter.Add(new FullNameFilter(t.FullName));
                }
           
            ITestListener listener = new TestRunListener(null);
            var testAssemblyRunner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());
            testAssemblyRunner.Load(assembly, new Dictionary<string, object>());
            var result = testAssemblyRunner.Run(listener, filter);
            if (result.FailCount > 0) {
                EditorApplication.Exit(1);
            }
        } catch (Exception e) {
            Debug.LogError(e);
            EditorApplication.Exit(1);
        }

    }
#endif



    static void RunAllTestsAndroid() {
        try {
            AltUnityTesterEditor.InitEditorConfiguration();
            Debug.Log("Started running test");
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly assembly = assemblies.FirstOrDefault(assemblyName => assemblyName.GetName().Name.Equals("Assembly-CSharp-Editor"));

            var testSuite2 =
                (TestSuite)new DefaultTestAssemblyBuilder().Build(assembly, new Dictionary<string, object>());

            OrFilter filter = new OrFilter();
            foreach (var test in testSuite2.Tests)
                foreach (var t in test.Tests) {
                    Debug.Log(t.FullName);
                    filter.Add(new FullNameFilter(t.FullName));
                }

            ITestListener listener = new TestRunListener(null);
            var testAssemblyRunner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());

            testAssemblyRunner.Load(assembly, new Dictionary<string, object>());


            var result = testAssemblyRunner.Run(listener, filter);
            if (result.FailCount > 0) {
                EditorApplication.Exit(1);
            }
        } catch (Exception e) {
            Debug.LogError(e);
            EditorApplication.Exit(1);
        }
    }


}
