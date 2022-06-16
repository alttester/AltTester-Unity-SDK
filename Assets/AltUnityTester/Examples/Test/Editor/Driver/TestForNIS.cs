using System;
using System.Collections.Generic;
using System.Threading;
using Altom.AltUnityDriver;
using NUnit.Framework;

public class TestForNIS
{
    public AltUnityDriver altUnityDriver;
    //Before any test it connects with the socket
    string scene7 = "Assets/AltUnityTester/Examples/Scenes/Scene 7 Drag And Drop NIS.unity";
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
        altUnityDriver.Scroll(300, 0.5f, true);
        Assert.True(player.GetComponentProperty<bool>("AltUnityNIPDebugScript", "wasScrolled", "Assembly-CSharp"));
    }


    [Test]
    public void TestTapElement()
    {
        altUnityDriver.LoadScene(scene8);
        var closeButton = altUnityDriver.FindObject(By.PATH, "//Panel Drag Area/Panel/Close Button");
        closeButton.Tap();
        altUnityDriver.WaitForObjectNotBePresent(By.PATH, "//Panel Drag Area/Panel");
    }

    [Test]
    public void TestTapCoordinates()
    {
        altUnityDriver.LoadScene(scene8);
        var closeButton = altUnityDriver.FindObject(By.PATH, "//Panel Drag Area/Panel/Close Button");
        altUnityDriver.Tap(closeButton.getScreenPosition());
        altUnityDriver.WaitForObjectNotBePresent(By.PATH, "//Panel Drag Area/Panel");
    }

    [Test]
    public void TestScrollElement()
    {
        altUnityDriver.LoadScene(scene9);
        var scrollbar = altUnityDriver.FindObject(By.NAME, "Scrollbar Vertical");
        var scrollbarPosition = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
        altUnityDriver.MoveMouse(altUnityDriver.FindObject(By.NAME, "Scroll View").getScreenPosition(), 0.5f);
        altUnityDriver.Scroll(new AltUnityVector2(-3000, -3000), 0.5f, true);
        var scrollbarPositionFinal = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
        Assert.AreNotEqual(scrollbarPosition, scrollbarPositionFinal);

    }

    [Test]
    public void TestClickElement()
    {
        altUnityDriver.LoadScene(scene11);
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        capsule.Click();
        var counter = capsule.GetComponentProperty<int>("AltUnityExampleNewInputSystem", "jumpCounter", "Assembly-CSharp");
        Assert.AreEqual(1, counter);
    }


    [Test]
    public void TestKeyDownAndKeyUp()
    {
        altUnityDriver.LoadScene(scene10);
        var player = altUnityDriver.FindObject(By.NAME, "Player");
        for (AltUnityKeyCode altUnityKeyCode = AltUnityKeyCode.Backspace; altUnityKeyCode <= AltUnityKeyCode.F12; altUnityKeyCode++) //because F13->F15 is present in KeyCode but not in Key
            keyboardKeyDownAndUp(player, altUnityKeyCode);
        for (AltUnityKeyCode altUnityKeyCode = AltUnityKeyCode.Numlock; altUnityKeyCode <= AltUnityKeyCode.Menu; altUnityKeyCode++)
            keyboardKeyDownAndUp(player, altUnityKeyCode);
        for (AltUnityKeyCode altUnityKeyCode = AltUnityKeyCode.Mouse0; altUnityKeyCode <= AltUnityKeyCode.Mouse4; altUnityKeyCode++)
            if (Enum.IsDefined(typeof(AltUnityKeyCode), altUnityKeyCode))
            {
                altUnityDriver.KeyDown(altUnityKeyCode);
                altUnityDriver.KeyUp(altUnityKeyCode);
                var keyPressed = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "MousePressed", "Assembly-CSharp");
                var keyReleased = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "MouseReleased", "Assembly-CSharp");
                Assert.AreEqual(altUnityKeyCode.ToString(), keyPressed);
                Assert.AreEqual(altUnityKeyCode.ToString(), keyReleased);
            }
        for (AltUnityKeyCode altUnityKeyCode = AltUnityKeyCode.JoystickButton0; altUnityKeyCode <= AltUnityKeyCode.JoystickButton19; altUnityKeyCode++)
            joystickKeyDownAndUp(player, altUnityKeyCode, 1);
        for (AltUnityKeyCode altUnityKeyCode = AltUnityKeyCode.JoystickButton16; altUnityKeyCode <= AltUnityKeyCode.JoystickButton19; altUnityKeyCode++) //for axis.x < 0 and axis.y < 0
            joystickKeyDownAndUp(player, altUnityKeyCode, -1);
    }

    private void keyboardKeyDownAndUp(AltUnityObject player, AltUnityKeyCode altUnityKeyCode)
    {
        if (Enum.IsDefined(typeof(AltUnityKeyCode), altUnityKeyCode))
        {
            altUnityDriver.KeyDown(altUnityKeyCode);
            altUnityDriver.KeyUp(altUnityKeyCode);
            var keyPressed = player.GetComponentProperty<List<int>>("AltUnityNIPDebugScript", "KeyPressed", "Assembly-CSharp");
            var keyReleased = player.GetComponentProperty<List<int>>("AltUnityNIPDebugScript", "KeyReleased", "Assembly-CSharp");
            Assert.Contains((int)altUnityKeyCode, keyPressed);
            Assert.Contains((int)altUnityKeyCode, keyReleased);
        }
    }

    private void joystickKeyDownAndUp(AltUnityObject player, AltUnityKeyCode altUnityKeyCode, float power)
    {
        altUnityDriver.KeyDown(altUnityKeyCode, power);
        altUnityDriver.KeyUp(altUnityKeyCode);
        var keyPressed = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "JoystickPressed", "Assembly-CSharp");
        var keyReleased = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "JoystickReleased", "Assembly-CSharp");
        Assert.AreEqual(altUnityKeyCode.ToString(), keyPressed);
        Assert.AreEqual(altUnityKeyCode.ToString(), keyReleased);
    }

    [Test]
    public void TestPressKey()
    {
        altUnityDriver.LoadScene(scene10);
        var player = altUnityDriver.FindObject(By.NAME, "Player");
        var initialPos = player.GetComponentProperty<AltUnityVector3>("UnityEngine.Transform", "position");
        altUnityDriver.PressKey(AltUnityKeyCode.A);
        var leftPos = player.GetComponentProperty<AltUnityVector3>("UnityEngine.Transform", "position");
        Assert.AreNotEqual(initialPos, leftPos);
        altUnityDriver.PressKey(AltUnityKeyCode.D);
        var rightPos = player.GetComponentProperty<AltUnityVector3>("UnityEngine.Transform", "position");
        Assert.AreNotEqual(leftPos, rightPos);
        altUnityDriver.PressKey(AltUnityKeyCode.W);
        var upPos = player.GetComponentProperty<AltUnityVector3>("UnityEngine.Transform", "position");
        Assert.AreNotEqual(rightPos, upPos);
        altUnityDriver.PressKey(AltUnityKeyCode.S);
        var downPos = player.GetComponentProperty<AltUnityVector3>("UnityEngine.Transform", "position");
        Assert.AreNotEqual(upPos, downPos);

    }

    [Test]
    public void TestPressKeys()
    {
        altUnityDriver.LoadScene(scene10);
        var player = altUnityDriver.FindObject(By.NAME, "Player");
        var initialPos = player.GetComponentProperty<AltUnityVector3>("UnityEngine.Transform", "position");
        AltUnityKeyCode[] keys = {AltUnityKeyCode.W, AltUnityKeyCode.Mouse0};
        altUnityDriver.PressKeys(keys);
        var finalPos = player.GetComponentProperty<AltUnityVector3>("UnityEngine.Transform", "position");
        altUnityDriver.WaitForObject(By.NAME,"SimpleProjectile(Clone)");
        Assert.AreNotEqual(initialPos, finalPos);
    }

    [Test]
    public void TestPressAllKeys()
    {
        altUnityDriver.LoadScene(scene10);

        var player = altUnityDriver.FindObject(By.NAME, "Player");
        for (AltUnityKeyCode altUnityKeyCode = AltUnityKeyCode.Backspace; altUnityKeyCode <= AltUnityKeyCode.F12; altUnityKeyCode++) //because F13->F15 is present in KeyCode but not in Key
            keyboardKeyPress(player, altUnityKeyCode);
        for (AltUnityKeyCode altUnityKeyCode = AltUnityKeyCode.Numlock; altUnityKeyCode <= AltUnityKeyCode.Menu; altUnityKeyCode++)
            keyboardKeyPress(player, altUnityKeyCode);
        for (AltUnityKeyCode altUnityKeyCode = AltUnityKeyCode.Mouse0; altUnityKeyCode <= AltUnityKeyCode.Mouse4; altUnityKeyCode++)
            if (Enum.IsDefined(typeof(AltUnityKeyCode), altUnityKeyCode))
            {
                altUnityDriver.PressKey(altUnityKeyCode);
                var keyPressed = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "MousePressed", "Assembly-CSharp");
                var keyReleased = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "MouseReleased", "Assembly-CSharp");
                Assert.AreEqual(altUnityKeyCode.ToString(), keyPressed);
                Assert.AreEqual(altUnityKeyCode.ToString(), keyReleased);
            }
        for (AltUnityKeyCode altUnityKeyCode = AltUnityKeyCode.JoystickButton0; altUnityKeyCode <= AltUnityKeyCode.JoystickButton19; altUnityKeyCode++)
            joystickKeyPress(player, altUnityKeyCode, 1);
        for (AltUnityKeyCode altUnityKeyCode = AltUnityKeyCode.JoystickButton16; altUnityKeyCode <= AltUnityKeyCode.JoystickButton19; altUnityKeyCode++) //for axis.x < 0 and axis.y < 0
            joystickKeyPress(player, altUnityKeyCode, -1);
    }

    private void keyboardKeyPress(AltUnityObject player, AltUnityKeyCode altUnityKeyCode)
    {
        if (Enum.IsDefined(typeof(AltUnityKeyCode), altUnityKeyCode))
        {
            altUnityDriver.PressKey(altUnityKeyCode);
            var keyPressed = player.GetComponentProperty<List<int>>("AltUnityNIPDebugScript", "KeyPressed", "Assembly-CSharp");
            var keyReleased = player.GetComponentProperty<List<int>>("AltUnityNIPDebugScript", "KeyReleased", "Assembly-CSharp");
            Assert.Contains((int)altUnityKeyCode, keyPressed);
            Assert.Contains((int)altUnityKeyCode, keyReleased);
        }
    }

    private void joystickKeyPress(AltUnityObject player, AltUnityKeyCode altUnityKeyCode, float power)
    {
        altUnityDriver.PressKey(altUnityKeyCode, power);
        var keyPressed = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "JoystickPressed", "Assembly-CSharp");
        var keyReleased = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "JoystickReleased", "Assembly-CSharp");
        Assert.AreEqual(altUnityKeyCode.ToString(), keyPressed);
        Assert.AreEqual(altUnityKeyCode.ToString(), keyReleased);

    }

    [Test]
    public void TestClickCoordinates()
    {
        altUnityDriver.LoadScene(scene11);
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        altUnityDriver.Click(capsule.getScreenPosition());
        altUnityDriver.WaitForObject(By.PATH, "//ActionText[@text=Capsule was clicked!]");
    }

    [Test]
    public void TestTilt()
    {
        altUnityDriver.LoadScene(scene11);
        var cube = altUnityDriver.FindObject(By.NAME, "Cube (1)");
        var initialPosition = cube.getWorldPosition();
        altUnityDriver.Tilt(new AltUnityVector3(5, 0, 5f), 1f);
        Assert.AreNotEqual(initialPosition, altUnityDriver.FindObject(By.NAME, "Cube (1)").getWorldPosition());
        Assert.IsTrue(cube.GetComponentProperty<bool>("AltUnityCubeNIS", "isMoved", "Assembly-CSharp"));
    }

    [Test]
    public void TestSwipe()
    {
        altUnityDriver.LoadScene(scene9);
        var scrollbarPosition = altUnityDriver.FindObject(By.NAME, "Handle").getScreenPosition();
        var button = altUnityDriver.FindObject(By.PATH, "//Scroll View/Viewport/Content/Button (4)");
        altUnityDriver.Swipe(new AltUnityVector2(button.x + 1, button.y + 1), new AltUnityVector2(button.x + 1, button.y + 20), 1);
        var scrollbarPositionFinal = altUnityDriver.FindObject(By.NAME, "Handle").getScreenPosition();
        Assert.AreNotEqual(scrollbarPosition.y, scrollbarPositionFinal.y);
    }

    [Test]
    public void TestMultipointSwipe()
    {
        altUnityDriver.LoadScene(scene7);
        var altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image1");
        var altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box1");
        altUnityDriver.MultipointSwipe(new[] { new AltUnityVector2(altElement1.x, altElement1.y), new AltUnityVector2(altElement2.x, altElement2.y) }, 2);

        altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image1");
        altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box1");
        var altElement3 = altUnityDriver.FindObject(By.NAME, "Drop Box2");
        var positions = new[]
        {
            new AltUnityVector2(altElement1.x, altElement1.y),
            new AltUnityVector2(altElement2.x, altElement2.y),
            new AltUnityVector2(altElement3.x, altElement3.y)
        };

        altUnityDriver.MultipointSwipe(positions, 3);
        var imageSource = altUnityDriver.FindObject(By.NAME, "Drag Image1").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
        var imageSourceDropZone = altUnityDriver.FindObject(By.NAME, "Drop Image").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
        Assert.AreNotEqual(imageSource["name"], imageSourceDropZone["name"]);

        imageSource = altUnityDriver.FindObject(By.NAME, "Drag Image2").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
        imageSourceDropZone = altUnityDriver.FindObject(By.NAME, "Drop").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
        Assert.AreNotEqual(imageSource["name"], imageSourceDropZone["name"]);
    }

    [Test]
    public void TestBeginMoveEndTouch()
    {
        altUnityDriver.LoadScene(scene8);
        var panelToDrag = altUnityDriver.FindObject(By.PATH, "//Panel/Drag Zone");
        var initialPanelPos = panelToDrag.getScreenPosition();
        var fingerId = altUnityDriver.BeginTouch(panelToDrag.getScreenPosition());
        altUnityDriver.MoveTouch(fingerId, new AltUnityVector2(initialPanelPos.x + 200, initialPanelPos.y + 20));
        altUnityDriver.EndTouch(fingerId);
        var finalPanelPos = altUnityDriver.FindObject(By.PATH, "//Panel/Drag Zone").getScreenPosition();
        Assert.AreNotEqual(initialPanelPos, finalPanelPos);
    }

    [Test]
    public void TestCapsuleJumps()
    {
        altUnityDriver.LoadScene(scene11);
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var fingerId = altUnityDriver.BeginTouch(capsule.getScreenPosition());
        altUnityDriver.EndTouch(fingerId);
        var text = capsule.GetComponentProperty<string>("AltUnityExampleNewInputSystem", "actionText.text", "Assembly-CSharp");
        Assert.AreEqual("Capsule was tapped!", text);
    }
}