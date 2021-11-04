using Altom.AltUnityDriver;
using NUnit.Framework;
using Altom.AltUnityDriver.Logging;
using Newtonsoft.Json.Linq;

public class AltUnityCommandsTests
{
    private AltUnityDriver altUnityDriver;

    [OneTimeSetUp]
    public void SetUp()
    {
        string portStr = System.Environment.GetEnvironmentVariable("PROXY_PORT");
        int port = 13000;
        if (!string.IsNullOrEmpty(portStr)) port = int.Parse(portStr);
        DriverLogManager.SetMinLogLevel(AltUnityLogger.Console, AltUnityLogLevel.Debug);
        altUnityDriver = new AltUnityDriver(port: port);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        altUnityDriver.SetServerLogging(AltUnityLogger.File, AltUnityLogLevel.Debug);
        altUnityDriver.Stop();
    }

    [Test]
    [Category("WebGLUnsupported")]
    public void TestSetServerLogging()
    {
        var rule = altUnityDriver.CallStaticMethod<dynamic>("Altom.AltUnityTester.Logging.ServerLogManager", "Instance.Configuration.FindRuleByName", new[] { "AltUnityServerFileRule" }, null, "Assembly-CSharp");

        var levels = (JArray)rule["Levels"];
        Assert.AreEqual(5, levels.Count, levels.ToString());

        altUnityDriver.SetServerLogging(AltUnityLogger.File, AltUnityLogLevel.Off);
        rule = altUnityDriver.CallStaticMethod<dynamic>("Altom.AltUnityTester.Logging.ServerLogManager", "Instance.Configuration.FindRuleByName", new[] { "AltUnityServerFileRule" }, null, "Assembly-CSharp");
        levels = (JArray)rule["Levels"];
        Assert.AreEqual(0, levels.Count, levels.ToString());
    }
}