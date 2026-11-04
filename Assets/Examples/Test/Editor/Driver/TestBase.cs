/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Logging;
using AltTester.AltTesterSDK.Driver.Tests;
using NUnit.Framework;

public class TestBase
{
    protected AltDriver altDriver;
    protected string sceneName;
    private int defaultCommandResponseTimeout = 5;

    [OneTimeSetUp]
    public void SetUpOneTime()
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
    public virtual void SetUp()
    {
        altDriver.ResetInput();

        altDriver.SetCommandResponseTimeout(defaultCommandResponseTimeout);
        altDriver.LoadScene(this.sceneName, true);
    }
}
