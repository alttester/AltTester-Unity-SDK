using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Logging;
using NUnit.Framework;

public class TestInputActions
{
    private AltUnityDriver altUnityDriver;
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
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        altUnityDriver.Stop();
    }

    [Test]
    public void TestScrollMouseAndWait()
    {
        altUnityDriver.LoadScene("Scene6");

        var scrollBar = altUnityDriver.WaitForObject(By.PATH, "//ScrollCanvas//Handle");

        AltUnityVector2 scrollBarInitialPosition = scrollBar.getScreenPosition();
        altUnityDriver.MoveMouseAndWait(scrollBarInitialPosition);
        altUnityDriver.ScrollMouseAndWait(-20, 0.1f);

        scrollBar = altUnityDriver.FindObject(By.PATH, "//ScrollCanvas//Handle");
        AltUnityVector2 scrollBarFinalPosition = scrollBar.getScreenPosition();
        Assert.AreNotEqual(scrollBarInitialPosition.y, scrollBarFinalPosition.y);
    }
}