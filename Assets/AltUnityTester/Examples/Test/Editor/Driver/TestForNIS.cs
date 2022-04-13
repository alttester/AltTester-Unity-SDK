using System;
using System.Threading;
using Altom.AltUnityDriver;
using Altom.AltUnityTester;
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
        Assert.AreEqual(counter, 1);
    }

    [Test]
    public void TestMultiTapElement()
    {
        altUnityDriver.LoadScene(scene11);
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        capsule.Tap(count: 2, interval: 1.0f);
        var counter = capsule.GetComponentProperty<int>("AltUnityExampleNewInputSystem", "jumpCounter", "Assembly-CSharp");
        Assert.AreEqual(counter, 2);
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
        Assert.AreEqual(counter, 1);
    }

    [Test]
    public void TestClickObject()
    {
        altUnityDriver.LoadScene(scene11);
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        capsule.Click();
        Assert.True(capsule.GetComponentProperty<bool>("AltUnityExampleNewInputSystem", "wasClicked", "Assembly-CSharp"));

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
                string mouseControl = null;
                var keyPressed = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp");
                var keyReleased = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyReleased", "Assembly-CSharp");
                foreach (var e in AltUnityKeyMapping.mouseKeyCodeToButtonControl)
                    if ((AltUnityKeyCode)e.Key == altUnityKeyCode)
                        mouseControl = e.Value.displayName;
                Assert.AreEqual(keyPressed, mouseControl);
                Assert.AreEqual(keyReleased, mouseControl);
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
            var keyPressed = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp");
            var keyReleased = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyReleased", "Assembly-CSharp");
            string keyControl = null;
            foreach (var e in AltUnityKeyMapping.StringToKeyCode)
                if ((AltUnityKeyCode)e.Value == altUnityKeyCode)
                {
                    keyControl = AltUnityKeyMapping.StringToKey[e.Key].ToString();
                    break;
                }
            Assert.AreEqual(keyPressed, keyControl);
            Assert.AreEqual(keyReleased, keyControl);
        }
    }

    private void joystickKeyDownAndUp(AltUnityObject player, AltUnityKeyCode altUnityKeyCode, float power)
    {
        altUnityDriver.KeyDown(altUnityKeyCode, power);
        altUnityDriver.KeyUp(altUnityKeyCode);
        var keyPressed = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp");
        var keyReleased = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyReleased", "Assembly-CSharp");
        string joystickControl = null;
        var altUnityKeyMapping = new AltUnityKeyMapping(power);
        foreach (var e in altUnityKeyMapping.joystickKeyCodeToGamepad)
            if ((AltUnityKeyCode)e.Key == altUnityKeyCode)
            {
                joystickControl = e.Value.displayName;
                break;
            }
        Assert.AreEqual(keyPressed, joystickControl);
        Assert.AreEqual(keyReleased, joystickControl);
    }

    [Test]
    public void TestPressKey()
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
                string mouseControl = null;
                var keyPressed = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp");
                var keyReleased = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyReleased", "Assembly-CSharp");
                foreach (var e in AltUnityKeyMapping.mouseKeyCodeToButtonControl)
                    if ((AltUnityKeyCode)e.Key == altUnityKeyCode)
                    {
                        mouseControl = e.Value.displayName;
                        break;
                    }
                Assert.AreEqual(keyPressed, mouseControl);
                Assert.AreEqual(keyReleased, mouseControl);
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
            var keyPressed = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp");
            var keyReleased = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyReleased", "Assembly-CSharp");
            string keyControl = null;
            foreach (var e in AltUnityKeyMapping.StringToKeyCode)
                if ((AltUnityKeyCode)e.Value == altUnityKeyCode)
                {
                    keyControl = AltUnityKeyMapping.StringToKey[e.Key].ToString();
                    break;
                }
            Assert.AreEqual(keyPressed, keyControl);
            Assert.AreEqual(keyReleased, keyControl);
        }
    }

    private void joystickKeyPress(AltUnityObject player, AltUnityKeyCode altUnityKeyCode, float power)
    {
        altUnityDriver.PressKey(altUnityKeyCode, power);
        var keyPressed = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp");
        var keyReleased = player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyReleased", "Assembly-CSharp");
        string joystickControl = null;
        var altUnityKeyMapping = new AltUnityKeyMapping(power);
        foreach (var e in altUnityKeyMapping.joystickKeyCodeToGamepad)
            if ((AltUnityKeyCode)e.Key == altUnityKeyCode)
            {
                joystickControl = e.Value.displayName;
                break;
            }
        Assert.AreEqual(keyPressed, joystickControl);
        Assert.AreEqual(keyReleased, joystickControl);

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
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var initialPosition = capsule.getWorldPosition();
        altUnityDriver.Tilt(new AltUnityVector3(1000, 10, 10), 3f);
        Assert.AreNotEqual(initialPosition, altUnityDriver.FindObject(By.NAME, "Capsule").getWorldPosition());
    }
}