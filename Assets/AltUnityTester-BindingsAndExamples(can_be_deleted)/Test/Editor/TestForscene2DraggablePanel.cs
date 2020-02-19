using NUnit.Framework;
using Assets.AltUnityTester.AltUnityDriver.UnityStruct;
using System.Threading;
using System.Linq;
using System.Diagnostics;

[Timeout(5000)]
public class TestForScene2DraggablePanel
{
    private AltUnityDriver altUnityDriver;
    
    [OneTimeSetUp]
    public void SetUp()
    {
        altUnityDriver = new AltUnityDriver(logFlag: true);
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
       var position = new AltUnityVector2(altElement.x, altElement.y);
       altUnityDriver.SwipeAndWait(altElement.getScreenPosition(), new AltUnityVector2(altElement.x - 200, altElement.y - 200), 2);

        Thread.Sleep(2000);
        altElement = altUnityDriver.FindObject(By.NAME, "Resize Zone");
        var position2 = new AltUnityVector2(altElement.x, altElement.y);
        Assert.AreNotEqual(position, position2);
    }
    
    [Test]
    public void ResizePanelWithMultipointSwipe()
    {
        var altElement = altUnityDriver.FindObject(By.NAME,"Resize Zone");
        var position = new AltUnityVector2(altElement.x, altElement.y);
        var pos = new []
        {
            altElement.getScreenPosition(),
            new AltUnityVector2(altElement.x - 200, altElement.y - 200),
            new AltUnityVector2(altElement.x - 300, altElement.y - 100),
            new AltUnityVector2(altElement.x - 50, altElement.y - 100),
            new AltUnityVector2(altElement.x - 100, altElement.y - 100)
        };
        altUnityDriver.MultipointSwipeAndWait(pos, 4);

        Thread.Sleep(4000);

        altElement = altUnityDriver.FindObject(By.NAME,"Resize Zone");
        var position2 = new AltUnityVector2(altElement.x, altElement.y);
        Assert.AreNotEqual(position, position2);
    }
    
    [Test]
    public void MovePanel()
    {
        var altElement = altUnityDriver.FindObject(By.NAME,"Drag Zone");
        var position = new AltUnityVector2(altElement.x, altElement.y);
        altUnityDriver.Swipe(new AltUnityVector2(altElement.x, altElement.y), new AltUnityVector2(altElement.x + 200, altElement.y + 200), 2);
        Thread.Sleep(2000);
        altElement = altUnityDriver.FindObject(By.NAME, "Drag Zone");
        var position2 = new AltUnityVector2(altElement.x, altElement.y);

        Assert.AreNotEqual(position, position2);
    }

    [Test]
    public void ClosePanel()
    {
        altUnityDriver.WaitForObject(By.NAME, "Panel Drag Area", timeout: 2);
        Assert.IsTrue(altUnityDriver.FindObject(By.NAME, "Panel").enabled);
        var altElement = altUnityDriver.FindObject(By.NAME, "Close Button");
        altElement.ClickEvent();

        altElement = altUnityDriver.FindObject(By.NAME, "Button");
        altElement.ClickEvent();
        Assert.IsTrue(altUnityDriver.FindObject(By.NAME,"Panel").enabled);
    }

    [Test]
    public void TestGetAllEnabledElements()
    {
        Thread.Sleep(1000);

        var altElements = altUnityDriver.GetAllElements(enabled: true);
        Assert.IsNotEmpty(altElements);

        string listOfElements = "";
        foreach (var element in altElements)
        {
            listOfElements += element.name + "; ";
        }

        Debug.WriteLine(listOfElements);

        Assert.AreEqual(22, altElements.Count, listOfElements);
        Assert.IsNotNull(altElements.Where(p => p.name == "EventSystem"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Canvas"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Panel Drag Area"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Panel"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Header"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Text"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Drag Zone"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Resize Zone"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Close Button"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Debugging"));
        Assert.IsNotNull(altElements.Where(p => p.name == "SF Scene Elements"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Main Camera"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Background"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Particle System"));
    }
    [Test]
    public void TestGetAllElements()
    {
        Thread.Sleep(1000);

        var altElements = altUnityDriver.GetAllElements(enabled: false);
        Assert.IsNotEmpty(altElements);

        string listOfElements = "";
        foreach (var element in altElements)
        {
            listOfElements += element.name + "; ";
        }

        Debug.WriteLine(listOfElements);


        Assert.AreEqual(27, altElements.Count);
        Assert.IsNotNull(altElements.Where(p => p.name == "EventSystem"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Canvas"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Panel Drag Area"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Panel"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Header"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Text"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Drag Zone"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Resize Zone"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Close Button"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Debugging"));
        Assert.IsNotNull(altElements.Where(p => p.name == "SF Scene Elements"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Main Camera"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Background"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Particle System"));
        Assert.IsNotNull(altElements.Where(p => p.name == "PopUp"));
    }


}
