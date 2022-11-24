using System.Diagnostics;
using System.Linq;
using System.Threading;
using Altom.AltDriver.Logging;
using NUnit.Framework;

namespace Altom.AltDriver.Tests
{
    [Timeout(10000)]
    public class TestForScene2DraggablePanel
    {
        private AltDriver altDriver;

        [OneTimeSetUp]
        public void SetUp()
        {
            altDriver = new AltDriver(host: TestsHelper.GetAltDriverHost(), port: TestsHelper.GetAltDriverPort(), enableLogging: true);
            DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Info);
            DriverLogManager.SetMinLogLevel(AltLogger.Unity, AltLogLevel.Info);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            altDriver.Stop();
        }

        [SetUp]
        public void LoadLevel()
        {
            altDriver.ResetInput();
            altDriver.LoadScene("Scene 2 Draggable Panel");
        }

        [Test]
        [Ignore("Testing")]
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
        [Ignore("Testing")]
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
        [Ignore("Testing")]
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
        [Ignore("Testing")]
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
        [Ignore("Testing")]
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

            Assert.IsTrue(altElements.Count >= 24);
            Assert.IsTrue(altElements.Count <= 25);
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
        [Ignore("Testing")]
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
            Assert.AreEqual(19, altElements.FindIndex(e => e.name == "AltTesterPrefab"));

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
        [Ignore("Testing")]
        public void TestPointerDownFromObject()
        {
            var panel = altDriver.FindObject(By.NAME, "Panel");
            var color1 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "normalColor", "Assembly-CSharp");
            panel.PointerDownFromObject();
            Thread.Sleep(1000);
            var color2 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color1, color2);
        }

        [Test]
        [Ignore("Testing")]
        public void TestPointerUpFromObject()
        {
            var panel = altDriver.FindObject(By.NAME, "Panel");
            var color1 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "normalColor", "Assembly-CSharp");
            panel.PointerDownFromObject();
            Thread.Sleep(1000);
            panel.PointerUpFromObject();
            var color2 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "highlightColor", "Assembly-CSharp");
            Assert.AreEqual(color1, color2);
        }
        [Test]
        [Ignore("Testing")]
        public void TestGetParent()
        {
            var altElement = altDriver.FindObject(By.NAME, "Panel", By.NAME, "Main Camera");
            var altElementParent = altElement.GetParent();
            Assert.AreEqual("Panel Drag Area", altElementParent.name);
        }
        [Test]
        [Ignore("Testing")]
        public void TestGetAllScenesAndElements()
        {
            var altElements = altDriver.GetAllLoadedScenesAndObjects();

            Assert.AreEqual(20, altElements.FindIndex(e => e.name == "DontDestroyOnLoad"));
            altElements = altDriver.GetAllLoadedScenesAndObjects(false);
            Assert.AreEqual(20, altElements.FindIndex(e => e.name == "DontDestroyOnLoad"));
        }
        [Test]
        [Ignore("Testing")]
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
        [Ignore("Testing")]
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