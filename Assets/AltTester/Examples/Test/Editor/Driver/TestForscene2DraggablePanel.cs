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
        private AltDriver altUnityDriver;

        [OneTimeSetUp]
        public void SetUp()
        {
            altUnityDriver = new AltDriver(host: TestsHelper.GetAltDriverHost(), port: TestsHelper.GetAltDriverPort(), enableLogging: true);
            DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Info);
            DriverLogManager.SetMinLogLevel(AltLogger.Unity, AltLogLevel.Info);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            altUnityDriver.Stop();
        }

        [SetUp]
        public void LoadLevel()
        {
            altUnityDriver.LoadScene("Scene 2 Draggable Panel");
        }

        [Test]
        public void TestResizePanel()
        {
            var altElement = altUnityDriver.FindObject(By.NAME, "Resize Zone");
            var position = new AltVector2(altElement.x, altElement.y);
            altUnityDriver.Swipe(altElement.getScreenPosition(), new AltVector2(altElement.x - 200, altElement.y - 200), 2);

            altElement = altUnityDriver.FindObject(By.NAME, "Resize Zone");
            var position2 = new AltVector2(altElement.x, altElement.y);
            Assert.AreNotEqual(position, position2);
        }

        [Test]
        public void TestResizePanelWithMultipointSwipe()
        {
            var altElement = altUnityDriver.FindObject(By.NAME, "Resize Zone");
            var position = new AltVector2(altElement.x, altElement.y);
            var pos = new[]
            {
            altElement.getScreenPosition(),
            new AltVector2(altElement.x - 200, altElement.y - 200),
            new AltVector2(altElement.x - 300, altElement.y - 100),
            new AltVector2(altElement.x - 50, altElement.y - 100),
            new AltVector2(altElement.x - 100, altElement.y - 100)
        };
            altUnityDriver.MultipointSwipe(pos, 4);

            altElement = altUnityDriver.FindObject(By.NAME, "Resize Zone");
            var position2 = new AltVector2(altElement.x, altElement.y);
            Assert.AreNotEqual(position, position2);
        }

        [Test]
        public void TestMovePanel()
        {
            var altElement = altUnityDriver.FindObject(By.NAME, "Drag Zone");
            var position = new AltVector2(altElement.x, altElement.y);
            altUnityDriver.Swipe(new AltVector2(altElement.x, altElement.y), new AltVector2(altElement.x + 200, altElement.y + 200), 2, wait: false);
            Thread.Sleep(2000);
            altElement = altUnityDriver.FindObject(By.NAME, "Drag Zone");
            var position2 = new AltVector2(altElement.x, altElement.y);

            Assert.AreNotEqual(position, position2);
        }

        [Test]
        public void TestClosePanel()
        {
            altUnityDriver.WaitForObject(By.NAME, "Panel Drag Area", timeout: 2);
            Assert.IsTrue(altUnityDriver.FindObject(By.NAME, "Panel").enabled);
            var altElement = altUnityDriver.FindObject(By.NAME, "Close Button");
            altElement.Click();

            altElement = altUnityDriver.FindObject(By.NAME, "Button");
            altElement.Click();
            Assert.IsTrue(altUnityDriver.FindObject(By.NAME, "Panel").enabled);
        }

        [Test]
        public void TestGetAllEnabledElements()
        {
            Thread.Sleep(2000);

            var altElements = altUnityDriver.GetAllElements(enabled: true);
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
        public void TestGetAllElements()
        {
            altUnityDriver.WaitForObject(By.NAME, "EventSystem", timeout: 2);

            var altElements = altUnityDriver.GetAllElements(enabled: false);
            Assert.IsNotEmpty(altElements);

            string listOfElements = "";
            foreach (var element in altElements)
            {
                listOfElements += element.name + "; ";
            }
            Assert.AreEqual(19, altElements.FindIndex(e => e.name == "AltRunnerPrefab"));

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
        public void TestPointerDownFromObject()
        {
            var panel = altUnityDriver.FindObject(By.NAME, "Panel");
            var color1 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "normalColor", "Assembly-CSharp");
            panel.PointerDownFromObject();
            Thread.Sleep(1000);
            var color2 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color1, color2);
        }

        [Test]
        public void TestPointerUpFromObject()
        {
            var panel = altUnityDriver.FindObject(By.NAME, "Panel");
            var color1 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "normalColor", "Assembly-CSharp");
            panel.PointerDownFromObject();
            Thread.Sleep(1000);
            panel.PointerUpFromObject();
            var color2 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "highlightColor", "Assembly-CSharp");
            Assert.AreEqual(color1, color2);
        }
        [Test]
        public void TestGetParent()
        {
            var altElement = altUnityDriver.FindObject(By.NAME, "Panel", By.NAME, "Main Camera");
            var altElementParent = altElement.getParent();
            Assert.AreEqual("Panel Drag Area", altElementParent.name);
        }
        [Test]
        public void TestGetAllScenesAndElements()
        {
            var altElements = altUnityDriver.GetAllLoadedScenesAndObjects();

            Assert.AreEqual(20, altElements.FindIndex(e => e.name == "DontDestroyOnLoad"));
            altElements = altUnityDriver.GetAllLoadedScenesAndObjects(false);
            Assert.AreEqual(20, altElements.FindIndex(e => e.name == "DontDestroyOnLoad"));
        }
        [Test]
        public void TestNewTouchCommands()
        {
            var draggableArea = altUnityDriver.FindObject(By.NAME, "Drag Zone");
            var initialPosition = draggableArea.getScreenPosition();
            int fingerId = altUnityDriver.BeginTouch(draggableArea.getScreenPosition());
            AltVector2 newPosition = new AltVector2(draggableArea.x + 20, draggableArea.y + 10);
            altUnityDriver.MoveTouch(fingerId, newPosition);
            altUnityDriver.EndTouch(fingerId);
            draggableArea = altUnityDriver.FindObject(By.NAME, "Drag Zone");
            Assert.AreNotEqual(initialPosition, draggableArea.getScreenPosition());

        }
        [Test]
        public void TestCreateTouchTwice()
        {
            var draggableArea = altUnityDriver.FindObject(By.NAME, "Drag Zone");
            var initialPosition = draggableArea.getScreenPosition();
            int fingerId = altUnityDriver.BeginTouch(draggableArea.getScreenPosition());
            AltVector2 newPosition = new AltVector2(draggableArea.x + 20, draggableArea.y + 10);
            altUnityDriver.MoveTouch(fingerId, newPosition);
            altUnityDriver.EndTouch(fingerId);
            draggableArea = altUnityDriver.FindObject(By.NAME, "Drag Zone");
            var secondPosition = draggableArea.getScreenPosition();
            Assert.AreNotEqual(initialPosition, secondPosition);

            fingerId = altUnityDriver.BeginTouch(draggableArea.getScreenPosition());
            newPosition = new AltVector2(draggableArea.x + 20, draggableArea.y + 10);
            altUnityDriver.MoveTouch(fingerId, newPosition);
            altUnityDriver.EndTouch(fingerId);
            draggableArea = altUnityDriver.FindObject(By.NAME, "Drag Zone");
            Assert.AreNotEqual(secondPosition, draggableArea.getScreenPosition());

        }
    }
}