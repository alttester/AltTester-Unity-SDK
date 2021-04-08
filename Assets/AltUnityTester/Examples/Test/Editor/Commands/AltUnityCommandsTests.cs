using Altom.AltUnityDriver;
using NUnit.Framework;
using Altom.AltUnityDriver.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AltUnityCommandsTests
{
    private AltUnityDriver altUnityDriver;

    [OneTimeSetUp]
    public void SetUp()
    {
        altUnityDriver = new AltUnityDriver();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        altUnityDriver.Stop();
    }

    [Test]
    public void TestSetServerLogging()
    {
        var result = altUnityDriver.CallStaticMethod("Altom.Server.Logging.ServerLogManager", "Instance.Configuration.FindRuleByName", "AltUnityServerFileRule", "", "Assembly-CSharp");
        var rule = JsonConvert.DeserializeObject<dynamic>(result);
        var levels = (JArray)rule["Levels"];
        Assert.AreEqual(5, levels.Count, levels.ToString());

        altUnityDriver.SetServerLogging(AltUnityLogger.File, AltUnityLogLevel.Off);
        result = altUnityDriver.CallStaticMethod("Altom.Server.Logging.ServerLogManager", "Instance.Configuration.FindRuleByName", "AltUnityServerFileRule", "", "Assembly-CSharp");
        rule = JsonConvert.DeserializeObject<dynamic>(result);
        levels = (JArray)rule["Levels"];
        Assert.AreEqual(0, levels.Count, levels.ToString());
    }
}