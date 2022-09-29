using System.Threading;
using Altom.AltDriver;
using Altom.AltDriver.Logging;
using NUnit.Framework;

namespace Altom.AltDriver.Tests
{
    [Timeout(10000)]
    public class TestForScene3DragAndDrop
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
            altUnityDriver.LoadScene("Scene 3 Drag And Drop");
        }

        [Test]
        public void MultipleDragAndDrop()
        {
            var altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image1");
            var altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box1");
            altUnityDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1, wait: false);

            altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image2");
            altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box2");
            altUnityDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 2, wait: false);

            altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image3");
            altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box1");
            altUnityDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 2, wait: false);


            altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image1");
            altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box1");
            altUnityDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 3, wait: false);

            Thread.Sleep(4000);

            var imageSource = altUnityDriver.FindObject(By.NAME, "Drag Image1").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            var imageSourceDropZone = altUnityDriver.FindObject(By.NAME, "Drop Image").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource["name"], imageSourceDropZone["name"]);

            imageSource = altUnityDriver.FindObject(By.NAME, "Drag Image2").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            imageSourceDropZone = altUnityDriver.FindObject(By.NAME, "Drop").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource["name"], imageSourceDropZone["name"]);
        }


        [Test]
        public void MultipleDragAndDropWait()
        {
            var altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image1");
            var altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box1");
            altUnityDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);

            altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image2");
            altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box2");
            altUnityDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);

            altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image3");
            altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box1");
            altUnityDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);


            altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image1");
            altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box1");
            altUnityDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);
            var imageSource = altUnityDriver.FindObject(By.NAME, "Drag Image1").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            var imageSourceDropZone = altUnityDriver.FindObject(By.NAME, "Drop Image").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource["name"], imageSourceDropZone["name"]);

            imageSource = altUnityDriver.FindObject(By.NAME, "Drag Image2").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            imageSourceDropZone = altUnityDriver.FindObject(By.NAME, "Drop").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource["name"], imageSourceDropZone["name"]);
        }


        [Test]
        public void MultipleDragAndDropWaitWithMultipointSwipe()
        {
            var altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image1");
            var altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box1");
            altUnityDriver.MultipointSwipe(new[] { new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y) }, 2, wait: false);
            Thread.Sleep(2000);

            altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image1");
            altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box1");
            var altElement3 = altUnityDriver.FindObject(By.NAME, "Drop Box2");
            var positions = new[]
            {
                new AltVector2(altElement1.x, altElement1.y),
                new AltVector2(altElement2.x, altElement2.y),
                new AltVector2(altElement3.x, altElement3.y)
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
        public void TestPointerEnterAndExit()
        {
            var altElement = altUnityDriver.FindObject(By.NAME, "Drop Image");
            var color1 = altElement.GetComponentProperty<dynamic>("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            altUnityDriver.FindObject(By.NAME, "Drop Image").PointerEnterObject();
            var color2 = altElement.GetComponentProperty<dynamic>("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color1, color2);
            altUnityDriver.FindObject(By.NAME, "Drop Image").PointerExitObject();
            var color3 = altElement.GetComponentProperty<dynamic>("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color3, color2);
            Assert.AreEqual(color1, color3);
        }

        [Test]
        public void TestDragAndDrop()
        {
            var altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image1");
            var altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box1");
            var initDropImage = altUnityDriver.FindObject(By.PATH, "//*/Drop Box1/Drop Image");

            int fingerId = altUnityDriver.BeginTouch(altElement1.getScreenPosition());
            altUnityDriver.MoveTouch(fingerId, altElement2.getScreenPosition());
            altUnityDriver.EndTouch(fingerId);
            var finalDropImage = altUnityDriver.FindObject(By.PATH, "//*/Drop Box1/Drop Image");

            Assert.AreNotEqual(initDropImage, finalDropImage);
        }
    }
}