using Altom.AltDriver;
using Altom.AltDriver.Logging;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Altom.AltDriver.Tests
{
    public class AltCommandsTests
    {
        private AltDriver altUnityDriver;

        [OneTimeSetUp]
        public void SetUp()
        {
            DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Debug);
            altUnityDriver = new AltDriver(host: TestsHelper.GetAltDriverHost(), port: TestsHelper.GetAltDriverPort());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            altUnityDriver.SetServerLogging(AltLogger.File, AltLogLevel.Debug);
            altUnityDriver.Stop();
        }

        [Test]
        [Category("WebGLUnsupported")]
        public void TestSetServerLogging()
        {
            var rule = altUnityDriver.CallStaticMethod<dynamic>("Altom.AltTester.Logging.ServerLogManager", "Instance.Configuration.FindRuleByName", new[] { "AltServerFileRule" }, null, "Assembly-CSharp");

            var levels = (JArray)rule["Levels"];
            Assert.AreEqual(5, levels.Count, levels.ToString());

            altUnityDriver.SetServerLogging(AltLogger.File, AltLogLevel.Off);
            rule = altUnityDriver.CallStaticMethod<dynamic>("Altom.AltTester.Logging.ServerLogManager", "Instance.Configuration.FindRuleByName", new[] { "AltServerFileRule" }, null, "Assembly-CSharp");
            levels = (JArray)rule["Levels"];
            Assert.AreEqual(0, levels.Count, levels.ToString());
        }
    }
}