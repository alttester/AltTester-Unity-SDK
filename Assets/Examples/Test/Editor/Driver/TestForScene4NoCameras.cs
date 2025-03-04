/*
    Copyright(C) 2025 Altom Consulting

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
            Assert.AreEqual(2, altObjects.Count);
        }
    }
}
