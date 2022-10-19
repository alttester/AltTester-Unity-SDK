using Altom.AltDriver;
using Altom.AltDriver.Logging;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Altom.AltDriver.Tests
{
    public class AltCommandsTests
    {
        private AltDriver altDriver;

        [OneTimeSetUp]
        public void SetUp()
        {
            DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Debug);
            altDriver = new AltDriver(host: TestsHelper.GetAltDriverHost(), port: TestsHelper.GetAltDriverPort());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            altDriver.SetServerLogging(AltLogger.File, AltLogLevel.Debug);
            altDriver.Stop();
        }

        [Test]
        [Category("WebGLUnsupported")]
        public void TestSetServerLogging()
        {
            var rule = altDriver.CallStaticMethod<dynamic>("Altom.AltTester.Logging.ServerLogManager", "Instance.Configuration.FindRuleByName", "Assembly-CSharp", new[] { "AltServerFileRule" }, null);

            var levels = (JArray)rule["Levels"];
            Assert.AreEqual(5, levels.Count, levels.ToString());

            altDriver.SetServerLogging(AltLogger.File, AltLogLevel.Off);
            rule = altDriver.CallStaticMethod<dynamic>("Altom.AltTester.Logging.ServerLogManager", "Instance.Configuration.FindRuleByName", "Assembly-CSharp", new[] { "AltServerFileRule" }, null);
            levels = (JArray)rule["Levels"];
            Assert.AreEqual(0, levels.Count, levels.ToString());
        }
    }
}