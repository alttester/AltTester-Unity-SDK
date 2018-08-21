using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CrossPlatformInputGame1 : MonoBehaviour {


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
        altUnityDriver.LoadScene("Scene 4 GameWithMobileSingleStickControls");
    }
    //ForMobileSingleStickControls
    [Test]
    public void PointerDownAndUp()
    {

        AltUnityObject cube = altUnityDriver.FindElement("Cube");
        string localScale = cube.GetComponentProperty("UnityEngine.Transform", "localScale");
        altUnityDriver.FindElement("JumpButton").PointerDownFromObject();
        cube = altUnityDriver.FindElement("Cube");
        string localScaleClick = cube.GetComponentProperty( "UnityEngine.Transform", "localScale");
        Assert.AreNotEqual(localScale, localScaleClick);
        altUnityDriver.FindElement("JumpButton").PointerUpFromObject();
        cube = altUnityDriver.FindElement("Cube");
        string localScaleRelease = cube.GetComponentProperty( "UnityEngine.Transform", "localScale");


        Assert.AreNotEqual(localScaleRelease, localScaleClick);
        Assert.AreEqual(localScale, localScaleRelease);
    }
    //ForMobileSingleStickControls

#if !UNITY_IOS
    [Test]
    public void DragAndRelease()
    {
        AltUnityObject altElement = altUnityDriver.FindElement("Cube");
        string velocityString = altElement.GetComponentProperty( "UnityEngine.Rigidbody2D", "velocity");

        Vector2 velocityStart = JsonConvert.DeserializeObject<Vector2>(velocityString);

        altElement = altUnityDriver.FindElement("MobileJoystick");
        float Xjoystick = altElement.x;
        float Yjoystick = altElement.y;



        altUnityDriver.FindElement("MobileJoystick").DragObject(new Vector2(200, 200));
        Thread.Sleep(100);

         altElement = altUnityDriver.FindElement("MobileJoystick");

        float XjoystickDuringDrag = altElement.x;
        float YjoystickDuringDrag = altElement.y;

        altElement = altUnityDriver.FindElement("Cube");
        velocityString = altElement.GetComponentProperty( "UnityEngine.Rigidbody2D", "velocity");

        Vector2 velocityDuringDrag = JsonConvert.DeserializeObject<Vector2>(velocityString);


        Assert.AreNotEqual(velocityDuringDrag, velocityStart);
        Assert.AreNotEqual(Xjoystick, XjoystickDuringDrag);
        Assert.AreNotEqual(Yjoystick, YjoystickDuringDrag);

        altUnityDriver.FindElement("MobileJoystick").PointerUpFromObject();
        Thread.Sleep(100);

        altElement = altUnityDriver.FindElement("MobileJoystick");


        float XJoystickAfterDrop = altElement.x;
        float YJoystickAfterDrop = altElement.y;
        altElement = altUnityDriver.FindElement("Cube");
        velocityString = altElement.GetComponentProperty( "UnityEngine.Rigidbody2D", "velocity");

        Vector2 velocityAfterDrop = JsonConvert.DeserializeObject<Vector2>(velocityString);

        Assert.AreNotEqual(velocityDuringDrag, velocityAfterDrop);
        Assert.AreNotEqual(XJoystickAfterDrop, XjoystickDuringDrag);
        Assert.AreNotEqual(YJoystickAfterDrop, YjoystickDuringDrag);

        Assert.AreEqual(velocityAfterDrop, velocityStart);
        Assert.AreEqual(Xjoystick, XJoystickAfterDrop);
        Assert.AreEqual(Yjoystick, YJoystickAfterDrop);
    }
#endif   
}
