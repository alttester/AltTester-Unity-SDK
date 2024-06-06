/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace AltTester.AltTesterUnitySDK.Driver.Tests
{
    [TestFixture]
    [Parallelizable]
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
            var rule = altDriver.CallStaticMethod<dynamic>("AltTester.AltTesterUnitySDK.Logging.ServerLogManager", "Instance.Configuration.FindRuleByName", "Assembly-CSharp", new[] { "AltServerFileRule" }, null);

            var levels = (JArray)rule["Levels"];
            Assert.AreEqual(5, levels.Count, levels.ToString());

            altDriver.SetServerLogging(AltLogger.File, AltLogLevel.Off);
            rule = altDriver.CallStaticMethod<dynamic>("AltTester.AltTesterUnitySDK.Logging.ServerLogManager", "Instance.Configuration.FindRuleByName", "Assembly-CSharp", new[] { "AltServerFileRule" }, null);
            levels = (JArray)rule["Levels"];
            Assert.AreEqual(0, levels.Count, levels.ToString());
        }
    }
}
