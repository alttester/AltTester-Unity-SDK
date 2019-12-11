using Assets.AltUnityTester.AltUnityDriver.UnityStruct;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

public class TestForScene5KeyboardAndMouseInput
{
#pragma warning disable CS0618

    public AltUnityDriver AltUnityDriver;
    //Before any test it connects with the socket
    [OneTimeSetUp]
    public void SetUp()
    {
        AltUnityDriver =new AltUnityDriver();
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        AltUnityDriver.Stop();
    }

    [Test]
    //Test input made with axis
    public void TestMovementCube()
    {
        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
       

        var cube = AltUnityDriver.FindObject(By.NAME,"Player1");
        UnityEngine.Vector3 cubeInitialPostion = new UnityEngine.Vector3(cube.worldX, cube.worldY, cube.worldY);
        AltUnityDriver.PressKey(KeyCode.K,1, 2);
        Thread.Sleep(2000);
        AltUnityDriver.PressKeyAndWait(KeyCode.O, 1,1);

        cube = AltUnityDriver.FindObject(By.NAME,"Player1");
        UnityEngine.Vector3 cubeFinalPosition = new UnityEngine.Vector3(cube.worldX, cube.worldY, cube.worldY);

        Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);


    }

    [Test]
    //Test Keyboard button press
    public void TestCameraMovement()
    {
        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");


        var cube = AltUnityDriver.FindObject(By.NAME,"Player1");
        UnityEngine.Vector3 cubeInitialPostion = new UnityEngine.Vector3(cube.worldX, cube.worldY, cube.worldY);

        AltUnityDriver.PressKey(KeyCode.W,1, 2);
        Thread.Sleep(2000);
        cube = AltUnityDriver.FindObject(By.NAME,"Player1");
        UnityEngine.Vector3 cubeFinalPosition = new UnityEngine.Vector3(cube.worldX, cube.worldY, cube.worldY);

        Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);

    }

    [Test]
    //Testing mouse movement and clicking
    public void TestCreatingStars()
    {
       AltUnityDriver.LoadScene("Scene 5 Keyboard Input");

       var stars = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Star","Player2");
       var pressingpoint1=  AltUnityDriver.FindObjectWhichContains(By.NAME, "PressingPoint1", "Player2");
        Assert.AreEqual(1, stars.Count);

       AltUnityDriver.MoveMouse(new Vector2(pressingpoint1.x, pressingpoint1.y), 1);
       Thread.Sleep(1500);

       AltUnityDriver.PressKey(KeyCode.Mouse0, 0);
       var pressingpoint2=  AltUnityDriver.FindObjectWhichContains(By.NAME, "PressingPoint2", "Player2");
       AltUnityDriver.MoveMouseAndWait(new Vector2(pressingpoint2.x, pressingpoint2.y), 1);
       AltUnityDriver.PressKeyAndWait(KeyCode.Mouse0, 1);

       stars = AltUnityDriver.FindObjectsWhichContain(By.NAME,"Star");
       Assert.AreEqual(3, stars.Count);


    }
    [Test]
    public void TestKeyboardPress()
    {
        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
        var lastKeyDown = AltUnityDriver.FindElement("LastKeyDownValue");
        var lastKeyUp = AltUnityDriver.FindElement("LastKeyUpValue");
        var lastKeyPress = AltUnityDriver.FindElement("LastKeyPressedValue");
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            AltUnityDriver.PressKeyAndWait(kcode,duration:0.2f);
            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(KeyCode), lastKeyDown.GetText(), true)); 
            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(KeyCode), lastKeyUp.GetText(), true));
            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(KeyCode), lastKeyPress.GetText(), true));
        }
    }

    [Test]
    public void TestButton()
    {
        var ButtonNames = new List<String>()
        {
           "Horizontal","Vertical"
        };
        var KeyToPressForButtons = new List<KeyCode>()
        {
            KeyCode.A,KeyCode.W
        };
        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
        var axisName = AltUnityDriver.FindElement("AxisName");
        int i = 0;
        foreach (KeyCode kcode in KeyToPressForButtons)
        {
            AltUnityDriver.PressKeyAndWait(kcode, duration: 0.05f);
            Assert.AreEqual(ButtonNames[i].ToString(), axisName.GetText());
            i++;
        }

    }

    [Test]
    public void TestPowerJoystick()
    {
        var ButtonNames = new List<String>()
        {
           "Horizontal","Vertical"
        };
        var KeyToPressForButtons = new List<KeyCode>()
        {
            KeyCode.D,KeyCode.W
        };
        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
        var axisName = AltUnityDriver.FindElement("AxisName");
        var axisValue = AltUnityDriver.FindElement("AxisValue");
        int i = 0;
        foreach (KeyCode kcode in KeyToPressForButtons)
        {
            AltUnityDriver.PressKeyAndWait(kcode,power:0.5f, duration: 0.1f);
            Assert.AreEqual("0.5", axisValue.GetText());
            Assert.AreEqual(ButtonNames[i].ToString(), axisName.GetText());
            i++;
        }
    }
  
#pragma warning restore CS0618
}