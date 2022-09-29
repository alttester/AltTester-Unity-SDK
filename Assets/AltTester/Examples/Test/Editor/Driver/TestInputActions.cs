using Altom.AltDriver;
using Altom.AltDriver.Logging;
using NUnit.Framework;


namespace Altom.AltDriver.Tests
{
    public class TestInputActions
    {
        private AltDriver altUnityDriver;
        //Before any test it connects with the socket
        [OneTimeSetUp]
        public void SetUp()
        {
            altUnityDriver = new AltDriver(host: TestsHelper.GetAltDriverHost(), port: TestsHelper.GetAltDriverPort(), enableLogging: true);
            DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Info);
            DriverLogManager.SetMinLogLevel(AltLogger.Unity, AltLogLevel.Info);
        }

        //At the end of the test closes the connection with the socket
        [OneTimeTearDown]
        public void TearDown()
        {
            altUnityDriver.Stop();
        }

        [Test]
        public void TestScrollAndWait()
        {
            altUnityDriver.LoadScene("Scene6");

            var scrollBar = altUnityDriver.WaitForObject(By.PATH, "//ScrollCanvas//Handle");

            AltVector2 scrollBarInitialPosition = scrollBar.getScreenPosition();
            altUnityDriver.MoveMouse(scrollBarInitialPosition);
            altUnityDriver.Scroll(-20, 0.1f);

            scrollBar = altUnityDriver.FindObject(By.PATH, "//ScrollCanvas//Handle");
            AltVector2 scrollBarFinalPosition = scrollBar.getScreenPosition();
            Assert.AreNotEqual(scrollBarInitialPosition.y, scrollBarFinalPosition.y);
        }
    }
}