using System.Collections.Generic;
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
            string imageSource, imageSourceDropZone;
            dropImage("Drag Image2", "Drop Box1", 1f, false);
            dropImage("Drag Image2", "Drop Box2", 1f, false);
            dropImage("Drag Image1", "Drop Box1", 2f, false);
            waitForSwipeToFinish();
            getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image1", "Drop Image");
            Assert.AreEqual(imageSource, imageSourceDropZone);

            getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image2", "Drop");
            Assert.AreEqual(imageSource, imageSourceDropZone);
        }

        private void waitForSwipeToFinish()
        {
            altDriver.WaitForObjectNotBePresent(By.NAME, "icon");
        }

        private void getSpriteName(out string imageSource, out string imageSourceDropZone, string sourceImageName, string imageSourceDropZoneName)
        {
            imageSource = altDriver.FindObject(By.NAME, sourceImageName).GetComponentProperty<string>("UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI");
            imageSourceDropZone = altDriver.FindObject(By.NAME, imageSourceDropZoneName).GetComponentProperty<string>("UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI");
        }

        private void dropImage(string dragLocationName, string dropLocationName, float duration = 1f, bool wait = true)
        {
            var dragLocation = altDriver.FindObject(By.NAME, dragLocationName);
            var dropLocation = altDriver.FindObject(By.NAME, dropLocationName);

            altDriver.Swipe(new AltVector2(dragLocation.x, dragLocation.y), new AltVector2(dropLocation.x, dropLocation.y), duration, wait: wait);
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
        public void MultipleDragAndDropWait()
        {
            string imageSource, imageSourceDropZone;
            dropImage("Drag Image2", "Drop Box1");
            dropImage("Drag Image2", "Drop Box2");
            dropImage("Drag Image1", "Drop Box1");
            getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image1", "Drop Image");
            Assert.AreEqual(imageSource, imageSourceDropZone);

            getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image2", "Drop");
            Assert.AreEqual(imageSource, imageSourceDropZone);
        }


        [Test]
        public void MultipleDragAndDropWaitWithMultipointSwipe()
        {
            string imageSource, imageSourceDropZone;
            dropImageWithMultipointSwipe(new[] { "Drag Image1", "Drop Box1" });
            dropImageWithMultipointSwipe(new[] { "Drag Image2", "Drop Box1", "Drop Box2" });

            getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image1", "Drop Image");
            Assert.AreEqual(imageSource, imageSourceDropZone);

            getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image2", "Drop");
            Assert.AreEqual(imageSource, imageSourceDropZone);
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
            string imageSource, imageSourceDropZone;
            var altElement1 = altDriver.FindObject(By.NAME, "Drag Image1");
            var altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");

            int fingerId = altDriver.BeginTouch(altElement1.GetScreenPosition());
            altDriver.MoveTouch(fingerId, altElement2.GetScreenPosition());
            altDriver.EndTouch(fingerId);
            getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image1", "Drop Image");

            Assert.AreEqual(imageSource, imageSourceDropZone);
        }
    }
}