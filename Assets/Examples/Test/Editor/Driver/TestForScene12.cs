using AltTester.AltTesterSDK.Driver;
using NUnit.Framework;

namespace AltTester.AltTesterSDK.Driver.Tests
{
    [TestFixture]
    [Parallelizable]
    public class TestForScene12 : TestBase
    {   //Important! If your test file is inside a folder that contains an .asmdef file, please make sure that the assembly definition references NUnit.

        public TestForScene12()
        {
            sceneName = "Sceme 12 2D Objects";
        }

        [TestCase("Square")]
        [TestCase("Circle")]
        [TestCase("Triangle")]
        public void TestClickOnObjets(string name)
        {
            var altObject = altDriver.FindObject(By.NAME, name);
            altObject.Click();
            Assert.That($"Clicked on {name}".Equals(altDriver.FindObject(By.NAME, "Text").GetText()));
        }
        [TestCase("Square")]
        [TestCase("Circle")]
        [TestCase("Triangle")]
        public void TestTapOnObjets(string name)
        {
            var altObject = altDriver.FindObject(By.NAME, name);
            altObject.Tap();
            Assert.That($"Clicked on {name}".Equals(altDriver.FindObject(By.NAME, "Text").GetText()));
        }
        [TestCase("Square")]
        [TestCase("Circle")]
        [TestCase("Triangle")]
        public void TestTapOnCoordinates(string name)
        {
            var altObject = altDriver.FindObject(By.NAME, name);
            altDriver.Tap(altObject.GetScreenPosition());
            Assert.That($"Clicked on {name}".Equals(altDriver.FindObject(By.NAME, "Text").GetText()));
        }

        [TestCase("Square")]
        [TestCase("Circle")]
        [TestCase("Triangle")]
        public void TestClickOnCoordinates(string name)
        {
            var altObject = altDriver.FindObject(By.NAME, name);
            altDriver.Click(altObject.GetScreenPosition());
            Assert.That($"Clicked on {name}".Equals(altDriver.FindObject(By.NAME, "Text").GetText()));
        }

        [TestCase("Square")]
        [TestCase("Circle")]
        [TestCase("Triangle")]
        public void TestSwipeOnCoordinates(string name)
        {
            var altObject = altDriver.FindObject(By.NAME, name);
            altDriver.HoldButton(altObject.GetScreenPosition(), 0.3f);
            Assert.That($"Clicked on {name}".Equals(altDriver.FindObject(By.NAME, "Text").GetText()));
        }
        [TestCase("Square")]
        [TestCase("Circle")]
        [TestCase("Triangle")]
        public void TestTouchOnCoordinates(string name)
        {
            var altObject = altDriver.FindObject(By.NAME, name);
            var id = altDriver.BeginTouch(altObject.GetScreenPosition());
            altDriver.EndTouch(id);
            Assert.That($"Clicked on {name}".Equals(altDriver.FindObject(By.NAME, "Text").GetText()));
        }
        [Test]
        public void TestDragObjet()
        {
            var altObject = altDriver.FindObject(By.NAME, "Hexagon Flat-Top");
            var currentPosition = altObject.GetScreenPosition();
            altDriver.Swipe(currentPosition, currentPosition * 1.1f);
            altObject = altDriver.FindObject(By.NAME, "Hexagon Flat-Top");
            Assert.That(currentPosition.x < altObject.GetScreenPosition().x, $"Expected x to be smaller: {currentPosition.x} but was {altObject.GetScreenPosition().x}");
            Assert.That(currentPosition.y < altObject.GetScreenPosition().y, $"Expected y to be smaller: {currentPosition.y} but was {altObject.GetScreenPosition().y}");
        }

    }
}