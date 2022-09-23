using System.Collections.Generic;
using System.Threading;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Logging;
using NUnit.Framework;

namespace Altom.AltUnityDriver.Tests
{
    [Timeout(10000)]
    public class TestForScene3DragAndDrop
    {
        private AltUnityDriver altUnityDriver;

        [OneTimeSetUp]
        public void SetUp()
        {
            altUnityDriver = new AltUnityDriver(host: TestsHelper.GetAltUnityDriverHost(), port: TestsHelper.GetAltUnityDriverPort(), enableLogging: true);
            DriverLogManager.SetMinLogLevel(AltUnityLogger.Console, AltUnityLogLevel.Info);
            DriverLogManager.SetMinLogLevel(AltUnityLogger.Unity, AltUnityLogLevel.Info);
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
            string imageSource, imageSourceDropZone;
            dropImage("Drag Image2", "Drop Box2", 0.1f, false);
            dropImage("Drag Image3", "Drop Box1", 0.1f, false);
            dropImage("Drag Image1", "Drop Box1", 0.2f, false);
            waitForSwipeToFinish();
            getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image1", "Drop Image");
            Assert.AreEqual(imageSource, imageSourceDropZone);

            getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image2", "Drop");
            Assert.AreEqual(imageSource, imageSourceDropZone);
        }

        private void waitForSwipeToFinish()
        {
            altUnityDriver.WaitForObjectNotBePresent(By.NAME, "icon");
        }

        private void getSpriteName(out string imageSource, out string imageSourceDropZone, string sourceImageName, string imageSourceDropZoneName)
        {
            imageSource = altUnityDriver.FindObject(By.NAME, sourceImageName).GetComponentProperty<string>("UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI");
            imageSourceDropZone = altUnityDriver.FindObject(By.NAME, imageSourceDropZoneName).GetComponentProperty<string>("UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI");
        }

        private void dropImage(string dragLocationName, string dropLocationName, float duration = 0.1f, bool wait = true)
        {
            var dragLocation = altUnityDriver.FindObject(By.NAME, dragLocationName);
            var dropLocation = altUnityDriver.FindObject(By.NAME, dropLocationName);

            altUnityDriver.Swipe(new AltUnityVector2(dragLocation.x, dragLocation.y), new AltUnityVector2(dropLocation.x, dropLocation.y), duration, wait: wait);
        }
        private void dropImageWithMultipointSwipe(string[] objectNames, float duration = 0.1f, bool wait = true)
        {
            AltUnityVector2[] listPositions = new AltUnityVector2[objectNames.Length];
            for (int i = 0; i < objectNames.Length; i++)
            {
                var obj = altUnityDriver.FindObject(By.NAME, objectNames[i]);
                listPositions[i] = obj.getScreenPosition();
            }
            altUnityDriver.MultipointSwipe(listPositions, duration, wait: wait);
        }

        [Test]
        public void MultipleDragAndDropWait()
        {
            string imageSource, imageSourceDropZone;
            dropImage("Drag Image2", "Drop Box2");
            dropImage("Drag Image3", "Drop Box1");
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
            var altElement = altUnityDriver.FindObject(By.NAME, "Drop Image");
            var color1 = altElement.GetComponentProperty<dynamic>("AltUnityExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            altUnityDriver.FindObject(By.NAME, "Drop Image").PointerEnterObject();
            var color2 = altElement.GetComponentProperty<dynamic>("AltUnityExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color1, color2);
            altUnityDriver.FindObject(By.NAME, "Drop Image").PointerExitObject();
            var color3 = altElement.GetComponentProperty<dynamic>("AltUnityExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color3, color2);
            Assert.AreEqual(color1, color3);
        }

        [Test]
        public void TestDragAndDrop()
        {
            string imageSource, imageSourceDropZone;
            var altElement1 = altUnityDriver.FindObject(By.NAME, "Drag Image1");
            var altElement2 = altUnityDriver.FindObject(By.NAME, "Drop Box1");

            int fingerId = altUnityDriver.BeginTouch(altElement1.getScreenPosition());
            altUnityDriver.MoveTouch(fingerId, altElement2.getScreenPosition());
            altUnityDriver.EndTouch(fingerId);
            getSpriteName(out imageSource, out imageSourceDropZone, "Drag Image1", "Drop Image");

            Assert.AreEqual(imageSource, imageSourceDropZone);
        }
    }
}