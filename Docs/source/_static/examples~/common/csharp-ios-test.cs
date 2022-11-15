using NUnit.Framework;
using AltTester.AltDriver;

public class MyFirstTest
{
    private AltDriver altDriver;

    [OneTimeSetUp]
    public void SetUp()
    {
        AltPortForwarding.ForwardIos();
        altDriver = new AltDriver();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        altDriver.Stop();
        AltPortForwarding.KillAllIproxyProcess();
    }

    [Test]
    public void TestStartGame()
    {
        altDriver.LoadScene("Scene 2 Draggable Panel");

        altDriver.FindObject(By.NAME, "Close Button").Tap();
        altDriver.FindObject(By.NAME, "Button").Tap();

        var panelElement = altDriver.WaitForObject(By.NAME, "Panel");
        Assert.IsTrue(panelElement.enabled);
    }
}
