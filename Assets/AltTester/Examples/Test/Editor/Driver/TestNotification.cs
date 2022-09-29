using System;
using System.Threading;
using Altom.AltDriver;
using Altom.AltDriver.Logging;
using Altom.AltDriver.MockClasses;
using Altom.AltDriver.Notifications;
using NUnit.Framework;

public class TestNotification
{
    private AltDriver altUnityDriver;
    [OneTimeSetUp]
    public void SetUp()
    {
        string portStr = System.Environment.GetEnvironmentVariable("PROXY_PORT");
        int port = 13000;
        if (!string.IsNullOrEmpty(portStr)) port = int.Parse(portStr);
        altUnityDriver = new AltDriver(port: port, enableLogging: true);
        INotificationCallbacks notificationCallbacks = new MockNotificationCallBacks();
        altUnityDriver.AddNotificationListener<AltLoadSceneNotificationResultParams>(NotificationType.LOADSCENE, notificationCallbacks.SceneLoadedCallback, true);
        altUnityDriver.AddNotificationListener<String>(NotificationType.UNLOADSCENE, notificationCallbacks.SceneUnloadedCallback, true);
        altUnityDriver.AddNotificationListener<AltLogNotificationResultParams>(NotificationType.LOG, notificationCallbacks.LogCallback, true);
        altUnityDriver.AddNotificationListener<bool>(NotificationType.APPLICATION_PAUSED, notificationCallbacks.ApplicationPausedCallback, true);
        DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Info);
        DriverLogManager.SetMinLogLevel(AltLogger.Unity, AltLogLevel.Info);
    }
    [OneTimeTearDown]
    public void TearDown()
    {
        altUnityDriver.RemoveNotificationListener(NotificationType.LOADSCENE);
        altUnityDriver.RemoveNotificationListener(NotificationType.UNLOADSCENE);
        altUnityDriver.RemoveNotificationListener(NotificationType.LOG);
        altUnityDriver.RemoveNotificationListener(NotificationType.APPLICATION_PAUSED);
        altUnityDriver.Stop();
    }

    [SetUp]
    public void LoadLevel()
    {

        altUnityDriver.LoadScene("Scene 1 AltDriverTestScene", true);
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
        altUnityDriver.LoadScene("Scene 2 Draggable Panel", false);
        altUnityDriver.UnloadScene("Scene 2 Draggable Panel");
        waitForNotificationToBeSent(MockNotificationCallBacks.LastSceneUnloaded, "Scene 2 Draggable Panel", 10);
        Assert.AreEqual("Scene 2 Draggable Panel", MockNotificationCallBacks.LastSceneUnloaded);
    }

    [Test]
    public void TestLogNotification()
    {
        StringAssert.Contains("\"commandName\":\"loadScene\"", MockNotificationCallBacks.LogMessage);
        Assert.AreEqual(AltLogLevel.Debug, MockNotificationCallBacks.LogLevel);
    }

    [Test]
    public void TestApplicationPaused()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "AltRunnerPrefab");
        altElement.CallComponentMethod<string>("Altom.AltTester.AltRunner", "OnApplicationPause", new object[] { true }, new string[] { "System.Boolean" }, "Assembly-CSharp");
        Assert.IsTrue(MockNotificationCallBacks.ApplicationPaused);
    }
}