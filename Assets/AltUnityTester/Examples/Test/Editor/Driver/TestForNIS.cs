using System;
using System.Threading;
using Altom.AltUnityDriver;
using NUnit.Framework;

public class TestForNIS
{
    public AltUnityDriver altUnityDriver;
    //Before any test it connects with the socket
    string scene7 = "Assets/AltUnityTester/Examples/Scenes/Scene 7 Drag And Drop NIS";
    string scene8 = "Assets/AltUnityTester/Examples/Scenes/Scene 8 Draggable Panel NIP.unity";
    string scene9 = "Assets/AltUnityTester/Examples/Scenes/scene 9 NIS.unity";
    string scene10 = "Assets/AltUnityTester/Examples/Scenes/Scene 10 Sample NIS.unity";
    string scene11 = "Assets/AltUnityTester/Examples/Scenes/Scene 7 New Input System Actions.unity";

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
    public void TestScroll()
    {
        altUnityDriver.LoadScene(scene10);
        var player = altUnityDriver.FindObject(By.NAME, "Player");
        Assert.False(player.GetComponentProperty<bool>("AltUnityNIPDebugScript", "wasScrolled", "Assembly-CSharp"));
        altUnityDriver.Scroll(300, 1, true);
        Assert.True(player.GetComponentProperty<bool>("AltUnityNIPDebugScript", "wasScrolled", "Assembly-CSharp"));
    }

    [Test]
    public void TestTapElement()
    {
        altUnityDriver.LoadScene(scene11);
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        capsule.Tap();
        var counter = capsule.GetComponentProperty<int>("AltUnityExampleNewInputSystem", "jumpCounter", "Assembly-CSharp");
        Assert.AreEqual(counter,1);
    }

    [Test]
    public void TestMultiTapElement()
    {
        altUnityDriver.LoadScene(scene11);
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        capsule.Tap(count:2, interval:1.0f);
        var counter = capsule.GetComponentProperty<int>("AltUnityExampleNewInputSystem", "jumpCounter", "Assembly-CSharp");
        Assert.AreEqual(counter,2);
    }

    [Test]
    public void TestTapCoordinates()
    {
        altUnityDriver.LoadScene(scene11);
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        altUnityDriver.Tap(capsule.getScreenPosition());
        altUnityDriver.WaitForObject(By.PATH, "//ActionText[@text=Capsule was tapped!]");
    }

    [Test]
    public void TestScrollElement()
    {
        altUnityDriver.LoadScene(scene9);
        var scrollbar = altUnityDriver.FindObject(By.NAME, "Handle");
        var scrollbarPosition = scrollbar.getScreenPosition();
        altUnityDriver.MoveMouse(scrollbarPosition);
        altUnityDriver.Scroll(300, 1, true);
        var scrollbarFinal = altUnityDriver.FindObject(By.NAME, "Handle");
        var scrollbarPositionFinal = scrollbarFinal.getScreenPosition();        
        Assert.AreNotEqual(scrollbarPosition.y, scrollbarPositionFinal.y);
    }

    [Test]
    public void TestClickElement()
    {
        altUnityDriver.LoadScene(scene11);
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        capsule.Click();
        var counter = capsule.GetComponentProperty<int>("AltUnityExampleNewInputSystem", "jumpCounter", "Assembly-CSharp");
        Assert.AreEqual(counter,1);
    }

    [Test]
    public void TestClickCoordinates()
    {
        altUnityDriver.LoadScene(scene11);
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        altUnityDriver.Click(capsule.getScreenPosition());
        altUnityDriver.WaitForObject(By.PATH, "//ActionText[@text=Capsule was clicked!]");
    }
}