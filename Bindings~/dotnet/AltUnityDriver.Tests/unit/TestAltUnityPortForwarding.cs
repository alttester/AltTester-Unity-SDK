using System;
using System.IO;
using NUnit.Framework;

namespace Altom.AltUnityDriver.Tests
{
    [Timeout(10000)]
    [Category("Android")]
    public class TestAltUnityPortForwarding
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
            AltUnityPortForwarding.ForwardAndroid();
        }
        [SetUp]
        public void SetUp()
        {
            AltUnityPortForwarding.RemoveAllForwardAndroid();
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
            Assert.AreEqual("overwrite", AltUnityPortForwarding.GetAdbPath("overwrite"));
        }

        [Test]
        public void TestGetAdbPathAndroidSdk()
        {
            var sdkPath = string.Join(Path.DirectorySeparatorChar, "path", "to", "sdk");
            Environment.SetEnvironmentVariable("ANDROID_SDK_ROOT", sdkPath);

            var expected = string.Join(Path.DirectorySeparatorChar, sdkPath, "platform-tools", "adb");
            Assert.AreEqual(expected, AltUnityPortForwarding.GetAdbPath(""));
        }
        [Test]
        public void TestGetAdbPathSystemPath()
        {
            Environment.SetEnvironmentVariable("ANDROID_SDK_ROOT", null);
            Assert.AreEqual("adb", AltUnityPortForwarding.GetAdbPath(""));
        }

        // Since changing the build to run on mac this test will fail TODO Filter this test out on mac
        // [Test]
        // public void TestRemoveForwardAndroid()
        // {
        //     AltUnityPortForwarding.ForwardAndroid();
        //     AltUnityPortForwarding.RemoveForwardAndroid(13000);
        //     try
        //     {
        //         var driver = new AltUnityDriver(enableLogging: true, connectTimeout: 2);
        //         driver.Stop();
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex.ToString());
        //         Assert.AreEqual("Failed to connect to AltUnity Tester on host: 127.0.0.1 port: 13000.", ex.Message);
        //         return;
        //     }
        //     Assert.Fail("Should not be able to connect");

        // }

        //TODO: enable when server is implemented
        // [Test]
        // public void TestForwardAndroid()
        // {
        //     AltUnityPortForwarding.RemoveAllForwardAndroid();
        //     AltUnityPortForwarding.ForwardAndroid();
        //     try
        //     {
        //         var driver = new AltUnityDriver(enableLogging: true, connectTimeout: 2);
        //         driver.Stop();
        //     }
        //     catch
        //     {
        //         Assert.Fail("ForwardAndroid failed");
        //     }
        // }
    }
}
