/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Logging;
using NUnit.Framework;

namespace AltTester.AltTesterSDK.Driver.Tests
{
    [TestFixture]
    [Parallelizable]
    public class TestForScene4NoCameras : TestBase
    {
        public TestForScene4NoCameras()
        {
            sceneName = "Scene 4 No Cameras";
        }

        [TestCase("Plane")]
        [TestCase("EventSystem")]
        [TestCase("Cube")]
        [Test]
        public void TestFindElementInASceneWithNoCameras(string ObjectName)
        {
            Assert.AreEqual(0, altDriver.GetAllCameras().Count);
            var altObject = altDriver.FindObject(By.NAME, ObjectName);
            Assert.AreEqual(0, altObject.worldX, "WorldX was: " + altObject.worldX + " when it should have been 0");
            Assert.AreEqual(0, altObject.worldY, "WorldY was: " + altObject.worldY + " when it should have been 0");
            Assert.AreEqual(0, altObject.worldZ, "WorldZ was: " + altObject.worldZ + " when it should have been 0");
            Assert.AreEqual(-1, altObject.x);
            Assert.AreEqual(-1, altObject.y);
            Assert.AreEqual(-1, altObject.z);
            Assert.AreEqual(-1, altObject.idCamera);
        }

        [Test]
        public void TestFindUIElementInASceneWithNoCameras()
        {
            Assert.AreEqual(0, altDriver.GetAllCameras().Count);
            var altObjects = altDriver.FindObjects(By.PATH, "//*[contains(@name,Button)]", enabled: false);

            foreach (var button in altObjects)
            {
                Assert.AreNotEqual(-1, button.x);
                Assert.AreNotEqual(-1, button.y);
            }
            Assert.AreEqual(10, altObjects.Count);
        }
    }
}
