/*
    Copyright(C) 2024 Altom Consulting

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

using System.Collections.Generic;
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using NUnit.Framework;

namespace AltTester.AltTesterUnitySDK.Driver.Tests
{
    [TestFixture]
    [Parallelizable]
    [Timeout(10000)]
    public class TestForScene3DragAndDrop : TestBase
    {
        public TestForScene3DragAndDrop()
        {
            sceneName = "Scene 3 Drag And Drop";
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
            altDriver.FindObject(By.NAME, "Drop Image").PointerEnter();
            var color2 = altElement.GetComponentProperty<dynamic>("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color1, color2);
            altDriver.FindObject(By.NAME, "Drop Image").PointerExit();
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
