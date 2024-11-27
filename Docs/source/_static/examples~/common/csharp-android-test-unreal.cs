using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.AltReversePortForwarding;
using NUnit.Framework;

public class MyFirstTest
{
    private AltDriver altDriver;

    [OneTimeSetUp]
    public void SetUp()
    {
        AltReversePortForwarding.ReversePortForwardingAndroid();
        altDriver = new AltDriver();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        altDriver.Stop();
        AltReversePortForwarding.RemoveReversePortForwardingAndroid();
    }

    [Test]
    public void TestStartGame()
    {
        altDriver.LoadScene("MainMenu");

        altDriver.FindObject(By.NAME, "Close Button").Click();
        altDriver.FindObject(By.NAME, "Button").Click();

        var panelElement = altDriver.WaitForObject(By.NAME, "Panel");
        Assert.IsTrue(panelElement.enabled);
    }
}
