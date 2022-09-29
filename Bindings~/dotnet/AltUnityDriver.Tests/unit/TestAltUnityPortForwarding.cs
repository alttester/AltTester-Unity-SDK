using System;
using System.IO;
using NUnit.Framework;

namespace Altom.AltDriver.Tests
{
    [Timeout(10000)]
    [Category("Android")]
    public class TestAltPortForwarding
    {
        private string androidSdkRoot;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //store ANDROID_SDK_ROOT original
            androidSdkRoot = Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT");
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            //restore ANDROID_SDK_ROOT to original
            Environment.SetEnvironmentVariable("ANDROID_SDK_ROOT", androidSdkRoot);
            AltPortForwarding.ForwardAndroid();
        }
        [SetUp]
        public void SetUp()
        {
            AltPortForwarding.RemoveAllForwardAndroid();
        }

        [TearDown]
        public void TearDown()
        {
            //restore ANDROID_SDK_ROOT to original
            Environment.SetEnvironmentVariable("ANDROID_SDK_ROOT", androidSdkRoot);
        }

        [Test]
        public void TestGetAdbPathOverwrite()
        {
            Assert.AreEqual("overwrite", AltPortForwarding.GetAdbPath("overwrite"));
        }

        [Test]
        public void TestGetAdbPathAndroidSdk()
        {
            var sdkPath = string.Join(Path.DirectorySeparatorChar, "path", "to", "sdk");
            Environment.SetEnvironmentVariable("ANDROID_SDK_ROOT", sdkPath);

            var expected = string.Join(Path.DirectorySeparatorChar, sdkPath, "platform-tools", "adb");
            Assert.AreEqual(expected, AltPortForwarding.GetAdbPath(""));
        }
        [Test]
        public void TestGetAdbPathSystemPath()
        {
            Environment.SetEnvironmentVariable("ANDROID_SDK_ROOT", null);
            Assert.AreEqual("adb", AltPortForwarding.GetAdbPath(""));
        }

        [Test]
        public void TestRemoveForwardAndroid()
        {
            AltPortForwarding.ForwardAndroid();
            AltPortForwarding.RemoveForwardAndroid(13000);
            try
            {
                var driver = new AltDriver(enableLogging: true, connectTimeout: 2);
                driver.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.AreEqual("Failed to connect to AltTester on host: 127.0.0.1 port: 13000.", ex.Message);
                return;
            }
            Assert.Fail("Should not be able to connect");

        }

        //TODO: enable when server is implemented
        // [Test]
        // public void TestForwardAndroid()
        // {
        //     AltPortForwarding.RemoveAllForwardAndroid();
        //     AltPortForwarding.ForwardAndroid();
        //     try
        //     {
        //         var driver = new AltDriver(enableLogging: true, connectTimeout: 2);
        //         driver.Stop();
        //     }
        //     catch
        //     {
        //         Assert.Fail("ForwardAndroid failed");
        //     }
        // }
    }
}
