using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Logging;
using NUnit.Framework;


namespace Altom.AltUnityDriver.Tests
{
    public class TestInputActions
    {
        private AltUnityDriver altUnityDriver;
        //Before any test it connects with the socket
        [OneTimeSetUp]
        public void SetUp()
        {
            altUnityDriver = new AltUnityDriver(host: TestsHelper.GetAltUnityDriverHost(), port: TestsHelper.GetAltUnityDriverPort(), enableLogging: true);
            DriverLogManager.SetMinLogLevel(AltUnityLogger.Console, AltUnityLogLevel.Info);
            DriverLogManager.SetMinLogLevel(AltUnityLogger.Unity, AltUnityLogLevel.Info);
        }

        //At the end of the test closes the connection with the socket
        [OneTimeTearDown]
        public void TearDown()
        {
            altUnityDriver.Stop();
        }

        [Test]
        [Retry(5)]
        public void TestScrollAndWait()
        {
            altUnityDriver.LoadScene("Scene6");

            var scrollBar = altUnityDriver.WaitForObject(By.PATH, "//ScrollCanvas//Handle");

            AltUnityVector2 scrollBarInitialPosition = scrollBar.getScreenPosition();
            altUnityDriver.MoveMouse(scrollBarInitialPosition);
            altUnityDriver.Scroll(-20, 0.1f);

            scrollBar = altUnityDriver.FindObject(By.PATH, "//ScrollCanvas//Handle");
            AltUnityVector2 scrollBarFinalPosition = scrollBar.getScreenPosition();
            Assert.AreNotEqual(scrollBarInitialPosition.y, scrollBarFinalPosition.y);
        }
    }
}