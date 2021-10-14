using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Logging;
using NUnit.Framework;

public class TestForScene4NoCameras
{
    public AltUnityDriver altUnityDriver;
    //Before any test it connects with the socket
    [OneTimeSetUp]
    public void SetUp()
    {
        string portStr = System.Environment.GetEnvironmentVariable("PROXY_PORT");
        int port = 13000;
        if (!string.IsNullOrEmpty(portStr)) port = int.Parse(portStr);
        altUnityDriver = new AltUnityDriver(port: port, enableLogging: true);
        DriverLogManager.SetMinLogLevel(AltUnityLogger.Console, AltUnityLogLevel.Info);
        DriverLogManager.SetMinLogLevel(AltUnityLogger.Unity, AltUnityLogLevel.Info);
        altUnityDriver.LoadScene("Scene 4 No Cameras");
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        altUnityDriver.Stop();
    }

    [Test]
    public void TestFindElementInASceneWithNoCameras()
    {
        Assert.AreEqual(0, altUnityDriver.GetAllCameras().Count);
        var altObject = altUnityDriver.FindObject(By.NAME, "Plane");
        Assert.AreEqual(0, altObject.worldX, "WorldX was: " + altObject.worldX + " when it should have been 0");
        Assert.AreEqual(0, altObject.worldY, "WorldY was: " + altObject.worldY + " when it should have been 0");
        Assert.AreEqual(0, altObject.worldZ, "WorldZ was: " + altObject.worldZ + " when it should have been 0");
        Assert.AreEqual(-1, altObject.x);
        Assert.AreEqual(-1, altObject.y);
        Assert.AreEqual(-1, altObject.z);
        Assert.AreEqual(-1, altObject.idCamera);
    }

    [Test]
    public void TestFindUIElementInASceneWithNoCameras()
    {
        Assert.AreEqual(0, altUnityDriver.GetAllCameras().Count);
        var altObjects = altUnityDriver.FindObjects(By.PATH, "//*[contains(@name,Button)]", enabled: false);

        foreach (var button in altObjects)
        {
            Assert.AreNotEqual(-1, button.x);
            Assert.AreNotEqual(-1, button.y);
        }
        Assert.AreEqual(1, altObjects.Count);
    }
}