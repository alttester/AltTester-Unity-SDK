using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
[Timeout(5000)]
public class TestForScene2DraggablePanel
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

    [SetUp]
    public void LoadLevel()
    {
        altUnityDriver.LoadScene("Scene 2 Draggable Panel");
    }
    [Test]
    public void ResizePanel()
    {

        var altElement = altUnityDriver.FindElement("Resize Zone");
        var position = new Vector2(altElement.x, altElement.y);
        altUnityDriver.SwipeAndWait(altElement.getScreenPosition(), new Vector2(altElement.x - 200, altElement.y - 200), 2);

        Thread.Sleep(2000);
        altElement = altUnityDriver.FindElement("Resize Zone");
        var position2 = new Vector2(altElement.x, altElement.y);
        Assert.AreNotEqual(position, position2);
    }
    [Test]
    public void MovePanel()
    {

        var altElement = altUnityDriver.FindElement("Drag Zone");
        var position = new Vector2(altElement.x, altElement.y);
        altUnityDriver.Swipe(new Vector2(altElement.x, altElement.y), new Vector2(altElement.x + 200, altElement.y + 200), 2);
        Thread.Sleep(2000);
        altElement = altUnityDriver.FindElement("Drag Zone");
        var position2 = new Vector2(altElement.x, altElement.y);

        Assert.AreNotEqual(position, position2);
    }

    [Test]
    public void ClosePanel()
    {
        altUnityDriver.WaitForElement("Panel Drag Area", timeout:2);
        Assert.IsTrue(altUnityDriver.FindElement("Panel").enabled);
        var altElement = altUnityDriver.FindElement("Close Button");
        altElement.ClickEvent();
        
        altElement = altUnityDriver.FindElement("Button");
        altElement.ClickEvent();
//        Assert.IsTrue(altUnityDriver.FindElement("Panel").enabled);
    }


}
