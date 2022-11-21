using System;
using System.Threading;
using AltTester.AltDriver;
using AltTester.AltDriver.Logging;
using AltTester.AltDriver.MockClasses;
using AltTester.AltDriver.Notifications;
using NUnit.Framework;

namespace AltTester.AltDriver.Tests
{
    public class TestNotification
    {
        private AltDriver altDriver;
        [OneTimeSetUp]
        public void SetUp()
        {
            altDriver = new AltDriver(host: TestsHelper.GetAltDriverHost(), port: TestsHelper.GetAltDriverPort(), enableLogging: true);
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
            StringAssert.Contains("\"commandName\":\"loadScene\"", MockNotificationCallBacks.LogMessage);
            Assert.AreEqual(AltLogLevel.Debug, MockNotificationCallBacks.LogLevel);
        }

        // [Test]
        // public void TestApplicationPaused()
        // {
        //     var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
        //     altElement.CallComponentMethod<string>("AltTester.AltRunner", "OnApplicationPause", new object[] { true }, new string[] { "System.Boolean" }, "Assembly-CSharp");
        //     Assert.IsTrue(MockNotificationCallBacks.ApplicationPaused);
        // }
    }
}
