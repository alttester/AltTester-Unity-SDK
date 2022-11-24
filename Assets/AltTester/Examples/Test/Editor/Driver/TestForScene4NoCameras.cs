using Altom.AltDriver;
using Altom.AltDriver.Logging;
using NUnit.Framework;

namespace Altom.AltDriver.Tests
{
    public class TestForScene4NoCameras
    {
        public AltDriver altDriver;
        //Before any test it connects with the socket
        [OneTimeSetUp]
        public void SetUp()
        {
            altDriver = new AltDriver(host: TestsHelper.GetAltDriverHost(), port: TestsHelper.GetAltDriverPort(), enableLogging: true);
            DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Info);
            DriverLogManager.SetMinLogLevel(AltLogger.Unity, AltLogLevel.Info);
            altDriver.LoadScene("Scene 4 No Cameras");
            altDriver.ResetInput();
        }

        //At the end of the test closes the connection with the socket
        [OneTimeTearDown]
        public void TearDown()
        {
            altDriver.Stop();
        }

        [Test]
        [Ignore("Testing")]
        public void TestFindElementInASceneWithNoCameras()
        {
            Assert.AreEqual(0, altDriver.GetAllCameras().Count);
            var altObject = altDriver.FindObject(By.NAME, "Plane");
            Assert.AreEqual(0, altObject.worldX, "WorldX was: " + altObject.worldX + " when it should have been 0");
            Assert.AreEqual(0, altObject.worldY, "WorldY was: " + altObject.worldY + " when it should have been 0");
            Assert.AreEqual(0, altObject.worldZ, "WorldZ was: " + altObject.worldZ + " when it should have been 0");
            Assert.AreEqual(-1, altObject.x);
            Assert.AreEqual(-1, altObject.y);
            Assert.AreEqual(-1, altObject.z);
            Assert.AreEqual(-1, altObject.idCamera);
        }

        [Test]
        [Ignore("Testing")]
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
