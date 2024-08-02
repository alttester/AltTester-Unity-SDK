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

using System;
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using AltTester.AltTesterUnitySDK.Driver.MockClasses;
using AltTester.AltTesterUnitySDK.Driver.Notifications;
using AltTester.AltTesterUnitySDK.Driver.Tests;
using NUnit.Framework;

namespace AltTester.AltTesterUnitySDK.Driver.Tests
{
    [TestFixture]
    [Parallelizable]
    public class TestNotification
    {
        private AltDriver altDriver;

        [OneTimeSetUp]
        public void SetUp()
        {

            altDriver = TestsHelper.GetAltDriver();

            INotificationCallbacks notificationCallbacks = new MockNotificationCallBacks();

            altDriver.AddNotificationListener<AltLoadSceneNotificationResultParams>(NotificationType.LOADSCENE, notificationCallbacks.SceneLoadedCallback, true);
            altDriver.AddNotificationListener<String>(NotificationType.UNLOADSCENE, notificationCallbacks.SceneUnloadedCallback, true);
            altDriver.AddNotificationListener<AltLogNotificationResultParams>(NotificationType.LOG, notificationCallbacks.LogCallback, true);
            altDriver.AddNotificationListener<bool>(NotificationType.APPLICATION_PAUSED, notificationCallbacks.ApplicationPausedCallback, true);
            DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Info);
            DriverLogManager.SetMinLogLevel(AltLogger.Unity, AltLogLevel.Info);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            altDriver.RemoveNotificationListener(NotificationType.LOADSCENE);
            altDriver.RemoveNotificationListener(NotificationType.UNLOADSCENE);
            altDriver.RemoveNotificationListener(NotificationType.LOG);
            altDriver.RemoveNotificationListener(NotificationType.APPLICATION_PAUSED);
            altDriver.Stop();
        }

        [SetUp]
        public void LoadLevel()
        {
            altDriver.ResetInput();
            altDriver.LoadScene("Scene 1 AltDriverTestScene", true);
        }

        [Test]
        public void TestLoadSceneNotification()
        {
            waitForNotificationToBeSent(MockNotificationCallBacks.LastSceneLoaded, "Scene 1 AltDriverTestScene", 10);
            Assert.AreEqual("Scene 1 AltDriverTestScene", MockNotificationCallBacks.LastSceneLoaded);
        }

        private void waitForNotificationToBeSent(string lastSceneLoaded, string expectedValue, float timeout)
        {
            while (!lastSceneLoaded.Equals(expectedValue))
            {
                Thread.Sleep(200);
                timeout -= 0.2f;
                if (timeout <= 0)
                    throw new TimeoutException("Notification variable not set to the desired value in time");
            }
        }

        [Test]
        public void TestUnloadSceneNotification()
        {
            altDriver.LoadScene("Scene 2 Draggable Panel", false);
            altDriver.UnloadScene("Scene 2 Draggable Panel");
            waitForNotificationToBeSent(MockNotificationCallBacks.LastSceneUnloaded, "Scene 2 Draggable Panel", 10);
            Assert.AreEqual("Scene 2 Draggable Panel", MockNotificationCallBacks.LastSceneUnloaded);
        }

        [Test]
        public void TestLogNotification()
        {
            StringAssert.Contains("\"commandName\":\"loadScene", MockNotificationCallBacks.LogMessage);
            Assert.AreEqual(AltLogLevel.Debug, MockNotificationCallBacks.LogLevel);
        }

        [Test]
        [Ignore("Testing")]
        public void TestApplicationPaused()
        {
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            altElement.CallComponentMethod<string>("AltTester.AltTesterUnitySDK.Commands.AltRunner", "OnApplicationPause", "Assembly-CSharp", new object[] { true }, new string[] { "System.Boolean" });
            Assert.IsTrue(MockNotificationCallBacks.ApplicationPaused);
        }
    }
}
