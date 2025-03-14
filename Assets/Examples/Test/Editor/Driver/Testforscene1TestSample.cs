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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace AltTester.AltTesterSDK.Driver.Tests
{
    [TestFixture]
    [Parallelizable]
    [Timeout(30000)]
    public class TestForScene1TestSample : TestBase
    {

        public TestForScene1TestSample()
        {
            sceneName = "Scene 1 AltDriverTestScene";
        }

        [Test]
        public void TestLoadNonExistentScene()
        {
            Assert.Throws<SceneNotFoundException>(() => altDriver.LoadScene("Scene 0", true));
        }

        [Test]
        public void TestGetCurrentScene()
        {
            Assert.AreEqual("Scene 1 AltDriverTestScene", altDriver.GetCurrentScene());
        }

        [Test]
        public void TestGetApplicationScreenSize()
        {
            var screensize = altDriver.GetApplicationScreenSize();
            Assert.That(screensize.x != 0);//We cannot set resolution on iOS so we don't know the exact resolution, we just want to see that it returns a value and is different than 0
            Assert.That(screensize.y != 0);
        }

        [Test]
        public void TestGetApplicationScreenSize2()
        {
            var screenSize = altDriver.GetApplicationScreenSize();

            var screenWidth = altDriver.CallStaticMethod<short>("UnityEngine.Screen", "get_width", "UnityEngine.CoreModule", new string[] { }, null);
            var screenHeight = altDriver.CallStaticMethod<short>("UnityEngine.Screen", "get_height", "UnityEngine.CoreModule", new string[] { }, null);

            Assert.That(screenSize.x, Is.EqualTo(screenWidth));
            Assert.That(screenSize.y, Is.EqualTo(screenHeight));
        }

        // UI disable elements are neede for this because at the moment, the test uses the AltDialog objects
        [TestCase("/AltTesterPrefab/AltDialog/Dialog", "Dialog")]
        [TestCase("/AltTesterPrefab/AltDialog/Dialog/Title", "Title")]
        [TestCase("/Cube", "Cube")]
        public void TestFindDisabledObject(string path, string name)
        {
            var altObject = altDriver.FindObject(By.PATH, path, enabled: false);
            Assert.NotNull(altObject);
            Assert.AreEqual(name, altObject.name);
        }

        [TestCase(By.COMPONENT, "CapsuleColl", "//Capsule")]
        [TestCase(By.ID, "13b211d0-eafa-452d-8708-cc70f5075e93", "//Capsule")]
        [TestCase(By.LAYER, "Wat", "//Capsule")]
        [TestCase(By.NAME, "Cap", "//Capsule")]
        [TestCase(By.TAG, "pla", "//Plane")]
        [TestCase(By.TEXT, "Change Camera", "/Canvas/Button/Text")]
        public void TestFindObjectWhichContains(By by, string value, string path)
        {
            var expectedObject = altDriver.FindObject(By.PATH, path);
            var altObject = altDriver.FindObjectWhichContains(by, value);
            Assert.NotNull(altObject);
            Assert.AreEqual(expectedObject.id, altObject.id);
        }

        [Test]
        public void TestFindElementThatContainsText()
        {
            const string text = "Change Camera";
            var altElement = altDriver.FindObject(By.PATH, "//*[contains(@text," + text + ")]");
            Assert.NotNull(altElement);
        }

        [TestCase(By.COMPONENT, "NonExistent")]
        [TestCase(By.ID, "NonExistent")]
        [TestCase(By.LAYER, "NonExistent")]
        [TestCase(By.NAME, "NonExistent")]
        [TestCase(By.TAG, "NonExistent")]
        [TestCase(By.TEXT, "NonExistent")]
        public void TestFindNonExistentObjectWhichContains(By by, string value)
        {
            Assert.Throws<NotFoundException>(() => altDriver.FindObjectWhichContains(by, value));
        }

        [TestCase(By.COMPONENT, "CapsuleColl", "//Capsule")]
        [TestCase(By.ID, "13b211d0-eafa-452d-8708-cc70f5075e93", "//Capsule")]
        [TestCase(By.LAYER, "Wat", "//Capsule")]
        [TestCase(By.TEXT, "Change Camera", "/Canvas/Button/Text")]
        public void TestFindObjectsWhichContain(By by, string value, string path)
        {
            var expectedObject = altDriver.FindObject(By.PATH, path);
            var altObjects = altDriver.FindObjectsWhichContain(by, value);
            Assert.AreEqual(1, altObjects.Count());
            Assert.AreEqual(expectedObject.id, altObjects[0].id);
        }

        [TestCase(By.NAME, "Cap", "//Capsule", "//CapsuleInfo")]
        [TestCase(By.TAG, "pla", "//Plane", "/UIWithWorldSpace/Plane")]
        public void TestFindObjectsWhichContain2(By by, string value, string path1, string path2)
        {
            var expectedObject1 = altDriver.FindObject(By.PATH, path1);
            var expectedObject2 = altDriver.FindObject(By.PATH, path2);
            var altObjects = altDriver.FindObjectsWhichContain(by, value);
            Assert.AreEqual(2, altObjects.Count());
            Assert.AreEqual(expectedObject1.id, altObjects[0].id);
            Assert.AreEqual(expectedObject2.id, altObjects[1].id);
        }

        [TestCase(By.COMPONENT, "NonExistent")]
        [TestCase(By.ID, "NonExistent")]
        [TestCase(By.LAYER, "NonExistent")]
        [TestCase(By.NAME, "NonExistent")]
        [TestCase(By.TAG, "NonExistent")]
        [TestCase(By.TEXT, "NonExistent")]
        public void TestFindNonExistentObjectsWhichContain(By by, string value)
        {
            var altObjects = altDriver.FindObjectsWhichContain(by, value);
            Assert.IsEmpty(altObjects);
        }

        [Test]
        public void TestWaitForCurrentSceneToBe()
        {
            const string name = "Scene 1 AltDriverTestScene";
            var timeStart = DateTime.Now;
            altDriver.WaitForCurrentSceneToBe(name);
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            var currentScene = altDriver.GetCurrentScene();
            Assert.AreEqual("Scene 1 AltDriverTestScene", currentScene);
        }

        [TestCase("UnityUIInputField")] // UI input field
        [TestCase("TextMeshInputField")] // text mesh input field
        public void TestSetTextForUnityUIInputField(string fieldName)
        {
            var inputField = altDriver.FindObject(By.NAME, fieldName).SetText("exampleUnityUIInputField", true);
            Assert.AreEqual("exampleUnityUIInputField", inputField.GetText());
            Assert.IsTrue(inputField.GetComponentProperty<bool>("AltInputFieldRaisedEvents", "onValueChangedInvoked", "Assembly-CSharp"), "onValueChangedInvoked was false");
            Assert.IsTrue(inputField.GetComponentProperty<bool>("AltInputFieldRaisedEvents", "onSubmitInvoked", "Assembly-CSharp"), "onSubmitInvoked was false");
            Assert.IsTrue(inputField.GetComponentProperty<bool>("AltInputFieldRaisedEvents", "onEndEditInvoked", "Assembly-CSharp"), "onEndEditInvoked was false");
        }

        [TestCase(By.COMPONENT, "AltRunner", "//AltTesterPrefab")]
        [TestCase(By.COMPONENT, "CapsuleCollider", "//Capsule")] //Unity component
        [TestCase(By.COMPONENT, "AltTester.AltDriver.AltRunner", "//AltTesterPrefab")] // namespace
        [TestCase(By.ID, "13b211d0-eafa-452d-8708-cc70f5075e93", "//Capsule")]
        [TestCase(By.LAYER, "Water", "//Capsule")]
        [TestCase(By.NAME, "Capsule", "//Capsule")]
        [TestCase(By.PATH, "/Sphere", "//Sphere")]
        [TestCase(By.PATH, "//PlaneS/..", "//Sphere")]
        [TestCase(By.PATH, "//Sphere/*", "//PlaneS")]
        [TestCase(By.PATH, "//*[@tag=plane]", "//Plane")]
        [TestCase(By.PATH, "//*[@layer=Water]", "//Capsule")]
        [TestCase(By.PATH, "//*[@name=Capsule]", "//Capsule")]
        [TestCase(By.PATH, "//*[@component=CapsuleCollider]", "//Capsule")]
        [TestCase(By.PATH, "//*[@id=13b211d0-eafa-452d-8708-cc70f5075e93]", "//Capsule")]
        [TestCase(By.PATH, "//*[@text=Change Camera Mode]", "/Canvas/Button/Text")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default]", "//Plane")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane]", "//Plane")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane][@component=MeshCollider]", "//Plane")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane][@component=MeshCollider][@id=58af4167-0971-415f-901c-7c5226c3c170]", "//Plane")]
        [TestCase(By.PATH, "//*[@tag=Untagged][@layer=UI][@name=Text][@component=CanvasRenderer][@id=0ffed8a8-3d77-4b03-965b-5ae094ba9511][@text=Change Camera Mode]", "/Canvas/Button/Text")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)]", "//Plane")]
        [TestCase(By.PATH, "//*[contains(@layer,Wat)]", "//Capsule")]
        [TestCase(By.PATH, "//*[contains(@name,Cap)]", "//Capsule")]
        [TestCase(By.PATH, "//*[contains(@component,CapsuleColl)]", "//Capsule")]
        [TestCase(By.PATH, "//*[contains(@id,13b211d0-eafa-452d-8708-cc70f5075e93)]", "//Capsule")]
        [TestCase(By.PATH, "//*[contains(@text,Change Camera)]", "/Canvas/Button/Text")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)]", "//Plane")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)]", "//Plane")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)][contains(@component,MeshColl)]", "//Plane")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)][contains(@component,MeshColl)][contains(@id,58af4167-0971-415f-901c-7c5226c3c170)]", "//Plane")]
        [TestCase(By.PATH, "//*[contains(@tag,Untag)][contains(@layer,U)][contains(@name,Tex)][contains(@component,CanvasRender)][contains(@id,0ffed8a8-3d77-4b03-965b-5ae094ba9511)][contains(@text,Change Camera)]", "/Canvas/Button/Text")]
        [TestCase(By.TAG, "plane", "//Plane")]
        [TestCase(By.TEXT, "Capsule Info", "//CapsuleInfo")] // text area
        [TestCase(By.TEXT, "Change Camera Mode", "//Canvas/Button/Text")] // button with text
        public void TestFindObject(By by, string value, string path)
        {
            int referenceId = altDriver.FindObject(By.PATH, path).id;
            var altObject = altDriver.FindObject(by, value);
            Assert.NotNull(altObject);
            Assert.AreEqual(referenceId, altObject.id);
        }

        [Test]
        public void TestGetComponentProperty()
        {
            const string componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
            const string propertyName = "InstrumentationSettings.AppName";
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            Assert.NotNull(altElement);
            var propertyValue = altElement.GetComponentProperty<string>(componentName, propertyName, "AltTester.AltTesterUnitySDK");

            Assert.AreEqual("__default__", propertyValue);
        }
        [Test]
        public void TestWaitForComponentPropertyComponentNotFound()
        {
            const string componentName = "AltTester.AltTesterUnitySDK.AltRunnerTest";
            const string propertyName = "InstrumentationSettings.AppName";
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            Assert.NotNull(altElement);
            Assert.Throws<ComponentNotFoundException>(() => altElement.WaitForComponentProperty(componentName, propertyName, "Test", "AltTester.AltTesterUnitySDK"));
        }
        [Test]
        public void TestWaitForComponentPropertyNotFound()
        {
            const string componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
            const string propertyName = "InstrumentationSettings.AltServerPortTest";
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            Assert.NotNull(altElement);
            Assert.Throws<PropertyNotFoundException>(() => altElement.WaitForComponentProperty(componentName, propertyName, "Test", "AltTester.AltTesterUnitySDK"));
        }
        [Test]
        public void TestWaitForComponentPropertyTimeOut()
        {
            const string componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
            const string propertyName = "InstrumentationSettings.AltServerPort";
            const string assemblyName = "AltTester.AltTesterUnitySDK";
            const string initialPropertyValue = "13005";
            const string testPropertyValue = "Test";
            const int timeout = 2;
            
            AltObject altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            Assert.NotNull(altElement);
            try
            {
                altElement.WaitForComponentProperty(componentName, propertyName, testPropertyValue, assemblyName, timeout);
                Assert.Fail();
            }
            catch (WaitTimeOutException exception)
            {
                Assert.That(exception.Message, Is.EqualTo($"Property {propertyName} was {initialPropertyValue} and was not {testPropertyValue} after {timeout} seconds"));
            }
        }

        [Category("WebGLUnsupported")] // Fails on WebGL in pipeline, skip until issue #1465 is fixed: https://github.com/alttester/AltTester-Unity-SDK/issues/1465
        [Test]
        public void TestWaitForComponentPropertyAssemblyNotFound()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "InstrumentationSettings.AltServerPort";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            Assert.Throws<AssemblyNotFoundException>(() => altElement.WaitForComponentProperty(componentName, propertyName, "13000", "Assembly-CSharpTest"));
        }

        [Test]
        public void TestWaitForComponentProperty()
        {
            const string componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
            const string propertyName = "InstrumentationSettings.AppName";
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");

            Assert.NotNull(altElement);

            var propertyValue = altElement.WaitForComponentProperty(componentName, propertyName, "__default__", "AltTester.AltTesterUnitySDK");

            Assert.AreEqual("__default__", propertyValue);
        }


        [Test]
        [Ignore("This test is failing because of https://github.com/alttester/AltTester-Unity-SDK/issues/1185")]
        public void TestWaitForNonExistingComponentProperty()
        {
            const string componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
            const string propertyName = "InstrumentationSettings.AltServerPort";
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            Assert.NotNull(altElement);
            Assert.Throws<NotFoundException>(() => altElement.WaitForComponentProperty<String>(componentName, propertyName, "UNEXISTING", "AltTester.AltTesterUnitySDK", timeout: 10));
        }

        [Category("WebGLUnsupported")] // Fails on WebGL in pipeline, skip until issue #1465 is fixed: https://github.com/alttester/AltTester-Unity-SDK/issues/1465
        [TestCase("UNEXISTING", "InstrumentationSettings.AltServerPort", "AltTester.AltTesterUnitySDK", "Component not found")]
        [TestCase("AltTester.AltTesterUnitySDK.Commands.AltRunner", "UNEXISTING", "AltTester.AltTesterUnitySDK", "Property UNEXISTING not found")]
        // [TestCase( "AltTester.AltTesterUnitySDK.Commands.AltRunner","InstrumentationSettings.AltServerPort", "UNEXISTING", "Assembly UNEXISTING not found")] -> This test is failing because of https://github.com/alttester/AltTester-Unity-SDK/issues/1185. This test can be uncomment when the issue is fixed
        public void TestWaitForComponentPropertyNonExistingParameters(string componentName, string propertyName, string assemblyName, string message)
        {
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            int port = TestsHelper.GetAltDriverPort();
            try
            {
                altElement.WaitForComponentProperty<int>(componentName, propertyName, port, assemblyName, timeout: 10);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(message, e.Message);
            }
        }

        [Test]
        public void TestGetComponentPropertyInvalidDeserialization()
        {
            const string componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
            const string propertyName = "InstrumentationSettings.ResetConnectionData";
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            try
            {
                var propertyValue = altElement.GetComponentProperty<int>(componentName, propertyName, "AltTester.AltTesterUnitySDK");
                Assert.Fail("Expected ResponseFormatException");
            }
            catch (ResponseFormatException ex)
            {
                Assert.AreEqual("Could not deserialize response data: `true` into System.Int32", ex.Message);
            }
        }

        [Category("WebGLUnsupported")] // Fails on WebGL in pipeline, skip until issue #1465 is fixed: https://github.com/alttester/AltTester-Unity-SDK/issues/1465
        [TestCase("UnityEngine.UI.Text", "InvalidProperty", "UnityEngine.UI", "Property InvalidProperty not found")]
        [TestCase("UNEXISTING", "m_Text", "UnityEngine.UI", "Component not found")]
        [TestCase("UnityEngine.UI.Text", "m_Text", "UNEXISTING", "Assembly not found")]
        public void TestGetComponentPropertyNonExistingParams(string component, string property, string assembly, string message)
        {
            Thread.Sleep(1000);
            var altElement = altDriver.FindObject(By.PATH, "/Canvas/UIButton/Text");
            Assert.NotNull(altElement);
            try
            {
                var propertyValue = altElement.GetComponentProperty<String>(component, property, assembly);
                Assert.Fail();
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.StartsWith(message), exception.Message);
            }
        }





        [Test]
        public void TestGetComponentPropertyArray()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "arrayOfInts";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            var propertyValue = altElement.GetComponentProperty<int[]>(componentName, propertyName, "Assembly-CSharp");

            Assert.AreEqual(3, propertyValue.Length);
            Assert.AreEqual(1, propertyValue[0]);
            Assert.AreEqual(2, propertyValue[1]);
            Assert.AreEqual(3, propertyValue[2]);
        }
        [Test]
        public void TestGetComponentPropertyPrivate()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "privateVariable";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            var propertyValue = altElement.GetComponentProperty<int>(componentName, propertyName, "Assembly-CSharp");
            Assert.AreEqual(0, propertyValue);
        }

        [Test]
        public void TestGetComponentPropertyStatic()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "privateStaticVariable";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            var propertyValue = altElement.GetComponentProperty<int>(componentName, propertyName, "Assembly-CSharp");
            Assert.AreEqual(0, propertyValue);
        }

        [Test]
        public void TestGetComponentPropertyNullValue()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "fieldNullValue";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            var propertyValue = altElement.GetComponentProperty<object>(componentName, propertyName, "Assembly-CSharp");
            Assert.AreEqual(null, propertyValue);
        }

        [Test]
        public void TestGetComponentPropertyStaticPublic()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "PublicStaticVariable";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            var propertyValue = altElement.GetComponentProperty<int>(componentName, propertyName, "Assembly-CSharp");
            Assert.AreEqual(0, propertyValue);
        }


#if !UNITY_IOS
        [Test]
        public void TestGetComponentPropertyUnityEngine()
        {
            const string componentName = "UnityEngine.CapsuleCollider";
            const string propertyName = "isTrigger";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            var propertyValue = altElement.GetComponentProperty<bool>(componentName, propertyName, "UnityEngine.PhysicsModule");
            Assert.AreEqual(false, propertyValue);
        }

#endif

        [Test]
        public void TestSetComponentProperty()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "stringToSetFromTests";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            altElement.SetComponentProperty(componentName, propertyName, "2", "Assembly-CSharp");

            var propertyValue = altElement.GetComponentProperty<string>(componentName, propertyName, "Assembly-CSharp");
            Assert.AreEqual("2", propertyValue);
        }

        [Test]
        public void TestSetNonExistingComponentProperty()
        {
            const string unexistingComponent = "Capsulee";
            const string propertyName = "stringToSetFromTests";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            try
            {
                altElement.SetComponentProperty(unexistingComponent, propertyName, "2", "Assembly-CSharp");
                Assert.Fail();
            }
            catch (ComponentNotFoundException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Component not found"), exception.Message);
            }
        }

        [Test]
        public void TestSetNonExistingProperty()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string unexistingPropertyName = "unexisting";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            try
            {
                altElement.SetComponentProperty(componentName, unexistingPropertyName, "2", "Assembly-CSharp");
                Assert.Fail();
            }
            catch (PropertyNotFoundException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith($"Property {unexistingPropertyName} not found"), exception.Message);
            }
        }

        [Category("WebGLUnsupported")] // Fails on WebGL in pipeline, skip until issue #1465 is fixed: https://github.com/alttester/AltTester-Unity-SDK/issues/1465
        [Test]
        public void TestSetComponentPropertyNonExistingAssembly()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "stringToSetFromTests";
            const string nonExistingAssembly = "unexisting";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            try
            {
                altElement.SetComponentProperty(componentName, propertyName, "2", nonExistingAssembly);
                Assert.Fail();
            }
            catch (AssemblyNotFoundException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Assembly not found"), exception.Message);
            }
        }

        // Ask if there should be a specific error for this case
        [Test]
        public void TestSetComponentPropertyBadValue()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "stringToSetFromTests";
            const string assembly = "Assembly-CSharp";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            altElement.SetComponentProperty(componentName, propertyName, null, assembly);
            var property_value = altElement.GetComponentProperty<String>(componentName, propertyName, assembly);
            Assert.AreEqual(property_value, null);

        }

        [Test]
        public void TestCallMethodWithNoParameters()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "UIButtonClicked";
            const string assemblyName = "Assembly-CSharp";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var data = altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, new object[] { });
            Assert.IsNull(data);
        }

        [Test]
        public void TestCallMethodWithParameters()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "Jump";
            const string assemblyName = "Assembly-CSharp";
            string[] parameters = new[] { "New Text" };
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var data = altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, parameters);
            Assert.IsNull(data);
        }

        [Test]
        public void TestGetTextCallMethodWithNoParameters()
        {
            const string componentName = "UnityEngine.UI.Text";
            const string methodName = "get_text";
            const string assemblyName = "UnityEngine.UI";
            const string elementText = "Change Camera Mode";
            var altElement = altDriver.FindObject(By.PATH, "/Canvas/Button/Text");
            var data = altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, new object[] { });
            Assert.AreEqual(elementText, data);
        }

        [Test]
        public void TestCallMethodSetFontSizeWithParameters()
        {
            const string componentName = "UnityEngine.UI.Text";
            const string methodName = "set_fontSize";
            const string methodToVerifyName = "get_fontSize";
            const string assemblyName = "UnityEngine.UI";
            Int32 fontSizeExpected = 16;
            string[] parameters = new[] { "16" };
            var altElement = altDriver.FindObject(By.PATH, "/Canvas/UnityUIInputField/Text");
            var data = altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, parameters);
            var fontSize = altElement.CallComponentMethod<Int32>(componentName, methodToVerifyName, assemblyName, new object[] { });
            Assert.AreEqual(fontSizeExpected, fontSize);
        }

        [Test]
        public void TestCallMethodWithManyParameters()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithManyParameters";
            const string assemblyName = "Assembly-CSharp";
            object[] parameters = new object[4] { 1, "stringparam", 0.5, new[] { 1, 2, 3 } };
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var data = altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, parameters);
            Assert.IsNull(data);
        }


        [Test]
        public void TestCallMethodWithOptionalParameters()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithOptionalParameters";
            const string assemblyName = "Assembly-CSharp";
            string[] parameters = new[] { "1", "2" };
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var data = altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, parameters);
            Assert.AreEqual("3", data);
        }

        [Test]
        public void TestCallMethodWithOptionalParametersString()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithOptionalParameters";
            const string assemblyName = "Assembly-CSharp";
            object[] parameters = new[] { "FirstParameter", "SecondParameter" };
            string[] typeOfParameters = new[] { "System.String", "System.String" };
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var data = altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, parameters, typeOfParameters);
            Assert.AreEqual("FirstParameterSecondParameter", data);
        }

        [Test]
        public void TestCallMethodWithOptionalParametersString2()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithOptionalParameters";
            const string assemblyName = "Assembly-CSharp";
            object[] parameters = new[] { "FirstParameter", "" };
            string[] typeOfParameters = new[] { "System.String", "System.String" };
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var data = altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, parameters, typeOfParameters);
            Assert.AreEqual("FirstParameter", data);
        }

        [Test]
        public void TestCallMethodWithIncorrectNumberOfParameters()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithManyParameters";
            const string assemblyName = "Assembly-CSharp";
            object[] parameters = new object[3] { 1, "stringparam", new int[] { 1, 2, 3 } };
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            try
            {
                altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, parameters);
                Assert.Fail();
            }
            catch (MethodWithGivenParametersNotFoundException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("No method found with 3 parameters matching signature: TestMethodWithManyParameters()"), exception.Message);
            }
        }

        [Test]
        public void TestCallMethodWithIncorrectNumberOfParameters2()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithManyParameters";
            const string assemblyName = "Assembly-CSharp";
            object[] parameters = new object[3] { "a", "stringparam", new[] { 1, 2, 3 } };
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            try
            {
                altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, parameters);
                Assert.Fail();
            }
            catch (MethodWithGivenParametersNotFoundException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("No method found with 3 parameters matching signature: TestMethodWithManyParameters()"), exception.Message);
            }
        }


        [Test]
        public void TestCallMethodAssemblyNotFound()
        {
            const string componentName = "RandomComponent";
            const string methodName = "TestMethodWithManyParameters";
            object[] parameters = new object[4] { "a", "stringparam", 0.5, new[] { 1, 2, 3 } };
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            try
            {
                altElement.CallComponentMethod<string>(componentName, methodName, "RandomAssembly", parameters, null);
                Assert.Fail();
            }
            catch (AssemblyNotFoundException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Assembly not found"), exception.Message);
            }
        }



        [Test]
        public void TestCallMethodInvalidMethodArgumentTypes()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithManyParameters";
            const string assemblyName = "Assembly-CSharp";

            object[] parameters = new object[4] { "stringnoint", "stringparam", 0.5, new[] { 1, 2, 3 } };
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            try
            {
                altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, parameters);
                Assert.Fail();
            }
            catch (FailedToParseArgumentsException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Could not parse parameter"), exception.Message);
            }
        }

        [Test]
        public void TestCallMethodInvalidParameterType()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithManyParameters";
            const string assemblyName = "Assembly-CSharp";

            object[] parameters = new object[4] { 1, "stringparam", 0.5, new int[] { 1, 2, 3 } };
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            try
            {
                altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, parameters, new[] { "System.Stringggggg" });
                Assert.Fail();
            }
            catch (InvalidParameterTypeException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Number of parameters different than number of types of parameters"), exception.Message);
            }
        }

        [Test]
        public void TestSetKeyInt()
        {
            altDriver.DeletePlayerPref();
            altDriver.SetKeyPlayerPref("test", 1);
            var val = altDriver.GetIntKeyPlayerPref("test");
            Assert.AreEqual(1, val);
        }

        [Test]
        public void TestSetKeyFloat()
        {
            altDriver.DeletePlayerPref();
            altDriver.SetKeyPlayerPref("test", 1f);
            var val = altDriver.GetFloatKeyPlayerPref("test");
            Assert.AreEqual(1f, val);
        }

        [Test]
        public void TestSetKeyString()
        {
            altDriver.DeletePlayerPref();
            altDriver.SetKeyPlayerPref("test", "test");
            var val = altDriver.GetStringKeyPlayerPref("test");
            Assert.AreEqual("test", val);
        }

        [Test]
        public void TestDeleteKey()
        {
            altDriver.DeletePlayerPref();
            altDriver.SetKeyPlayerPref("test", 1);
            var val = altDriver.GetIntKeyPlayerPref("test");
            Assert.AreEqual(1, val);
            altDriver.DeleteKeyPlayerPref("test");
            try
            {
                altDriver.GetIntKeyPlayerPref("test");
                Assert.Fail();
            }
            catch (NotFoundException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("PlayerPrefs key test not found"), exception.Message);
            }

        }

        [Test]
        public void TestDifferentCamera()
        {
            var altButton = altDriver.FindObject(By.NAME, "Button", By.NAME, "Main Camera");
            altButton.Click();
            altButton.Click();
            var altElement = altDriver.FindObject(By.NAME, "Capsule", By.NAME, "Main Camera");
            var altElement2 = altDriver.FindObject(By.NAME, "Capsule", By.NAME, "Camera");
            AltVector2 pozOnScreenFromMainCamera = new AltVector2(altElement.x, altElement.y);
            AltVector2 pozOnScreenFromSecondaryCamera = new AltVector2(altElement2.x, altElement2.y);

            Assert.AreNotEqual(pozOnScreenFromSecondaryCamera, pozOnScreenFromMainCamera);

        }


        [TestCase(By.COMPONENT, "NonExistent")]
        [TestCase(By.ID, "NonExistent")]
        [TestCase(By.LAYER, "NonExistent")]
        [TestCase(By.NAME, "NonExistent")]
        [TestCase(By.PATH, "/NonExistent")]
        [TestCase(By.PATH, "//NonExistent/..")]
        [TestCase(By.PATH, "//NonExistent/*")]
        [TestCase(By.PATH, "//*[@tag=NonExistent]")]
        [TestCase(By.PATH, "//*[@layer=NonExistent]")]
        [TestCase(By.PATH, "//*[@name=NonExistent]")]
        [TestCase(By.PATH, "//*[@component=NonExistent]")]
        [TestCase(By.PATH, "//*[@id=NonExistent]")]
        [TestCase(By.PATH, "//*[@text=NonExistent]")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=NonExistent]")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=NonExistent]")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane][@component=NonExistent]")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane][@component=MeshCollider][@id=NonExistent]")]
        [TestCase(By.PATH, "//*[@tag=Untagged][@layer=UI][@name=Text][@component=CanvasRenderer][@id=f9dc3b3c-2791-42dc-9c0f-87ef7ff5e11d][@text=NonExistent]")]
        [TestCase(By.PATH, "//*[contains(@tag,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@layer,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@name,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@component,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@id,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@text,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)][contains(@component,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)][contains(@component,MeshColl)][contains(@id,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,Untag)][contains(@layer,U)][contains(@name,Tex)][contains(@component,CanvasRender)][contains(@id,f9dc3b3c-2791-42dc-9c0f-87ef7ff5e11d)][contains(@text,NonExistent)]")]
        [TestCase(By.PATH, "//Canvas[100]")]
        [TestCase(By.PATH, "//Canvas[-100]")]
        [TestCase(By.TAG, "NonExistent")]
        [TestCase(By.TEXT, "NonExistent")]
        [TestCase(By.PATH, "//DisabledObject")]
        [TestCase(By.PATH, "//DisabledObject/DisabledChild")]
        public void TestFindNonExistentObject(By by, string value)
        {
            Assert.Throws<NotFoundException>(() => altDriver.FindObject(by, value));
        }

        [Test]
        public void TestHoldButton()
        {
            const int duration = 1;
            var button = altDriver.FindObject(By.NAME, "UIButton");
            altDriver.HoldButton(button.GetScreenPosition(), duration);
            var capsuleInfo = altDriver.FindObject(By.NAME, "CapsuleInfo");
            var text = capsuleInfo.GetText();
            Assert.AreEqual(text, "UIButton clicked to jump capsule!");
            var time = float.Parse(altDriver.FindObject(By.NAME, "ChineseLetters").GetText());
            Assert.Greater(time, duration);
        }

        [Test]
        [Ignore("Ignore PressKey method")]
        public void TestPressKeyWaitTheDuration()
        {
            const float duration = 1.0f;
            var button = altDriver.FindObject(By.NAME, "UIButton");
            altDriver.MoveMouse(button.GetScreenPosition());
            altDriver.PressKey(AltKeyCode.Mouse0, 1, duration);
            var time = float.Parse(altDriver.FindObject(By.NAME, "ChineseLetters").GetText());
            Assert.That(time, Is.GreaterThanOrEqualTo(duration));
            Assert.That(time, Is.LessThan(duration + 0.1f));
        }

        [Test]
        [Obsolete]
        public void TestClickElement()
        {
            const string name = "Capsule";
            var altElement = altDriver.FindObject(By.NAME, name).Tap();
            altDriver.WaitForObject(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]");
            var CapsuleInfo = altDriver.WaitForObject(By.PATH, "/Canvas/CapsuleInfo");
            var m_Text = CapsuleInfo.GetComponentProperty<String>("UnityEngine.UI.Text", "m_Text", "UnityEngine.UI").ToString();
            Assert.That(m_Text, Is.EqualTo("Capsule was clicked to jump!"));
        }

        [Test]
        public void CapsuleJumpWhenHold()
        {
            const string name = "Capsule";
            var altElement = altDriver.FindObject(By.NAME, name);
            altDriver.HoldButton(altElement.GetScreenPosition(), 1.5f);
            altDriver.WaitForObject(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]");
            var CapsuleInfo = altDriver.WaitForObject(By.PATH, "/Canvas/CapsuleInfo");
            var m_Text = CapsuleInfo.GetComponentProperty<String>("UnityEngine.UI.Text", "m_Text", "UnityEngine.UI").ToString();
            Assert.That(m_Text, Is.EqualTo("Capsule was clicked to jump!"));
        }

        [Test]
        public void TestWaitForObjectToNotExist()
        {
            altDriver.WaitForObjectNotBePresent(By.NAME, "ObjectDestroyedIn5Secs");
            altDriver.WaitForObjectNotBePresent(By.NAME, "Capsulee", timeout: 1, interval: 0.5f);
        }

        [Test]
        public void TestWaitForObjectToNotExistFail()
        {
            try
            {
                altDriver.WaitForObjectNotBePresent(By.NAME, "Capsule", timeout: 1, interval: 0.5f);
                Assert.Fail();
            }
            catch (WaitTimeOutException exception)
            {
                Assert.AreEqual("Element //Capsule still found after 1 seconds", exception.Message);
            }
        }

        [Test]
        public void TestWaitForCurrentSceneToBeANonExistingScene()
        {
            const string name = "AltDriverTestScene";
            try
            {
                altDriver.WaitForCurrentSceneToBe(name, 1);
                Assert.Fail();
            }
            catch (WaitTimeOutException exception)
            {
                Assert.AreEqual("Scene AltDriverTestScene not loaded after 1 seconds", exception.Message);
            }
        }

        [Test]
        public void TestCallStaticMethod()
        {
            altDriver.CallStaticMethod<string>("UnityEngine.PlayerPrefs", "SetInt", "UnityEngine.CoreModule", new[] { "Test", "1" });
            int a = altDriver.CallStaticMethod<int>("UnityEngine.PlayerPrefs", "GetInt", "UnityEngine.CoreModule", new[] { "Test", "2" });
            Assert.AreEqual(1, a);
        }

        [Test]
        public void TestCallStaticNonExistentMethod()
        {
            Assert.Throws<MethodNotFoundException>(() => altDriver.CallStaticMethod<int>("UnityEngine.PlayerPrefs", "UNEXISTING", "UnityEngine.CoreModule", new[] { "Test", "2" }));
        }

        [Category("WebGLUnsupported")] // Fails on WebGL in pipeline, skip until issue #1465 is fixed: https://github.com/alttester/AltTester-Unity-SDK/issues/1465
        [Test]
        public void TestCallStaticMethodNonExistingAssembly()
        {
            Assert.Throws<AssemblyNotFoundException>(() => altDriver.CallStaticMethod<int>("UnityEngine.PlayerPrefs", "GetInt", "UNEXISTING", new[] { "Test", "2" }));
        }

        [Category("WebGLUnsupported")] // Fails on WebGL in pipeline, skip until issue #1465 is fixed: https://github.com/alttester/AltTester-Unity-SDK/issues/1465
        [Test] // to check what error should be triggered
        public void TestCallStaticMethodNonExistingTypeName()
        {
            Assert.Throws<ComponentNotFoundException>(() => altDriver.CallStaticMethod<int>("UNEXISTING", "GetInt", "UnityEngine.CoreModule", new[] { "Test", "2" }));
        }

        [Test]
        public void TestCallMethodWithMultipleDefinitions()
        {

            AltObject capsule = altDriver.FindObject(By.NAME, "Capsule");
            capsule.CallComponentMethod<string>("AltExampleScriptCapsule", "Test", "Assembly-CSharp", new[] { "2" }, new[] { "System.Int32" });
            AltObject capsuleInfo = altDriver.FindObject(By.NAME, "CapsuleInfo");
            Assert.AreEqual("6", capsuleInfo.GetText());
        }

        [Test]
        public void TestCallMethodWithAssembly()
        {
            AltObject capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialRotation = capsule.GetComponentProperty<dynamic>("UnityEngine.Transform", "rotation", "UnityEngine.CoreModule");
            capsule.CallComponentMethod<string>("UnityEngine.Transform", "Rotate", "UnityEngine.CoreModule", new object[3] { 10, 10, 10 }, new[] { "System.Single", "System.Single", "System.Single" });
            AltObject capsuleAfterRotation = altDriver.FindObject(By.NAME, "Capsule");
            var finalRotation = capsuleAfterRotation.GetComponentProperty<dynamic>("UnityEngine.Transform", "rotation", "UnityEngine.CoreModule");
            Assert.IsTrue(initialRotation["x"] != finalRotation["x"] || initialRotation["y"] != finalRotation["y"] || initialRotation["z"] != finalRotation["z"] || initialRotation["w"] != finalRotation["w"]);
        }

        [Test]
        public void TestGetAllComponents()
        {
            List<AltComponent> components = altDriver.FindObject(By.NAME, "Canvas").GetAllComponents();
            Assert.AreEqual(5, components.Count);
            Assert.AreEqual("UnityEngine.RectTransform", components[0].componentName);
            Assert.AreEqual("UnityEngine.CoreModule", components[0].assemblyName);
        }

        [Test]
        public void TestGetAllMethodsFromClass()
        {
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var component2 = altElement.GetAllComponents().First(component => component.componentName.Equals("AltExampleScriptCapsule"));
            List<string> methods = altElement.GetAllMethods(component2, AltMethodSelection.CLASSMETHODS);
            Assert.IsTrue(methods.Contains("Void UIButtonClicked()"));
            Assert.IsFalse(methods.Contains("Void CancelInvoke(System.String)"));
        }
        [Test]
        public void TestGetAllMethodsFromInherited()
        {
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var component2 = altElement.GetAllComponents().First(component => component.componentName.Equals("AltExampleScriptCapsule"));
            List<string> methods = altElement.GetAllMethods(component2, AltMethodSelection.INHERITEDMETHODS);
            Assert.IsTrue(methods.Contains("Void CancelInvoke(System.String)"));
            Assert.IsFalse(methods.Contains("Void UIButtonClicked()"));
        }
        [Test]
        public void TestGetAllMethods()
        {
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var component2 = altElement.GetAllComponents().First(component => component.componentName.Equals("AltExampleScriptCapsule"));
            List<string> methods = altElement.GetAllMethods(component2, AltMethodSelection.ALLMETHODS);
            Assert.IsTrue(methods.Contains("Void CancelInvoke(System.String)"));
            Assert.IsTrue(methods.Contains("Void UIButtonClicked()"));
        }

        [Test]
        public void TestGetAllProperties()
        {
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var componentList = altElement.GetAllComponents();
            var component = componentList.First(componenta =>
                componenta.componentName.Equals("AltExampleScriptCapsule") && componenta.assemblyName.Equals("Assembly-CSharp"));
            List<AltProperty> properties = altElement.GetAllProperties(component, AltPropertiesSelections.ALLPROPERTIES);
            if (properties.Exists(prop => prop.name.Equals("runInEditMode")))
            {
                Assert.AreEqual(12, properties.Count); // runInEditMode and allowPrefabModeInPlayMode
            }
            else
            {
                Assert.IsTrue(properties.Count >= 9 && properties.Count <= 10);// if runned from editor then there are 12 properties, runInEditMode is only available in Editor
            }
            AltProperty property = properties.First(prop => prop.name.Equals("TestProperty"));
            Assert.NotNull(property);
            Assert.AreEqual("False", property.value);
        }
        [Test]
        public void TestGetAllClassProperties()
        {
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var componentList = altElement.GetAllComponents();
            var component = componentList.First(componenta =>
                componenta.componentName.Equals("AltExampleScriptCapsule") && componenta.assemblyName.Equals("Assembly-CSharp"));
            List<AltProperty> properties = altElement.GetAllProperties(component, AltPropertiesSelections.CLASSPROPERTIES);
            Assert.AreEqual(2, properties.Count);
            AltProperty property = properties.First(prop => prop.name.Equals("TestProperty"));
            Assert.NotNull(property);
            Assert.AreEqual("False", property.value);
        }
        [Test]
        public void TestGetAllInheritedProperties()
        {
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var componentList = altElement.GetAllComponents();
            var component = componentList.First(componenta =>
                componenta.componentName.Equals("AltExampleScriptCapsule") && componenta.assemblyName.Equals("Assembly-CSharp"));
            List<AltProperty> properties = altElement.GetAllProperties(component, AltPropertiesSelections.INHERITEDPROPERTIES);
            if (properties.Exists(prop => prop.name.Equals("runInEditMode")))
            {
                Assert.AreEqual(10, properties.Count);//runInEditMode and allowPrefabModeInPlayMode
            }
            else
            {
                Assert.IsTrue(properties.Count >= 7 && properties.Count <= 8);// if runned from editor then there are 10 properties, runInEditMode is only available in Editor
            }
        }

        [Test]
        public void TestGetAllClassFields()
        {
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var componentList = altElement.GetAllComponents();
            var component = componentList.First(componenta =>
                componenta.componentName.Equals("AltExampleScriptCapsule") && componenta.assemblyName.Equals("Assembly-CSharp"));

            List<AltProperty> fields = altElement.GetAllFields(component, AltFieldsSelections.CLASSFIELDS);
            Assert.AreEqual(16, fields.Count);
        }

        [Test]
        public void TestGetAllInheritedFields()
        {
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var componentList = altElement.GetAllComponents();
            var component = componentList.First(componenta =>
                componenta.componentName.Equals("AltExampleScriptCapsule") && componenta.assemblyName.Equals("Assembly-CSharp"));
            List<AltProperty> fields = altElement.GetAllFields(component, AltFieldsSelections.INHERITEDFIELDS);
            AltProperty field = fields.First(fld => fld.name.Equals("inheritedField"));
            Assert.AreEqual("False", field.value);
            Assert.AreEqual(1, fields.Count);
            Assert.AreEqual(AltType.PRIMITIVE, field.type);
        }

        [Test]
        public void TestGetAllFields()
        {
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var componentList = altElement.GetAllComponents();
            var component = componentList.First(componenta =>
                componenta.componentName.Equals("AltExampleScriptCapsule") && componenta.assemblyName.Equals("Assembly-CSharp"));
            List<AltProperty> fields = altElement.GetAllFields(component, AltFieldsSelections.ALLFIELDS);
            Assert.AreEqual(17, fields.Count);
        }

        [Test]
        public void TestGetAllScenes()
        {
            var scenes = altDriver.GetAllScenes();
            Assert.AreEqual(14, scenes.Count);
            Assert.AreEqual("Scene 1 AltDriverTestScene", scenes[0]);
        }


        [Test]
        public void TestSetTimeScale()
        {
            altDriver.SetTimeScale(0.1f);
            Thread.Sleep(1000);
            var timeScaleFromGame = altDriver.GetTimeScale();
            Assert.AreEqual(0.1f, timeScaleFromGame);
            altDriver.SetTimeScale(1);
        }

        [Test]
        public void TestGetAllCameras()
        {
            var cameras = altDriver.GetAllCameras();
            Assert.AreEqual(2, cameras.Count);
        }

        [Test]
        public void TestGetAllActiveCameras()
        {
            var cameras = altDriver.GetAllActiveCameras();
            Assert.AreEqual(1, cameras.Count);
        }

        [Test]
        public void TestGetAllElementsLight()
        {
            var altElements = altDriver.GetAllElementsLight();

            Assert.IsTrue(altElements.Count > 0);
            foreach (var altElementLight in altElements)
            {
                Assert.IsTrue(altElementLight.enabled);
                Assert.IsTrue(altElementLight.id != 0);
            }
        }

        [Test]
        public void TestGetScreenshot()
        {
            var path = "testC.png";
            altDriver.GetPNGScreenshot(path);
            FileAssert.Exists(path);
        }

        [TestCase("ChineseLetters", "哦伊娜哦")]
        [TestCase("NonEnglishText", "BJÖRN'S PASS")]
        public void TestGetChineseLetters(string name, string nonEnglishText)
        {
            var text = altDriver.FindObject(By.NAME, name).GetText();
            Assert.AreEqual(nonEnglishText, text);
        }

        [Test]
        public void TestDoubleTap()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var counterButtonText = altDriver.FindObject(By.NAME, "ButtonCounter/Text");
            counterButton.Tap(2);
            Thread.Sleep(500);
            Assert.AreEqual("2", counterButtonText.GetText());
        }

        [Test]
        public void TestSwipeWithDuration0()
        {
            altDriver.FindObject(By.NAME, "ButtonCounter");
            var counterButtonText = altDriver.FindObject(By.NAME, "ButtonCounter/Text");
            altDriver.Swipe(counterButtonText.GetScreenPosition(), counterButtonText.GetScreenPosition(), 0, wait: false);
            Thread.Sleep(500);
            Assert.AreEqual("1", counterButtonText.GetText());
        }

        [Test]
        public void TestCustomTap()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var counterButtonText = altDriver.FindObject(By.NAME, "ButtonCounter/Text");
            altDriver.Tap(counterButton.GetScreenPosition(), 4);
            Thread.Sleep(1000);
            Assert.AreEqual("4", counterButtonText.GetText());
        }
        [Test]
        public void TestGet3DObjectFromScreenshot()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            AltObject altObject;
            altDriver.GetScreenshot(new AltVector2(capsule.x, capsule.y), new AltColor(1, 0, 0, 1), 1, out altObject, new AltVector2(1920, 1080));
            Assert.AreEqual("Capsule", altObject.name);
        }

        [Test]
        public void TestGetUIObjectFromScreenshot()
        {
            var capsuleInfo = altDriver.FindObject(By.NAME, "CapsuleInfo");
            AltObject altObject;
            altDriver.GetScreenshot(new AltVector2(capsuleInfo.x, capsuleInfo.y), new AltColor(1, 0, 0, 1), 1, out altObject, new AltVector2(1920, 1080));
            Assert.AreEqual("CapsuleInfo", altObject.name);
        }
        [Test]
        public void TestObjectFromScreenshot()
        {
            var icon = altDriver.FindObject(By.NAME, "Icon");
            AltVector2 offscreenCoordinates = new AltVector2(icon.x + 400, icon.y);
            AltObject altObject;
            altDriver.GetScreenshot(offscreenCoordinates, new AltColor(1, 0, 0, 1), 1, out altObject, new AltVector2(1920, 1080));
            Assert.IsNull(altObject);
        }

        [Test]
        public void TestPressNextSceneButton()
        {
            var initialScene = altDriver.GetCurrentScene();
            altDriver.FindObject(By.NAME, "NextScene").Tap();
            var currentScene = altDriver.GetCurrentScene();
            Assert.AreNotEqual(initialScene, currentScene);
        }
        [Test]
        public void TestForSetText()
        {
            var text = altDriver.FindObject(By.NAME, "NonEnglishText");
            var originalText = text.GetText();
            var afterText = text.SetText("ModifiedText").GetText();
            Assert.AreNotEqual(originalText, afterText);
        }

        [Test]
        public void TestFindObjectWithCameraId()
        {
            var altButton = altDriver.FindObject(By.PATH, "//Button");
            altButton.Click();
            altButton.Click();
            var camera = altDriver.FindObject(By.PATH, "//Camera");
            var altElement = altDriver.FindObject(By.COMPONENT, "CapsuleCollider", By.ID, camera.id.ToString());
            Assert.True(altElement.name.Equals("Capsule"));
            var camera2 = altDriver.FindObject(By.PATH, "//Main Camera");
            var altElement2 = altDriver.FindObject(By.COMPONENT, "CapsuleCollider", By.ID, camera2.id.ToString());
            Assert.AreNotEqual(altElement.GetScreenPosition(), altElement2.GetScreenPosition());
        }

        [Test]
        public void TestWaitForObjectWithCameraId()
        {
            var altButton = altDriver.FindObject(By.PATH, "//Button");
            altButton.Click();
            altButton.Click();
            var camera = altDriver.FindObject(By.PATH, "//Camera");
            var altElement = altDriver.WaitForObject(By.COMPONENT, "CapsuleCollider", By.ID, camera.id.ToString());
            Assert.True(altElement.name.Equals("Capsule"));
            var camera2 = altDriver.FindObject(By.PATH, "//Main Camera");
            var altElement2 = altDriver.WaitForObject(By.COMPONENT, "CapsuleCollider", By.ID, camera2.id.ToString());
            Assert.AreNotEqual(altElement.GetScreenPosition(), altElement2.GetScreenPosition());
        }

        [Test]
        public void TestFindObjectsWithCameraId()
        {
            var altButton = altDriver.FindObject(By.PATH, "//Button");
            altButton.Click();
            altButton.Click();
            var camera = altDriver.FindObject(By.PATH, "//Camera");
            var altElement = altDriver.FindObjects(By.NAME, "Plane", By.ID, camera.id.ToString());
            Assert.True(altElement[0].name.Equals("Plane"));
            var camera2 = altDriver.FindObject(By.PATH, "//Main Camera");
            var altElement2 = altDriver.FindObjects(By.NAME, "Plane", By.ID, camera2.id.ToString());
            Assert.AreNotEqual(altElement[0].GetScreenPosition(), altElement2[0].GetScreenPosition());
        }

        [Test]
        public void TestWaitForObjectNotBePresentWithCameraId()
        {
            var camera2 = altDriver.FindObject(By.PATH, "//Main Camera");
            altDriver.WaitForObjectNotBePresent(By.NAME, "ObjectDestroyedIn5Secs", By.ID, camera2.id.ToString());

            var allObjectsInTheScene = altDriver.GetAllElements();
            Assert.IsTrue(!allObjectsInTheScene.Any(obj => obj.name.Equals("ObjectDestroyedIn5Secs")));
        }

        [Test]
        [Obsolete]
        public void TestWaitForElementWithTextWithCameraId()
        {
            const string name = "CapsuleInfo";
            string text = altDriver.FindObject(By.NAME, name).GetText();
            var timeStart = DateTime.Now;
            var camera2 = altDriver.FindObject(By.PATH, "//Main Camera");
            // var altElement = altDriver.WaitForObjectWithText(By.NAME, name, text, By.ID, camera2.id.ToString());
            var altElement = altDriver.WaitForObject(By.PATH, "//" + name + "[@text=" + text + "]", By.ID, camera2.id.ToString());
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            Assert.NotNull(altElement);
            Assert.AreEqual(altElement.GetText(), text);
        }

        [Test]
        public void TestWaitForObjectWhichContainsWithCameraId()
        {
            var camera2 = altDriver.FindObject(By.PATH, "//Main Camera");
            var altElement = altDriver.WaitForObjectWhichContains(By.NAME, "Canva", By.ID, camera2.id.ToString());
            Assert.AreEqual("Canvas", altElement.name);

        }

        [Test]
        public void TestWaitForObjectWhichContainsWithTag()
        {
            var altElement = altDriver.WaitForObjectWhichContains(By.NAME, "Canva", By.TAG, "MainCamera");
            Assert.AreEqual("Canvas", altElement.name);

        }

        [Test]
        public void TestWaitForObjectWhichContainsNonExistingCriteria()
        {
            Assert.Throws<WaitTimeOutException>(() => altDriver.WaitForObjectWhichContains(By.NAME, "Unexisting", By.TAG, "MainCamera", timeout: 2));
        }

        [Test]
        public void TestWaitForObjectWhichContainsExistingCriteriaButNonExistingCamera()
        {
            Assert.Throws<AltCameraNotFoundException>(() => altDriver.WaitForObjectWhichContains(By.NAME, "Canva", By.TAG, "Unexisting", timeout: 2));
        }

        [Test]
        public void TestClickOnTextAndTheParentIsClicked()
        {
            var UiButton = altDriver.FindObject(By.NAME, "UIButton/Text");
            UiButton.Click();
            var capsuleInfo = altDriver.FindObject(By.NAME, "CapsuleInfo");
            var text = capsuleInfo.GetText();
            Assert.AreEqual(text, "UIButton clicked to jump capsule!");
        }

        [Test]
        public void TestAcceleration()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialWorldCoordinates = capsule.GetWorldPosition();
            altDriver.Tilt(new AltVector3(1, 1, 1), 1, wait: false);
            Thread.Sleep(1000);
            capsule = altDriver.FindObject(By.NAME, "Capsule");
            var afterTiltCoordinates = capsule.GetWorldPosition();
            Assert.AreNotEqual(initialWorldCoordinates, afterTiltCoordinates);

        }
        [Test]
        public void TestAccelerationAndWait()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialWorldCoordinates = capsule.GetWorldPosition();
            altDriver.Tilt(new AltVector3(1, 1, 1), 1);
            Thread.Sleep(100);
            capsule = altDriver.FindObject(By.NAME, "Capsule");
            var afterTiltCoordinates = capsule.GetWorldPosition();
            Assert.AreNotEqual(initialWorldCoordinates, afterTiltCoordinates);
        }

        [TestCase(By.NAME, "Main Camera")]
        [TestCase(By.COMPONENT, "Camera")]
        [TestCase(By.TAG, "MainCamera")]
        [TestCase(By.PATH, "/Main Camera")]
        [TestCase(By.LAYER, "Default")]
        [TestCase(By.ID, "4eb39f50-3403-473c-b684-915f7a40c393")]
        public void TestFindObjectByCamera(By cameraBy, string cameraValue)
        {
            int referenceId = altDriver.FindObject(By.PATH, "//Capsule").id;
            var altObject = altDriver.FindObject(By.PATH, "//Capsule", cameraBy, cameraValue);
            Assert.NotNull(altObject);
            Assert.AreEqual(referenceId, altObject.id);
        }

        [Test]
        public void TestWaitForObjectByCamera()
        {
            var altButton = altDriver.FindObject(By.PATH, "//Button");
            altButton.Click();
            altButton.Click();
            var altElement = altDriver.WaitForObject(By.COMPONENT, "CapsuleCollider", By.NAME, "Camera");
            Assert.True(altElement.name.Equals("Capsule"));
            var altElement2 = altDriver.WaitForObject(By.COMPONENT, "CapsuleCollider", By.NAME, "Main Camera");
            Assert.AreNotEqual(altElement.GetScreenPosition(), altElement2.GetScreenPosition());
        }

        [Test]
        public void TestFindObjectsByCamera()
        {
            var altButton = altDriver.FindObject(By.PATH, "//Button");
            altButton.Click();
            altButton.Click();
            var altElement = altDriver.FindObjects(By.NAME, "Plane", By.NAME, "Camera");
            Assert.True(altElement[0].name.Equals("Plane"));
            var altElement2 = altDriver.FindObjects(By.NAME, "Plane", By.NAME, "Main Camera");
            Assert.AreNotEqual(altElement[0].GetScreenPosition(), altElement2[0].GetScreenPosition());
        }

        [Test]
        public void TestWaitForObjectNotBePresentByCamera()
        {
            altDriver.WaitForObjectNotBePresent(By.NAME, "ObjectDestroyedIn5Secs", By.NAME, "Main Camera");

            var allObjectsInTheScene = altDriver.GetAllElements();
            Assert.IsTrue(!allObjectsInTheScene.Any(obj => obj.name.Equals("ObjectDestroyedIn5Secs")));
        }

        [Test]
        public void TestLoadAdditiveScenes()
        {
            var initialNumberOfElements = altDriver.GetAllElements();
            altDriver.LoadScene("Scene 2 Draggable Panel", false);
            var finalNumberOfElements = altDriver.GetAllElements();
            Assert.IsTrue(initialNumberOfElements.Count < finalNumberOfElements.Count);
            var scenes = altDriver.GetAllLoadedScenes();
            Assert.IsTrue(scenes.Count == 2);
            altDriver.LoadScene("Scene 2 Draggable Panel", true);

        }
        [Test]
        public void TestGetAltObjectWithCanvasParentButOnlyTransform()
        {
            var altObject = altDriver.FindObject(By.NAME, "UIWithWorldSpace/Plane");
            Assert.NotNull(altObject);
        }

        [Test]
        public void TestGetScreensizeScreenshot()
        {
            var screenWidth = altDriver.CallStaticMethod<short>("UnityEngine.Screen", "get_width", "UnityEngine.CoreModule", new string[] { }, null);
            var screenHeight = altDriver.CallStaticMethod<short>("UnityEngine.Screen", "get_height", "UnityEngine.CoreModule", new string[] { }, null);
            var screenshot = altDriver.GetScreenshot();
            Assert.True(screenshot.textureSize.x == screenWidth);
            Assert.True(screenshot.textureSize.y == screenHeight);

            screenshot = altDriver.GetScreenshot(screenShotQuality: 50);
            Assert.True(screenshot.textureSize.x == screenWidth);
            Assert.True(screenshot.textureSize.y == screenHeight);

            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            screenshot = altDriver.GetScreenshot(capsule.id, new AltColor(1, 0, 0), 1.5f);
            Assert.True(screenshot.textureSize.x == screenWidth);
            Assert.True(screenshot.textureSize.y == screenHeight);

            screenshot = altDriver.GetScreenshot(capsule.id, new AltColor(1, 0, 0), 1.5f, screenShotQuality: 50);
            Assert.True(screenshot.textureSize.x == screenWidth);
            Assert.True(screenshot.textureSize.y == screenHeight);
        }

        [Test]
        public void TestGetComponentPropertyComplexClass()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "AltSampleClass.testInt";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            var propertyValue = altElement.GetComponentProperty<int>(componentName, propertyName, "Assembly-CSharp");
            Assert.AreEqual(1, propertyValue);
        }

        [Test]
        public void TestGetComponentPropertyComplexClass2()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "listOfSampleClass[1].testString";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            var propertyValue = altElement.GetComponentProperty<string>(componentName, propertyName, "Assembly-CSharp");
            Assert.AreEqual("test2", propertyValue);
        }

        [Test]
        public void TestSetComponentPropertyComplexClass()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "AltSampleClass.testInt";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            altElement.SetComponentProperty(componentName, propertyName, 2, "Assembly-CSharp");
            var propertyValue = altElement.GetComponentProperty<int>(componentName, propertyName, "Assembly-CSharp");
            Assert.AreEqual(2, propertyValue);
        }
        [Test]
        public void TestSetComponentPropertyComplexClass2()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "listOfSampleClass[1].testString";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            altElement.SetComponentProperty(componentName, propertyName, "test3", "Assembly-CSharp");
            var propertyValue = altElement.GetComponentProperty<string>(componentName, propertyName, "Assembly-CSharp");
            Assert.AreEqual("test3", propertyValue);
        }

        [Test]
        public void TestSetComponentPropertyComplexClass3()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "arrayOfInts[0]";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            altElement.SetComponentProperty(componentName, propertyName, 11, "Assembly-CSharp");
            var propertyValue = altElement.GetComponentProperty<int>(componentName, propertyName, "Assembly-CSharp");
            Assert.AreEqual(11, propertyValue);
        }
        [Test]
        public void TestClickWithMouse0Capsule()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialCapsulePosition = capsule.GetWorldPosition();
            altDriver.MoveMouse(capsule.GetScreenPosition(), 0.1f);
            Thread.Sleep(400);
            altDriver.PressKey(AltKeyCode.Mouse0, 1, 0.2f);
            capsule = altDriver.FindObject(By.NAME, "Capsule");
            var finalCapsulePosition = capsule.GetWorldPosition();
            Assert.AreNotEqual(initialCapsulePosition, finalCapsulePosition);
        }
        [Test]
        public void TestClickWithMouse1Capsule()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialCapsulePosition = capsule.GetWorldPosition();
            altDriver.MoveMouse(capsule.GetScreenPosition(), 0.1f);
            Thread.Sleep(400);
            altDriver.PressKey(AltKeyCode.Mouse1, 1, 0.2f);

            capsule = altDriver.FindObject(By.NAME, "Capsule");
            var finalCapsulePosition = capsule.GetWorldPosition();
            Assert.True(FastApproximately(initialCapsulePosition.x, finalCapsulePosition.x, 0.01f));
            Assert.True(FastApproximately(initialCapsulePosition.y, finalCapsulePosition.y, 0.01f));
            Assert.True(FastApproximately(initialCapsulePosition.z, finalCapsulePosition.z, 0.01f));
        }
        [Test]
        public void TestClickWithMouse2Capsule()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialCapsulePosition = capsule.GetWorldPosition();
            altDriver.MoveMouse(capsule.GetScreenPosition(), 0.1f);
            Thread.Sleep(400);
            altDriver.PressKey(AltKeyCode.Mouse2, 1, 0.2f);
            capsule = altDriver.FindObject(By.NAME, "Capsule");
            var finalCapsulePosition = capsule.GetWorldPosition();
            Assert.True(FastApproximately(initialCapsulePosition.x, finalCapsulePosition.x, 0.01f));
            Assert.True(FastApproximately(initialCapsulePosition.y, finalCapsulePosition.y, 0.01f));
            Assert.True(FastApproximately(initialCapsulePosition.z, finalCapsulePosition.z, 0.01f));
        }
        [Test]
        public void TestGetVersion()
        {
            Assert.AreEqual(AltDriver.VERSION, altDriver.GetServerVersion());
        }

        [Test]
        public void TestStringIsMarkedAsPrimitive()
        {
            var altElement = altDriver.FindObject(By.NAME, "ChineseLetters");
            var componentList = altElement.GetAllComponents();
            var component = componentList.First(componenta =>
                componenta.componentName.Equals("UnityEngine.UI.Text"));
            List<AltProperty> properties = altElement.GetAllProperties(component, AltPropertiesSelections.ALLPROPERTIES);

            AltProperty property = properties.First(prop => prop.name.Equals("text"));
            Assert.NotNull(property);
            Assert.AreEqual(AltType.PRIMITIVE, property.type);
        }
        [Test]
        public void TestKeyPressNumberOfReads()
        {
            var counterElement = altDriver.FindObject(By.NAME, "ButtonCounter");
            altDriver.PressKey(AltKeyCode.LeftArrow);

            var pressDownCounter = counterElement.GetComponentProperty<int>("AltExampleScriptIncrementOnClick", "keyPressDownCounter", "Assembly-CSharp");
            var pressUpCounter = counterElement.GetComponentProperty<int>("AltExampleScriptIncrementOnClick", "keyPressUpCounter", "Assembly-CSharp");
            Assert.AreEqual(1, pressDownCounter);
            Assert.AreEqual(1, pressUpCounter);
        }
        [Test]
        public void TestSwipeClickWhenMovedButRemainsOnTheSameObject()
        {
            var counterElement = altDriver.FindObject(By.NAME, "ButtonCounter");
            var counterButtonText = altDriver.FindObject(By.NAME, "ButtonCounter/Text");
            altDriver.Swipe(new AltVector2(counterElement.x + 1, counterElement.y + 1), new AltVector2(counterElement.x + 2, counterElement.y + 1), 1);
            Thread.Sleep(500);
            Assert.AreEqual("1", counterButtonText.GetText());
        }

        [Test]
        public void TestCallMethodInsideASubObject()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "AltSampleClass.TestMethod";
            const string assemblyName = "Assembly-CSharp";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var data = altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, new string[] { });
            Assert.AreEqual("Test", data);
        }


        [Test]
        public void TestCallMethodInsideAListOfSubObject()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "listOfSampleClass[0].TestMethod";
            const string assemblyName = "Assembly-CSharp";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var data = altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, new string[] { });
            Assert.AreEqual("Test", data);
        }

        [Test]
        public void TestCallStaticMethodInsideASubObject()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "StaticSampleClass.TestMethod";
            const string assemblyName = "Assembly-CSharp";

            var data = altDriver.CallStaticMethod<string>(componentName, methodName, assemblyName, new string[] { });
            Assert.AreEqual("Test", data);
        }

        [Test]
        public void TestCallGameObjectMethod()
        {
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var data = altElement.CallComponentMethod<bool>("UnityEngine.GameObject", "CompareTag", "UnityEngine.CoreModule", new[] { "Untagged" }, new[] { "System.String" });
            Assert.IsTrue(data);
        }

        [Test]
        public void TestCallMethodInsideSubObjectOfGameObject()
        {
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var data = altElement.CallComponentMethod<bool>("UnityEngine.GameObject", "scene.IsValid", "UnityEngine.CoreModule", new string[] { }, null);
            Assert.IsTrue(data);
        }

        [TestCase("//Dialog[0]", "Dialog", false)]
        [TestCase("//Text[-1]", "Text", true)]
        public void TestFindIndexer(string path, string expectedResult, bool enabled)
        {
            var altElement = altDriver.FindObject(By.PATH, path, enabled: enabled);
            Assert.AreEqual(expectedResult, altElement.name);
        }

        [Test]
        public void TestUnloadScene()
        {
            altDriver.LoadScene("Scene 2 Draggable Panel", false);
            Assert.AreEqual(2, altDriver.GetAllLoadedScenes().Count);
            altDriver.UnloadScene("Scene 2 Draggable Panel");
            Assert.AreEqual(1, altDriver.GetAllLoadedScenes().Count);
            Assert.AreEqual("Scene 1 AltDriverTestScene", altDriver.GetAllLoadedScenes()[0]);
        }
        [Test]
        public void TestUnloadOnlyScene()
        {
            Assert.Throws<CouldNotPerformOperationException>(() => altDriver.UnloadScene("Scene 1 AltDriverTestScene"));
            Assert.Throws<CouldNotPerformOperationException>(() => altDriver.UnloadScene("Scene 2 Draggable Panel"));
        }

        [Test]
        public void TestClickButtonInWolrdSpaceCanvas()
        {
            altDriver.LoadScene("Scene6");
            var screenPosition = altDriver.FindObject(By.NAME, "WorldSpaceButton").GetScreenPosition();
            altDriver.Tap(screenPosition, 1);
            var worldSpaceButton = altDriver.FindObject(By.NAME, "WorldSpaceButton", enabled: false);
            Assert.IsFalse(worldSpaceButton.enabled);
        }
        [Test]
        public void TestDifferentObjectSelectedClickingOnScreenshot()
        {
            var uiButton = altDriver.FindObject(By.NAME, "UIButton");
            AltObject selectedObject;
            altDriver.GetScreenshot(uiButton.GetScreenPosition(), new AltColor(1, 1, 1, 1), 1, out selectedObject);
            Assert.AreEqual("Text", selectedObject.name);
            altDriver.GetScreenshot(uiButton.GetScreenPosition(), new AltColor(1, 1, 1, 1), 1, out selectedObject);
            Assert.AreEqual("UIButton", selectedObject.name);
        }

        [Test]
        public void TestGetAllScenesAndObjectDisableEnableOption()
        {
            var allEnableObjects = altDriver.GetAllLoadedScenesAndObjects(true);
            foreach (var enabledObject in allEnableObjects)
            {
                Assert.IsTrue(enabledObject.enabled);
            }
            var allObjects = altDriver.GetAllLoadedScenesAndObjects(false);
            Assert.IsTrue(allEnableObjects.Count < allObjects.Count);
            Assert.IsTrue(allObjects.Exists(AltObject => AltObject.name.Equals("Cube") && !AltObject.enabled));
        }

        [Test]
        public void TestGetObjectWithNumberAsName()
        {
            var numberObject = altDriver.FindObject(By.NAME, "1234", enabled: false);
            Assert.NotNull(numberObject);
            numberObject = altDriver.FindObject(By.PATH, "//1234", enabled: false);
            Assert.NotNull(numberObject);
        }

        [Test]
        public void TestInvalidPaths()
        {
            Assert.Throws<InvalidPathException>(() => altDriver.FindObject(By.PATH, "//[1]"));
            Assert.Throws<InvalidPathException>(() => altDriver.FindObject(By.PATH, "CapsuleInfo[@tag=UI]"));
            Assert.Throws<InvalidPathException>(() => altDriver.FindObject(By.PATH, "//CapsuleInfo[@tag=UI/Text"));
            Assert.Throws<InvalidPathException>(() => altDriver.FindObject(By.PATH, "//CapsuleInfo[0/Text"));
        }

        [Test]
        public void TestTap()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var counterButtonText = altDriver.FindObject(By.NAME, "ButtonCounter/Text");
            counterButton.Tap(1);
            altDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=1]");
        }

        [Test]
        public void TestTap_MultipleTaps()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var counterButtonText = altDriver.FindObject(By.NAME, "ButtonCounter/Text");
            counterButton.Tap(2);
            altDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=2]");
        }

        [Test]
        public void TestClick()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var counterButtonText = altDriver.FindObject(By.NAME, "ButtonCounter/Text");
            counterButton.Click(1);
            altDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=1]");
        }

        [Test]
        public void TestClick_MultipleClicks()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var counterButtonText = altDriver.FindObject(By.NAME, "ButtonCounter/Text");
            counterButton.Click(2);
            altDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=2]");
        }

        [Test]
        // [Ignore("mousePressCounter remains stuck and is 13 when asserting instead of 1")]
        public void TestClick_MouseDownUp()
        {
            var counterElement = altDriver.FindObject(By.NAME, "ButtonCounter");
            counterElement.SetComponentProperty("AltExampleScriptIncrementOnClick", "mouseUpCounter", 0, "Assembly-CSharp");
            counterElement.SetComponentProperty("AltExampleScriptIncrementOnClick", "mousePressedCounter", 0, "Assembly-CSharp");

            altDriver.MoveMouse(new AltVector2(0, 0));
            counterElement.Click();

            var mouseDownCounter = counterElement.GetComponentProperty<int>("AltExampleScriptIncrementOnClick", "mouseDownCounter", "Assembly-CSharp");
            var mouseUpCounter = counterElement.GetComponentProperty<int>("AltExampleScriptIncrementOnClick", "mouseUpCounter", "Assembly-CSharp");
            var mousePressedCounter = counterElement.GetComponentProperty<int>("AltExampleScriptIncrementOnClick", "mousePressedCounter", "Assembly-CSharp");

            List<string> eventsRaised = counterElement.GetComponentProperty<List<string>>("AltExampleScriptIncrementOnClick", "eventsRaised", "Assembly-CSharp");

            Assert.AreEqual(1, mouseDownCounter);
            Assert.AreEqual(1, mouseUpCounter);
            Assert.AreEqual(1, mousePressedCounter);

            Assert.IsTrue(eventsRaised.Contains("OnMouseDown"));
            Assert.IsTrue(eventsRaised.Contains("OnMouseUp"));
            Assert.IsTrue(eventsRaised.Contains("OnMouseUpAsButton"));
        }
        [Test]
        [Ignore("Failing in pipeline but passing in local, will fix it later")]
        public void TestPointerEnter_PointerExit()
        {
            altDriver.MoveMouse(new AltVector2(0, 0), 1f);
            var counterElement = altDriver.FindObject(By.NAME, "ButtonCounter");
            counterElement.CallComponentMethod<string>("AltExampleScriptIncrementOnClick", "eventsRaised.Clear", "Assembly-CSharp", new object[] { }, null);
            var eventsRaised = counterElement.GetComponentProperty<List<string>>("AltExampleScriptIncrementOnClick", "eventsRaised", "Assembly-CSharp");
            Assert.IsFalse(eventsRaised.Contains("OnPointerEnter"));
            Assert.IsFalse(eventsRaised.Contains("OnPointerExit"));
            var counterElementPosition = counterElement.GetScreenPosition() + new AltVector2(2, 4);
            altDriver.MoveMouse(counterElementPosition, 1f);

            eventsRaised = counterElement.GetComponentProperty<List<string>>("AltExampleScriptIncrementOnClick", "eventsRaised", "Assembly-CSharp");
            Assert.IsTrue(eventsRaised.Contains("OnPointerEnter"));
            Assert.IsFalse(eventsRaised.Contains("OnPointerExit"));
            altDriver.MoveMouse(new AltVector2(-50, -50), 1f);

            eventsRaised = counterElement.GetComponentProperty<List<string>>("AltExampleScriptIncrementOnClick", "eventsRaised", "Assembly-CSharp");
            Assert.IsTrue(eventsRaised.Contains("OnPointerEnter"));
            Assert.IsTrue(eventsRaised.Contains("OnPointerExit"));
        }

        [Test]
        public void TestClickCoordinates()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var counterButtonText = altDriver.FindObject(By.NAME, "ButtonCounter/Text");
            altDriver.Click(new AltVector2(80 + counterButton.x, 15 + counterButton.y));
            altDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=1]");
        }
        [Test]
        public void TestClickCoordinates_MultipleClicks()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var counterButtonText = altDriver.FindObject(By.NAME, "ButtonCounter/Text");
            altDriver.Click(new AltVector2(80 + counterButton.x, 15 + counterButton.y), 2);
            altDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=2]");
        }


        [Test]
        public void TestClickCoordinates_MouseDownUp()
        {
            var button2dCollider = altDriver.FindObject(By.NAME, "TapClickEventsButtonCollider");

            altDriver.Click(button2dCollider.GetScreenPosition());

            var monoBehaviourEventsRaised = button2dCollider.GetComponentProperty<List<string>>("AltTapClickEventsScript", "monoBehaviourEventsRaised", "Assembly-CSharp");
            // Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseEnter")); does not work on mobile
            Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseDown"));
            Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseUp"));
            Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseUpAsButton"));
            // Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseExit")); does not work on mobile

            var eventSystemEventsRaised = button2dCollider.GetComponentProperty<List<string>>("AltTapClickEventsScript", "eventSystemEventsRaised", "Assembly-CSharp");
            // Assert.IsTrue(eventSystemEventsRaised.Contains("OnPointerEnter")); does not work on mobile
            Assert.IsTrue(eventSystemEventsRaised.Contains("OnPointerDown"));
            Assert.IsTrue(eventSystemEventsRaised.Contains("OnPointerUp"));
            Assert.IsTrue(eventSystemEventsRaised.Contains("OnPointerClick"));
            // Assert.IsTrue(eventSystemEventsRaised.Contains("OnPointerExit")); does not work on mobile
        }

        [Test]
        public void TestClickCoordinates_ColliderNoParent()
        {
            var sphere = altDriver.FindObject(By.PATH, "/Sphere");
            altDriver.Click(sphere.GetScreenPosition());

            var monoBehaviourEventsRaised = sphere.GetComponentProperty<List<string>>("AltSphereColliderScript", "monoBehaviourEventsRaised", "Assembly-CSharp");
            Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseOver"));
            Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseEnter"));
            Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseDown"));
            Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseUp"));
            Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseUpAsButton"));
            Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseExit"));
        }

        [Test]
        public void TestClickCoordinates_ColliderParentCollider()
        {
            altDriver.MoveMouse(new AltVector2(0, 0));
            var sphere = altDriver.FindObject(By.PATH, "/Sphere");
            var plane = altDriver.FindObject(By.PATH, "/Sphere/PlaneS");
            altDriver.Click(plane.GetScreenPosition());

            var planeEvents = plane.GetComponentProperty<List<string>>("AltPlaneColliderScript", "monoBehaviourEventsRaised", "Assembly-CSharp");
            Assert.IsTrue(planeEvents.Contains("OnMouseOver"));
            Assert.IsTrue(planeEvents.Contains("OnMouseEnter"));
            Assert.IsTrue(planeEvents.Contains("OnMouseDown"));
            Assert.IsTrue(planeEvents.Contains("OnMouseUp"));
            Assert.IsTrue(planeEvents.Contains("OnMouseUpAsButton"));
            Assert.IsTrue(planeEvents.Contains("OnMouseExit"));

            var sphereevents = sphere.GetComponentProperty<List<string>>("AltSphereColliderScript", "monoBehaviourEventsRaised", "Assembly-CSharp");
            Assert.IsFalse(sphereevents.Contains("OnMouseEnter"));
            Assert.IsFalse(sphereevents.Contains("OnMouseDown"));
            Assert.IsFalse(sphereevents.Contains("OnMouseUp"));
            Assert.IsFalse(sphereevents.Contains("OnMouseUpAsButton"));
        }
        public static bool FastApproximately(float a, float b, float threshold)
        {
            return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
        }
        [Test]
        public void TestClickCoordinatesCheckPressRaycastObject()
        {
            var incrementClick = altDriver.FindObject(By.NAME, "ButtonCounter");
            altDriver.Click(incrementClick.GetScreenPosition());
            Assert.AreEqual("Text", incrementClick.GetComponentProperty<string>("AltExampleScriptIncrementOnClick", "eventDataPressRaycastObject", "Assembly-CSharp"));
        }

        [Test]
        public void TestPointerPressSet()
        {
            var incrementalClick = altDriver.FindObject(By.NAME, "ButtonCounter");
            var swipeCoordinate = new AltVector2(incrementalClick.x + 10, incrementalClick.y + 10);
            altDriver.Swipe(swipeCoordinate, swipeCoordinate, 0.2f);
            var pointerPress = incrementalClick.GetComponentProperty<AltVector2>("AltExampleScriptIncrementOnClick", "pointerPress", "Assembly-CSharp");
            Assert.AreEqual(swipeCoordinate.x, pointerPress.x);
            Assert.AreEqual(swipeCoordinate.y, pointerPress.y);
        }

        [Test]
        public void TestKeyDownAndKeyUpMouse0()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialCapsulePosition = capsule.GetWorldPosition();
            altDriver.MoveMouse(capsule.GetScreenPosition(), 0.1f);
            Thread.Sleep(400);
            altDriver.KeyDown(AltKeyCode.Mouse0);
            altDriver.KeyUp(AltKeyCode.Mouse0);
            capsule = altDriver.FindObject(By.NAME, "Capsule");
            var finalCapsulePosition = capsule.GetWorldPosition();
            Assert.AreNotEqual(initialCapsulePosition, finalCapsulePosition);
        }
        [Test]
        public void TestTouchAreUpdatedWhenMoved()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var id = altDriver.BeginTouch(capsule.GetScreenPosition());
            var phase = capsule.GetComponentProperty<int>("AltExampleScriptCapsule", "TouchPhase", "Assembly-CSharp");
            Assert.AreEqual(0, phase);
            altDriver.MoveTouch(id, capsule.GetScreenPosition());
            Thread.Sleep(100);
            phase = capsule.GetComponentProperty<int>("AltExampleScriptCapsule", "TouchPhase", "Assembly-CSharp");
            Assert.AreEqual(1, phase);
            altDriver.EndTouch(id);
        }

        [Test]
        public void TestSetStructureProperty()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            capsule.SetComponentProperty("AltExampleScriptCapsule", "testStructure.text", "changed", "Assembly-CSharp");
            var value = capsule.GetComponentProperty<string>("AltExampleScriptCapsule", "testStructure.text", "Assembly-CSharp");
            Assert.AreEqual("changed", value);
        }

        [Test]
        public void TestSetStructureProperty2()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            capsule.SetComponentProperty("AltExampleScriptCapsule", "testStructure.list[0]", 1, "Assembly-CSharp");
            var value = capsule.GetComponentProperty<int>("AltExampleScriptCapsule", "testStructure.list[0]", "Assembly-CSharp");
            Assert.AreEqual(1, value);
        }

        [Test]
        public void TestSetStructureProperty3()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            capsule.SetComponentProperty("AltExampleScriptCapsule", "testStructure.List[0]", 1, "Assembly-CSharp");
            var value = capsule.GetComponentProperty<int>("AltExampleScriptCapsule", "testStructure.list[0]", "Assembly-CSharp");
            Assert.AreEqual(1, value);
        }
        [Test]
        public void TestCameraNotFoundException()
        {
            Assert.Throws<AltCameraNotFoundException>(() => altDriver.FindObject(By.NAME, "Capsule", By.NAME, "Camera"));
        }
        [Test]
        public void TestClickOnChangingSceneButton()
        {
            var initialScene = altDriver.GetCurrentScene();
            altDriver.FindObject(By.NAME, "NextScene").Click();
            var currentScene = altDriver.GetCurrentScene();
            Assert.AreNotEqual(initialScene, currentScene);
        }


        [Test]
        public void TestGetStaticProperty()
        {
            var screenOrientation = altDriver.GetStaticProperty<int>("UnityEngine.Screen", "orientation", "UnityEngine.CoreModule");
            Assert.GreaterOrEqual(screenOrientation, 1);
            Assert.LessOrEqual(screenOrientation, 4);
        }

        [Test]
        public void TestGetStaticNonExistingProperty()
        {
            Assert.Throws<PropertyNotFoundException>(() => altDriver.GetStaticProperty<int>("UnityEngine.Screen", "UNEXISTING", "UnityEngine.CoreModule"));
        }

        [Test]
        public void TestGetStaticpropertyNonExistingComponent()
        {
            Assert.Throws<ComponentNotFoundException>(() => altDriver.GetStaticProperty<int>("UNEXISTING", "orientation", "UnityEngine.CoreModule"));
        }

        [Category("WebGLUnsupported")] // Fails on WebGL in pipeline, skip until issue #1465 is fixed: https://github.com/alttester/AltTester-Unity-SDK/issues/1465
        [Test]
        public void TestGetStaticpropertyNonExistingAssembly()
        {
            Assert.Throws<AssemblyNotFoundException>(() => altDriver.GetStaticProperty<int>("UnityEngine.Screen", "orientation", "UNEXISTING"));
        }

        [Test]
        public void TestGetStaticPropertyInstanceNull()
        {
            var screenWidth = altDriver.CallStaticMethod<short>("UnityEngine.Screen", "get_width", "UnityEngine.CoreModule", new string[] { }, null);
            var width = altDriver.GetStaticProperty<int>("UnityEngine.Screen", "width", "UnityEngine.CoreModule");
            Assert.AreEqual(screenWidth, width);
        }

        [Test]
        public void TestSetCommandTimeout()
        {
            altDriver.SetCommandResponseTimeout(1);
            Assert.Throws<CommandResponseTimeoutException>(() => altDriver.Tilt(new AltVector3(1, 1, 1), 3, true));
            altDriver.SetCommandResponseTimeout(60);
        }

        [Test]
        public void TestKeysDown()
        {
            AltKeyCode[] keys = { AltKeyCode.K, AltKeyCode.L };
            altDriver.KeysDown(keys);
            altDriver.KeysUp(keys);
            var altObject = altDriver.FindObject(By.NAME, "Capsule");
            var finalPropertyValue = altObject.GetComponentProperty<string>("AltExampleScriptCapsule", "stringToSetFromTests", "Assembly-CSharp");
            Assert.AreEqual("multiple keys pressed", finalPropertyValue);
        }

        [Test]
        public void TestPressKeys()
        {
            AltKeyCode[] keys = { AltKeyCode.K, AltKeyCode.L };
            altDriver.PressKeys(keys);
            var altObject = altDriver.FindObject(By.NAME, "Capsule");
            var finalPropertyValue = altObject.GetComponentProperty<string>("AltExampleScriptCapsule", "stringToSetFromTests", "Assembly-CSharp");
            Assert.AreEqual("multiple keys pressed", finalPropertyValue);
        }

        public void TestFindElementAtCoordinates()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var element = altDriver.FindObjectAtCoordinates(new AltVector2(80 + counterButton.x, 15 + counterButton.y));
            Assert.AreEqual("ButtonCounter", element.name);
        }

        [Test]
        public void TestFindElementAtCoordinates_NoElement()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var element = altDriver.FindObjectAtCoordinates(new AltVector2(-1, -1));
            Assert.IsNull(element);
        }

        [Test]
        public void TestFindElementAtCoordinates_3dElement()
        {
            var counterButton = altDriver.FindObject(By.NAME, "Capsule");
            var element = altDriver.FindObjectAtCoordinates(counterButton.GetScreenPosition());
            Assert.AreEqual("Capsule", element.name);
        }

        [Test]
        public void TestScrollViewSwipe()
        {
            altDriver.LoadScene("Scene 11 ScrollView Scene");
            var buttons = altDriver.FindObjects(By.PATH, "//Content/*");
            for (int i = 1; i <= buttons.Count - 3; i++)
            {
                altDriver.Swipe(buttons[i].GetScreenPosition(), buttons[i - 1].GetScreenPosition(), 0.2f);
            }
            Assert.AreEqual(0, buttons[0].GetComponentProperty<int>("AltScrollViewButtonController", "Counter", "Assembly-CSharp"));
        }
        [Test]
        public void TestCallPrivateMethod()
        {
            var altObject = altDriver.FindObject(By.NAME, "Capsule");
            altObject.CallComponentMethod<string>("AltExampleScriptCapsule", "callJump", "Assembly-CSharp", new object[] { });
            var capsuleInfo = altDriver.FindObject(By.NAME, "CapsuleInfo");
            var text = capsuleInfo.GetText();
            Assert.AreEqual("Capsule jumps!", text);
        }

        [Test]
        public void TestOpenDialogWithBeginEndTouch()
        {
            var icon = altDriver.FindObject(By.NAME, "Icon");
            var id = altDriver.BeginTouch(new AltVector2(icon.x - 25, icon.y + 25));
            altDriver.EndTouch(id);
            Assert.NotNull(altDriver.WaitForObject(By.NAME, "Dialog"));

            id = altDriver.BeginTouch(new AltVector2(icon.x - 25, icon.y + 25));
            altDriver.EndTouch(id);
            Assert.Throws<WaitTimeOutException>(() => altDriver.WaitForObject(By.NAME, "Dialog", timeout: 0.5f));
        }

        [Ignore("Skip")]
        [Test]
        public void TestResetInput()
        {
            altDriver.KeyDown(AltKeyCode.P, 1);
            Assert.True(altDriver.FindObject(By.NAME, "AltTesterPrefab").GetComponentProperty<bool>("AltTester.AltTesterUnitySDK.NewInputSystem", "Keyboard.pKey.isPressed", "AltTester.AltTesterUnitySDK"));
            altDriver.ResetInput();
            Assert.False(altDriver.FindObject(By.NAME, "AltTesterPrefab").GetComponentProperty<bool>("AltTester.AltTesterUnitySDK.NewInputSystem", "Keyboard.pKey.isPressed", "AltTester.AltTesterUnitySDK"));

            int countKeyDown = altDriver.FindObject(By.NAME, "AltTesterPrefab").GetComponentProperty<int>("Input", "_keyCodesPressed.Count", "Assembly-CSharp");
            Assert.AreEqual(0, countKeyDown);
        }
        [Category("WebGLUnsupported")]
        [Test]
        public void TestWaitForComponentPropertyMultipleTypes()
        {
            var Canvas = altDriver.WaitForObject(By.PATH, "/Canvas");
            Canvas.WaitForComponentProperty<JToken>("UnityEngine.RectTransform", "rect.x", JToken.Parse("-960.0"), "UnityEngine.CoreModule", 1, getPropertyAsString: true);
            Canvas.WaitForComponentProperty<JToken>("UnityEngine.RectTransform", "rect.center.x", JToken.Parse("0.0"), "UnityEngine.CoreModule", 1, getPropertyAsString: true);
            Canvas.WaitForComponentProperty<JToken>("UnityEngine.RectTransform", "parentInternal", JToken.Parse("null"), "UnityEngine.CoreModule", 1, getPropertyAsString: true);
            Canvas.WaitForComponentProperty<JToken>("UnityEngine.RectTransform", "hasChanged", JToken.Parse("true"), "UnityEngine.CoreModule", 1, getPropertyAsString: true);
            Canvas.WaitForComponentProperty<JToken>("UnityEngine.RectTransform", "name", JToken.Parse("\"Canvas\""), "UnityEngine.CoreModule", 1, getPropertyAsString: true).ToString();
            Canvas.WaitForComponentProperty<JToken>("UnityEngine.RectTransform", "hideFlags", JToken.Parse("0"), "UnityEngine.CoreModule", 1, getPropertyAsString: true);
            Canvas.WaitForComponentProperty("UnityEngine.Canvas", "transform", JToken.Parse("[[], [[]], [[]], [[]], [[]], [[], [], []], [[[], [], []]], [], [], [[]], [[]], [[]]]"), "UnityEngine.UIModule", 1, getPropertyAsString: true);
        }

        [Test]
        public void TestSetStaticProperty()
        {
            var expectedValue = 5;
            altDriver.SetStaticProperty("AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp", expectedValue);
            var value = altDriver.GetStaticProperty<int>("AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp");
            Assert.AreEqual(expectedValue, value);
        }

        [Test]
        public void TestSetStaticNonExistingProperty()
        {
            Assert.Throws<PropertyNotFoundException>(() => altDriver.SetStaticProperty("AltExampleScriptCapsule", "UNEXISTING", "Assembly-CSharp", 5));
        }
        [Test]
        public void TestSetStaticPropertyNonExistingComponent()
        {
            Assert.Throws<ComponentNotFoundException>(() => altDriver.SetStaticProperty("UNEXISTING", "privateStaticVariable", "Assembly-CSharp", 5));
        }

        [Category("WebGLUnsupported")] // Fails on WebGL in pipeline, skip until issue #1465 is fixed: https://github.com/alttester/AltTester-Unity-SDK/issues/1465
        [Test]
        public void TestSetStaticPropertyNonExistingAssembly()
        {
            Assert.Throws<AssemblyNotFoundException>(() => altDriver.SetStaticProperty("AltExampleScriptCapsule", "privateStaticVariable", "UNEXISTING", 5));
        }

        [Test]
        public void TestSetStaticProperty2()
        {
            var newValue = 5;
            int[] expectedArray = new int[3] { 1, 5, 3 };
            altDriver.SetStaticProperty("AltExampleScriptCapsule", "staticArrayOfInts[1]", "Assembly-CSharp", newValue);
            var value = altDriver.GetStaticProperty<int[]>("AltExampleScriptCapsule", "staticArrayOfInts", "Assembly-CSharp");
            Assert.AreEqual(expectedArray, value);
        }
        [Test]
        public void TestInputToggleDoesntUntoggleWhenADriverDisconnects()
        {
            var toggle = altDriver.FindObject(By.PATH, "//Dialog/Toggle", enabled: false);
            Assert.True(toggle.GetComponentProperty<bool>("UnityEngine.UI.Toggle", "isOn", "UnityEngine.UI"));
            var secondDriver = new AltDriver(TestsHelper.GetAltDriverHost(), TestsHelper.GetAltDriverPort(), driverType: "Desktop");
            Assert.True(toggle.GetComponentProperty<bool>("UnityEngine.UI.Toggle", "isOn", "UnityEngine.UI"));
            secondDriver.Stop();
            Assert.True(toggle.GetComponentProperty<bool>("UnityEngine.UI.Toggle", "isOn", "UnityEngine.UI"));

        }
        [TestCase(By.TAG, "Finish", "Button")]
        [TestCase(By.LAYER, "ButtonLayer", "Button")]
        [TestCase(By.NAME, "Button", "Button")]
        [TestCase(By.COMPONENT, "Button", "UIButton")]
        [TestCase(By.PATH, "/Button", "Button")]
        [TestCase(By.ID, "049eccc5-b072-468b-83bf-119d868ca311", "Button")]
        [TestCase(By.TEXT, "Change Camera Mode", "Text")]
        public void TestFindObjectFromObject(By by, string value, string nameOfChild)
        {
            var parent = altDriver.FindObject(By.NAME, "Canvas");
            var child = parent.FindObjectFromObject(by, value);
            Assert.True(child.name == nameOfChild);
        }

        [Test]
        public void TestImplicitTimeout()
        {
            var timeStart = DateTime.Now;
            altDriver.SetImplicitTimeout(1);
            Assert.AreEqual(altDriver.GetImplicitTimeout(), 1, 0.1f);
            try{
            altDriver.WaitForObject(By.NAME, "Capsulee");
            }
            catch(Exception)
            {

            }
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.LessOrEqual(time.TotalSeconds, 2);
            altDriver.SetImplicitTimeout(20);
        }

        [Test]
        public void TestImplicitTimeoutOutOfRange()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => altDriver.SetImplicitTimeout(-5));
            Assert.That(ex.Message, Does.Contain("Timeout cannot be negative"));
        }
    }
}
