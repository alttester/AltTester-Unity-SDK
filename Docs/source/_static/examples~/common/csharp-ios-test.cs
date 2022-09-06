using NUnit.Framework;
using Altom.AltUnityDriver;

public class MyFirstTest
{
  private AltUnityDriver altUnityDriver;

  [OneTimeSetUp]
  public void SetUp()
  {
    altUnityDriver = new AltUnityDriver();
  }

  [OneTimeTearDown]
  public void TearDown()
  {
    altUnityDriver.Stop();
  }

  [Test]
  public void TestStartGame()
  {
    altUnityDriver.LoadScene("Scene 2 Draggable Panel");

    altUnityDriver.FindObject(By.NAME, "Close Button").Tap();
    altUnityDriver.FindObject(By.NAME, "Button").Tap();

    var panelElement = altUnityDriver.WaitForObject(By.NAME, "Panel");
    Assert.IsTrue(panelElement.enabled);
  }
}
