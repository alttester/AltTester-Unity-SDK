using AltTester.AltDriver;
using AltTester.AltDriver.Logging;
using NUnit.Framework;


namespace AltTester.AltDriver.Tests
{
    public class TestInputActions : TestBase
    {
        public TestInputActions()
        {
            sceneName = "Scene6";
        }

        [Test]
        public void TestScrollAndWait()
        {

            var scrollBar = altDriver.WaitForObject(By.PATH, "//ScrollCanvas//Handle");

            AltVector2 scrollBarInitialPosition = scrollBar.GetScreenPosition();
            altDriver.MoveMouse(scrollBarInitialPosition);
            altDriver.Scroll(-20, 0.1f);

            scrollBar = altDriver.FindObject(By.PATH, "//ScrollCanvas//Handle");
            AltVector2 scrollBarFinalPosition = scrollBar.GetScreenPosition();
            Assert.AreNotEqual(scrollBarInitialPosition.y, scrollBarFinalPosition.y);
        }
    }
}