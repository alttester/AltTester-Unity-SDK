/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Logging;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;


namespace AltTester.AltTesterSDK.Driver.Tests
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

        /// <summary>
        /// Subset of the fields UnityEngine.Touch serializes. Only the ones these tests assert on
        /// are declared; Newtonsoft ignores the rest of the payload.
        /// </summary>
#pragma warning disable 0649 // assigned by the JSON deserializer, never in code
        private class InjectedTouch
        {
            public int fingerId;
            public int tapCount;
            public float deltaTime;
            public int phase;
        }
#pragma warning restore 0649

        // UnityEngine.TouchPhase, serialized as an int (no StringEnumConverter is configured).
        private const int PhaseBegan = 0;
        private const int PhaseMoved = 1;
        private const int PhaseStationary = 2;

        /// <summary>
        /// Reads a touch straight out of the SDK's Input shim, which is what a polling game sees.
        /// The shim is a global-namespace class inside the AltTester.AltTesterUnitySDK assembly.
        /// </summary>
        private InjectedTouch GetInjectedTouch(int index)
        {
            return altDriver.CallStaticMethod<InjectedTouch>(
                "Input", "GetTouch", "AltTester.AltTesterUnitySDK",
                new object[] { index }, new[] { "System.Int32" });
        }

        private int GetInjectedTouchCount()
        {
            return altDriver.CallStaticMethod<int>(
                "Input", "get_touchCount", "AltTester.AltTesterUnitySDK",
                new object[] { }, null);
        }

        /// <summary>
        /// A held touch only decays to Stationary after AltInput.Update() has run on the frames
        /// following BeginTouch. The driver's next command can arrive before those frames, so poll
        /// instead of asserting on the first sample.
        /// </summary>
        private InjectedTouch WaitForStationaryTouch(int index, int timeoutMs = 2000)
        {
            var stopwatch = Stopwatch.StartNew();
            var touch = GetInjectedTouch(index);
            while (touch.phase != PhaseStationary && stopwatch.ElapsedMilliseconds < timeoutMs)
            {
                Thread.Sleep(50);
                touch = GetInjectedTouch(index);
            }
            return touch;
        }

        [Test]
        public void TestHeldTouchReportsCompleteTouchData()
        {
            var fingerId = altDriver.BeginTouch(new AltVector2(100, 100));
            try
            {
                var touch = WaitForStationaryTouch(0);

                // The driver hands out 1-based ids (InputController.BeginTouch returns oldFingerId + 1),
                // while the legacy Input shim reports Unity's 0-based Touch.fingerId.
                Assert.AreEqual(fingerId - 1, touch.fingerId);
                Assert.AreEqual(1, touch.tapCount,
                    "tapCount must be 1, matching a real device and the New Input System backend");
                Assert.AreEqual(PhaseStationary, touch.phase,
                    "a held touch must decay from Began to Stationary instead of reporting Began forever");
                Assert.Greater(touch.deltaTime, 0f,
                    "deltaTime must advance once the touch has changed at least once");
            }
            finally
            {
                altDriver.EndTouch(fingerId);
            }
        }

        [Test]
        public void TestHeldTouchKeepsRefreshingDeltaTime()
        {
            var fingerId = altDriver.BeginTouch(new AltVector2(100, 100));
            try
            {
                // Both samples are taken while the touch is Stationary. deltaTime is the time since
                // the touch last changed, so it must be re-stamped every frame rather than frozen at
                // the value it had when the phase last changed.
                var first = WaitForStationaryTouch(0);
                Assert.AreEqual(PhaseStationary, first.phase);
                Assert.Greater(first.deltaTime, 0f);

                Thread.Sleep(100);
                var second = GetInjectedTouch(0);
                Assert.AreEqual(PhaseStationary, second.phase);
                Assert.Greater(second.deltaTime, 0f);
            }
            finally
            {
                altDriver.EndTouch(fingerId);
            }
        }

        [Test]
        public void TestMovedTouchKeepsTapCount()
        {
            var fingerId = altDriver.BeginTouch(new AltVector2(100, 100));
            try
            {
                altDriver.MoveTouch(fingerId, new AltVector2(200, 200));

                var touch = GetInjectedTouch(0);
                Assert.AreEqual(1, touch.tapCount, "tapCount must survive a phase change");
                Assert.Greater(touch.deltaTime, 0f);
                Assert.Contains(touch.phase, new[] { PhaseMoved, PhaseStationary },
                    "after a move the touch is Moved, then decays to Stationary; Began would mean the phase never advanced");
            }
            finally
            {
                altDriver.EndTouch(fingerId);
            }
        }

        [Test]
        public void TestTouchIsRemovedAfterEndTouch()
        {
            var fingerId = altDriver.BeginTouch(new AltVector2(100, 100));
            Assert.AreEqual(1, GetInjectedTouchCount());

            altDriver.EndTouch(fingerId);

            Assert.AreEqual(0, GetInjectedTouchCount(),
                "the touch and its metadata must be dropped once the touch ends");
        }

        [Test]
        public void TestTapLeavesNoResidualTouch()
        {
            altDriver.Tap(new AltVector2(100, 100));

            Assert.AreEqual(0, GetInjectedTouchCount(),
                "a completed tap must not leave a touch behind");
        }
    }
}
