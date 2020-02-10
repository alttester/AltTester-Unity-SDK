

public class AltUnityTestRunListener : NUnit.Framework.Interfaces.ITestListener
{
    public readonly TestRunDelegate CallRunDelegate;

    public AltUnityTestRunListener(TestRunDelegate callRunDelegate) {
        this.CallRunDelegate = callRunDelegate;
    }

    public void TestStarted(NUnit.Framework.Interfaces.ITest test) {
        if (!test.IsSuite) {
            if (CallRunDelegate != null)
                CallRunDelegate(test.Name);
        }
    }

    public void TestFinished(NUnit.Framework.Interfaces.ITestResult result) {
        if (!result.Test.IsSuite) {
            UnityEngine.Debug.Log("==============> TEST " + result.Test.FullName + ": " + result.ResultState.ToString().ToUpper());
            if (result.ResultState != NUnit.Framework.Interfaces.ResultState.Success) {
                UnityEngine.Debug.Log("Error Message: " + result.Message);
                UnityEngine.Debug.Log(result.StackTrace);
            }
            UnityEngine.Debug.Log("======================================================");
        }
    }

    public void TestOutput(NUnit.Framework.Interfaces.TestOutput output) {
    }
}
