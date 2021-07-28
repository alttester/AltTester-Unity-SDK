using Altom.AltUnityDriver;
using NUnit.Framework;

public class TestInputActions
{
    private AltUnityDriver altUnityDriver;
    //Before any test it connects with the socket
    [OneTimeSetUp]
    public void SetUp()
    {
        altUnityDriver = new AltUnityDriver();
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