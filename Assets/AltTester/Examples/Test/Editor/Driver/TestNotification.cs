using System;
using System.Threading;
using AltTester.AltDriver;
using AltTester.AltDriver.Logging;
using AltTester.AltDriver.MockClasses;
using AltTester.AltDriver.Notifications;
using NUnit.Framework;
using Altom.AltDriver.Tests;

namespace AltTester.AltDriver.Tests
{
    public class TestNotification
    {
        string portStr = System.Environment.GetEnvironmentVariable("PROXY_PORT");
        int port = 13000;
        if (!string.IsNullOrEmpty(portStr)) port = int.Parse(portStr);
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

            // [Test]
            // [Ignore("Testing")]
            // public void TestApplicationPaused()
            // {
            //     var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            //     altElement.CallComponentMethod<string>("Altom.AltTester.AltRunner", "OnApplicationPause", new object[] { true }, new string[] { "System.Boolean" }, "Assembly-CSharp");
            //     Assert.IsTrue(MockNotificationCallBacks.ApplicationPaused);
            // }
        }
