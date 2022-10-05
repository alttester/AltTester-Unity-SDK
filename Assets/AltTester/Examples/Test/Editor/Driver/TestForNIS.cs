using System;
using System.Collections.Generic;
using System.Threading;
using Altom.AltDriver;
using NUnit.Framework;

public class TestForNIS
{
    public AltDriver AltDriver;
    //Before any test it connects with the socket
    string scene7 = "Assets/AltTester/Examples/Scenes/Scene 7 Drag And Drop NIS.unity";
    string scene8 = "Assets/AltTester/Examples/Scenes/Scene 8 Draggable Panel NIP.unity";
    string scene9 = "Assets/AltTester/Examples/Scenes/scene 9 NIS.unity";
    string scene10 = "Assets/AltTester/Examples/Scenes/Scene 10 Sample NIS.unity";
    string scene11 = "Assets/AltTester/Examples/Scenes/Scene 7 New Input System Actions.unity";

    [OneTimeSetUp]
    public void SetUp()
    {
        AltDriver = new AltDriver();
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        AltDriver.Stop();
    }
    private void getSpriteName(out string imageSource, out string imageSourceDropZone, string sourceImageName, string imageSourceDropZoneName)
    {
        imageSource = AltDriver.FindObject(By.NAME, sourceImageName).GetComponentProperty<string>("UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI");
        imageSourceDropZone = AltDriver.FindObject(By.NAME, imageSourceDropZoneName).GetComponentProperty<string>("UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI");
    }
    private void dropImageWithMultipointSwipe(string[] objectNames, float duration = 0.1f, bool wait = true)
    {
        AltVector2[] listPositions = new AltVector2[objectNames.Length];
        for (int i = 0; i < objectNames.Length; i++)
        {
            var obj = AltDriver.FindObject(By.NAME, objectNames[i]);
            listPositions[i] = obj.getScreenPosition();
        }
         AltDriver.MultipointSwipe(listPositions, duration, wait: wait);
    }

    [Test]
    public void TestScroll()
    {
        AltDriver.LoadScene(scene10);
        var player = AltDriver.FindObject(By.NAME, "Player");
        Assert.False(player.GetComponentProperty<bool>("AltNIPDebugScript", "wasScrolled", "Assembly-CSharp"));
        AltDriver.Scroll(300, 0.5f, true);
        Assert.True(player.GetComponentProperty<bool>("AltNIPDebugScript", "wasScrolled", "Assembly-CSharp"));
    }


    [Test]
    public void TestTapElement()
    {
        AltDriver.LoadScene(scene8);
        var closeButton = AltDriver.FindObject(By.PATH, "//Panel Drag Area/Panel/Close Button");
        closeButton.Tap();
        AltDriver.WaitForObjectNotBePresent(By.PATH, "//Panel Drag Area/Panel");
    }

    [Test]
    public void TestTapCoordinates()
    {
        AltDriver.LoadScene(scene8);
        var closeButton = AltDriver.FindObject(By.PATH, "//Panel Drag Area/Panel/Close Button");
        AltDriver.Tap(closeButton.getScreenPosition());
        AltDriver.WaitForObjectNotBePresent(By.PATH, "//Panel Drag Area/Panel");
    }

    [Test]
    public void TestScrollElement()
    {
        AltDriver.LoadScene(scene9);
        var scrollbar = AltDriver.FindObject(By.NAME, "Scrollbar Vertical");
        var scrollbarPosition = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
        AltDriver.MoveMouse(AltDriver.FindObject(By.NAME, "Scroll View").getScreenPosition(), 0.5f);
        AltDriver.Scroll(new AltVector2(-3000, -3000), 0.5f, true);
        var scrollbarPositionFinal = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
        Assert.AreNotEqual(scrollbarPosition, scrollbarPositionFinal);

    }

    [Test]
    public void TestClickElement()
    {
        AltDriver.LoadScene(scene11);
        var capsule = AltDriver.FindObject(By.NAME, "Capsule");
        capsule.Click();
        var counter = capsule.GetComponentProperty<int>("AltExampleNewInputSystem", "jumpCounter", "Assembly-CSharp");
        Assert.AreEqual(1, counter);
    }


    [Test]
    public void TestKeyDownAndKeyUp()
    {
        AltDriver.LoadScene(scene10);
        var player = AltDriver.FindObject(By.NAME, "Player");
        for (AltKeyCode AltKeyCode = AltKeyCode.Backspace; AltKeyCode <= AltKeyCode.F12; AltKeyCode++) //because F13->F15 is present in KeyCode but not in Key
            keyboardKeyDownAndUp(player, AltKeyCode);
        for (AltKeyCode AltKeyCode = AltKeyCode.Numlock; AltKeyCode <= AltKeyCode.Menu; AltKeyCode++)
            keyboardKeyDownAndUp(player, AltKeyCode);
        for (AltKeyCode AltKeyCode = AltKeyCode.Mouse0; AltKeyCode <= AltKeyCode.Mouse4; AltKeyCode++)
            if (Enum.IsDefined(typeof(AltKeyCode), AltKeyCode))
            {
                AltDriver.KeyDown(AltKeyCode);
                AltDriver.KeyUp(AltKeyCode);
                var keyPressed = player.GetComponentProperty<string>("AltNIPDebugScript", "MousePressed", "Assembly-CSharp");
                var keyReleased = player.GetComponentProperty<string>("AltNIPDebugScript", "MouseReleased", "Assembly-CSharp");
                Assert.AreEqual(AltKeyCode.ToString(), keyPressed);
                Assert.AreEqual(AltKeyCode.ToString(), keyReleased);
            }
        for (AltKeyCode AltKeyCode = AltKeyCode.JoystickButton0; AltKeyCode <= AltKeyCode.JoystickButton19; AltKeyCode++)
            joystickKeyDownAndUp(player, AltKeyCode, 1);
        for (AltKeyCode AltKeyCode = AltKeyCode.JoystickButton16; AltKeyCode <= AltKeyCode.JoystickButton19; AltKeyCode++) //for axis.x < 0 and axis.y < 0
            joystickKeyDownAndUp(player, AltKeyCode, -1);
    }

    private void keyboardKeyDownAndUp(AltObject player, AltKeyCode AltKeyCode)
    {
        if (Enum.IsDefined(typeof(AltKeyCode), AltKeyCode))
        {
            AltDriver.KeyDown(AltKeyCode);
            AltDriver.KeyUp(AltKeyCode);
            var keyPressed = player.GetComponentProperty<List<int>>("AltNIPDebugScript", "KeyPressed", "Assembly-CSharp");
            var keyReleased = player.GetComponentProperty<List<int>>("AltNIPDebugScript", "KeyReleased", "Assembly-CSharp");
            Assert.Contains((int)AltKeyCode, keyPressed);
            Assert.Contains((int)AltKeyCode, keyReleased);
        }
    }

    private void joystickKeyDownAndUp(AltObject player, AltKeyCode AltKeyCode, float power)
    {
        AltDriver.KeyDown(AltKeyCode, power);
        AltDriver.KeyUp(AltKeyCode);
        var keyPressed = player.GetComponentProperty<string>("AltNIPDebugScript", "JoystickPressed", "Assembly-CSharp");
        var keyReleased = player.GetComponentProperty<string>("AltNIPDebugScript", "JoystickReleased", "Assembly-CSharp");
        Assert.AreEqual(AltKeyCode.ToString(), keyPressed);
        Assert.AreEqual(AltKeyCode.ToString(), keyReleased);
    }

    [Test]
    public void TestPressKey()
    {
        AltDriver.LoadScene(scene10);
        var player = AltDriver.FindObject(By.NAME, "Player");
        var initialPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position");
        AltDriver.PressKey(AltKeyCode.A);
        var leftPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position");
        Assert.AreNotEqual(initialPos, leftPos);
        AltDriver.PressKey(AltKeyCode.D);
        var rightPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position");
        Assert.AreNotEqual(leftPos, rightPos);
        AltDriver.PressKey(AltKeyCode.W);
        var upPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position");
        Assert.AreNotEqual(rightPos, upPos);
        AltDriver.PressKey(AltKeyCode.S);
        var downPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position");
        Assert.AreNotEqual(upPos, downPos);

    }

    [Test]
    public void TestPressKeys()
    {
        AltDriver.LoadScene(scene10);
        var player = AltDriver.FindObject(By.NAME, "Player");
        var initialPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position");
        AltKeyCode[] keys = {AltKeyCode.W, AltKeyCode.Mouse0};
        AltDriver.PressKeys(keys);
        var finalPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position");
        AltDriver.WaitForObject(By.NAME,"SimpleProjectile(Clone)");
        Assert.AreNotEqual(initialPos, finalPos);
    }

    [Test]
    public void TestPressAllKeys()
    {
        AltDriver.LoadScene(scene10);

        var player = AltDriver.FindObject(By.NAME, "Player");
        for (AltKeyCode AltKeyCode = AltKeyCode.Backspace; AltKeyCode <= AltKeyCode.F12; AltKeyCode++) //because F13->F15 is present in KeyCode but not in Key
            keyboardKeyPress(player, AltKeyCode);
        for (AltKeyCode AltKeyCode = AltKeyCode.Numlock; AltKeyCode <= AltKeyCode.Menu; AltKeyCode++)
            keyboardKeyPress(player, AltKeyCode);
        for (AltKeyCode AltKeyCode = AltKeyCode.Mouse0; AltKeyCode <= AltKeyCode.Mouse4; AltKeyCode++)
            if (Enum.IsDefined(typeof(AltKeyCode), AltKeyCode))
            {
                AltDriver.PressKey(AltKeyCode);
                var keyPressed = player.GetComponentProperty<string>("AltNIPDebugScript", "MousePressed", "Assembly-CSharp");
                var keyReleased = player.GetComponentProperty<string>("AltNIPDebugScript", "MouseReleased", "Assembly-CSharp");
                Assert.AreEqual(AltKeyCode.ToString(), keyPressed);
                Assert.AreEqual(AltKeyCode.ToString(), keyReleased);
            }
        for (AltKeyCode AltKeyCode = AltKeyCode.JoystickButton0; AltKeyCode <= AltKeyCode.JoystickButton19; AltKeyCode++)
            joystickKeyPress(player, AltKeyCode, 1);
        for (AltKeyCode AltKeyCode = AltKeyCode.JoystickButton16; AltKeyCode <= AltKeyCode.JoystickButton19; AltKeyCode++) //for axis.x < 0 and axis.y < 0
            joystickKeyPress(player, AltKeyCode, -1);
    }

    private void keyboardKeyPress(AltObject player, AltKeyCode AltKeyCode)
    {
        if (Enum.IsDefined(typeof(AltKeyCode), AltKeyCode))
        {
            AltDriver.PressKey(AltKeyCode);
            var keyPressed = player.GetComponentProperty<List<int>>("AltNIPDebugScript", "KeyPressed", "Assembly-CSharp");
            var keyReleased = player.GetComponentProperty<List<int>>("AltNIPDebugScript", "KeyReleased", "Assembly-CSharp");
            Assert.Contains((int)AltKeyCode, keyPressed);
            Assert.Contains((int)AltKeyCode, keyReleased);
        }
    }

    private void joystickKeyPress(AltObject player, AltKeyCode AltKeyCode, float power)
    {
        AltDriver.PressKey(AltKeyCode, power);
        var keyPressed = player.GetComponentProperty<string>("AltNIPDebugScript", "JoystickPressed", "Assembly-CSharp");
        var keyReleased = player.GetComponentProperty<string>("AltNIPDebugScript", "JoystickReleased", "Assembly-CSharp");
        Assert.AreEqual(AltKeyCode.ToString(), keyPressed);
        Assert.AreEqual(AltKeyCode.ToString(), keyReleased);

    }

    [Test]
    public void TestClickCoordinates()
    {
        AltDriver.LoadScene(scene11);
        var capsule = AltDriver.FindObject(By.NAME, "Capsule");
        AltDriver.Click(capsule.getScreenPosition());
        AltDriver.WaitForObject(By.PATH, "//ActionText[@text=Capsule was clicked!]");
    }

    [Test]
    public void TestTilt()
    {
        AltDriver.LoadScene(scene11);
        var cube = AltDriver.FindObject(By.NAME, "Cube (1)");
        var initialPosition = cube.getWorldPosition();
        AltDriver.Tilt(new AltVector3(5, 0, 5f), 1f);
        Assert.AreNotEqual(initialPosition, AltDriver.FindObject(By.NAME, "Cube (1)").getWorldPosition());
        Assert.IsTrue(cube.GetComponentProperty<bool>("AltCubeNIS", "isMoved", "Assembly-CSharp"));
    }

    [Test]
    public void TestSwipe()
    {
        AltDriver.LoadScene(scene9);
        var scrollbarPosition = AltDriver.FindObject(By.NAME, "Handle").getScreenPosition();
        var button = AltDriver.FindObject(By.PATH, "//Scroll View/Viewport/Content/Button (4)");
        AltDriver.Swipe(new AltVector2(button.x + 1, button.y + 1), new AltVector2(button.x + 1, button.y + 20), 1);
        var scrollbarPositionFinal = AltDriver.FindObject(By.NAME, "Handle").getScreenPosition();
        Assert.AreNotEqual(scrollbarPosition.y, scrollbarPositionFinal.y);
    }

    [Test]
    public void TestMultipointSwipe()
    {
        AltDriver.LoadScene(scene7);
        string imageSource, imageSourceDropZone;
        dropImageWithMultipointSwipe(new[] { "Drag Image1", "Drop Box1" });
        dropImageWithMultipointSwipe(new[] { "Drag Image2", "Drop Box1", "Drop Box2" });

        getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image1", "Drop Image");
        Assert.AreEqual(imageSource, imageSourceDropZone);

        getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image2", "Drop");
        Assert.AreEqual(imageSource, imageSourceDropZone);
    }

    [Test]
    public void TestBeginMoveEndTouch()
    {
        AltDriver.LoadScene(scene8);
        var panelToDrag = AltDriver.FindObject(By.PATH, "//Panel/Drag Zone");
        var initialPanelPos = panelToDrag.getScreenPosition();
        var fingerId = AltDriver.BeginTouch(panelToDrag.getScreenPosition());
        AltDriver.MoveTouch(fingerId, new AltVector2(initialPanelPos.x + 200, initialPanelPos.y + 20));
        AltDriver.EndTouch(fingerId);
        var finalPanelPos = AltDriver.FindObject(By.PATH, "//Panel/Drag Zone").getScreenPosition();
        Assert.AreNotEqual(initialPanelPos, finalPanelPos);
    }

    [Test]
    public void TestCapsuleJumps()
    {
        AltDriver.LoadScene(scene11);
        var capsule = AltDriver.FindObject(By.NAME, "Capsule");
        var fingerId = AltDriver.BeginTouch(capsule.getScreenPosition());
        AltDriver.EndTouch(fingerId);
        var text = capsule.GetComponentProperty<string>("AltExampleNewInputSystem", "actionText.text", "Assembly-CSharp");
        Assert.AreEqual("Capsule was tapped!", text);
    }
}