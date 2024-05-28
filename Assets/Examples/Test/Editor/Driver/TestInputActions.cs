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

using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using NUnit.Framework;


namespace AltTester.AltTesterUnitySDK.Driver.Tests
{
    [TestFixture]
    [Parallelizable]
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
            altDriver.Scroll(-20, 0.3f);

            scrollBar = altDriver.FindObject(By.PATH, "//ScrollCanvas//Handle");
            AltVector2 scrollBarFinalPosition = scrollBar.GetScreenPosition();
            Assert.AreNotEqual(scrollBarInitialPosition.y, scrollBarFinalPosition.y);
        }
    }
}
