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

    //TODO Test with scroll on an UI element
    /*
    I checked already and it's working with the current implementation but to write a test for it I need to move the mouse to the UI element
    */

    [Test]
    public void TestKeyDownAndKeyUp()
    {
        altUnityDriver.LoadScene(scene10);
        var player = altUnityDriver.FindObject(By.NAME, "Player");
        Assert.AreEqual("no key", player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp"));
        for (AltUnityKeyCode keyCode = AltUnityKeyCode.A; keyCode <= AltUnityKeyCode.Z; keyCode++)
        {
            altUnityDriver.KeyDown(keyCode);
            altUnityDriver.KeyUp(keyCode);
            Assert.AreEqual(keyCode.ToString(), player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp"));
        }
        for (AltUnityKeyCode keyCode = AltUnityKeyCode.F1; keyCode <= AltUnityKeyCode.F12; keyCode++)
        {
            altUnityDriver.KeyDown(keyCode);
            altUnityDriver.KeyUp(keyCode);
            Assert.AreEqual(keyCode.ToString(), player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp"));
        }
        for (AltUnityKeyCode keyCode = AltUnityKeyCode.Alpha0; keyCode <= AltUnityKeyCode.Alpha9; keyCode++)
        {
            altUnityDriver.KeyDown(keyCode);
            altUnityDriver.KeyUp(keyCode);
            string strKeyCode = keyCode.ToString();
            Assert.AreEqual(strKeyCode[strKeyCode.Length - 1].ToString(), player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp"));
        }
        for (AltUnityKeyCode keyCode = AltUnityKeyCode.Backspace; keyCode <= AltUnityKeyCode.Slash; keyCode++)
            if (Enum.IsDefined(typeof(AltUnityKeyCode), keyCode))
            {
                altUnityDriver.KeyDown(keyCode);
                altUnityDriver.KeyUp(keyCode);
                string strKeyCode = keyCode.ToString();
                switch (strKeyCode)
                {
                    case "Clear":
                        strKeyCode = "Delete";
                        break;
                    case "Return":
                        strKeyCode = "Enter";
                        break;
                    case "Pause":
                        strKeyCode = "Pause/Break";
                        break;
                    case "Exclaim":
                        strKeyCode = "1";
                        break;
                    case "Hash":
                        strKeyCode = "3";
                        break;
                    case "Dollar":
                        strKeyCode = "4";
                        break;
                    case "Percent":
                        strKeyCode = "5";
                        break;
                    case "Ampersand":
                        strKeyCode = "7";
                        break;
                    case "Asterisk":
                        strKeyCode = "8";
                        break;
                    case "LeftParen":
                        strKeyCode = "9";
                        break;
                    case "RightParen":
                        strKeyCode = "0";
                        break;
                    case "DoubleQuote":
                        strKeyCode = "'";
                        break;
                    case "Quote":
                        strKeyCode = "'";
                        break;
                    case "Plus":
                        strKeyCode = "Numpad +";
                        break;
                    case "Minus":
                        strKeyCode = "-";
                        break;
                    case "Comma":
                        strKeyCode = ",";
                        break;
                    case "Period":
                        strKeyCode = ".";
                        break;
                    case "Slash":
                        strKeyCode = "/";
                        break;
                }
                Assert.AreEqual(strKeyCode, player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp"));
            }
        for (AltUnityKeyCode keyCode = AltUnityKeyCode.Colon; keyCode <= AltUnityKeyCode.BackQuote; keyCode++)
            if (Enum.IsDefined(typeof(AltUnityKeyCode), keyCode))
            {
                altUnityDriver.KeyDown(keyCode);
                altUnityDriver.KeyUp(keyCode);
                string strKeyCode = keyCode.ToString();
                switch (strKeyCode)
                {
                    case "Colon":
                        strKeyCode = ";";
                        break;
                    case "Semicolon":
                        strKeyCode = ";";
                        break;
                    case "Less":
                        strKeyCode = ",";
                        break;
                    case "Equals":
                        strKeyCode = "Numpad +";
                        break;
                    case "Greater":
                        strKeyCode = ".";
                        break;
                    case "Question":
                        strKeyCode = "/";
                        break;
                    case "At":
                        strKeyCode = "2";
                        break;
                    case "LeftBracket":
                        strKeyCode = "[";
                        break;
                    case "Backslash":
                        strKeyCode = "\\";
                        break;
                    case "RightBracket":
                        strKeyCode = "]";
                        break;
                    case "Caret":
                        strKeyCode = "6";
                        break;
                    case "Underscore":
                        strKeyCode = "-";
                        break;
                    case "BackQuote":
                        strKeyCode = "`";
                        break;
                }
                Assert.AreEqual(strKeyCode, player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp"));
            }
        for (AltUnityKeyCode keyCode = AltUnityKeyCode.LeftCurlyBracket; keyCode <= AltUnityKeyCode.Delete; keyCode++)
            if (Enum.IsDefined(typeof(AltUnityKeyCode), keyCode))
            {
                altUnityDriver.KeyDown(keyCode);
                altUnityDriver.KeyUp(keyCode);
                string strKeyCode = keyCode.ToString();
                switch (strKeyCode)
                {
                    case "LeftCurlyBracket":
                        strKeyCode = "[";
                        break;
                    case "Pipe":
                        strKeyCode = "\\";
                        break;
                    case "RightCurlyBracket":
                        strKeyCode = "]";
                        break;
                    case "Tilde":
                        strKeyCode = "`";
                        break;
                }
                Assert.AreEqual(strKeyCode, player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp"));
            }
        for (AltUnityKeyCode keyCode = AltUnityKeyCode.UpArrow; keyCode <= AltUnityKeyCode.PageDown; keyCode++)
        {
            altUnityDriver.KeyDown(keyCode);
            altUnityDriver.KeyUp(keyCode);
            string strKeyCode = keyCode.ToString();
            switch (strKeyCode)
            {
                case "UpArrow":
                    strKeyCode = "Up Arrow";
                    break;
                case "DownArrow":
                    strKeyCode = "Down Arrow";
                    break;
                case "RightArrow":
                    strKeyCode = "Right Arrow";
                    break;
                case "LeftArrow":
                    strKeyCode = "Left Arrow";
                    break;
                case "PageUp":
                    strKeyCode = "Page Up";
                    break;
                case "PageDown":
                    strKeyCode = "Page Down";
                    break;
            }
            Assert.AreEqual(strKeyCode, player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp"));
        }
        for (AltUnityKeyCode keyCode = AltUnityKeyCode.Numlock; keyCode <= AltUnityKeyCode.Menu; keyCode++)
        {
            if (Enum.IsDefined(typeof(AltUnityKeyCode), keyCode))
            {
                altUnityDriver.KeyDown(keyCode);
                altUnityDriver.KeyUp(keyCode);
                string strKeyCode = keyCode.ToString();
                switch (strKeyCode)
                {
                    case "Numlock":
                        strKeyCode = "Num Lock";
                        break;
                    case "CapsLock":
                        strKeyCode = "Caps Lock";
                        break;
                    case "ScrollLock":
                        strKeyCode = "Scroll Lock";
                        break;
                    case "RightShift":
                        strKeyCode = "Right Shift";
                        break;
                    case "LeftShift":
                        strKeyCode = "Left Shift";
                        break;
                    case "RightControl":
                        strKeyCode = "Right Control";
                        break;
                    case "LeftControl":
                        strKeyCode = "Left Control";
                        break;
                    case "RightAlt":
                        strKeyCode = "Right Alt";
                        break;
                    case "LeftAlt":
                        strKeyCode = "Left Alt";
                        break;
                    case "RightCommand":
                        strKeyCode = "Right System";
                        break;
                    case "LeftCommand":
                        strKeyCode = "Left System";
                        break;
                    case "RightWindows":
                        strKeyCode = "Right System";
                        break;
                    case "LeftWindows":
                        strKeyCode = "Left System";
                        break;
                    case "AltGr":
                        strKeyCode = "Right Alt";
                        break;
                    case "Help":
                        strKeyCode = "F1";
                        break;
                    case "Print":
                        strKeyCode = "Print Screen";
                        break;
                    case "SysReq":
                        strKeyCode = "Print Screen";
                        break;
                    case "Break":
                        strKeyCode = "Delete";
                        break;
                    case "Menu":
                        strKeyCode = "Context Menu";
                        break;
                }
                Assert.AreEqual(strKeyCode, player.GetComponentProperty<string>("AltUnityNIPDebugScript", "keyPressed", "Assembly-CSharp"));
            }
        }
    }

}