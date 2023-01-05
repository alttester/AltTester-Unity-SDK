using System.Threading;
using Altom.AltDriver;
using Altom.AltDriver.Logging;
using Altom.AltDriver.Tests;
using NUnit.Framework;

public class TestBase
{
    private AltDriver altDriver;
    string sceneName;

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
        altDriver.LoadScene(sceneName, true);
    }
}
