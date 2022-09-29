using System.Threading;
using Altom.AltDriver;
using Altom.AltDriver.Logging;
using NUnit.Framework;

namespace Altom.AltDriver.Tests
{
    [Timeout(10000)]
    public class TestForScene3DragAndDrop
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
            altDriver.LoadScene("Scene 3 Drag And Drop");
        }

        [Test]
        public void MultipleDragAndDrop()
        {
            var altElement1 = altDriver.FindObject(By.NAME, "Drag Image1");
            var altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1, wait: false);

            altElement1 = altDriver.FindObject(By.NAME, "Drag Image2");
            altElement2 = altDriver.FindObject(By.NAME, "Drop Box2");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 2, wait: false);

            altElement1 = altDriver.FindObject(By.NAME, "Drag Image3");
            altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 2, wait: false);


            altElement1 = altDriver.FindObject(By.NAME, "Drag Image1");
            altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 3, wait: false);

            Thread.Sleep(4000);

            var imageSource = altDriver.FindObject(By.NAME, "Drag Image1").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            var imageSourceDropZone = altDriver.FindObject(By.NAME, "Drop Image").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource["name"], imageSourceDropZone["name"]);

            imageSource = altDriver.FindObject(By.NAME, "Drag Image2").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            imageSourceDropZone = altDriver.FindObject(By.NAME, "Drop").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource["name"], imageSourceDropZone["name"]);
        }


        [Test]
        public void MultipleDragAndDropWait()
        {
            var altElement1 = altDriver.FindObject(By.NAME, "Drag Image1");
            var altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);

            altElement1 = altDriver.FindObject(By.NAME, "Drag Image2");
            altElement2 = altDriver.FindObject(By.NAME, "Drop Box2");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);

            altElement1 = altDriver.FindObject(By.NAME, "Drag Image3");
            altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);


            altElement1 = altDriver.FindObject(By.NAME, "Drag Image1");
            altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);
            var imageSource = altDriver.FindObject(By.NAME, "Drag Image1").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            var imageSourceDropZone = altDriver.FindObject(By.NAME, "Drop Image").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource["name"], imageSourceDropZone["name"]);

            imageSource = altDriver.FindObject(By.NAME, "Drag Image2").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            imageSourceDropZone = altDriver.FindObject(By.NAME, "Drop").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource["name"], imageSourceDropZone["name"]);
        }


        [Test]
        public void MultipleDragAndDropWaitWithMultipointSwipe()
        {
            var altElement1 = altDriver.FindObject(By.NAME, "Drag Image1");
            var altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            altDriver.MultipointSwipe(new[] { new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y) }, 2, wait: false);
            Thread.Sleep(2000);

            altElement1 = altDriver.FindObject(By.NAME, "Drag Image1");
            altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            var altElement3 = altDriver.FindObject(By.NAME, "Drop Box2");
            var positions = new[]
            {
                new AltVector2(altElement1.x, altElement1.y),
                new AltVector2(altElement2.x, altElement2.y),
                new AltVector2(altElement3.x, altElement3.y)
            };

            altDriver.MultipointSwipe(positions, 3);
            var imageSource = altDriver.FindObject(By.NAME, "Drag Image1").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            var imageSourceDropZone = altDriver.FindObject(By.NAME, "Drop Image").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource["name"], imageSourceDropZone["name"]);

            imageSource = altDriver.FindObject(By.NAME, "Drag Image2").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            imageSourceDropZone = altDriver.FindObject(By.NAME, "Drop").GetComponentProperty<dynamic>("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource["name"], imageSourceDropZone["name"]);
        }

        [Test]
        public void TestPointerEnterAndExit()
        {
            var altElement = altDriver.FindObject(By.NAME, "Drop Image");
            var color1 = altElement.GetComponentProperty<dynamic>("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            altDriver.FindObject(By.NAME, "Drop Image").PointerEnterObject();
            var color2 = altElement.GetComponentProperty<dynamic>("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color1, color2);
            altDriver.FindObject(By.NAME, "Drop Image").PointerExitObject();
            var color3 = altElement.GetComponentProperty<dynamic>("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color3, color2);
            Assert.AreEqual(color1, color3);
        }

        [Test]
        public void TestDragAndDrop()
        {
            var altElement1 = altDriver.FindObject(By.NAME, "Drag Image1");
            var altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            var initDropImage = altDriver.FindObject(By.PATH, "//*/Drop Box1/Drop Image");

            int fingerId = altDriver.BeginTouch(altElement1.getScreenPosition());
            altDriver.MoveTouch(fingerId, altElement2.getScreenPosition());
            altDriver.EndTouch(fingerId);
            var finalDropImage = altDriver.FindObject(By.PATH, "//*/Drop Box1/Drop Image");

            Assert.AreNotEqual(initDropImage, finalDropImage);
        }
    }
}