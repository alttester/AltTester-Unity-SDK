using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using UnityEngine;
namespace AltTester.AltTesterUnitySDK
{
    public class AltTestContext : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            TestExecutionContext testExecutionContext = new TestExecutionContext();
            IMethodInfo methodInfo = new MethodWrapper(typeof(TestExample), typeof(TestExample).GetMethod("Test"));
            testExecutionContext.CurrentTest = new TestMethod(methodInfo);
            TestContext testContext = new TestContext(testExecutionContext);
            TestContext.CurrentTestExecutionContext = testExecutionContext;
            Application.runInBackground = true;

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
    public class TestExample
    {
        [Test]
        public void Test()
        {

        }
    }
}
