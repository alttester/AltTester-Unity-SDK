using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Logging;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace AltTester.AltTesterUnitySdk.Driver.Tests
{
    public class AltCommandsTests
    {
        private AltDriver altDriver;

        [OneTimeSetUp]
        public void SetUp()
        {
            DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Debug);
            altDriver = TestsHelper.GetAltDriver();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            altDriver.SetServerLogging(AltLogger.File, AltLogLevel.Debug);
            altDriver.Stop();
        }

        [Test]
        [Category("WebGLUnsupported")] //in WebGL we do not save logs to file but in the console
        public void TestSetServerLogging()
        {
            var rule = altDriver.CallStaticMethod<dynamic>("AltTester.AltTesterUnitySdk.Logging.ServerLogManager", "Instance.Configuration.FindRuleByName", "Assembly-CSharp", new[] { "AltServerFileRule" }, null);

            var levels = (JArray)rule["Levels"];
            Assert.AreEqual(5, levels.Count, levels.ToString());

            altDriver.SetServerLogging(AltLogger.File, AltLogLevel.Off);
            rule = altDriver.CallStaticMethod<dynamic>("AltTester.AltTesterUnitySdk.Logging.ServerLogManager", "Instance.Configuration.FindRuleByName", "Assembly-CSharp", new[] { "AltServerFileRule" }, null);
            levels = (JArray)rule["Levels"];
            Assert.AreEqual(0, levels.Count, levels.ToString());
        }
    }
}