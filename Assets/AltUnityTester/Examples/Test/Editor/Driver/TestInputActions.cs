using Altom.AltUnityDriver;
using NUnit.Framework;

public class TestInputActions
{
    private AltUnityDriver driver;
    //Before any test it connects with the socket
    [OneTimeSetUp]
    public void SetUp()
    {
        driver = new AltUnityDriver();
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        driver.Stop();
    }

    [Test]
    public void TestScrollMouseAndWait()
    {
        driver.LoadScene("Scene6");

        var scrollBar = driver.WaitForObject(By.PATH, "//ScrollCanvas//Handle");

        AltUnityVector2 scrollBarInitialPosition = scrollBar.getScreenPosition();
        driver.MoveMouseAndWait(scrollBarInitialPosition);
        driver.ScrollMouseAndWait(-20, 0.1f);

        scrollBar = driver.FindObject(By.PATH, "//ScrollCanvas//Handle");
        AltUnityVector2 scrollBarFinalPosition = scrollBar.getScreenPosition();
        Assert.AreNotEqual(scrollBarInitialPosition.y, scrollBarFinalPosition.y);
    }
}