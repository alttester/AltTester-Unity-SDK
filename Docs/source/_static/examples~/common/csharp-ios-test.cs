// On iOS there is no driver-side reverse port forwarding: IProxy cannot set it up.
// Connect over the network instead - see "In case of iOS" in Advanced Usage for the
// personal-hotspot-over-USB workaround. The test code itself is unchanged; only the
// IP configured in the instrumented app differs.
using NUnit.Framework;
using AltTester.AltTesterSDK.Driver;

public class MyFirstTest
{
    private AltDriver altDriver;

    [OneTimeSetUp]
    public void SetUp()
    {
        altDriver = new AltDriver();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        altDriver.Stop();
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
