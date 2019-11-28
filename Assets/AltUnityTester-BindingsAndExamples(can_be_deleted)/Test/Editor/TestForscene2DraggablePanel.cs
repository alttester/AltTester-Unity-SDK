using NUnit.Framework;
using Assets.AltUnityTester.AltUnityDriver.UnityStruct;
using System.Threading;

[Timeout(5000)]
public class TestForScene2DraggablePanel
{


    private AltUnityDriver altUnityDriver;
    [OneTimeSetUp]
    public void SetUp()
    {
        altUnityDriver = new AltUnityDriver(logFlag:true);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        altUnityDriver.Stop();
    }

    [SetUp]
    public void LoadLevel()
    {
        altUnityDriver.LoadScene("Scene 2 Draggable Panel");
    }
    [Test]
    public void ResizePanel()
    {

       var altElement = altUnityDriver.FindObject(By.NAME,"Resize Zone");
       var position = new Vector2(altElement.x, altElement.y);
       altUnityDriver.SwipeAndWait(altElement.getScreenPosition(), new Vector2(altElement.x - 200, altElement.y - 200), 2);

       Thread.Sleep(2000);
       altElement = altUnityDriver.FindObject(By.NAME,"Resize Zone");
       var position2 = new Vector2(altElement.x, altElement.y);
       Assert.AreNotEqual(position, position2);
    }
    [Test]
    public void MovePanel()
    {

        var altElement = altUnityDriver.FindObject(By.NAME,"Drag Zone");
        var position = new Vector2(altElement.x, altElement.y);
        altUnityDriver.Swipe(new Vector2(altElement.x, altElement.y), new Vector2(altElement.x + 200, altElement.y + 200), 2);
        Thread.Sleep(2000);
        altElement = altUnityDriver.FindObject(By.NAME,"Drag Zone");
        var position2 = new Vector2(altElement.x, altElement.y);

        Assert.AreNotEqual(position, position2);
    }

    [Test]
    public void ClosePanel()
    {
        altUnityDriver.WaitForObject(By.NAME,"Panel Drag Area", timeout:2);
        Assert.IsTrue(altUnityDriver.FindObject(By.NAME,"Panel").enabled);
        var altElement = altUnityDriver.FindObject(By.NAME,"Close Button");
        altElement.ClickEvent();
        
        altElement = altUnityDriver.FindObject(By.NAME,"Button");
        altElement.ClickEvent();
//        Assert.IsTrue(altUnityDriver.FindElement("Panel").enabled);
    }


}
