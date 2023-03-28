using System;
using System.Threading;
using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Logging;
using AltTester.AltTesterUnitySdk.Driver.MockClasses;
using AltTester.AltTesterUnitySdk.Driver.Notifications;
using AltTester.AltTesterUnitySdk.Driver.Tests;
using NUnit.Framework;

public class TestNotification
{
    private AltDriver altDriver;
    [OneTimeSetUp]
    public void SetUp()
    {
        string portStr = System.Environment.GetEnvironmentVariable("ALTSERVER_PORT");
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
        // altDriver.LoadScene("Scene 1 AltDriverTestScene", true);
    }
    [Test]
    public void Test()
    {
        Thread.Sleep(2000);
        altDriver.MoveMouse(new AltVector2(490.0625f, 615.9324f), 0.01f);
        altDriver.KeyDown(AltKeyCode.Mouse0, 1);
        altDriver.MoveMouse(new AltVector2(484.9176f, 615.9324f), 0.01f);
        altDriver.KeyUp(AltKeyCode.Mouse0);
        altDriver.KeyDown(AltKeyCode.LeftArrow, 1);
        altDriver.KeyUp(AltKeyCode.LeftArrow);
        altDriver.KeyDown(AltKeyCode.LeftArrow, 1);
        altDriver.KeyUp(AltKeyCode.LeftArrow);
        altDriver.KeyDown(AltKeyCode.RightArrow, 1);
        altDriver.KeyUp(AltKeyCode.RightArrow);
        altDriver.KeyDown(AltKeyCode.RightArrow, 1);
        altDriver.KeyUp(AltKeyCode.RightArrow);
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
        altElement.CallComponentMethod<string>("Altom.AltTester.AltRunner", "OnApplicationPause", "Assembly-CSharp", new object[] { true }, new string[] { "System.Boolean" });
        Assert.IsTrue(MockNotificationCallBacks.ApplicationPaused);
    }
}