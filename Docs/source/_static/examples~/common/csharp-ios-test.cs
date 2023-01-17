using NUnit.Framework;
using Altom.AltDriver;

public class MyFirstTest
{
    private AltDriver altDriver;

    [OneTimeSetUp]
    public void SetUp()
    {
        AltPortForwarding.ForwardIos();
        altDriver = TestsHelper.getAltDriver();
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
