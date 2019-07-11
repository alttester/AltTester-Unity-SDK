using NUnit.Framework;
using System.Threading;

public class TestScene5
{
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
       

        var cube = AltUnityDriver.FindElement("Player1");
        UnityEngine.Vector3 cubeInitialPostion = new UnityEngine.Vector3(cube.worldX, cube.worldY, cube.worldY);
        AltUnityDriver.PressKey(UnityEngine.KeyCode.K,1, 2);
        Thread.Sleep(2000);
        AltUnityDriver.PressKeyAndWait(UnityEngine.KeyCode.O, 1,1);

        cube = AltUnityDriver.FindElement("Player1");
        UnityEngine.Vector3 cubeFinalPosition = new UnityEngine.Vector3(cube.worldX, cube.worldY, cube.worldY);

        Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);


    }

    [Test]
    //Test Keyboard button press
    public void TestCameraMovement()
    {
        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");


        var cube = AltUnityDriver.FindElement("Player1");
        UnityEngine.Vector3 cubeInitialPostion = new UnityEngine.Vector3(cube.worldX, cube.worldY, cube.worldY);

        AltUnityDriver.PressKey(UnityEngine.KeyCode.W,1, 2);
        Thread.Sleep(2000);
        cube = AltUnityDriver.FindElement("Player1");
        UnityEngine.Vector3 cubeFinalPosition = new UnityEngine.Vector3(cube.worldX, cube.worldY, cube.worldY);

        Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);

    }

    [Test]
    //Testing mouse movement and clicking
    public void TestCreatingStars()
    {
        AltUnityDriver.LoadScene("Scene 5 Keyboard Input");

        var stars = AltUnityDriver.FindElementsWhereNameContains("Star","Player2");
        Assert.AreEqual(1, stars.Count);

        AltUnityDriver.MoveMouse(new UnityEngine.Vector2(stars[0].x,stars[0].y+100), 1);
        UnityEngine.Debug.Log(stars[0].x+"  "+stars[0].y);
        Thread.Sleep(1500);

        AltUnityDriver.PressKey(UnityEngine.KeyCode.Mouse0,1, 0);
        AltUnityDriver.MoveMouseAndWait(new UnityEngine.Vector2(stars[0].x, stars[0].y-100), 1);
        Thread.Sleep(1500);
        AltUnityDriver.PressKeyAndWait(UnityEngine.KeyCode.Mouse0,1, 1);

        stars = AltUnityDriver.FindElementsWhereNameContains("Star");
        Assert.AreEqual(3, stars.Count);


    }

}