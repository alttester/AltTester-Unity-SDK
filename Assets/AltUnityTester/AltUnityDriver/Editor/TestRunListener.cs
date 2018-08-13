using System;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class TestRunListener : ITestListener {
    public readonly TestRunDelegate CallRunDelegate;

    public TestRunListener(TestRunDelegate callRunDelegate) {
        this.CallRunDelegate = callRunDelegate;
    }

    public void TestStarted(ITest test) {
        if (!test.IsSuite) {
            if (CallRunDelegate != null)
                CallRunDelegate(test.Name);
        }
    }

    public void TestFinished(ITestResult result) {
        if (!result.Test.IsSuite) {
            Debug.Log("==============> TEST " + result.Test.FullName + ": " + result.ResultState.ToString().ToUpper());
            if (result.ResultState != ResultState.Success) {
                Debug.Log("Error Message: " + result.Message);
                Debug.Log(result.StackTrace);
            }
            Debug.Log("======================================================");
        }
    }

    public void TestOutput(TestOutput output) {
    }
}
