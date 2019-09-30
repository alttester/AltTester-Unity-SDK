using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

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
        AltUnityDriver.PressKey(UnityEngine.KeyCode.K,1, 2);
        Thread.Sleep(2000);
        AltUnityDriver.PressKeyAndWait(UnityEngine.KeyCode.O, 1,1);

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

        AltUnityDriver.PressKey(UnityEngine.KeyCode.W,1, 2);
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
       var player = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Player", "Player2");
        Assert.AreEqual(1, stars.Count);

       AltUnityDriver.MoveMouse(new UnityEngine.Vector2(player[0].x, player[0].y+500), 1);
       UnityEngine.Debug.Log(stars[0].x+"  "+stars[0].y);
       Thread.Sleep(1500);

       AltUnityDriver.PressKey(UnityEngine.KeyCode.Mouse0, 0);
       AltUnityDriver.MoveMouseAndWait(new UnityEngine.Vector2(player[0].x, player[0].y-500), 1);
       Thread.Sleep(1500);
       AltUnityDriver.PressKeyAndWait(UnityEngine.KeyCode.Mouse0, 1);

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
            Assert.AreEqual(kcode.ToString(), lastKeyDown.GetText());
            Assert.AreEqual(kcode.ToString(), lastKeyUp.GetText());
            Assert.AreEqual(kcode.ToString(), lastKeyPress.GetText());
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