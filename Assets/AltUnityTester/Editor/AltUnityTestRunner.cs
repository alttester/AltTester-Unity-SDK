using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Altom.AltUnityTesterEditor.Logging;
using Newtonsoft.Json;
using Unity.EditorCoroutines.Editor;

namespace Altom.AltUnityTesterEditor
{
    public delegate void TestRunDelegate(string name);

    public class AltUnityTestRunner
    {
        private static readonly NLog.Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();

        public enum TestRunMode { RunAllTest, RunSelectedTest, RunFailedTest }

        //This are for progressBar when are runned
        private static float progress;
        private static float total;
        private static string testName;

        public static TestRunDelegate CallRunDelegate = new TestRunDelegate(showProgresBar);


        public static void RunTests(TestRunMode testMode)
        {
            logger.Debug("Started running test");
            System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            System.Reflection.Assembly assembly = assemblies.FirstOrDefault(assemblyName => assemblyName.GetName().Name.Equals("Assembly-CSharp-Editor"));

            var filters = addTestToBeRun(testMode);
            NUnit.Framework.Interfaces.ITestListener listener = new AltUnityTestRunListener(CallRunDelegate);
            var testAssemblyRunner = new NUnit.Framework.Api.NUnitTestAssemblyRunner(new NUnit.Framework.Api.DefaultTestAssemblyBuilder());

            testAssemblyRunner.Load(assembly, new Dictionary<string, object>());
            progress = 0;
            total = filters.Filters.Count;
            var runTestThread = new System.Threading.Thread(() =>
            {
                var result = testAssemblyRunner.Run(listener, filters);
                setTestStatus(result);
                AltUnityTesterEditorWindow.IsTestRunResultAvailable = true;
                AltUnityTesterEditorWindow.SelectedTest = -1;
            });

            runTestThread.Start();
            if (AltUnityTesterEditorWindow.EditorConfiguration.platform != AltUnityPlatform.Editor)
            {
                float previousProgres = progress - 1;
                while (runTestThread.IsAlive)
                {
                    if (previousProgres == progress) continue;
                    UnityEditor.EditorUtility.DisplayProgressBar(progress == total ? "This may take a few seconds" : testName,
                        progress + "/" + total, progress / total);
                    previousProgres = progress;
                }
            }

            runTestThread.Join();
            if (AltUnityTesterEditorWindow.EditorConfiguration.platform != AltUnityPlatform.Editor)
            {
                AltUnityTesterEditorWindow.NeedsRepaiting = true;
                UnityEditor.EditorUtility.ClearProgressBar();
            }
        }



        private static void showProgresBar(string name)
        {
            progress++;
            testName = name;
        }

        private void setTestStatus(List<NUnit.Framework.Interfaces.ITestResult> results)
        {
            bool passed = true;
            int numberOfTestPassed = 0;
            int numberOfTestFailed = 0;
            double totalTime = 0;
            foreach (var test in AltUnityTesterEditorWindow.EditorConfiguration.MyTests)
            {
                int counter = 0;
                // int testPassed = 0;
                int testPassedCounter = 0;
                int testFailedCounter = 0;
                foreach (var result in results)
                {
                    switch (test.Type.ToString())
                    {
                        case "NUnit.Framework.Internal.TestMethod":
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
                            break;
                        case "NUnit.Framework.Internal.TestFixture":
                            enumerator = result.Children.GetEnumerator();
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
                            break;
                        case "NUnit.Framework.Internal.TestAssembly":
                            counter++;
                            enumerator = result.Children.GetEnumerator();
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
                            break;
                    }

                }

                if (test.Type.Equals("NUnit.Framework.Internal.TestMethod"))
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
            var listOfTests = AltUnityTesterEditorWindow.EditorConfiguration.MyTests;
            var serializeTests = JsonConvert.SerializeObject(listOfTests, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            UnityEditor.EditorPrefs.SetString("tests", serializeTests);

            AltUnityTesterEditorWindow.ReportTestPassed = numberOfTestPassed;
            AltUnityTesterEditorWindow.ReportTestFailed = numberOfTestFailed;
            AltUnityTesterEditorWindow.IsTestRunResultAvailable = true;
            AltUnityTesterEditorWindow.SelectedTest = -1;
            AltUnityTesterEditorWindow.TimeTestRan = totalTime;
            if (passed)
            {
                logger.Debug("All test passed");
            }
            else
            {
                logger.Debug("Test failed");
            }
        }

        private static NUnit.Framework.Internal.Filters.OrFilter addTestToBeRun(TestRunMode testMode)
        {
            var filter = new NUnit.Framework.Internal.Filters.OrFilter();
            switch (testMode)
            {
                case TestRunMode.RunAllTest:
                    foreach (var test in AltUnityTesterEditorWindow.EditorConfiguration.MyTests)
                        if (!test.IsSuite)
                            filter.Add(new NUnit.Framework.Internal.Filters.FullNameFilter(test.TestName));
                    break;
                case TestRunMode.RunSelectedTest:
                    foreach (var test in AltUnityTesterEditorWindow.EditorConfiguration.MyTests)
                        if (test.Selected && !test.IsSuite)
                            filter.Add(new NUnit.Framework.Internal.Filters.FullNameFilter(test.TestName));
                    break;
                case TestRunMode.RunFailedTest:
                    foreach (var test in AltUnityTesterEditorWindow.EditorConfiguration.MyTests)
                        if (test.Status == -1 && !test.IsSuite)
                            filter.Add(new NUnit.Framework.Internal.Filters.FullNameFilter(test.TestName));
                    break;
            }

            return filter;
        }

        private static int setTestStatus(NUnit.Framework.Interfaces.ITestResult test)
        {

            if (!test.Test.IsSuite)
            {
                var status = 0;
                if (test.PassCount == 1)
                {
                    status = 1;
                    AltUnityTesterEditorWindow.ReportTestPassed++;
                }
                else if (test.FailCount == 1)
                {
                    status = -1;
                    AltUnityTesterEditorWindow.ReportTestFailed++;
                }
                AltUnityTesterEditorWindow.TimeTestRan += test.Duration;
                int index = AltUnityTesterEditorWindow.EditorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName));
                AltUnityTesterEditorWindow.EditorConfiguration.MyTests[index].Status = status;
                AltUnityTesterEditorWindow.EditorConfiguration.MyTests[index].TestDuration = test.Duration;
                AltUnityTesterEditorWindow.EditorConfiguration.MyTests[index].TestStackTrace = test.StackTrace;
                AltUnityTesterEditorWindow.EditorConfiguration.MyTests[index].TestResultMessage = test.Message;
                return status;
            }

            var failCount = 0;
            var notExecutedCount = 0;
            var passCount = 0;
            foreach (var testChild in test.Children)
            {
                var status = setTestStatus(testChild);
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
                AltUnityTesterEditorWindow.EditorConfiguration.MyTests[AltUnityTesterEditorWindow.EditorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName))].Status = 0;
                return 0;
            }

            if (failCount > 0)
            {
                AltUnityTesterEditorWindow.EditorConfiguration.MyTests[AltUnityTesterEditorWindow.EditorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName))].Status = -1;
                return -1;

            }
            AltUnityTesterEditorWindow.EditorConfiguration.MyTests[AltUnityTesterEditorWindow.EditorConfiguration.MyTests.FindIndex(a => a.TestName.Equals(test.Test.FullName))].Status = 1;
            return 1;
        }

        public static IEnumerator SetUpListTest()
        {
            var myTests = new List<AltUnityMyTest>();
            System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            const string ENGINE_TEST_RUNNER_ASSEMBLYNAME = "UnityEngine.TestRunner";
            const string EDITOR_TEST_RUNNER_ASSEMBLYNAME = "UnityEditor.TestRunner";
            const string EDITOR_ASSEMBLYNAME = "Assembly-CSharp-Editor";
            foreach (var assembly in assemblies)
            {
                /*
                 * Skips test runner assemblies and assemblies that do not contain references to test assemblies
                 */
                bool isEditorAssembly = assembly.GetName().Name.Equals(EDITOR_ASSEMBLYNAME);
                if (!isEditorAssembly)
                {
                    bool isEngineTestRunnerAssembly = assembly.GetName().Name.Contains(ENGINE_TEST_RUNNER_ASSEMBLYNAME);
                    bool isEditorTestRunnerAssembly = assembly.GetName().Name.Contains(EDITOR_TEST_RUNNER_ASSEMBLYNAME);
                    bool isTestAssembly = assembly.GetReferencedAssemblies().FirstOrDefault(
                                reference => reference.Name.Contains(ENGINE_TEST_RUNNER_ASSEMBLYNAME)
                                             || reference.Name.Contains(EDITOR_TEST_RUNNER_ASSEMBLYNAME)) == null;
                    if (isEngineTestRunnerAssembly ||
                        isEditorTestRunnerAssembly ||
                        isTestAssembly)
                    {
                        continue;
                    }
                }
                var testSuite = (NUnit.Framework.Internal.TestSuite)new NUnit.Framework.Api.DefaultTestAssemblyBuilder().Build(assembly, new Dictionary<string, object>());
                var coroutine = EditorCoroutineUtility.StartCoroutineOwnerless(addTestSuiteToMyTest(testSuite, myTests));
                yield return coroutine;
            }
            setCorrectCheck(myTests);
            AltUnityTesterEditorWindow.EditorConfiguration.MyTests = myTests;
            AltUnityTesterEditorWindow.loadTestCompleted = true;
            AltUnityTesterEditorWindow.Window.Repaint();
        }

        private static void setCorrectCheck(List<AltUnityMyTest> myTests)
        {
            bool classCheck = true;
            bool assemblyCheck = true;
            for (int i = myTests.Count - 1; i >= 0; i--)
            {
                AltUnityMyTest test = myTests[i];
                switch (test.Type.ToString())
                {
                    case "NUnit.Framework.Internal.TestMethod":
                        if (!test.Selected)//test not selected then the class which the test belong must be not selected
                        {
                            classCheck = false;
                        }
                        else
                        {
                            var parentTest = AltUnityTesterEditorWindow.EditorConfiguration.MyTests.FirstOrDefault(a => a.TestName.Equals(test.ParentName));
                            parentTest.TestSelectedCount++;
                        }
                        break;
                    case "NUnit.Framework.Internal.TestFixture":
                        if (classCheck)
                        {
                            test.Selected = true;
                        }
                        else
                        {
                            test.Selected = false;
                            assemblyCheck = false;//class not selected then the assembly which the test belong must be not selected
                        }
                        classCheck = true;//Reset value for new class
                        break;
                    case "NUnit.Framework.Internal.TestAssembly":
                        if (assemblyCheck)
                        {
                            test.Selected = true;
                        }
                        else
                        {
                            test.Selected = false;
                        }
                        assemblyCheck = true;//Reset value for new assembly
                        break;
                }
            }
        }

        private static IEnumerator addTestSuiteToMyTest(NUnit.Framework.Interfaces.ITest testSuite, List<AltUnityMyTest> newMyTests)
        {
            string path = null;

            if (testSuite.GetType() == typeof(NUnit.Framework.Internal.TestMethod))
            {
                string fullName = testSuite.FullName;
                int indexOfParantheses = fullName.IndexOf("(");
                if (indexOfParantheses > -1)
                    fullName = testSuite.FullName.Substring(0, indexOfParantheses);
                var hierarchyNames = fullName.Split('.');
                var className = hierarchyNames[hierarchyNames.Length - 2];
                var assets = UnityEditor.AssetDatabase.FindAssets(className);
                if (assets.Length != 0)
                {
                    path = UnityEditor.AssetDatabase.GUIDToAssetPath(assets[0]);
                }
            }
            var parentName = string.Empty;
            if (testSuite.Parent != null)
                parentName = testSuite.Parent.FullName;
            AltUnityMyTest index = null;
            if (AltUnityTesterEditorWindow.EditorConfiguration.MyTests != null)
                index = AltUnityTesterEditorWindow.EditorConfiguration.MyTests.FirstOrDefault(a => a.TestName.Equals(testSuite.FullName) && a.ParentName.Equals(parentName));
            if (index == null)
            {
                newMyTests.Add(new AltUnityMyTest(false, testSuite.FullName, 0, testSuite.IsSuite, testSuite.GetType().ToString(),
                    parentName, testSuite.TestCaseCount, false, null, null, 0, path, 0));
            }
            else
            {
                newMyTests.Add(new AltUnityMyTest(index.Selected, index.TestName, index.Status, index.IsSuite, testSuite.GetType().ToString(),
                   index.ParentName, testSuite.TestCaseCount, index.FoldOut, index.TestResultMessage, index.TestStackTrace, index.TestDuration, path, index.TestSelectedCount));
            }


            foreach (var test in testSuite.Tests)
            {
                var coroutine = EditorCoroutineUtility.StartCoroutineOwnerless(addTestSuiteToMyTest(test, newMyTests));
                yield return coroutine;
            }
        }

        public static void RunTestFromCommandLine()
        {
            var arguments = System.Environment.GetCommandLineArgs();

            bool runAllTests = true;
            var testAssemblyRunner = new NUnit.Framework.Api.NUnitTestAssemblyRunner(new NUnit.Framework.Api.DefaultTestAssemblyBuilder());
            NUnit.Framework.Internal.TestSuite testSuite = null;
            var filter = new NUnit.Framework.Internal.Filters.OrFilter();
            NUnit.Framework.Interfaces.ITestListener listener = new AltUnityTestRunListener(null);
            System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            System.Reflection.Assembly assembly = assemblies.FirstOrDefault(assemblyName => assemblyName.GetName().Name.Equals("Assembly-CSharp-Editor"));
            testAssemblyRunner.Load(assembly, new Dictionary<string, object>());

            foreach (var arg in arguments)
            {
                if (arg.Equals("-testsClass") || arg.Equals("-tests"))
                {
                    runAllTests = false;
                    break;
                }
            }
            AltUnityTesterEditorWindow.InitEditorConfiguration();
            var tests = AltUnityTesterEditorWindow.EditorConfiguration.MyTests;

            if (!runAllTests)
            {
                var ClassToTest = new List<string>();
                var Tests = new List<string>();
                int argumentFound = 0;
                for (int i = 0; i < arguments.Length; i++)
                {
                    if (argumentFound != 0)
                    {
                        if (arguments[i].StartsWith("-"))
                        {
                            argumentFound = 0;
                        }
                        else
                        {
                            switch (argumentFound)
                            {
                                case 1:
                                    ClassToTest.Add(arguments[i]);
                                    break;
                                case 2:
                                    Tests.Add(arguments[i]);
                                    break;
                            }
                        }
                    }
                    if (arguments[i].Equals("-testsClass"))
                    {
                        argumentFound = 1;
                        continue;
                    }
                    if (arguments[i].Equals("-tests"))
                    {
                        argumentFound = 2;
                        continue;
                    }
                }
                foreach (var className in ClassToTest)
                {
                    var classIndex = tests.FindIndex(test => test.TestName.Equals(className));
                    if (classIndex != -1)
                    {
                        var classFoundInList = tests[classIndex];
                        for (int i = 0; i < classFoundInList.TestCaseCount; i++)
                        {
                            filter.Add(new NUnit.Framework.Internal.Filters.FullNameFilter(tests[i + classIndex + 1].TestName));
                        }
                    }
                    else
                    {
                        throw new System.Exception("Class name: " + className + " not found");
                    }

                }
                foreach (var testName in Tests)
                {
                    var classIndex = tests.FindIndex(test => test.TestName.Equals(testName));
                    if (classIndex != -1)
                    {
                        filter.Add(new NUnit.Framework.Internal.Filters.FullNameFilter(tests[classIndex].TestName));
                    }
                    else
                    {
                        throw new System.Exception("Test name: " + testName + " not found");
                    }

                }

            }
            else //NoArgumentsGiven
            {

                testSuite = (NUnit.Framework.Internal.TestSuite)new NUnit.Framework.Api.DefaultTestAssemblyBuilder().Build(assembly, new Dictionary<string, object>());
                foreach (var test in testSuite.Tests)
                    foreach (var t in test.Tests)
                    {
                        filter.Add(new NUnit.Framework.Internal.Filters.FullNameFilter(t.FullName));
                    }
            }
            var result = testAssemblyRunner.Run(listener, filter);
            if (result.FailCount > 0)
            {
                UnityEditor.EditorApplication.Exit(1);
            }
            else
            {
                UnityEditor.EditorApplication.Exit(0);
            }
        }
    }
}
