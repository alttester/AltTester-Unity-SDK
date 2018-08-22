using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
[Timeout(5000)]
public class GameWithCarTiltImport  {

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
        altUnityDriver.LoadScene("Scene 5 GameWithCarTiltControls");
    }
    //ForCarTiltControls
    // [Test]
    // public void Swipe()
    // {

    //     AltUnityObject camera = altUnityDriver.FindElement("Main Camera");
    //     string rotation = camera.GetComponentProperty("UnityEngine.Transform", "rotation");

    //     AltUnityObject altUnityObject = altUnityDriver.FindElement("LookUpAndDownTouchpad");

    //     Vector2 start = new Vector2(altUnityObject.x, altUnityObject.mobileY);
    //     Vector2 end = new Vector2(altUnityObject.x+100, altUnityObject.mobileY);
    //     altUnityDriver.Swipe(start, end, 2);
        
    //     camera = altUnityDriver.FindElement("Main Camera");
    //     Thread.Sleep(1000);
    //     string rotationAfter = camera.GetComponentProperty( "UnityEngine.Transform", "rotation");

    //     Assert.AreNotEqual(rotation, rotationAfter);
    // }
    [Test]
    public void Tilt()
    {

        AltUnityObject cube = altUnityDriver.FindElement("Cube");
        string position = cube.GetComponentProperty("UnityEngine.Transform", "position");

        altUnityDriver.Tilt(new Vector3(2, 2, 2));
        Thread.Sleep(1000);
        altUnityDriver.Tilt(Vector3.zero);
        cube = altUnityDriver.FindElement("Cube");

        string positionAfter = cube.GetComponentProperty( "UnityEngine.Transform", "position");

        Assert.AreNotEqual(position, positionAfter);
    }
    [Test]
    public void Acceleration()
    {

        AltUnityObject cube = altUnityDriver.FindElement("Cube");
        string position = cube.GetComponentProperty("UnityEngine.Transform", "position");


        altUnityDriver.FindElement("Accelerator").PointerDownFromObject();
        Thread.Sleep(1000);
        altUnityDriver.FindElement("Accelerator").PointerUpFromObject();
        cube = altUnityDriver.FindElement("Cube");
        string positionAfter = cube.GetComponentProperty("UnityEngine.Transform", "position");
        Assert.AreNotEqual(position, positionAfter);
    }
    [Test]
    public void HoldButton()
    {

        AltUnityObject cube = altUnityDriver.FindElement("Cube");
        string position = cube.GetComponentProperty("UnityEngine.Transform", "position");


        AltUnityObject accelerator = altUnityDriver.FindElement("Accelerator");
        altUnityDriver.HoldButton(accelerator.getScreenPosition(),1);
        cube = altUnityDriver.FindElement("Cube");
        string positionAfter = cube.GetComponentProperty("UnityEngine.Transform", "position");
        Assert.AreNotEqual(position, positionAfter);
    }

    [Test]
    public void Brake()

    { 
    
        AltUnityObject cube = altUnityDriver.FindElement("Cube");
        string position = cube.GetComponentProperty("UnityEngine.Transform", "position");

        altUnityDriver.FindElement("Brake").PointerDownFromObject();
        Thread.Sleep(1000);
        altUnityDriver.FindElement("Brake").PointerUpFromObject();
        cube = altUnityDriver.FindElement("Cube");
        string positionAfter = cube.GetComponentProperty("UnityEngine.Transform", "position");
        Assert.AreNotEqual(position, positionAfter);
    }
}
