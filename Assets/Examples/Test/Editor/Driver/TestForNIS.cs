/*
    Copyright(C) 2025 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Threading;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Tests;
using NUnit.Framework;

namespace AltTester.AltTesterSDK.Driver.Tests
{
    [TestFixture]
    [Parallelizable]
    public class TestForNIS
    {
        public AltDriver altDriver;
        //Before any test it connects with the socket
        string scene7 = "Assets/Examples/Scenes/Scene 7 Drag And Drop NIS.unity";
        string scene8 = "Assets/Examples/Scenes/Scene 8 Draggable Panel NIP.unity";
        string scene9 = "Assets/Examples/Scenes/scene 9 NIS.unity";
        string scene10 = "Assets/Examples/Scenes/Scene 10 Sample NIS.unity";
        string scene11 = "Assets/Examples/Scenes/Scene 7 New Input System Actions.unity";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            altDriver = TestsHelper.GetAltDriver();
        }

        //At the end of the test closes the connection with the socket
        [OneTimeTearDown]
        public void TearDown()
        {
            altDriver.Stop();
        }
        [SetUp]
        public void SetUp()
        {
            altDriver.ResetInput();
        }
        private void getSpriteName(out string imageSource, out string imageSourceDropZone, string sourceImageName, string imageSourceDropZoneName)
        {
            imageSource = altDriver.FindObject(By.NAME, sourceImageName).GetComponentProperty<string>("UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI");
            imageSourceDropZone = altDriver.FindObject(By.NAME, imageSourceDropZoneName).GetComponentProperty<string>("UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI");
        }
        private void dropImageWithMultipointSwipe(string[] objectNames, float duration = 1f, bool wait = true)
        {
            AltVector2[] listPositions = new AltVector2[objectNames.Length];
            for (int i = 0; i < objectNames.Length; i++)
            {
                var obj = altDriver.FindObject(By.NAME, objectNames[i]);
                listPositions[i] = obj.GetScreenPosition();
            }
            altDriver.MultipointSwipe(listPositions, duration, wait: wait);
        }

        [Test]
        public void TestScroll()
        {
            altDriver.LoadScene(scene10);
            var player = altDriver.FindObject(By.NAME, "Player");
            Assert.False(player.GetComponentProperty<bool>("AltNIPDebugScript", "wasScrolled", "Assembly-CSharp"));
            altDriver.Scroll(300, 0.5f, true);
            Assert.True(player.GetComponentProperty<bool>("AltNIPDebugScript", "wasScrolled", "Assembly-CSharp"));
        }


        [Test]
        public void TestTapElement()
        {
            altDriver.LoadScene(scene8);
            var closeButton = altDriver.FindObject(By.PATH, "//Panel Drag Area/Panel/Close Button");
            closeButton.Tap();
            altDriver.WaitForObjectNotBePresent(By.PATH, "//Panel Drag Area/Panel");
        }

        [Test]
        public void TestTapCoordinates()
        {
            altDriver.LoadScene(scene8);
            var closeButton = altDriver.FindObject(By.PATH, "//Panel Drag Area/Panel/Close Button");
            altDriver.Tap(closeButton.GetScreenPosition());
            altDriver.WaitForObjectNotBePresent(By.PATH, "//Panel Drag Area/Panel");
        }

        [Test]
        public void TestScrollElement()
        {
            altDriver.LoadScene(scene9);
            var scrollbar = altDriver.FindObject(By.NAME, "Scrollbar Vertical");
            var scrollbarPosition = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
            altDriver.MoveMouse(altDriver.FindObject(By.NAME, "Scroll View").GetScreenPosition(), 0.5f);
            altDriver.Scroll(new AltVector2(-3000, -3000), 0.5f, true);
            var scrollbarPositionFinal = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
            Assert.AreNotEqual(scrollbarPosition, scrollbarPositionFinal);

        }

        [Test]
        public void TestClickElement()
        {
            altDriver.LoadScene(scene11);
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            capsule.Click();
            var counter = capsule.GetComponentProperty<int>("AltExampleNewInputSystem", "jumpCounter", "Assembly-CSharp");
            Assert.AreEqual(1, counter);
        }


        private void keyboardKeyDownAndUp(AltObject player, AltKeyCode altKeyCode)
        {
            if (Enum.IsDefined(typeof(AltKeyCode), altKeyCode))
            {
                altDriver.KeyDown(altKeyCode);
                Thread.Sleep(100);
                altDriver.KeyUp(altKeyCode);
                var keyPressed = player.GetComponentProperty<List<int>>("AltNIPDebugScript", "KeyPressed", "Assembly-CSharp");
                var keyReleased = player.GetComponentProperty<List<int>>("AltNIPDebugScript", "KeyReleased", "Assembly-CSharp");
                Assert.Contains((int)altKeyCode, keyPressed);
                Assert.Contains((int)altKeyCode, keyReleased);
            }
        }

        private void joystickKeyDownAndUp(AltObject player, AltKeyCode altKeyCode, float power)
        {
            altDriver.KeyDown(altKeyCode, power);
            Thread.Sleep(100);
            altDriver.KeyUp(altKeyCode);
            var keyPressed = player.GetComponentProperty<string>("AltNIPDebugScript", "JoystickPressed", "Assembly-CSharp");
            var keyReleased = player.GetComponentProperty<string>("AltNIPDebugScript", "JoystickReleased", "Assembly-CSharp");
            Assert.AreEqual(altKeyCode.ToString(), keyPressed);
            Assert.AreEqual(altKeyCode.ToString(), keyReleased);
        }

        [Test]
        public void TestPressKey()
        {
            altDriver.LoadScene(scene10);
            var player = altDriver.FindObject(By.NAME, "Player");
            var initialPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position", "UnityEngine.CoreModule");
            altDriver.PressKey(AltKeyCode.A);
            var leftPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position", "UnityEngine.CoreModule");
            Assert.AreNotEqual(initialPos, leftPos);
            altDriver.PressKey(AltKeyCode.D);
            var rightPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position", "UnityEngine.CoreModule");
            Assert.AreNotEqual(leftPos, rightPos);
            altDriver.PressKey(AltKeyCode.W);
            var upPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position", "UnityEngine.CoreModule");
            Assert.AreNotEqual(rightPos, upPos);
            altDriver.PressKey(AltKeyCode.S);
            var downPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position", "UnityEngine.CoreModule");
            Assert.AreNotEqual(upPos, downPos);

        }

        [Test]
        public void TestPressKeys()
        {
            altDriver.LoadScene(scene10);
            var player = altDriver.FindObject(By.NAME, "Player");
            var initialPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position", "UnityEngine.CoreModule");
            AltKeyCode[] keys = { AltKeyCode.W, AltKeyCode.Mouse0 };
            altDriver.PressKeys(keys);
            var finalPos = player.GetComponentProperty<AltVector3>("UnityEngine.Transform", "position", "UnityEngine.CoreModule");
            altDriver.WaitForObject(By.NAME, "SimpleProjectile(Clone)");
            Assert.AreNotEqual(initialPos, finalPos);
        }

        [Test]
        public void TestPressAllKeys()
        {
            altDriver.LoadScene(scene10);

            var player = altDriver.FindObject(By.NAME, "Player");
            for (AltKeyCode altKeyCode = AltKeyCode.Backspace; altKeyCode <= AltKeyCode.F12; altKeyCode++) //because F13->F15 is present in KeyCode but not in Key
                keyboardKeyPress(player, altKeyCode);
            for (AltKeyCode altKeyCode = AltKeyCode.Numlock; altKeyCode <= AltKeyCode.Menu; altKeyCode++)
                keyboardKeyPress(player, altKeyCode);
            for (AltKeyCode altKeyCode = AltKeyCode.Mouse0; altKeyCode <= AltKeyCode.Mouse4; altKeyCode++)
                if (Enum.IsDefined(typeof(AltKeyCode), altKeyCode))
                {
                    altDriver.PressKey(altKeyCode);
                    var keyPressed = player.GetComponentProperty<string>("AltNIPDebugScript", "MousePressed", "Assembly-CSharp");
                    var keyReleased = player.GetComponentProperty<string>("AltNIPDebugScript", "MouseReleased", "Assembly-CSharp");
                    Assert.AreEqual(altKeyCode.ToString(), keyPressed);
                    Assert.AreEqual(altKeyCode.ToString(), keyReleased);
                }
        }


        private void keyboardKeyPress(AltObject player, AltKeyCode altKeyCode)
        {
            if (Enum.IsDefined(typeof(AltKeyCode), altKeyCode))
            {
                altDriver.PressKey(altKeyCode);
                var keyPressed = player.GetComponentProperty<List<int>>("AltNIPDebugScript", "KeyPressed", "Assembly-CSharp");
                var keyReleased = player.GetComponentProperty<List<int>>("AltNIPDebugScript", "KeyReleased", "Assembly-CSharp");
                Assert.Contains((int)altKeyCode, keyPressed);
                Assert.Contains((int)altKeyCode, keyReleased);
            }
        }

        private void joystickKeyPress(AltObject player, AltKeyCode altKeyCode, float power)
        {
            altDriver.PressKey(altKeyCode, power);
            var keyPressed = player.GetComponentProperty<string>("AltNIPDebugScript", "JoystickPressed", "Assembly-CSharp");
            var keyReleased = player.GetComponentProperty<string>("AltNIPDebugScript", "JoystickReleased", "Assembly-CSharp");
            Assert.AreEqual(altKeyCode.ToString(), keyPressed);
            Assert.AreEqual(altKeyCode.ToString(), keyReleased);

        }

        [Test]
        public void TestClickCoordinates()
        {
            altDriver.LoadScene(scene11);
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            altDriver.Click(capsule.GetScreenPosition());
            altDriver.WaitForObject(By.PATH, "//ActionText[@text=Capsule was clicked!]", timeout: 1);
        }


        [Test]
        public void TestTilt()
        {
            altDriver.LoadScene(scene11);
            var cube = altDriver.FindObject(By.NAME, "Cube (1)");
            var initialPosition = cube.GetWorldPosition();
            altDriver.Tilt(new AltVector3(5, 0, 5f), 1f);
            Assert.AreNotEqual(initialPosition, altDriver.FindObject(By.NAME, "Cube (1)").GetWorldPosition());
            Assert.IsTrue(cube.GetComponentProperty<bool>("AltCubeNIS", "isMoved", "Assembly-CSharp"));
        }

        [Test]
        public void TestSwipe()
        {
            altDriver.LoadScene(scene9);
            var scrollbarPosition = altDriver.FindObject(By.NAME, "Handle").GetScreenPosition();
            var button = altDriver.FindObject(By.PATH, "//Scroll View/Viewport/Content/Button (4)");
            altDriver.Swipe(new AltVector2(button.x + 1, button.y + 1), new AltVector2(button.x + 1, button.y + 20), 1);
            var scrollbarPositionFinal = altDriver.FindObject(By.NAME, "Handle").GetScreenPosition();
            Assert.AreNotEqual(scrollbarPosition.y, scrollbarPositionFinal.y);
        }

        [Test]
        public void TestMultipointSwipe()
        {
            altDriver.LoadScene(scene7);
            string imageSource, imageSourceDropZone;
            dropImageWithMultipointSwipe(new[] { "Drag Image1", "Drop Box1" });
            dropImageWithMultipointSwipe(new[] { "Drag Image2", "Drop Box1", "Drop Box2" });

            getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image1", "Drop Image");
            Assert.AreEqual(imageSource, imageSourceDropZone);

            getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image2", "Drop");
            Assert.AreEqual(imageSource, imageSourceDropZone);
        }
        [Test]
        public void TestKeyDownAndKeyUp()
        {
            altDriver.LoadScene(scene8);
            var panelToDrag = altDriver.FindObject(By.PATH, "//Panel/Drag Zone");
            var initialPanelPos = panelToDrag.GetScreenPosition();
            altDriver.KeyDown(AltKeyCode.Mouse0);
            altDriver.MoveMouse(new AltVector2(initialPanelPos.x + 1, initialPanelPos.y + 1), 0.1f);
            altDriver.MoveMouse(new AltVector2(initialPanelPos.x + 200, initialPanelPos.y + 20), 0.1f);
            altDriver.KeyUp(AltKeyCode.Mouse0);
            var finalPanelPos = altDriver.FindObject(By.PATH, "//Panel/Drag Zone").GetScreenPosition();
            Assert.AreNotEqual(initialPanelPos, finalPanelPos);
        }

        [Test]
        public void TestBeginMoveEndTouch()
        {
            altDriver.LoadScene(scene8);
            var panelToDrag = altDriver.FindObject(By.PATH, "//Panel/Drag Zone");
            var initialPanelPos = panelToDrag.GetScreenPosition();
            var fingerId = altDriver.BeginTouch(initialPanelPos);
            altDriver.MoveTouch(fingerId, new AltVector2(initialPanelPos.x + 1, initialPanelPos.y + 1));
            altDriver.MoveTouch(fingerId, new AltVector2(initialPanelPos.x + 200, initialPanelPos.y + 20));
            altDriver.EndTouch(fingerId);
            var finalPanelPos = altDriver.FindObject(By.PATH, "//Panel/Drag Zone").GetScreenPosition();
            Assert.AreNotEqual(initialPanelPos, finalPanelPos);
        }

        [Test]
        public void TestCapsuleJumps()
        {
            altDriver.LoadScene(scene11);
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var fingerId = altDriver.BeginTouch(capsule.GetScreenPosition());
            altDriver.EndTouch(fingerId);
            var text = capsule.GetComponentProperty<string>("AltExampleNewInputSystem", "actionText.text", "Assembly-CSharp");
            Assert.AreEqual("Capsule was tapped!", text);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [Ignore("Flaky test, needs investigation")]
        public void TestCheckActionDoNotDoubleClick(int numberOfClicks)
        {
            altDriver.LoadScene(scene11);
            float interval = 0.3f;
            altDriver.SetDelayAfterCommand(0.1f);
            var counterButton = altDriver.FindObject(By.NAME, "Canvas/Button");
            var text = altDriver.FindObject(By.NAME, "Canvas/Button/Text");
            counterButton.Click(numberOfClicks, interval);
            Assert.AreEqual(numberOfClicks, int.Parse(text.GetText()));
            counterButton.Tap(numberOfClicks, interval);
            Assert.AreEqual(2 * numberOfClicks, int.Parse(text.GetText()));
            altDriver.Click(counterButton.GetScreenPosition(), numberOfClicks, interval);
            Assert.AreEqual(3 * numberOfClicks, int.Parse(text.GetText()));
            altDriver.Tap(counterButton.GetScreenPosition(), numberOfClicks, interval);
            Assert.AreEqual(4 * numberOfClicks, int.Parse(text.GetText()));
            altDriver.MoveMouse(counterButton.GetScreenPosition(), interval);
            for (int i = 0; i < numberOfClicks; i++)
            {
                altDriver.KeyDown(AltKeyCode.Mouse0);
                altDriver.KeyUp(AltKeyCode.Mouse0);
            }
            Assert.AreEqual(5 * numberOfClicks, int.Parse(text.GetText()));
            for (int i = 0; i < numberOfClicks; i++)
            {
                altDriver.HoldButton(counterButton.GetScreenPosition(), 0.1f);
            }
            Assert.AreEqual(6 * numberOfClicks, int.Parse(text.GetText()));
            altDriver.SetDelayAfterCommand(0);
        }

        [Test]
        public void TestOnScreenInput()
        {
            altDriver.LoadScene(scene10);
            var initialPlayerPos = altDriver.FindObject(By.NAME, "Player").GetWorldPosition();
            altDriver.FindObject(By.NAME, "Button").Tap();
            var playerPos = altDriver.FindObject(By.NAME, "Player").GetWorldPosition();
            Assert.AreNotEqual(initialPlayerPos, playerPos);
            var stick = altDriver.FindObject(By.NAME, "Stick");
            altDriver.Swipe(stick.GetScreenPosition(), stick.GetScreenPosition() * 2, 1);
            var finalPlayerPos = altDriver.FindObject(By.NAME, "Player").GetWorldPosition();
            Assert.AreNotEqual(playerPos, finalPlayerPos);
        }
    }
}
