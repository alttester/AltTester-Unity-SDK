using NUnit.Framework;

namespace AltTester.AltTesterSDK.Driver.Tests
{

    [TestFixture]
    [Parallelizable]
    public class TestForScene14 : TestBase
    {
        public TestForScene14()
        {
            sceneName = "Scene 14";
        }

        [Test]
        public void Test()
        {
            var nameOfTheObject = "TopObject";
            var topObject = altDriver.FindObject(By.NAME, nameOfTheObject);
            var screenPosition = topObject.GetScreenPosition();
            var foundObject = altDriver.FindObjectAtCoordinates(screenPosition);
            Assert.AreEqual(nameOfTheObject, foundObject.name);
        }

    }
}