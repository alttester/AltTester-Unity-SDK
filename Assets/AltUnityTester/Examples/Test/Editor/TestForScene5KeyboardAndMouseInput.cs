using Altom.AltUnityDriver;
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
        AltUnityDriver = new AltUnityDriver();
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


        var cube = AltUnityDriver.FindObject(By.NAME, "Player1");
        AltUnityVector3 cubeInitialPostion = new AltUnityVector3(cube.worldX, cube.worldY, cube.worldY);
        AltUnityDriver.PressKey(AltUnityKeyCode.K, 1, 2);
        Thread.Sleep(2000);
        AltUnityDriver.PressKeyAndWait(AltUnityKeyCode.O, 1, 1);

        cube = AltUnityDriver.FindObject(By.NAME, "Player1");
        AltUnityVector3 cubeFinalPosition = new AltUnityVector3(cube.worldX, cube.worldY, cube.worldY);

        Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);


    }

    [Test]
    //Test Keyboard button press
    public void TestCameraMovement()
    {
        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");


        var cube = AltUnityDriver.FindObject(By.NAME, "Player1");
        AltUnityVector3 cubeInitialPostion = new AltUnityVector3(cube.worldX, cube.worldY, cube.worldY);

        AltUnityDriver.PressKey(AltUnityKeyCode.W, 1, 2);
        Thread.Sleep(2000);
        cube = AltUnityDriver.FindObject(By.NAME, "Player1");
        AltUnityVector3 cubeFinalPosition = new AltUnityVector3(cube.worldX, cube.worldY, cube.worldY);

        Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);

    }

    [Test]
    //Testing mouse movement and clicking
    public void TestCreatingStars()
    {
        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");

        var stars = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Star", cameraPath: "Player2");
        var pressingpoint1 = AltUnityDriver.FindObjectWhichContains(By.NAME, "PressingPoint1", cameraValue: "Player2");
        Assert.AreEqual(1, stars.Count);

        AltUnityDriver.MoveMouse(new AltUnityVector2(pressingpoint1.x, pressingpoint1.y), 1);
        Thread.Sleep(1500);

        AltUnityDriver.PressKey(AltUnityKeyCode.Mouse0, 0);
        var pressingpoint2 = AltUnityDriver.FindObjectWhichContains(By.NAME, "PressingPoint2", cameraValue: "Player2");
        AltUnityDriver.MoveMouseAndWait(new AltUnityVector2(pressingpoint2.x, pressingpoint2.y), 1);
        AltUnityDriver.PressKeyAndWait(AltUnityKeyCode.Mouse0, 1);

        stars = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Star");
        Assert.AreEqual(3, stars.Count);


    }
    [Test]
    public void TestKeyboardPress()
    {
        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
        var lastKeyDown = AltUnityDriver.FindObject(By.NAME, "LastKeyDownValue");
        var lastKeyUp = AltUnityDriver.FindObject(By.NAME, "LastKeyUpValue");
        var lastKeyPress = AltUnityDriver.FindObject(By.NAME, "LastKeyPressedValue");
        foreach (AltUnityKeyCode kcode in Enum.GetValues(typeof(AltUnityKeyCode)))
        {
            if (kcode != AltUnityKeyCode.None)
            {
                AltUnityDriver.PressKeyAndWait(kcode, duration: 0.2f);
                Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltUnityKeyCode), lastKeyDown.GetText(), true));
                Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltUnityKeyCode), lastKeyUp.GetText(), true));
                Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltUnityKeyCode), lastKeyPress.GetText(), true));
            }
        }
    }

    [Test]
    public void TestButton()
    {
        var ButtonNames = new List<String>()
        {
           "Horizontal","Vertical"
        };
        var KeyToPressForButtons = new List<AltUnityKeyCode>()
        {
            AltUnityKeyCode.A,AltUnityKeyCode.W
        };
        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
        var axisName = AltUnityDriver.FindObject(By.NAME, "AxisName");
        int i = 0;
        foreach (AltUnityKeyCode kcode in KeyToPressForButtons)
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
        var KeyToPressForButtons = new List<AltUnityKeyCode>()
        {
            AltUnityKeyCode.D,AltUnityKeyCode.W
        };
        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
        var axisName = AltUnityDriver.FindObject(By.NAME, "AxisName");
        var axisValue = AltUnityDriver.FindObject(By.NAME, "AxisValue");
        int i = 0;
        foreach (AltUnityKeyCode kcode in KeyToPressForButtons)
        {
            AltUnityDriver.PressKeyAndWait(kcode, power: 0.5f, duration: 0.1f);
            Assert.AreEqual("0.5", axisValue.GetText());
            Assert.AreEqual(ButtonNames[i].ToString(), axisName.GetText());
            i++;
        }
    }
    [Test]
    public void TestScroll()
    {

        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
        var player2 = AltUnityDriver.FindObject(By.NAME, "Player2");

        AltUnityVector3 cubeInitialPostion = new AltUnityVector3(player2.worldX, player2.worldY, player2.worldY);
        AltUnityDriver.ScrollMouse(4, 2);
        Thread.Sleep(2000);
        player2 = AltUnityDriver.FindObject(By.NAME, "Player2");
        AltUnityVector3 cubeFinalPosition = new AltUnityVector3(player2.worldX, player2.worldY, player2.worldY);

        Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);
    }
    [Test]
    public void TestScrollAndWait()
    {

        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
        var player2 = AltUnityDriver.FindObject(By.NAME, "Player2");
        AltUnityVector3 cubeInitialPostion = new AltUnityVector3(player2.worldX, player2.worldY, player2.worldY);
        AltUnityDriver.ScrollMouseAndWait(4, 2);
        player2 = AltUnityDriver.FindObject(By.NAME, "Player2");
        AltUnityVector3 cubeFinalPosition = new AltUnityVector3(player2.worldX, player2.worldY, player2.worldY);

        Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);
    }

    [Test]
    public void TestCheckShadersSetCorrectlyAfterHighlight()
    {
        var cube = AltUnityDriver.FindObject(By.NAME, "2MaterialCube");
        var count = int.Parse(cube.GetComponentProperty("UnityEngine.Renderer", "materials.Length", "UnityEngine.CoreModule"));
        var shadersName = new List<string>();
        for (int i= 0; i < count; i++)
        {
            shadersName.Add(cube.GetComponentProperty("UnityEngine.Renderer", "materials["+i+"].shader.name", "UnityEngine.CoreModule"));
        }

        AltUnityDriver.GetScreenshot(cube.id,new AltUnityColor(1,1,1),1.1f);
        Thread.Sleep(1000);
        var newShadersName = new List<string>();
        for (int i = 0; i < count; i++)
        {
            newShadersName.Add(cube.GetComponentProperty("UnityEngine.Renderer", "materials[" + i + "].shader.name", "UnityEngine.CoreModule"));
        }
        Assert.AreEqual(newShadersName, shadersName);

    }

#pragma warning restore CS0618
}