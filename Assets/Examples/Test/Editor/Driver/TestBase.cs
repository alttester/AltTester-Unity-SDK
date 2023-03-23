using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Logging;
using AltTester.AltTesterUnitySdk.Driver.Tests;
using NUnit.Framework;

public class TestBase
{
    protected AltDriver altDriver;
    protected string sceneName;

    [OneTimeSetUp]
    public void SetUp()
    {
        altDriver = TestsHelper.GetAltDriver();
        DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Info);
        DriverLogManager.SetMinLogLevel(AltLogger.Unity, AltLogLevel.Info);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        altDriver.Stop();
    }

    [SetUp]
    protected void LoadLevel()
    {
        altDriver.ResetInput();

        altDriver.SetCommandResponseTimeout(60);
        altDriver.LoadScene(this.sceneName, true);
    }
}
