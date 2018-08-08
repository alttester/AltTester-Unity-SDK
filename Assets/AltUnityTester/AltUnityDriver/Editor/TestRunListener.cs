using System;
using NUnit.Framework.Interfaces;

public class TestRunListener : ITestListener
{
    public readonly TestRunDelegate CallRunDelegate;

    public TestRunListener(TestRunDelegate callRunDelegate)
    {
        this.CallRunDelegate = callRunDelegate;
    }

    public void TestStarted(ITest test)
    {
        if (!test.IsSuite)
            CallRunDelegate(test.Name);
    }

    public void TestFinished(ITestResult result)
    {
    }

    public void TestOutput(TestOutput output)
    {
    }
}
