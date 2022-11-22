using System;
using System.Collections.Generic;
using System.Threading;
using Altom.AltDriver;
using Altom.AltDriver.Logging;
using NUnit.Framework;

namespace Altom.AltDriver.Tests
{
    public class TestForScene5KeyboardAndMouseInput
    {
#pragma warning disable CS0618

        public AltDriver altDriver;
        //Before any test it connects with the socket
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            altDriver = new AltDriver(host: TestsHelper.GetAltDriverHost(), port: TestsHelper.GetAltDriverPort(), enableLogging: true);
            DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Info);
            DriverLogManager.SetMinLogLevel(AltLogger.Unity, AltLogLevel.Info);
        }

        //At the end of the test closes the connection with the socket
        [OneTimeTearDown]
        public void TearDown()
        {
            altDriver.Stop();
        }
        [SetUp]
        public void SetUp()
        {
            altDriver.ResetInput();
            altDriver.LoadScene("Scene 5 Keyboard Input");

        }

        [Test]
        //Test input made with axis
        public void TestMovementCube()
        {


            var cube = altDriver.FindObject(By.NAME, "Player1");
            AltVector3 cubeInitialPostion = new AltVector3(cube.worldX, cube.worldY, cube.worldY);
            altDriver.PressKey(AltKeyCode.K, 1, 2, wait: false);
            Thread.Sleep(2000);
            altDriver.PressKey(AltKeyCode.O, 1, 1);

            cube = altDriver.FindObject(By.NAME, "Player1");
            AltVector3 cubeFinalPosition = new AltVector3(cube.worldX, cube.worldY, cube.worldY);

            Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);


        }

        [Test]
        public void TestCameraMovement()
        {
            var cube = altDriver.FindObject(By.NAME, "Player1");
            AltVector3 cubeInitialPostion = cube.GetWorldPosition();

            altDriver.PressKey(AltKeyCode.W, 1, 2);
            cube = altDriver.FindObject(By.NAME, "Player1");
            AltVector3 cubeFinalPosition = cube.GetWorldPosition();

            Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);
        }

        [Test]
        //Testing mouse movement and clicking
        public void TestCreatingStars()
        {
            var stars = altDriver.FindObjectsWhichContain(By.NAME, "Star", cameraValue: "Player2");
            var pressingpoint1 = altDriver.FindObjectWhichContains(By.NAME, "PressingPoint1", cameraValue: "Player2");
            Assert.AreEqual(1, stars.Count);

            altDriver.MoveMouse(new AltVector2(pressingpoint1.x, pressingpoint1.y), 1, wait: false);
            Thread.Sleep(1500);

            altDriver.PressKey(AltKeyCode.Mouse0, 0.1f);

            var pressingpoint2 = altDriver.FindObjectWhichContains(By.NAME, "PressingPoint2", cameraValue: "Player2");
            altDriver.MoveMouse(new AltVector2(pressingpoint2.x, pressingpoint2.y), 1);
            altDriver.PressKey(AltKeyCode.Mouse0, 0.1f);

            stars = altDriver.FindObjectsWhichContain(By.NAME, "Star");
            Assert.AreEqual(3, stars.Count);
        }
        [Test]
        public void TestKeyboardPress()
        {
            var lastKeyDown = altDriver.FindObject(By.NAME, "LastKeyDownValue");
            var lastKeyUp = altDriver.FindObject(By.NAME, "LastKeyUpValue");
            var lastKeyPress = altDriver.FindObject(By.NAME, "LastKeyPressedValue");
            var runner = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            foreach (AltKeyCode kcode in Enum.GetValues(typeof(AltKeyCode)))
            {
                if (kcode != AltKeyCode.NoKey && kcode < AltKeyCode.Joystick1Button0)
                {
                    altDriver.PressKey(kcode, duration: 0.2f);

                    Assert.AreEqual((int)kcode, Int32.Parse(lastKeyDown.GetText()));
                    Assert.AreEqual((int)kcode, Int32.Parse(lastKeyUp.GetText()));
                    Assert.AreEqual((int)kcode, Int32.Parse(lastKeyPress.GetText()));
                }
            }
        }

        [Test]
        public void TestKeyDownAndKeyUp()
        {
            AltKeyCode kcode = AltKeyCode.A;

            altDriver.KeyDown(kcode, 1);
            var lastKeyDown = altDriver.FindObject(By.NAME, "LastKeyDownValue");
            var lastKeyPress = altDriver.FindObject(By.NAME, "LastKeyPressedValue");

            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyDown.GetText(), true));
            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyPress.GetText(), true));

            altDriver.KeyUp(kcode);
            var lastKeyUp = altDriver.FindObject(By.NAME, "LastKeyUpValue");

            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyUp.GetText(), true));
        }

        [Test]
        public void TestButton()
        {
            var ButtonNames = new List<String>()
        {
           "Horizontal","Vertical"
        };
            var KeyToPressForButtons = new List<AltKeyCode>()
        {
            AltKeyCode.A,AltKeyCode.W
        };
            altDriver.LoadScene("Scene 5 Keyboard Input");
            var axisName = altDriver.FindObject(By.NAME, "AxisName");
            int i = 0;
            foreach (AltKeyCode kcode in KeyToPressForButtons)
            {
                altDriver.PressKey(kcode, duration: 0.05f);
                Assert.AreEqual(ButtonNames[i].ToString(), axisName.GetText());
                i++;
            }

        }

        [Test]
        public void TestPowerJoystick()
        {
            var ButtonNames = new List<String>()
        {
           "Horizontal","Vertical"
        };
            var KeyToPressForButtons = new List<AltKeyCode>()
        {
            AltKeyCode.D,AltKeyCode.W
        };
            altDriver.LoadScene("Scene 5 Keyboard Input");
            var axisName = altDriver.FindObject(By.NAME, "AxisName");
            var axisValue = altDriver.FindObject(By.NAME, "AxisValue");
            int i = 0;
            foreach (AltKeyCode kcode in KeyToPressForButtons)
            {
                altDriver.PressKey(kcode, power: 0.5f, duration: 0.1f);
                Assert.AreEqual("0.5", axisValue.GetText());
                Assert.AreEqual(ButtonNames[i].ToString(), axisName.GetText());
                i++;
            }
        }
        [Test]
        public void TestScroll()
        {
            var player2 = altDriver.FindObject(By.NAME, "Player2");

            AltVector3 cubeInitialPostion = new AltVector3(player2.worldX, player2.worldY, player2.worldY);
            altDriver.Scroll(4, 2, false);
            Thread.Sleep(4000);
            player2 = altDriver.FindObject(By.NAME, "Player2");
            AltVector3 cubeFinalPosition = new AltVector3(player2.worldX, player2.worldY, player2.worldY);

            Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);
        }
        [Test]
        public void TestScrollAndWait()
        {
            var player2 = altDriver.FindObject(By.NAME, "Player2");
            AltVector3 cubeInitialPostion = new AltVector3(player2.worldX, player2.worldY, player2.worldY);
            altDriver.Scroll(4, 2);

            player2 = altDriver.FindObject(By.NAME, "Player2");
            AltVector3 cubeFinalPosition = new AltVector3(player2.worldX, player2.worldY, player2.worldY);
            Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);
        }

        [Test]
        [Category("WebGLUnsupported")]
        public void TestCheckShadersSetCorrectlyAfterHighlight()
        {
            var cube = altDriver.FindObject(By.NAME, "2MaterialCube");
            var count = cube.GetComponentProperty<int>("UnityEngine.Renderer", "materials.Length", "UnityEngine.CoreModule");
            var shadersName = new List<string>();
            for (int i = 0; i < count; i++)
            {
                shadersName.Add(cube.GetComponentProperty<string>("UnityEngine.Renderer", "materials[" + i + "].shader.name", "UnityEngine.CoreModule"));
            }

            altDriver.GetScreenshot(cube.id, new AltColor(1, 1, 1), 1.1f);
            Thread.Sleep(1000);
            var newShadersName = new List<string>();
            for (int i = 0; i < count; i++)
            {
                newShadersName.Add(cube.GetComponentProperty<string>("UnityEngine.Renderer", "materials[" + i + "].shader.name", "UnityEngine.CoreModule"));
            }
            Assert.AreEqual(newShadersName, shadersName);

        }

#pragma warning restore CS0618
    }
}