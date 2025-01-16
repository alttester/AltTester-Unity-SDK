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

using System.Diagnostics;
using System.Linq;
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using NUnit.Framework;

namespace AltTester.AltTesterUnitySDK.Driver.Tests
{
    [TestFixture]
    [Parallelizable]
    [Timeout(10000)]
    public class TestForScene2DraggablePanel : TestBase
    {
        public TestForScene2DraggablePanel()
        {
            sceneName = "Scene 2 Draggable Panel";
        }

        [Test]
        public void TestResizePanel()
        {
            var altElement = altDriver.FindObject(By.NAME, "Resize Zone");
            var position = new AltVector2(altElement.x, altElement.y);
            altDriver.Swipe(altElement.GetScreenPosition(), new AltVector2(altElement.x - 200, altElement.y - 200), 2);

            altElement = altDriver.FindObject(By.NAME, "Resize Zone");
            var position2 = new AltVector2(altElement.x, altElement.y);
            Assert.AreNotEqual(position, position2);
        }

        [Test]
        public void TestResizePanelWithMultipointSwipe()
        {
            var altElement = altDriver.FindObject(By.NAME, "Resize Zone");
            var position = new AltVector2(altElement.x, altElement.y);
            var pos = new[]
            {
            altElement.GetScreenPosition(),
            new AltVector2(altElement.x - 200, altElement.y - 200),
            new AltVector2(altElement.x - 300, altElement.y - 100),
            new AltVector2(altElement.x - 50, altElement.y - 100),
            new AltVector2(altElement.x - 100, altElement.y - 100)
        };
            altDriver.MultipointSwipe(pos, 4);

            altElement = altDriver.FindObject(By.NAME, "Resize Zone");
            var position2 = new AltVector2(altElement.x, altElement.y);
            Assert.AreNotEqual(position, position2);
        }

        [Test]
        public void TestMovePanel()
        {
            var altElement = altDriver.FindObject(By.NAME, "Drag Zone");
            var position = new AltVector2(altElement.x, altElement.y);
            altDriver.Swipe(new AltVector2(altElement.x, altElement.y), new AltVector2(altElement.x + 200, altElement.y + 200), 2, wait: false);
            Thread.Sleep(2000);
            altElement = altDriver.FindObject(By.NAME, "Drag Zone");
            var position2 = new AltVector2(altElement.x, altElement.y);

            Assert.AreNotEqual(position, position2);
        }

        [Test]
        public void TestClosePanel()
        {
            altDriver.WaitForObject(By.NAME, "Panel Drag Area", timeout: 2);
            Assert.IsTrue(altDriver.FindObject(By.NAME, "Panel").enabled);
            var altElement = altDriver.FindObject(By.NAME, "Close Button");
            altElement.Click();

            altElement = altDriver.FindObject(By.NAME, "Button");
            altElement.Click();
            Assert.IsTrue(altDriver.FindObject(By.NAME, "Panel").enabled);
        }

        [Test]
        public void TestGetAllEnabledElements()
        {
            Thread.Sleep(2000);

            var altElements = altDriver.GetAllElements(enabled: true);
            Assert.IsNotEmpty(altElements);

            string listOfElements = "";
            foreach (var element in altElements)
            {
                listOfElements += element.name + "; ";
            }

            Assert.IsTrue(altElements.Count >= 22);
            Assert.IsNotNull(altElements.Where(p => p.name == "EventSystem"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Canvas"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Panel Drag Area"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Panel"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Header"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Text"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Drag Zone"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Resize Zone"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Close Button"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Debugging"));
            Assert.IsNotNull(altElements.Where(p => p.name == "SF Scene Elements"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Main Camera"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Background"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Particle System"));
        }
        [Test]
        public void TestGetAllElements()
        {
            altDriver.WaitForObject(By.NAME, "EventSystem", timeout: 2);

            var altElements = altDriver.GetAllElements(enabled: false);
            Assert.IsNotEmpty(altElements);

            string listOfElements = "";
            foreach (var element in altElements)
            {
                listOfElements += element.name + "; ";
            }
            Assert.IsTrue(altElements.Count >= 45, "Number of elements returned: " + altElements.Count);

            Assert.IsNotNull(altElements.Where(p => p.name == "EventSystem"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Canvas"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Panel Drag Area"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Panel"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Header"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Text"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Drag Zone"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Resize Zone"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Close Button"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Debugging"));
            Assert.IsNotNull(altElements.Where(p => p.name == "SF Scene Elements"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Main Camera"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Background"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Particle System"));
            Assert.IsNotNull(altElements.Where(p => p.name == "PopUp"));
        }


        [Test]
        public void TestPointerDown()
        {
            var panel = altDriver.FindObject(By.NAME, "Panel");
            var color1 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "normalColor", "Assembly-CSharp");
            panel.PointerDown();
            Thread.Sleep(1000);
            var color2 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color1, color2);
        }

        [Test]
        public void TestPointerUp()
        {
            var panel = altDriver.FindObject(By.NAME, "Panel");
            var color1 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "normalColor", "Assembly-CSharp");
            panel.PointerDown();
            Thread.Sleep(1000);
            panel.PointerUp();
            var color2 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "highlightColor", "Assembly-CSharp");
            Assert.AreEqual(color1, color2);
        }
        [TestCase("Main Camera", "SF Scene Elements")]
        [TestCase("Background", "SF Scene Elements")]
        [TestCase("Particle System", "SF Scene Elements")]
        [TestCase("Panel Drag Area", "Canvas")]
        [TestCase("Panel", "Panel Drag Area")]
        [TestCase("Drag Zone", "Panel")]
        [TestCase("Close Button/Text", "Close Button")]
        [TestCase("Button/Text", "Button")]
        [TestCase("Debugging", "Canvas")]
        [Test]
        public void TestGetParent(string NameValue, string ParentValue)
        {
            var altElement = altDriver.FindObject(By.NAME, NameValue);
            var altElementParent = altElement.GetParent();
            Assert.AreEqual(ParentValue, altElementParent.name);
        }

        [TestCase("EventSystem")]
        [TestCase("Canvas")]
        [TestCase("SF Scene Elements")]
        public void TestGetNonExistingParent(string NameValue)
        {
            var altElement = altDriver.FindObject(By.NAME, NameValue);
            Assert.Throws<NotFoundException>(() => altElement.GetParent());
        }

        [Test]
        public void TestGetAllScenesAndElements()
        {
            var altElements = altDriver.GetAllLoadedScenesAndObjects();

            Assert.AreEqual(20, altElements.FindIndex(e => e.name == "DontDestroyOnLoad"));
            altElements = altDriver.GetAllLoadedScenesAndObjects(false);
            Assert.AreEqual(20, altElements.FindIndex(e => e.name == "DontDestroyOnLoad"));
        }
        [Test]
        public void TestNewTouchCommands()
        {
            var draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            var initialPosition = draggableArea.GetScreenPosition();
            int fingerId = altDriver.BeginTouch(draggableArea.GetScreenPosition());
            AltVector2 newPosition = new AltVector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.MoveTouch(fingerId, newPosition);
            altDriver.EndTouch(fingerId);
            draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            Assert.AreNotEqual(initialPosition, draggableArea.GetScreenPosition());

        }
        [Test]
        public void TestCreateTouchTwice()
        {
            var draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            var initialPosition = draggableArea.GetScreenPosition();
            int fingerId = altDriver.BeginTouch(draggableArea.GetScreenPosition());
            AltVector2 newPosition = new AltVector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.MoveTouch(fingerId, newPosition);
            altDriver.EndTouch(fingerId);
            draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            var secondPosition = draggableArea.GetScreenPosition();
            Assert.AreNotEqual(initialPosition, secondPosition);

            fingerId = altDriver.BeginTouch(draggableArea.GetScreenPosition());
            newPosition = new AltVector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.MoveTouch(fingerId, newPosition);
            altDriver.EndTouch(fingerId);
            draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            Assert.AreNotEqual(secondPosition, draggableArea.GetScreenPosition());

        }
    }
}
