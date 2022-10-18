using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Altom.AltDriver.Logging;
using NUnit.Framework;

namespace Altom.AltDriver.Tests
{
    [Timeout(30000)]
    public class TestForScene1TestSample
    {
        private AltDriver altDriver;
        [OneTimeSetUp]
        public void SetUp()
        {
            altDriver = new AltDriver(host: TestsHelper.GetAltDriverHost(), port: TestsHelper.GetAltDriverPort(), enableLogging: true);
            DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Info);
            DriverLogManager.SetMinLogLevel(AltLogger.Unity, AltLogLevel.Info);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            altDriver.Stop();
        }

        [SetUp]
        public void LoadLevel()
        {
            altDriver.SetCommandResponseTimeout(60);
            altDriver.LoadScene("Scene 1 AltDriverTestScene", true);
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
        public void TestFindElement()
        {
            const string name = "Capsule";
            var altElement = altDriver.FindObject(By.NAME, name);
            Assert.NotNull(altElement);
            Assert.AreEqual(name, altElement.name);
        }

        [Test]
        public void TestFindElementWithText()
        {
            const string text = "Change Camera Mode";
            var altElement = altDriver.FindObject(By.TEXT, text);
            Assert.NotNull(altElement);
        }

        [Test]
        public void TestFindElementWithTextByPath()
        {
            const string text = "Change Camera Mode";
            var altElement = altDriver.FindObject(By.PATH, "//*[@text=" + text + "]");
            Assert.NotNull(altElement);
        }

        [Test]
        public void TestFindElementThatContainsText()
        {
            const string text = "Change Camera";
            var altElement = altDriver.FindObject(By.PATH, "//*[contains(@text," + text + ")]");
            Assert.NotNull(altElement);
        }

        [Test]
        public void TestFindElements()
        {
            const string name = "Plane";
            var altElements = altDriver.FindObjects(By.NAME, name);
            Assert.IsNotEmpty(altElements);
            Assert.AreEqual(altElements[0].name, name);
        }

        [Test]
        public void TestFindElementWhereNameContains()
        {
            const string name = "Cap";
            var altElement = altDriver.FindObject(By.PATH, "//*[contains(@name," + name + ")]");
            Assert.NotNull(altElement);
            Assert.True(altElement.name.Contains(name));
        }
        [Test]
        public void TestFindElementsWhereNameContains()
        {
            const string name = "Pla";
            var altElements = altDriver.FindObjects(By.PATH, "//*[contains(@name," + name + ")]");
            Assert.IsNotEmpty(altElements);
            Assert.True(altElements[0].name.Contains(name));
        }

        [Test]
        public void TestWaitForExistingElement()
        {
            const string name = "Capsule";
            var timeStart = DateTime.Now;
            var altElement = altDriver.WaitForObject(By.NAME, name);
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            Assert.NotNull(altElement);
            Assert.AreEqual(altElement.name, name);
        }

        [Test]
        public void TestWaitForExistingDisabledElement()
        {
            const string name = "Cube";
            var timeStart = DateTime.Now;
            var altElement = altDriver.WaitForObject(By.NAME, name, enabled: false);
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            Assert.NotNull(altElement);
            Assert.AreEqual(altElement.name, name);
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

        [Test]
        public void TestWaitForExistingElementWhereNameContains()
        {
            const string name = "Dir";
            var timeStart = DateTime.Now;
            var altElement = altDriver.WaitForObject(By.PATH, "//*[contains(@name," + name + ")]");
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            Assert.NotNull(altElement);
            Assert.AreEqual(altElement.name, "Directional Light");
        }


        [Test]
        [Obsolete]
        public void TestWaitForElementWithText()
        {
            const string name = "CapsuleInfo";
            string text = altDriver.FindObject(By.NAME, name).GetText();
            var timeStart = DateTime.Now;
            var altElement = altDriver.WaitForObject(By.PATH, "//" + name + "[@text=" + text + "]");
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            Assert.NotNull(altElement);
            Assert.AreEqual(altElement.GetText(), text);
        }

        [Test]
        public void TestSetTextForUnityUIInputField()
        {
            var inputField = altDriver.FindObject(By.NAME, "UnityUIInputField").SetText("exampleUnityUIInputField", true);
            Assert.AreEqual("exampleUnityUIInputField", inputField.GetText());
            Assert.IsTrue(inputField.GetComponentProperty<bool>("AltInputFieldRaisedEvents", "onValueChangedInvoked", "Assembly-CSharp"), "onValueChangedInvoked was false");
            Assert.IsTrue(inputField.GetComponentProperty<bool>("AltInputFieldRaisedEvents", "onSubmitInvoked", "Assembly-CSharp"), "onSubmitInvoked was false");
            Assert.IsTrue(inputField.GetComponentProperty<bool>("AltInputFieldRaisedEvents", "onEndEditInvoked", "Assembly-CSharp"), "onEndEditInvoked was false");

        }

        [Test]
        public void TestSetTextForTextMeshInputField()
        {
            var inputField = altDriver.FindObject(By.NAME, "TextMeshInputField").SetText("exampleTextMeshInputField", true);
            Assert.AreEqual("exampleTextMeshInputField", inputField.GetText());
            Assert.IsTrue(inputField.GetComponentProperty<bool>("AltInputFieldRaisedEvents", "onValueChangedInvoked", "Assembly-CSharp"), "onValueChangedInvoked was false");
            Assert.IsTrue(inputField.GetComponentProperty<bool>("AltInputFieldRaisedEvents", "onSubmitInvoked", "Assembly-CSharp"), "onSubmitInvoked was false");
            Assert.IsTrue(inputField.GetComponentProperty<bool>("AltInputFieldRaisedEvents", "onEndEditInvoked", "Assembly-CSharp"), "onEndEditInvoked was false");

        }

        [Test]
        public void TestFindObjectByComponent()
        {
            Thread.Sleep(1000);
            const string componentName = "AltRunner";
            var altElement = altDriver.FindObject(By.COMPONENT, componentName);
            Assert.NotNull(altElement);
            Assert.AreEqual(altElement.name, "AltTesterPrefab");
        }
        [Test]
        public void TestFindObjectByComponentWithNamespace()
        {
            Thread.Sleep(1000);
            const string componentName = "AltTester.AltDriver.AltRunner";
            var altElement = altDriver.FindObject(By.COMPONENT, componentName);
            Assert.NotNull(altElement);
            Assert.AreEqual(altElement.name, "AltTesterPrefab");
        }
        [Test]
        public void TestFindObjectByComponent2()
        {
            var altElement = altDriver.FindObject(By.COMPONENT, "AltExampleScriptCapsule");
            Assert.True(altElement.name.Equals("Capsule"));
        }

        [Test]
        public void TestGetComponentProperty()
        {
            const string componentName = "Altom.AltTester.AltRunner";
            const string propertyName = "InstrumentationSettings.ProxyPort";
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            Assert.NotNull(altElement);
            var propertyValue = altElement.GetComponentProperty<int>(componentName, propertyName, "Assembly-CSharp");
            string portStr = System.Environment.GetEnvironmentVariable("PROXY_PORT");
            if (string.IsNullOrEmpty(portStr)) portStr = "13000";
            int port = int.Parse(portStr);

            Assert.AreEqual(port, propertyValue);
        }

        [Test]
        public void TestGetComponentPropertyInvalidDeserialization()
        {
            const string componentName = "Altom.AltTester.AltRunner";
            const string propertyName = "InstrumentationSettings.ShowPopUp";
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            try
            {
                var propertyValue = altElement.GetComponentProperty<int>(componentName, propertyName, "Assembly-CSharp");
                Assert.Fail("Expected ResponseFormatException");
            }
            catch (ResponseFormatException ex)
            {
                Assert.AreEqual("Could not deserialize response data: `true` into System.Int32", ex.Message);
            }
        }

        [Test]
        public void TestGetComponentPropertyNotFoundWithAssembly()
        {
            Thread.Sleep(1000);
            const string componentName = "Altom.AltTester.AltRunner";
            const string propertyName = "InvalidProperty";
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            Assert.NotNull(altElement);
            try
            {
                var propertyValue = altElement.GetComponentProperty<bool>(componentName, propertyName, "Assembly-CSharp");
                Assert.Fail();
            }
            catch (PropertyNotFoundException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Property InvalidProperty not found"), exception.Message);
            }
        }

        [Test]
        public void TestFindObjectsByComponent()
        {
            var a = altDriver.FindObjects(By.COMPONENT, "MeshFilter");
            Assert.AreEqual(5, a.Count);
        }
        [Test]
        public void TestGetNonExistingComponentProperty()
        {
            Thread.Sleep(1000);
            const string componentName = "Altom.AltTester.AltRunner";
            const string propertyName = "socketPort";
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            Assert.NotNull(altElement);
            try
            {
                altElement.GetComponentProperty<int>(componentName, propertyName, "Assembly-CSharp");
                Assert.Fail();
            }
            catch (PropertyNotFoundException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Property socketPort not found"), exception.Message);
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
            const string componentName = "Capsulee";
            const string propertyName = "stringToSetFromTests";
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            try
            {
                altElement.SetComponentProperty(componentName, propertyName, "2", "Assembly-CSharp");
                Assert.Fail();
            }
            catch (ComponentNotFoundException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Component not found"), exception.Message);
            }
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
        public void TestCallMethodSetFontSizeWithParameters(){
            const string componentName = "UnityEngine.UI.Text";
            const string methodName = "set_fontSize";
            const string methodToVerifyName = "get_fontSize";
            const string assemblyName = "UnityEngine.UI";
            Int32 fontSizeExpected = 16;
            string[] parameters = new[] {"16"};
            var altElement = altDriver.FindObject(By.PATH, "/Canvas/UnityUIInputField/Text");
            var data = altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, parameters);
            var fontSize =  altElement.CallComponentMethod<Int32>(componentName, methodToVerifyName, assemblyName, new object[] { });
            Assert.AreEqual(fontSizeExpected,fontSize);
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
        public void TestCallMethodWithOptionalParemeters()
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
        public void TestCallMethodWithOptionalParemetersString()
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
        public void TestCallMethodWithOptionalParemetersString2()
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
        public void TestCallMethodAssmeblyNotFound()
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

        [Test]
        public void TestFindNonExistentObject()
        {
            try
            {
                altDriver.FindObject(By.NAME, "NonExistent");
                Assert.Fail();
            }
            catch (NotFoundException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Object //NonExistent not found"), exception.Message);
            }

        }

        [Test]
        public void TestFindNonExistentObjectByName()
        {
            try
            {
                altDriver.FindObject(By.PATH, "//*[contains(@name,NonExistent)]");
                Assert.Fail();
            }
            catch (NotFoundException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Object //*[contains(@name,NonExistent)] not found"), exception.Message);
            }
        }

        [Test]
        public void TestHoldButton()
        {
            var button = altDriver.FindObject(By.NAME, "UIButton");
            altDriver.HoldButton(button.getScreenPosition(), 1);
            var capsuleInfo = altDriver.FindObject(By.NAME, "CapsuleInfo");
            var text = capsuleInfo.GetText();
            Assert.AreEqual(text, "UIButton clicked to jump capsule!");
        }

        [Test]
        [Obsolete]
        public void TestClickElement()
        {
            const string name = "Capsule";
            var altElement = altDriver.FindObject(By.NAME, name).Tap();
            Assert.AreEqual(name, altElement.name);
            // altDriver.WaitForObjectWithText(By.NAME, "CapsuleInfo", "Capsule was clicked to jump!");
            altDriver.WaitForObject(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]");
        }

        [Test]
        public void TestWaitForNonExistingObject()
        {
            try
            {
                altDriver.WaitForObject(By.NAME, "dlkasldkas", timeout: 1, interval: 1);
                Assert.Fail();
            }
            catch (WaitTimeOutException exception)
            {
                Assert.AreEqual("Element //dlkasldkas not loaded after 1 seconds", exception.Message);
            }
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
        public void TestWaitForObjectWithTextWrongText()
        {
            try
            {
                altDriver.WaitForObject(By.PATH, "//CapsuleInfo[@text=aaaaa]", timeout: 1);
                Assert.Fail();
            }
            catch (WaitTimeOutException exception)
            {
                Assert.AreEqual("Element //CapsuleInfo[@text=aaaaa] not loaded after 1 seconds", exception.Message);
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
        public void TestWaitForNonExistingElementWhereNameContains()
        {
            const string name = "xyz";
            try
            {
                altDriver.WaitForObject(By.PATH, "//*[contains(@name," + name + ")]", timeout: 1);
                Assert.Fail();
            }
            catch (WaitTimeOutException exception)
            {
                Assert.AreEqual("Element //*[contains(@name,xyz)] not loaded after 1 seconds", exception.Message);
            }
        }

        [Test]
        public void TestFindObjects()
        {
            var planes = altDriver.FindObjectsWhichContain(By.NAME, "Plan");
            Assert.AreEqual(3, planes.Count);
        }


        [Test]
        public void TestCallStaticMethod()
        {
            altDriver.CallStaticMethod<string>("UnityEngine.PlayerPrefs", "SetInt", "UnityEngine.CoreModule", new[] { "Test", "1" });
            int a = altDriver.CallStaticMethod<int>("UnityEngine.PlayerPrefs", "GetInt", "UnityEngine.CoreModule", new[] { "Test", "2" });
            Assert.AreEqual(1, a);
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
            Assert.AreEqual(15, fields.Count);
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
            Assert.AreEqual(16, fields.Count);
        }

        [Test]
        public void TestFindObjectByTag()
        {
            var altElement = altDriver.FindObject(By.TAG, "plane");
            Assert.True(altElement.name.Equals("Plane"));
        }
        [Test]
        public void TestFindObjectByLayer()
        {
            var altElement = altDriver.FindObject(By.LAYER, "Water");
            Assert.True(altElement.name.Equals("Capsule"));
        }
        [Test]
        public void TestFindObjectByUnityComponent()
        {
            var altElement = altDriver.FindObject(By.COMPONENT, "CapsuleCollider");
            Assert.True(altElement.name.Equals("Capsule"));
        }

        [Test]
        public void TestFindChild()
        {
            var altElement = altDriver.FindObject(By.PATH, "//UIButton/*");
            Assert.True(altElement.name.Equals("Text"));
        }
        [TestCase("//*[contains(@name,Cub)]", "Cube")]
        [TestCase("//RotateMainCameraButton/../*[contains(@name,Seconda)]/Text", "Text")]
        [TestCase("//*[@component=BoxCollider]", "Cube")]
        [TestCase("/Capsule/../Plane", "Plane")]
        public void TestFindingDifferentObjects(string path, string result)
        {
            var altElement = altDriver.FindObject(By.PATH, path, enabled: false);
            Assert.True(altElement.name.Equals(result));

        }

        [Test]
        public void TestFindObjectsByTag()
        {
            var altElements = altDriver.FindObjects(By.TAG, "plane");
            Assert.AreEqual(2, altElements.Count);
            foreach (var altElement in altElements)
            {
                Assert.AreEqual("Plane", altElement.name);
            }
        }

        [Test]
        public void TestFindObjectsByLayer()
        {
            var altElements = altDriver.FindObjects(By.LAYER, "Default");
            Assert.IsTrue(altElements.Count >= 12);
            Assert.IsTrue(altElements.Count <= 13);
        }
        [Test]
        public void TestFindObjectsByContainName()
        {
            var altElements = altDriver.FindObjects(By.PATH, "//*[contains(@name,Ro)]");
            foreach (var altElement in altElements)
            {
                Assert.True(altElement.name.Contains("Ro"));
            }
            Assert.AreEqual(2, altElements.Count);

        }


        [Test]
        public void TestInactiveObject()
        {
            AltObject cube = altDriver.FindObject(By.NAME, "Cube", enabled: false);
            Assert.AreEqual(false, cube.enabled);
        }
        [Test]
        public void TestGetAllScenes()
        {
            var scenes = altDriver.GetAllScenes();
            Assert.AreEqual(12, scenes.Count);
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
        public void TestWaitForObjectWhichContains()
        {
            var altElement = altDriver.WaitForObjectWhichContains(By.NAME, "Canva");
            Assert.AreEqual("Canvas", altElement.name);

        }
        [Test]
        public void TestFindObjectWhichContains()
        {
            var altElement = altDriver.FindObjectWhichContains(By.NAME, "EventSys");
            Assert.AreEqual("EventSystem", altElement.name);
        }
        [Test]
        public void TestFindWithFindObjectWhichContainsNotExistingObject()
        {
            try
            {
                var altElement = altDriver.FindObjectWhichContains(By.NAME, "EventNonExisting");
                Assert.Fail("Error should have been thrown");
            }
            catch (NotFoundException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Object //*[contains(@name,EventNonExisting)] not found"), exception.Message);
            }
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
        public void TestFindObjectScene1()
        {
            var altElements = altDriver.FindObjects(By.PATH, "//Canvas/*/Text");
            Assert.AreEqual(8, altElements.Count);
        }

        [Test]
        public void TestFindObjectScene6()
        {
            altDriver.LoadScene("Scene6");

            Thread.Sleep(1000);
            altDriver.WaitForCurrentSceneToBe("Scene6");
            var altElements = altDriver.FindObjects(By.PATH, "//Canvas/*/Text");
            Assert.AreEqual(3, altElements.Count);
            altElements = altDriver.FindObjects(By.PATH, "/*/*/Text");
            Assert.AreEqual(3, altElements.Count);
            altElements = altDriver.FindObjects(By.PATH, "/*/Text");
            Assert.AreEqual(1, altElements.Count);
            altElements = altDriver.FindObjects(By.PATH, "//Canvas//Text");
            Assert.AreEqual(5, altElements.Count);
            altElements = altDriver.FindObjects(By.PATH, "//Canvas/*//Text");
            Assert.AreEqual(4, altElements.Count);

            Assert.AreEqual("First", altDriver.FindObject(By.PATH, "/Canvas/First").name);
            Assert.AreEqual("WorldSpaceButton", altDriver.FindObject(By.PATH, "/Canvas/WorldSpaceButton").name);
        }
        [Test]
        public void TestGetScreenshot()
        {
            var path = "testC.png";
            altDriver.GetPNGScreenshot(path);
            FileAssert.Exists(path);
        }
        [Test]
        public void TestGetChineseLetters()
        {
            var text = altDriver.FindObject(By.NAME, "ChineseLetters").GetText();
            Assert.AreEqual("哦伊娜哦", text);
        }
        [Test]
        public void TestNonEnglishText()
        {
            var text = altDriver.FindObject(By.NAME, "NonEnglishText").GetText();
            Assert.AreEqual("BJÖRN'S PASS", text);
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
            altDriver.Swipe(counterButtonText.getScreenPosition(), counterButtonText.getScreenPosition(), 0, wait: false);
            Thread.Sleep(500);
            Assert.AreEqual("1", counterButtonText.GetText());
        }

        [Test]
        public void TestCustomTap()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var counterButtonText = altDriver.FindObject(By.NAME, "ButtonCounter/Text");
            altDriver.Tap(counterButton.getScreenPosition(), 4);
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
        public void TestPressNextSceneButtton()
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
        public void TestFindParentUsingPath()
        {
            var parent = altDriver.FindObject(By.PATH, "//CapsuleInfo/..");
            Assert.AreEqual("Canvas", parent.name);
        }

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
            Assert.AreNotEqual(altElement.getScreenPosition(), altElement2.getScreenPosition());
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
            Assert.AreNotEqual(altElement.getScreenPosition(), altElement2.getScreenPosition());
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
            Assert.AreNotEqual(altElement[0].getScreenPosition(), altElement2[0].getScreenPosition());
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
        public void TestFindObjectWithTag()
        {
            var altButton = altDriver.FindObject(By.PATH, "//Button");
            altButton.Click();
            altButton.Click();
            var altElement = altDriver.FindObject(By.COMPONENT, "CapsuleCollider", By.TAG, "MainCamera");
            Assert.True(altElement.name.Equals("Capsule"));
            var altElement2 = altDriver.FindObject(By.COMPONENT, "CapsuleCollider", By.TAG, "Untagged");
            Assert.AreNotEqual(altElement.getScreenPosition(), altElement2.getScreenPosition());
        }

        [Test]
        public void TestWaitForObjectWithTag()
        {
            var altButton = altDriver.FindObject(By.PATH, "//Button");
            altButton.Click();
            altButton.Click();
            var altElement = altDriver.WaitForObject(By.COMPONENT, "CapsuleCollider", By.TAG, "MainCamera");
            Assert.True(altElement.name.Equals("Capsule"));
            var altElement2 = altDriver.WaitForObject(By.COMPONENT, "CapsuleCollider", By.TAG, "Untagged");
            Assert.AreNotEqual(altElement.getScreenPosition(), altElement2.getScreenPosition());
        }

        [Test]
        public void TestFindObjectsWithTag()
        {
            var altButton = altDriver.FindObject(By.PATH, "//Button");
            altButton.Click();
            altButton.Click();
            var altElement = altDriver.FindObjects(By.NAME, "Capsule", By.TAG, "MainCamera");
            Assert.True(altElement[0].name.Equals("Capsule"));
            var altElement2 = altDriver.FindObjects(By.NAME, "Capsule", By.TAG, "Untagged");
            Assert.AreNotEqual(altElement[0].getScreenPosition(), altElement2[0].getScreenPosition());
        }

        [Test]
        public void TestWaitForObjectNotBePresentWithTag()
        {
            var camera2 = altDriver.FindObject(By.PATH, "//Main Camera");
            altDriver.WaitForObjectNotBePresent(By.NAME, "ObjectDestroyedIn5Secs", By.TAG, "MainCamera");

            var allObjectsInTheScene = altDriver.GetAllElements();
            Assert.IsTrue(!allObjectsInTheScene.Any(obj => obj.name.Equals("ObjectDestroyedIn5Secs")));
        }

        [Test]
        public void TestWaitForElementWithTextWithTag()
        {
            const string name = "CapsuleInfo";
            string text = altDriver.FindObject(By.NAME, name).GetText();
            var timeStart = DateTime.Now;
            // var altElement = altDriver.WaitForObjectWithText(By.NAME, name, text, By.TAG, "MainCamera");
            var altElement = altDriver.WaitForObject(By.PATH, "//" + name + "[@text=" + text + "]", By.TAG, "MainCamera");
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            Assert.NotNull(altElement);
            Assert.AreEqual(altElement.GetText(), text);
        }

        [Test]
        public void TestWaitForObjectWhichContainsWithTag()
        {
            var altElement = altDriver.WaitForObjectWhichContains(By.NAME, "Canva", By.TAG, "MainCamera");
            Assert.AreEqual("Canvas", altElement.name);

        }

        [Test]
        public void TestAcceleration()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialWorldCoordinates = capsule.getWorldPosition();
            altDriver.Tilt(new AltVector3(1, 1, 1), 1, wait: false);
            Thread.Sleep(1000);
            capsule = altDriver.FindObject(By.NAME, "Capsule");
            var afterTiltCoordinates = capsule.getWorldPosition();
            Assert.AreNotEqual(initialWorldCoordinates, afterTiltCoordinates);

        }
        [Test]
        public void TestAccelerationAndWait()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialWorldCoordinates = capsule.getWorldPosition();
            altDriver.Tilt(new AltVector3(1, 1, 1), 1);
            Thread.Sleep(100);
            capsule = altDriver.FindObject(By.NAME, "Capsule");
            var afterTiltCoordinates = capsule.getWorldPosition();
            Assert.AreNotEqual(initialWorldCoordinates, afterTiltCoordinates);
        }

        [Test]
        public void TestFindObjectByCamera()
        {
            var altButton = altDriver.FindObject(By.PATH, "//Button");
            altButton.Click();
            altButton.Click();
            var altElement = altDriver.FindObject(By.COMPONENT, "CapsuleCollider", By.NAME, "Camera");
            Assert.True(altElement.name.Equals("Capsule"));
            var altElement2 = altDriver.FindObject(By.COMPONENT, "CapsuleCollider", By.NAME, "Main Camera");
            Assert.AreNotEqual(altElement.getScreenPosition(), altElement2.getScreenPosition());
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
            Assert.AreNotEqual(altElement.getScreenPosition(), altElement2.getScreenPosition());
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
            Assert.AreNotEqual(altElement[0].getScreenPosition(), altElement2[0].getScreenPosition());
        }

        [Test]
        public void TestWaitForObjectNotBePresentByCamera()
        {
            altDriver.WaitForObjectNotBePresent(By.NAME, "ObjectDestroyedIn5Secs", By.NAME, "Main Camera");

            var allObjectsInTheScene = altDriver.GetAllElements();
            Assert.IsTrue(!allObjectsInTheScene.Any(obj => obj.name.Equals("ObjectDestroyedIn5Secs")));
        }

        [Test]
        public void TestWaitForElementWithTextByCamera()
        {
            const string name = "CapsuleInfo";
            string text = altDriver.FindObject(By.NAME, name).GetText();
            var timeStart = DateTime.Now;
            // var altElement = altDriver.WaitForObjectWithText(By.NAME, name, text, By.NAME, "Main Camera");
            var altElement = altDriver.WaitForObject(By.PATH, "//" + name + "[@text=" + text + "]", By.NAME, "Main Camera");
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            Assert.NotNull(altElement);
            Assert.AreEqual(altElement.GetText(), text);
        }

        [Test]
        public void TestWaitForObjectWhichContainsByCamera()
        {
            var altElement = altDriver.WaitForObjectWhichContains(By.NAME, "Canva", By.NAME, "Main Camera");
            Assert.AreEqual("Canvas", altElement.name);

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
            Assert.True(screenshot.textureSize.x == screenWidth / 2);
            Assert.True(screenshot.textureSize.y == screenHeight / 2);

            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            screenshot = altDriver.GetScreenshot(capsule.id, new AltColor(1, 0, 0), 1.5f);
            Assert.True(screenshot.textureSize.x == screenWidth);
            Assert.True(screenshot.textureSize.y == screenHeight);

            screenshot = altDriver.GetScreenshot(capsule.id, new AltColor(1, 0, 0), 1.5f, screenShotQuality: 50);
            Assert.True(screenshot.textureSize.x == screenWidth / 2);
            Assert.True(screenshot.textureSize.y == screenHeight / 2);
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
            var initialCapsulePosition = capsule.getWorldPosition();
            altDriver.MoveMouse(capsule.getScreenPosition(), 0.1f);
            Thread.Sleep(400);
            altDriver.PressKey(AltKeyCode.Mouse0, 1, 0.2f);
            capsule = altDriver.FindObject(By.NAME, "Capsule");
            var finalCapsulePosition = capsule.getWorldPosition();
            Assert.AreNotEqual(initialCapsulePosition, finalCapsulePosition);
        }
        [Test]
        public void TestClickWithMouse1Capsule()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialCapsulePosition = capsule.getWorldPosition();
            altDriver.MoveMouse(capsule.getScreenPosition(), 0.1f);
            Thread.Sleep(400);
            altDriver.PressKey(AltKeyCode.Mouse1, 1, 0.2f);

            capsule = altDriver.FindObject(By.NAME, "Capsule");
            var finalCapsulePosition = capsule.getWorldPosition();
            Assert.True(FastApproximately(initialCapsulePosition.x, finalCapsulePosition.x, 0.01f));
            Assert.True(FastApproximately(initialCapsulePosition.y, finalCapsulePosition.y, 0.01f));
            Assert.True(FastApproximately(initialCapsulePosition.z, finalCapsulePosition.z, 0.01f));
        }
        [Test]
        public void TestClickWithMouse2Capsule()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialCapsulePosition = capsule.getWorldPosition();
            altDriver.MoveMouse(capsule.getScreenPosition(), 0.1f);
            Thread.Sleep(400);
            altDriver.PressKey(AltKeyCode.Mouse2, 1, 0.2f);
            capsule = altDriver.FindObject(By.NAME, "Capsule");
            var finalCapsulePosition = capsule.getWorldPosition();
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

        [Test]
        public void TestFindObjectByAltId()
        {
            var capsule = altDriver.FindObject(By.ID, "2b78431c-2251-4489-8d50-7634304a5630");
            Assert.AreEqual("Capsule", capsule.name);
            var plane = altDriver.FindObject(By.PATH, "//*[@id=5277849a-16c3-469e-b3aa-ead06f0a37d2]");
            Assert.AreEqual("Plane", plane.name);
            var mainCamera = altDriver.FindObject(By.NAME, "Main Camera");
            mainCamera = altDriver.FindObject(By.ID, mainCamera.id.ToString());
            Assert.AreEqual("Main Camera", mainCamera.name);
        }


        [TestCase("/Canvas[0]", "CapsuleInfo", true)]
        [TestCase("/Canvas[1]", "UIButton", true)]
        [TestCase("/Canvas[-1]", "TapClickEventsButtonCollider", true)]
        [TestCase("/Canvas[-2]", "NextScene", true)]
        [TestCase("/Canvas[@layer=UI][5]", "UnityUIInputField", true)]
        [TestCase("/Canvas[1]/Text", "Text", true)]
        [TestCase("//Dialog[0]", "Title", false)]
        [TestCase("//Dialog[1]", "Message", false)]
        [TestCase("//Dialog[-1]", "CloseButton", false)]
        public void TestFindNthChild(string path, string expectedResult, bool enabled)
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
            var screenPosition = altDriver.FindObject(By.NAME, "WorldSpaceButton").getScreenPosition();
            altDriver.Tap(screenPosition, 1);
            var worldSpaceButton = altDriver.FindObject(By.NAME, "WorldSpaceButton", enabled: false);
            Assert.IsFalse(worldSpaceButton.enabled);
        }
        [Test]
        public void TestDifferentObjectSelectedClickingOnScreenshot()
        {
            var uiButton = altDriver.FindObject(By.NAME, "UIButton");
            AltObject selectedObject;
            altDriver.GetScreenshot(uiButton.getScreenPosition(), new AltColor(1, 1, 1, 1), 1, out selectedObject);
            Assert.AreEqual("Text", selectedObject.name);
            altDriver.GetScreenshot(uiButton.getScreenPosition(), new AltColor(1, 1, 1, 1), 1, out selectedObject);
            Assert.AreEqual("UIButton", selectedObject.name);
        }
        [Test]
        public void TestFindObjectWithMultipleSelector()
        {
            var capsule = altDriver.FindObject(By.PATH, "//*[@tag=Untagged][@layer=Water]");
            Assert.AreEqual("Capsule", capsule.name);
            var capsuleInfo = altDriver.FindObject(By.PATH, "//*[contains(@name,Capsule)][@layer=UI]");
            Assert.AreEqual("CapsuleInfo", capsuleInfo.name);
            var rotateMainCamera = altDriver.FindObject(By.PATH, "//*[@component=Button][@tag=Untagged][@layer=UI]");
            Assert.AreEqual("UIButton", rotateMainCamera.name);
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
        public void TestClick_MouseDownUp()
        {
            altDriver.MoveMouse(new AltVector2(0, 0));
            var counterElement = altDriver.FindObject(By.NAME, "ButtonCounter");
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
        public void TestPointerEnter_PointerExit()
        {
            altDriver.MoveMouse(new AltVector2(-1, -1), 1);
            altDriver.LoadScene("Scene 1 AltDriverTestScene", true);

            var counterElement = altDriver.FindObject(By.NAME, "ButtonCounter");

            altDriver.MoveMouse(counterElement.getScreenPosition(), 1);
            Thread.Sleep(800); // OnPointerEnter, OnPointerExit events are raised during the Update function. right now there is a delay from mouse moved to events raised.

            var eventsRaised = counterElement.GetComponentProperty<List<string>>("AltExampleScriptIncrementOnClick", "eventsRaised", "Assembly-CSharp");
            Assert.IsTrue(eventsRaised.Contains("OnPointerEnter"));
            Assert.IsFalse(eventsRaised.Contains("OnPointerExit"));
            altDriver.MoveMouse(new AltVector2(200, 200));
            Thread.Sleep(800);

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

            altDriver.Click(button2dCollider.getScreenPosition());

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
            altDriver.Click(sphere.getScreenPosition());

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
            altDriver.Click(plane.getScreenPosition());

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
            altDriver.Click(incrementClick.getScreenPosition());
            Assert.AreEqual("Text", incrementClick.GetComponentProperty<string>("AltExampleScriptIncrementOnClick", "eventDataPressRaycastObject", "Assembly-CSharp"));
        }

        [Test]
        public void TestPointerPressSet()
        {
            var incrementalClick = altDriver.FindObject(By.NAME, "ButtonCounter");
            var swipeCoordinate = new AltVector2(incrementalClick.x + 10, incrementalClick.y + 10);
            altDriver.Swipe(swipeCoordinate, swipeCoordinate, 0.2f);
            var pointerPress = incrementalClick.GetComponentProperty<AltVector2>("AltExampleScriptIncrementOnClick", "pointerPress", "Assembly-CSharp");
            Assert.AreEqual(10.0f, pointerPress.x);
            Assert.AreEqual(10.0f, pointerPress.y);
        }

        [Test]
        public void TestKeyDownAndKeyUpMouse0()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialCapsulePosition = capsule.getWorldPosition();
            altDriver.MoveMouse(capsule.getScreenPosition(), 0.1f);
            Thread.Sleep(400);
            altDriver.KeyDown(AltKeyCode.Mouse0);
            altDriver.KeyUp(AltKeyCode.Mouse0);
            capsule = altDriver.FindObject(By.NAME, "Capsule");
            var finalCapsulePosition = capsule.getWorldPosition();
            Assert.AreNotEqual(initialCapsulePosition, finalCapsulePosition);
        }
        [Test]
        public void TestTouchAreUpdatedWhenMoved()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var id = altDriver.BeginTouch(capsule.getScreenPosition());
            var phase = capsule.GetComponentProperty<int>("AltExampleScriptCapsule", "TouchPhase", "Assembly-CSharp");
            Assert.AreEqual(0, phase);
            altDriver.MoveTouch(id, capsule.getScreenPosition());
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
        //uses InvokeMethod
        [Category("WebGLUnsupported")]
        public void TestGetStaticProperty()
        {
            altDriver.CallStaticMethod<string>("UnityEngine.Screen", "SetResolution", "UnityEngine.CoreModule", new string[] { "1920", "1080", "true" }, new string[] { "System.Int32", "System.Int32", "System.Boolean" });
            var width = altDriver.GetStaticProperty<int>("UnityEngine.Screen", "currentResolution.width", "UnityEngine.CoreModule");
            Assert.AreEqual(1920, width);
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
            Assert.AreEqual("Text", element.name);
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
            var element = altDriver.FindObjectAtCoordinates(counterButton.getScreenPosition());
            Assert.AreEqual("Capsule", element.name);
        }
        // [Test]
        // public void TestScrollViewSwipe()
        // {
        //     altUnityDriver.LoadScene("Scene 11 ScrollView Scene");
        //     var buttons = altUnityDriver.FindObjects(By.PATH, "//Content/*");
        //     for (int i = 1; i <= buttons.Count - 3; i++)
        //     {
        //         altUnityDriver.Swipe(buttons[i].getScreenPosition(), buttons[i - 1].getScreenPosition());

        //     }
        //     Assert.AreEqual(0, buttons[0].GetComponentProperty<int>("AltUnityScrollViewButtonController", "Counter", "Assembly-CSharp"));
        // }

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
        }
    }
}
