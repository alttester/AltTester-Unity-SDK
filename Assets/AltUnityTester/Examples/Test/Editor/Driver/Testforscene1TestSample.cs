using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Altom.AltUnityDriver;
using NUnit.Framework;

[Timeout(10000)]
public class TestForScene1TestSample
{
    private AltUnityDriver altUnityDriver;
    [OneTimeSetUp]
    public void SetUp()
    {
        altUnityDriver = new AltUnityDriver(logFlag: true);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        altUnityDriver.Stop();
    }

    [SetUp]
    public void LoadLevel()
    {
        altUnityDriver.LoadScene("Scene 1 AltUnityDriverTestScene", true);
    }

    [Test]
    public void TestGetCurrentScene()
    {
        Assert.AreEqual("Scene 1 AltUnityDriverTestScene", altUnityDriver.GetCurrentScene());
    }

    [Test]
    public void TestFindElement()
    {
        const string name = "Capsule";
        var altElement = altUnityDriver.FindObject(By.NAME, name);
        Assert.NotNull(altElement);
        Assert.AreEqual(name, altElement.name);
    }

    [Test]
    public void TestFindElementWithText()
    {
        const string text = "Change Camera Mode";
        var altElement = altUnityDriver.FindObject(By.TEXT, text);
        Assert.NotNull(altElement);
    }

    [Test]
    public void TestFindElementWithTextByPath()
    {
        const string text = "Change Camera Mode";
        var altElement = altUnityDriver.FindObject(By.PATH, "//*[@text=" + text + "]");
        Assert.NotNull(altElement);
    }

    [Test]
    public void TestFindElementThatContainsText()
    {
        const string text = "Change Camera";
        var altElement = altUnityDriver.FindObject(By.PATH, "//*[contains(@text," + text + ")]");
        Assert.NotNull(altElement);
    }

    [Test]
    public void TestFindElements()
    {
        const string name = "Plane";
        var altElements = altUnityDriver.FindObjects(By.NAME, name);
        Assert.IsNotEmpty(altElements);
        Assert.AreEqual(altElements[0].name, name);
    }

    [Test]
    public void TestFindElementWhereNameContains()
    {
        const string name = "Cap";
        var altElement = altUnityDriver.FindObject(By.PATH, "//*[contains(@name," + name + ")]");
        Assert.NotNull(altElement);
        Assert.True(altElement.name.Contains(name));
    }
    [Test]
    public void TestFindElementsWhereNameContains()
    {
        const string name = "Pla";
        var altElements = altUnityDriver.FindObjects(By.PATH, "//*[contains(@name," + name + ")]");
        Assert.IsNotEmpty(altElements);
        Assert.True(altElements[0].name.Contains(name));
    }

    [Test]
    public void TestWaitForExistingElement()
    {
        const string name = "Capsule";
        var timeStart = DateTime.Now;
        var altElement = altUnityDriver.WaitForObject(By.NAME, name);
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
        var altElement = altUnityDriver.WaitForObject(By.NAME, name, enabled: false);
        var timeEnd = DateTime.Now;
        var time = timeEnd - timeStart;
        Assert.Less(time.TotalSeconds, 20);
        Assert.NotNull(altElement);
        Assert.AreEqual(altElement.name, name);
    }



    [Test]
    public void TestWaitForCurrentSceneToBe()
    {
        const string name = "Scene 1 AltUnityDriverTestScene";
        var timeStart = DateTime.Now;
        var currentScene = altUnityDriver.WaitForCurrentSceneToBe(name);
        var timeEnd = DateTime.Now;
        var time = timeEnd - timeStart;
        Assert.Less(time.TotalSeconds, 20);
        Assert.NotNull(currentScene);
        Assert.AreEqual("Scene 1 AltUnityDriverTestScene", currentScene);
    }



    [Test]
    public void TestWaitForExistingElementWhereNameContains()
    {
        const string name = "Dir";
        var timeStart = DateTime.Now;
        var altElement = altUnityDriver.WaitForObject(By.PATH, "//*[contains(@name," + name + ")]");
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
        string text = altUnityDriver.FindObject(By.NAME, name).GetText();
        var timeStart = DateTime.Now;
        var altElement = altUnityDriver.WaitForObjectWithText(By.NAME, name, text);
        var timeEnd = DateTime.Now;
        var time = timeEnd - timeStart;
        Assert.Less(time.TotalSeconds, 20);
        Assert.NotNull(altElement);
        Assert.AreEqual(altElement.GetText(), text);
    }

    [Test]
    public void TestSetTextForElement()
    {
        const string name = "InputField";
        const string text = "InputFieldTest";
        var input = altUnityDriver.FindObject(By.NAME, name).SetText(text);
        Assert.NotNull(input);
        Assert.AreEqual(input.GetText(), text);
    }

    [Test]
    public void TestFindObjectByComponent()
    {
        Thread.Sleep(1000);
        const string componentName = "AltUnityRunner";
        var altElement = altUnityDriver.FindObject(By.COMPONENT, componentName);
        Assert.NotNull(altElement);
        Assert.AreEqual(altElement.name, "AltUnityRunnerPrefab");
    }

    [Test]
    public void TestFindObjectByComponentWithNamespace()
    {
        Thread.Sleep(1000);
        const string componentName = "AltUnityTester.AltUnityDriver.AltUnityRunner";
        var altElement = altUnityDriver.FindObject(By.COMPONENT, componentName);
        Assert.NotNull(altElement);
        Assert.AreEqual(altElement.name, "AltUnityRunnerPrefab");
    }

    [Test]
    public void TestFindObjectByComponent2()
    {
        var altElement = altUnityDriver.FindObject(By.COMPONENT, "AltUnityExampleScriptCapsule");
        Assert.True(altElement.name.Equals("Capsule"));
    }
    [Test]
    public void TestFindObjectsByComponent()
    {
        var a = altUnityDriver.FindObjects(By.COMPONENT, "MeshFilter");
        Assert.AreEqual(5, a.Count);
    }

    [Test]
    public void TestGetComponentProperty()
    {
        Thread.Sleep(1000);
        const string componentName = "AltUnityRunner";
        const string propertyName = "SocketPortNumber";
        var altElement = altUnityDriver.FindObject(By.NAME, "AltUnityRunnerPrefab");
        Assert.NotNull(altElement);
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual(propertyValue, "13000");
    }

    [Test]
    public void TestGetComponentPropertyNotFoundWithAssembly()
    {
        Thread.Sleep(1000);
        const string componentName = "AltUnityRunner";
        const string propertyName = "InvalidProperty";
        var altElement = altUnityDriver.FindObject(By.NAME, "AltUnityRunnerPrefab");
        Assert.NotNull(altElement);
        try
        {
            var propertyValue = altElement.GetComponentProperty(componentName, propertyName, "Assembly-CSharp");
            Assert.Fail();
        }
        catch (PropertyNotFoundException exception)
        {
            Assert.IsTrue(exception.Message.StartsWith("Property InvalidProperty not found"), exception.Message);
        }
    }

    [Test]
    public void TestGetNonExistingComponentProperty()
    {
        Thread.Sleep(1000);
        const string componentName = "AltUnityRunner";
        const string propertyName = "socketPort";
        var altElement = altUnityDriver.FindObject(By.NAME, "AltUnityRunnerPrefab");
        Assert.NotNull(altElement);
        try
        {
            altElement.GetComponentProperty(componentName, propertyName);
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
        const string componentName = "AltUnityExampleScriptCapsule";
        const string propertyName = "arrayOfInts";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        Assert.NotNull(altElement);
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual("[1,2,3]", propertyValue);
    }

    [Test]
    public void TestGetComponentPropertyPrivate()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string propertyName = "privateVariable";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        Assert.NotNull(altElement);
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual("0", propertyValue);
    }

    [Test]
    public void TestGetComponentPropertyStatic()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string propertyName = "privateStaticVariable";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        Assert.NotNull(altElement);
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual("0", propertyValue);
    }
    [Test]
    public void TestGetComponentPropertyStaticPublic()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string propertyName = "PublicStaticVariable";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        Assert.NotNull(altElement);
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual("0", propertyValue);
    }

#if !UNITY_IOS
    [Test]
    public void TestGetComponentPropertyUnityEngine()
    {
        const string componentName = "UnityEngine.CapsuleCollider";
        const string propertyName = "isTrigger";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        Assert.NotNull(altElement);
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual("false", propertyValue);
    }
#endif

    [Test]
    public void TestSetComponentProperty()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string propertyName = "stringToSetFromTests";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        Assert.NotNull(altElement);
        var propertyValue = altElement.SetComponentProperty(componentName, propertyName, "2");
        Assert.AreEqual("valueSet", propertyValue);
        propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual("2", propertyValue);
    }

    [Test]
    public void TestSetNonExistingComponentProperty()
    {
        const string componentName = "Capsulee";
        const string propertyName = "stringToSetFromTests";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        Assert.NotNull(altElement);
        try
        {
            altElement.SetComponentProperty(componentName, propertyName, "2");
            Assert.Fail();
        }
        catch (ComponentNotFoundException exception)
        {
            Assert.IsTrue(exception.Message.StartsWith("Component not found"), exception.Message);
        }
    }

    [Test]
    [Obsolete]
    public void TestCallMethodWithNoParameters_Obsolete()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "UIButtonClicked";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, "");
        Assert.AreEqual("null", data);
    }
    [Test]
    public void TestCallMethodWithNoParameters()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "UIButtonClicked";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod<string>(componentName, methodName, new string[] { });
        Assert.IsNull(data);
    }

    [Test]
    [Obsolete]
    public void TestCallMethodWithParameters_Obsolete()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "Jump";
        const string parameters = "New Text";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, parameters);
        Assert.AreEqual("null", data);
    }
    [Test]
    public void TestCallMethodWithParameters()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "Jump";
        string[] parameters = new[] { "New Text" };
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod<string>(componentName, methodName, parameters);
        Assert.IsNull(data);
    }

    [Test]
    [Obsolete]
    public void TestCallMethodWithManyParameters_Obsolete()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithManyParameters";
        const string parameters = "1?stringparam?0.5?[1,2,3]";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, parameters);
        Assert.AreEqual("null", data);
    }
    [Test]
    public void TestCallMethodWithManyParameters()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithManyParameters";
        string[] parameters = new[] { "1", "stringparam", "0.5", "[1,2,3]" };
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod<string>(componentName, methodName, parameters);
        Assert.IsNull(data);
    }

    [Test]
    [Obsolete]
    public void TestCallMethodWithOptionalParemeters_Obsolete()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithOptionalParameters";
        const string parameters = "1?2";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, parameters);
        Assert.AreEqual("3", data);
    }
    [Test]
    public void TestCallMethodWithOptionalParemeters()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithOptionalParameters";
        string[] parameters = new[] { "1", "2" };
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod<string>(componentName, methodName, parameters);
        Assert.AreEqual("3", data);
    }

    [Test]
    [Obsolete]
    public void TestCallMethodWithOptionalParemetersString_Obsolete()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithOptionalParameters";
        const string parameters = "FirstParameter?SecondParameter";
        const string typeOfParameters = "System.String?System.String";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, parameters, typeOfParameters);
        Assert.AreEqual("\"FirstParameterSecondParameter\"", data);
    }

    [Test]
    public void TestCallMethodWithOptionalParemetersString()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithOptionalParameters";
        string[] parameters = new[] { "FirstParameter", "SecondParameter" };
        string[] typeOfParameters = new[] { "System.String", "System.String" };
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod<string>(componentName, methodName, parameters, typeOfParameters);
        Assert.AreEqual("FirstParameterSecondParameter", data);
    }

    [Test]
    [Obsolete]
    public void TestCallMethodWithOptionalParemetersString2_Obsolete()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithOptionalParameters";
        const string parameters = "FirstParameter?";
        const string typeOfParameters = "System.String?System.String";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, parameters, typeOfParameters);
        Assert.AreEqual("\"FirstParameter\"", data);
    }

    [Test]
    public void TestCallMethodWithOptionalParemetersString2()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithOptionalParameters";
        string[] parameters = new[] { "FirstParameter", "" };
        string[] typeOfParameters = new[] { "System.String", "System.String" };
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod<string>(componentName, methodName, parameters, typeOfParameters);
        Assert.AreEqual("FirstParameter", data);
    }

    [Test]
    [Obsolete]
    public void TestCallMethodWithIncorrectNumberOfParameters_Obsolete()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithManyParameters";
        const string parameters = "1?stringparam?[1,2,3]";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        try
        {
            altElement.CallComponentMethod(componentName, methodName, parameters);
            Assert.Fail();
        }
        catch (MethodWithGivenParametersNotFoundException exception)
        {
            Assert.IsTrue(exception.Message.StartsWith("No method found with 3 parameters matching signature: TestMethodWithManyParameters()"), exception.Message);
        }
    }
    [Test]
    public void TestCallMethodWithIncorrectNumberOfParameters()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithManyParameters";
        string[] parameters = new[] { "1", "stringparam", "[1,2,3]" };
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        try
        {
            altElement.CallComponentMethod<string>(componentName, methodName, parameters);
            Assert.Fail();
        }
        catch (MethodWithGivenParametersNotFoundException exception)
        {
            Assert.IsTrue(exception.Message.StartsWith("No method found with 3 parameters matching signature: TestMethodWithManyParameters()"), exception.Message);
        }
    }

    [Test]
    [Obsolete]
    public void TestCallMethodWithIncorrectNumberOfParameters2_Obsolete()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithManyParameters";
        const string parameters = "a?stringparam?[1,2,3]";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        try
        {
            altElement.CallComponentMethod(componentName, methodName, parameters);
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
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithManyParameters";
        string[] parameters = new[] { "a", "stringparam", "[1,2,3]" };
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        try
        {
            altElement.CallComponentMethod<string>(componentName, methodName, parameters);
            Assert.Fail();
        }
        catch (MethodWithGivenParametersNotFoundException exception)
        {
            Assert.IsTrue(exception.Message.StartsWith("No method found with 3 parameters matching signature: TestMethodWithManyParameters()"), exception.Message);
        }
    }

    [Test]
    [Obsolete]
    public void TestCallMethodAssmeblyNotFound_Obsolete()
    {
        const string componentName = "RandomComponent";
        const string methodName = "TestMethodWithManyParameters";
        const string parameters = "a?stringparam?0.5?[1,2,3]";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        try
        {
            altElement.CallComponentMethod(componentName, methodName, parameters, "", "RandomAssembly");
            Assert.Fail();
        }
        catch (AssemblyNotFoundException exception)
        {
            Assert.IsTrue(exception.Message.StartsWith("Assembly not found"), exception.Message);
        }
    }
    [Test]
    public void TestCallMethodAssmeblyNotFound()
    {
        const string componentName = "RandomComponent";
        const string methodName = "TestMethodWithManyParameters";
        string[] parameters = new[] { "a", "stringparam", "0.5", "[1,2,3]" };
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        try
        {
            altElement.CallComponentMethod<string>(componentName, methodName, parameters, null, "RandomAssembly");
            Assert.Fail();
        }
        catch (AssemblyNotFoundException exception)
        {
            Assert.IsTrue(exception.Message.StartsWith("Assembly not found"), exception.Message);
        }
    }

    [Test]
    [Obsolete]
    public void TestCallMethodInvalidMethodArgumentTypes_Obsolete()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithManyParameters";
        const string parameters = "stringnotint?stringparam?0.5?[1,2,3]";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        try
        {
            altElement.CallComponentMethod(componentName, methodName, parameters);
            Assert.Fail();
        }
        catch (FailedToParseArgumentsException exception)
        {
            Assert.IsTrue(exception.Message.StartsWith("Could not parse parameter 'stringnotint' to type System.Int"), exception.Message);
        }
    }

    [Test]
    public void TestCallMethodInvalidMethodArgumentTypes()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithManyParameters";
        string[] parameters = new[] { "stringnotint", "stringparam", "0.5", "1,2,3]" };
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        try
        {
            altElement.CallComponentMethod<string>(componentName, methodName, parameters);
            Assert.Fail();
        }
        catch (FailedToParseArgumentsException exception)
        {
            Assert.IsTrue(exception.Message.StartsWith("Could not parse parameter 'stringnotint' to type System.Int"), exception.Message);
        }
    }
    [Test]
    [Obsolete]
    public void TestCallMethodInvalidParameterType_Obsolete()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithManyParameters";
        const string parameters = "1?stringparam?0.5?[1,2,3]";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        try
        {
            altElement.CallComponentMethod(componentName, methodName, parameters, "System.Stringggggg");
            Assert.Fail();
        }
        catch (InvalidParameterTypeException exception)
        {
            Assert.IsTrue(exception.Message.StartsWith("Number of parameters different than number of types of parameters"), exception.Message);
        }
    }

    [Test]
    public void TestCallMethodInvalidParameterType()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "TestMethodWithManyParameters";
        string[] parameters = new[] { "1", "stringparam", "0.5", "[1,2,3]" };
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        try
        {
            altElement.CallComponentMethod<string>(componentName, methodName, parameters, new[] { "System.Stringggggg" });
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
        altUnityDriver.DeletePlayerPref();
        altUnityDriver.SetKeyPlayerPref("test", 1);
        var val = altUnityDriver.GetIntKeyPlayerPref("test");
        Assert.AreEqual(1, val);
    }
    [Test]
    public void TestSetKeyFloat()
    {
        altUnityDriver.DeletePlayerPref();
        altUnityDriver.SetKeyPlayerPref("test", 1f);
        var val = altUnityDriver.GetFloatKeyPlayerPref("test");
        Assert.AreEqual(1f, val);
    }

    [Test]
    public void TestSetKeyString()
    {
        altUnityDriver.DeletePlayerPref();
        altUnityDriver.SetKeyPlayerPref("test", "test");
        var val = altUnityDriver.GetStringKeyPlayerPref("test");
        Assert.AreEqual("test", val);
    }

    [Test]
    public void TestDeleteKey()
    {
        altUnityDriver.DeletePlayerPref();
        altUnityDriver.SetKeyPlayerPref("test", 1);
        var val = altUnityDriver.GetIntKeyPlayerPref("test");
        Assert.AreEqual(1, val);
        altUnityDriver.DeleteKeyPlayerPref("test");
        try
        {
            altUnityDriver.GetIntKeyPlayerPref("test");
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
        var altButton = altUnityDriver.FindObject(By.NAME, "Button", By.NAME, "Main Camera");
        altButton.Click();
        altButton.Click();
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule", By.NAME, "Main Camera");
        var altElement2 = altUnityDriver.FindObject(By.NAME, "Capsule", By.NAME, "Camera");
        AltUnityVector2 pozOnScreenFromMainCamera = new AltUnityVector2(altElement.x, altElement.y);
        AltUnityVector2 pozOnScreenFromSecondaryCamera = new AltUnityVector2(altElement2.x, altElement2.y);

        Assert.AreNotEqual(pozOnScreenFromSecondaryCamera, pozOnScreenFromMainCamera);

    }

    [Test]
    public void TestFindNonExistentObject()
    {
        try
        {
            altUnityDriver.FindObject(By.NAME, "NonExistent");
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
            altUnityDriver.FindObject(By.PATH, "//*[contains(@name,NonExistent)]");
            Assert.Fail();
        }
        catch (NotFoundException exception)
        {
            Assert.IsTrue(exception.Message.StartsWith("Object //*[contains(@name,NonExistent)] not found"), exception.Message);
        }

    }

    [Test]
    public void TestButtonClickWithSwipe()
    {
        var button = altUnityDriver.FindObject(By.NAME, "UIButton");
        AltUnityVector2 vector2 = new AltUnityVector2(button.x, button.y);
        altUnityDriver.HoldButtonAndWait(vector2, 1);
        var capsuleInfo = altUnityDriver.FindObject(By.NAME, "CapsuleInfo");
        Thread.Sleep(1400);
        var text = capsuleInfo.GetText();
        Assert.AreEqual(text, "UIButton clicked to jump capsule!");
    }

    [Test]
    [Obsolete]
    public void TestClickElement()
    {
        const string name = "Capsule";
        var altElement = altUnityDriver.FindObject(By.NAME, name).Tap();
        Assert.AreEqual(name, altElement.name);
        altUnityDriver.WaitForObjectWithText(By.NAME, "CapsuleInfo", "Capsule was clicked to jump!");
    }

    [Test]
    [Obsolete]
    public void TestClickScreen()
    {
        const string name = "UIButton";
        var altElement2 = altUnityDriver.FindObject(By.NAME, name);
        var altElement = altUnityDriver.TapScreen(altElement2.x, altElement2.y);
        Assert.AreEqual(name, altElement.name);
        altUnityDriver.WaitForObjectWithText(By.NAME, "CapsuleInfo", "UIButton clicked to jump capsule!");
    }

    [Test]
    public void TestWaitForNonExistingObject()
    {
        try
        {
            altUnityDriver.WaitForObject(By.NAME, "dlkasldkas", timeout: 1, interval: 1);
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
        altUnityDriver.WaitForObjectNotBePresent(By.NAME, "ObjectDestroyedIn5Secs");
        altUnityDriver.WaitForObjectNotBePresent(By.NAME, "Capsulee", timeout: 1, interval: 0.5f);
    }

    [Test]
    public void TestWaitForObjectToNotExistFail()
    {
        try
        {
            altUnityDriver.WaitForObjectNotBePresent(By.NAME, "Capsule", timeout: 1, interval: 0.5f);
            Assert.Fail();
        }
        catch (WaitTimeOutException exception)
        {
            Assert.AreEqual("Element //Capsule still found after 1 seconds", exception.Message);
        }
    }

    [Test]
    [Obsolete]
    public void TestWaitForObjectWithTextWrongText()
    {
        try
        {
            altUnityDriver.WaitForObjectWithText(By.NAME, "CapsuleInfo", "aaaaa", timeout: 1);
            Assert.Fail();
        }
        catch (WaitTimeOutException exception)
        {
            Assert.AreEqual("Element with text: aaaaa not loaded after 1 seconds", exception.Message);
        }
    }

    [Test]
    public void TestWaitForCurrrentSceneToBeANonExistingScene()
    {
        const string name = "AltUnityDriverTestScenee";
        try
        {
            altUnityDriver.WaitForCurrentSceneToBe(name, 1);
            Assert.Fail();
        }
        catch (WaitTimeOutException exception)
        {
            Assert.AreEqual("Scene AltUnityDriverTestScenee not loaded after 1 seconds", exception.Message);
        }

    }


    [Test]
    public void TestWaitForNonExistingElementWhereNameContains()
    {
        const string name = "xyz";
        try
        {
            altUnityDriver.WaitForObject(By.PATH, "//*[contains(@name," + name + ")]", timeout: 1);
            Assert.Fail();
        }
        catch (WaitTimeOutException exception)
        {
            Assert.AreEqual("Element //*[contains(@name,xyz)] not loaded after 1 seconds", exception.Message);
        }
    }

    [Test]
    [Obsolete]
    public void TestCallStaticMethod_Obsolete()
    {
        altUnityDriver.CallStaticMethod("UnityEngine.PlayerPrefs", "SetInt", "Test?1");
        int a = int.Parse(altUnityDriver.CallStaticMethod("UnityEngine.PlayerPrefs", "GetInt", "Test?2"));
        Assert.AreEqual(1, a);

    }
    [Test]
    public void TestCallStaticMethod()
    {
        altUnityDriver.CallStaticMethod<string>("UnityEngine.PlayerPrefs", "SetInt", new[] { "Test", "1" });
        int a = altUnityDriver.CallStaticMethod<int>("UnityEngine.PlayerPrefs", "GetInt", new[] { "Test", "2" });
        Assert.AreEqual(1, a);

    }
    [Test]
    [Obsolete]
    public void TestCallMethodWithMultipleDefinitions_Obsolete()
    {

        AltUnityObject capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        capsule.CallComponentMethod("AltUnityExampleScriptCapsule", "Test", "2", "System.Int32");
        AltUnityObject capsuleInfo = altUnityDriver.FindObject(By.NAME, "CapsuleInfo");
        Assert.AreEqual("6", capsuleInfo.GetText());
    }
    [Test]
    public void TestCallMethodWithMultipleDefinitions()
    {

        AltUnityObject capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        capsule.CallComponentMethod<string>("AltUnityExampleScriptCapsule", "Test", new[] { "2" }, new[] { "System.Int32" });
        AltUnityObject capsuleInfo = altUnityDriver.FindObject(By.NAME, "CapsuleInfo");
        Assert.AreEqual("6", capsuleInfo.GetText());
    }
    [Test]
    [Obsolete]
    public void TestCallMethodWithAssembly_Obsolete()
    {
        AltUnityObject capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var initialRotation = capsule.GetComponentProperty("UnityEngine.Transform", "rotation");
        capsule.CallComponentMethod("UnityEngine.Transform", "Rotate", "10?10?10", "System.Single?System.Single?System.Single", "UnityEngine.CoreModule");
        AltUnityObject capsuleAfterRotation = altUnityDriver.FindObject(By.NAME, "Capsule");
        var finalRotation = capsuleAfterRotation.GetComponentProperty("UnityEngine.Transform", "rotation");
        Assert.AreNotEqual(initialRotation, finalRotation);
    }
    [Test]
    public void TestCallMethodWithAssembly()
    {
        AltUnityObject capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var initialRotation = capsule.GetComponentProperty("UnityEngine.Transform", "rotation");
        capsule.CallComponentMethod<string>("UnityEngine.Transform", "Rotate", new[] { "10", "10", "10" }, new[] { "System.Single", "System.Single", "System.Single" }, "UnityEngine.CoreModule");
        AltUnityObject capsuleAfterRotation = altUnityDriver.FindObject(By.NAME, "Capsule");
        var finalRotation = capsuleAfterRotation.GetComponentProperty("UnityEngine.Transform", "rotation");
        Assert.AreNotEqual(initialRotation, finalRotation);
    }

    [Test]
    public void TestGetAllComponents()
    {
        List<AltUnityComponent> components = altUnityDriver.FindObject(By.NAME, "Canvas").GetAllComponents();
        Assert.AreEqual(5, components.Count);
        Assert.AreEqual("UnityEngine.RectTransform", components[0].componentName);
        Assert.AreEqual("UnityEngine.CoreModule", components[0].assemblyName);
    }

    public void TestGetAllMethodsFromClass()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var component2 = altElement.GetAllComponents().First(component => component.componentName.Equals("AltUnityExampleScriptCapsule"));
        List<string> methods = altElement.GetAllMethods(component2, AltUnityMethodSelection.CLASSMETHODS);
        Assert.IsTrue(methods.Contains("Void UIButtonClicked()"));
        Assert.IsFalse(methods.Contains("Void CancelInvoke(System.String)"));
    }
    [Test]
    public void TestGetAllMethodsFromInherited()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var component2 = altElement.GetAllComponents().First(component => component.componentName.Equals("AltUnityExampleScriptCapsule"));
        List<string> methods = altElement.GetAllMethods(component2, AltUnityMethodSelection.INHERITEDMETHODS);
        Assert.IsTrue(methods.Contains("Void CancelInvoke(System.String)"));
        Assert.IsFalse(methods.Contains("Void UIButtonClicked()"));
    }
    [Test]
    public void TestGetAllMethods()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var component2 = altElement.GetAllComponents().First(component => component.componentName.Equals("AltUnityExampleScriptCapsule"));
        List<string> methods = altElement.GetAllMethods(component2, AltUnityMethodSelection.ALLMETHODS);
        Assert.IsTrue(methods.Contains("Void CancelInvoke(System.String)"));
        Assert.IsTrue(methods.Contains("Void UIButtonClicked()"));
    }

    [Test]
    public void TestGetAllProperties()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var componentList = altElement.GetAllComponents();
        var component = componentList.First(componenta =>
            componenta.componentName.Equals("AltUnityExampleScriptCapsule") && componenta.assemblyName.Equals("Assembly-CSharp"));
        List<AltUnityProperty> properties = altElement.GetAllProperties(component, AltUnityPropertiesSelections.ALLPROPERTIES);
        if (properties.Exists(prop => prop.name.Equals("runInEditMode")))
        {
            Assert.AreEqual(11, properties.Count);
        }
        else
        {
            Assert.AreEqual(10, properties.Count);// if runned from editor then there are 11 properties, runInEditMode is only available in Editor
        }
        AltUnityProperty property = properties.First(prop => prop.name.Equals("TestProperty"));
        Assert.NotNull(property);
        Assert.AreEqual("False", property.value);
    }
    [Test]
    public void TestGetAllClassProperties()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var componentList = altElement.GetAllComponents();
        var component = componentList.First(componenta =>
            componenta.componentName.Equals("AltUnityExampleScriptCapsule") && componenta.assemblyName.Equals("Assembly-CSharp"));
        List<AltUnityProperty> properties = altElement.GetAllProperties(component, AltUnityPropertiesSelections.CLASSPROPERTIES);
        Assert.AreEqual(2, properties.Count);
        AltUnityProperty property = properties.First(prop => prop.name.Equals("TestProperty"));
        Assert.NotNull(property);
        Assert.AreEqual("False", property.value);
    }
    [Test]
    public void TestGetAllInheritedProperties()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var componentList = altElement.GetAllComponents();
        var component = componentList.First(componenta =>
            componenta.componentName.Equals("AltUnityExampleScriptCapsule") && componenta.assemblyName.Equals("Assembly-CSharp"));
        List<AltUnityProperty> properties = altElement.GetAllProperties(component, AltUnityPropertiesSelections.INHERITEDPROPERTIES);
        if (properties.Exists(prop => prop.name.Equals("runInEditMode")))
        {
            Assert.AreEqual(9, properties.Count);
        }
        else
        {
            Assert.AreEqual(8, properties.Count);// if runned from editor then there are 9 properties, runInEditMode is only available in Editor
        }
    }

    [Test]
    public void TestGetAllClassFields()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var componentList = altElement.GetAllComponents();
        var component = componentList.First(componenta =>
            componenta.componentName.Equals("AltUnityExampleScriptCapsule") && componenta.assemblyName.Equals("Assembly-CSharp"));

        List<AltUnityProperty> fields = altElement.GetAllFields(component, AltUnityFieldsSelections.CLASSFIELDS);
        Assert.AreEqual(13, fields.Count);
    }

    [Test]
    public void TestGetAllInheritedFields()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var componentList = altElement.GetAllComponents();
        var component = componentList.First(componenta =>
            componenta.componentName.Equals("AltUnityExampleScriptCapsule") && componenta.assemblyName.Equals("Assembly-CSharp"));
        List<AltUnityProperty> fields = altElement.GetAllFields(component, AltUnityFieldsSelections.INHERITEDFIELDS);
        AltUnityProperty field = fields.First(fld => fld.name.Equals("inheritedField"));
        Assert.AreEqual("False", field.value);
        Assert.AreEqual(1, fields.Count);
        Assert.AreEqual(AltUnityType.PRIMITIVE, field.type);
    }

    [Test]
    public void TestGetAllFields()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var componentList = altElement.GetAllComponents();
        var component = componentList.First(componenta =>
            componenta.componentName.Equals("AltUnityExampleScriptCapsule") && componenta.assemblyName.Equals("Assembly-CSharp"));
        List<AltUnityProperty> fields = altElement.GetAllFields(component, AltUnityFieldsSelections.ALLFIELDS);
        Assert.AreEqual(14, fields.Count);
    }

    [Test]
    public void TestFindObjectByTag()
    {
        var altElement = altUnityDriver.FindObject(By.TAG, "plane");
        Assert.True(altElement.name.Equals("Plane"));
    }
    [Test]
    public void TestFindObjectByLayer()
    {
        var altElement = altUnityDriver.FindObject(By.LAYER, "Water");
        Assert.True(altElement.name.Equals("Capsule"));
    }
    [Test]
    public void TestFindObjectByUnityComponent()
    {
        var altElement = altUnityDriver.FindObject(By.COMPONENT, "CapsuleCollider");
        Assert.True(altElement.name.Equals("Capsule"));
    }

    [Test]
    public void TestFindChild()
    {
        var altElement = altUnityDriver.FindObject(By.PATH, "//UIButton/*");
        Assert.True(altElement.name.Equals("Text"));
    }
    [TestCase("//*[contains(@name,Cub)]", "Cube")]
    [TestCase("//RotateMainCameraButton/../*[contains(@name,Seconda)]/Text", "Text")]
    [TestCase("//*[@component=BoxCollider]", "Cube")]
    [TestCase("/Capsule/../Plane", "Plane")]
    public void TestFindingDifferentObjects(string path, string result)
    {
        var altElement = altUnityDriver.FindObject(By.PATH, path, enabled: false);
        Assert.True(altElement.name.Equals(result));

    }

    [Test]
    public void TestFindObjectsByTag()
    {
        var altElements = altUnityDriver.FindObjects(By.TAG, "plane");
        Assert.AreEqual(2, altElements.Count);
        foreach (var altElement in altElements)
        {
            Assert.AreEqual("Plane", altElement.name);
        }
    }

    [Test]
    public void TestFindObjectsByLayer()
    {
        var altElements = altUnityDriver.FindObjects(By.LAYER, "Default");
        Assert.AreEqual(12, altElements.Count);
    }
    [Test]
    public void TestFindObjectsByContainName()
    {
        var altElements = altUnityDriver.FindObjects(By.PATH, "//*[contains(@name,Ro)]");
        foreach (var altElement in altElements)
        {
            Assert.True(altElement.name.Contains("Ro"));
        }
        Assert.AreEqual(2, altElements.Count);

    }


    [Test]
    public void TestInactiveObject()
    {
        AltUnityObject cube = altUnityDriver.FindObject(By.NAME, "Cube", enabled: false);
        Assert.AreEqual(false, cube.enabled);
    }
    [Test]
    public void TestGetAllScenes()
    {
        var scenes = altUnityDriver.GetAllScenes();
        Assert.AreEqual(6, scenes.Count);
        Assert.AreEqual("Scene 1 AltUnityDriverTestScene", scenes[0]);
    }

    [Test]
    [Obsolete("The new tap method does not return the object tapped.")]
    public void TestTapScreenWhereThereIsNoObjects()
    {
        var counterButton = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        AltUnityObject altObject = altUnityDriver.TapScreen(counterButton.x + 50, counterButton.y + 50);
        Assert.AreEqual(null, altObject);
    }
    [Test]
    public void TestSetTimeScale()
    {
        altUnityDriver.SetTimeScale(0.1f);
        Thread.Sleep(1000);
        var timeScaleFromGame = altUnityDriver.GetTimeScale();
        Assert.AreEqual(0.1f, timeScaleFromGame);
        altUnityDriver.SetTimeScale(1);
    }
    [Test]
    public void TestWaitForObjectWhichContains()
    {
        var altElement = altUnityDriver.WaitForObjectWhichContains(By.NAME, "Canva");
        Assert.AreEqual("Canvas", altElement.name);

    }
    [Test]
    public void TestFindObjectWhichContains()
    {
        var altElement = altUnityDriver.FindObjectWhichContains(By.NAME, "EventSys");
        Assert.AreEqual("EventSystem", altElement.name);
    }
    [Test]
    public void TestFindWithFindObjectWhichContainsNotExistingObject()
    {
        try
        {
            var altElement = altUnityDriver.FindObjectWhichContains(By.NAME, "EventNonExisting");
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
        var cameras = altUnityDriver.GetAllCameras();
        Assert.AreEqual(2, cameras.Count);
    }

    [Test]
    public void TestGetAllActiveCameras()
    {
        var cameras = altUnityDriver.GetAllActiveCameras();
        Assert.AreEqual(1, cameras.Count);
    }

    [Test]
    public void TestGetAllElementsLight()
    {
        var altElements = altUnityDriver.GetAllElementsLight();

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
        var altElements = altUnityDriver.FindObjects(By.PATH, "//Canvas/*/Text");
        Assert.AreEqual(8, altElements.Count);
    }

    [Test]
    public void TestFindObjectScene6()
    {
        altUnityDriver.LoadScene("Scene6");

        Thread.Sleep(1000);
        altUnityDriver.WaitForCurrentSceneToBe("Scene6");
        var altElements = altUnityDriver.FindObjects(By.PATH, "//Canvas/*/Text");
        Assert.AreEqual(3, altElements.Count);
        altElements = altUnityDriver.FindObjects(By.PATH, "/*/*/Text");
        Assert.AreEqual(3, altElements.Count);
        altElements = altUnityDriver.FindObjects(By.PATH, "/*/Text");
        Assert.AreEqual(1, altElements.Count);
        altElements = altUnityDriver.FindObjects(By.PATH, "//Canvas//Text");
        Assert.AreEqual(5, altElements.Count);
        altElements = altUnityDriver.FindObjects(By.PATH, "//Canvas/*//Text");
        Assert.AreEqual(4, altElements.Count);

        Assert.AreEqual("First", altUnityDriver.FindObject(By.PATH, "/Canvas/First").name);
        Assert.AreEqual("WorldSpaceButton", altUnityDriver.FindObject(By.PATH, "/Canvas/WorldSpaceButton").name);
    }
    [Test]
    public void TestGetScreenshot()
    {
        var path = "testC.png";
        altUnityDriver.GetPNGScreenshot(path);
        FileAssert.Exists(path);
    }
    [Test]
    public void TestGetChineseLetters()
    {
        var text = altUnityDriver.FindObject(By.NAME, "ChineseLetters").GetText();
        Assert.AreEqual("哦伊娜哦", text);
    }
    [Test]
    public void TestNonEnglishText()
    {
        var text = altUnityDriver.FindObject(By.NAME, "NonEnglishText").GetText();
        Assert.AreEqual("BJÖRN'S PASS", text);
    }
    [Test]
    [Obsolete]
    public void TestDoubleTap()
    {
        var counterButton = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        var counterButtonText = altUnityDriver.FindObject(By.NAME, "ButtonCounter/Text");
        counterButton.DoubleTap();
        Thread.Sleep(500);
        Assert.AreEqual("2", counterButtonText.GetText());
    }

    [Test]
    public void TestSwipeWithDuration0()
    {
        altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        var counterButtonText = altUnityDriver.FindObject(By.NAME, "ButtonCounter/Text");
        altUnityDriver.Swipe(counterButtonText.getScreenPosition(), counterButtonText.getScreenPosition(), 0);
        Thread.Sleep(500);
        Assert.AreEqual("1", counterButtonText.GetText());
    }

    [Test]
    public void TestCustomTap()
    {
        var counterButton = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        var counterButtonText = altUnityDriver.FindObject(By.NAME, "ButtonCounter/Text");
        altUnityDriver.Tap(counterButton.getScreenPosition(), 4);
        Thread.Sleep(1000);
        Assert.AreEqual("4", counterButtonText.GetText());
    }
    [Test]
    public void TestGet3DObjectFromScreenshot()
    {
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        AltUnityObject altUnityObject;
        altUnityDriver.GetScreenshot(new AltUnityVector2(capsule.x, capsule.y), new AltUnityColor(1, 0, 0, 1), 1, out altUnityObject, new AltUnityVector2(1920, 1080));
        Assert.AreEqual("Capsule", altUnityObject.name);
    }

    [Test]
    public void TestGetUIObjectFromScreenshot()
    {
        var capsuleInfo = altUnityDriver.FindObject(By.NAME, "CapsuleInfo");
        AltUnityObject altUnityObject;
        altUnityDriver.GetScreenshot(new AltUnityVector2(capsuleInfo.x, capsuleInfo.y), new AltUnityColor(1, 0, 0, 1), 1, out altUnityObject, new AltUnityVector2(1920, 1080));
        Assert.AreEqual("CapsuleInfo", altUnityObject.name);
    }
    [Test]
    public void TestObjectFromScreenshot()
    {
        var icon = altUnityDriver.FindObject(By.NAME, "Icon");
        AltUnityVector2 offscreenCoordinates = new AltUnityVector2(icon.x + 400, icon.y);
        AltUnityObject altUnityObject;
        altUnityDriver.GetScreenshot(offscreenCoordinates, new AltUnityColor(1, 0, 0, 1), 1, out altUnityObject, new AltUnityVector2(1920, 1080));
        Assert.IsNull(altUnityObject);
    }

    [Test]
    public void TestPressNextSceneButtton()
    {
        var initialScene = altUnityDriver.GetCurrentScene();
        altUnityDriver.FindObject(By.NAME, "NextScene").Tap();
        var currentScene = altUnityDriver.GetCurrentScene();
        Assert.AreNotEqual(initialScene, currentScene);
    }
    [Test]
    public void TestForSetText()
    {
        var text = altUnityDriver.FindObject(By.NAME, "NonEnglishText");
        var originalText = text.GetText();
        var afterText = text.SetText("ModifiedText").GetText();
        Assert.AreNotEqual(originalText, afterText);
    }
    [Test]
    public void TestFindParentUsingPath()
    {
        var parent = altUnityDriver.FindObject(By.PATH, "//CapsuleInfo/..");
        Assert.AreEqual("Canvas", parent.name);
    }

    public void TestFindObjectWithCameraId()
    {
        var altButton = altUnityDriver.FindObject(By.PATH, "//Button");
        altButton.Click();
        altButton.Click();
        var camera = altUnityDriver.FindObject(By.PATH, "//Camera");
        var altElement = altUnityDriver.FindObject(By.COMPONENT, "CapsuleCollider", By.ID, camera.id.ToString());
        Assert.True(altElement.name.Equals("Capsule"));
        var camera2 = altUnityDriver.FindObject(By.PATH, "//Main Camera");
        var altElement2 = altUnityDriver.FindObject(By.COMPONENT, "CapsuleCollider", By.ID, camera2.id.ToString());
        Assert.AreNotEqual(altElement.getScreenPosition(), altElement2.getScreenPosition());
    }

    [Test]
    public void TestWaitForObjectWithCameraId()
    {
        var altButton = altUnityDriver.FindObject(By.PATH, "//Button");
        altButton.Click();
        altButton.Click();
        var camera = altUnityDriver.FindObject(By.PATH, "//Camera");
        var altElement = altUnityDriver.WaitForObject(By.COMPONENT, "CapsuleCollider", By.ID, camera.id.ToString());
        Assert.True(altElement.name.Equals("Capsule"));
        var camera2 = altUnityDriver.FindObject(By.PATH, "//Main Camera");
        var altElement2 = altUnityDriver.WaitForObject(By.COMPONENT, "CapsuleCollider", By.ID, camera2.id.ToString());
        Assert.AreNotEqual(altElement.getScreenPosition(), altElement2.getScreenPosition());
    }

    [Test]
    public void TestFindObjectsWithCameraId()
    {
        var altButton = altUnityDriver.FindObject(By.PATH, "//Button");
        altButton.Click();
        altButton.Click();
        var camera = altUnityDriver.FindObject(By.PATH, "//Camera");
        var altElement = altUnityDriver.FindObjects(By.NAME, "Plane", By.ID, camera.id.ToString());
        Assert.True(altElement[0].name.Equals("Plane"));
        var camera2 = altUnityDriver.FindObject(By.PATH, "//Main Camera");
        var altElement2 = altUnityDriver.FindObjects(By.NAME, "Plane", By.ID, camera2.id.ToString());
        Assert.AreNotEqual(altElement[0].getScreenPosition(), altElement2[0].getScreenPosition());
    }

    [Test]
    public void TestWaitForObjectNotBePresentWithCameraId()
    {
        var camera2 = altUnityDriver.FindObject(By.PATH, "//Main Camera");
        altUnityDriver.WaitForObjectNotBePresent(By.NAME, "ObjectDestroyedIn5Secs", By.ID, camera2.id.ToString());

        var allObjectsInTheScene = altUnityDriver.GetAllElements();
        Assert.IsTrue(!allObjectsInTheScene.Any(obj => obj.name.Equals("ObjectDestroyedIn5Secs")));
    }

    [Test]
    [Obsolete]
    public void TestWaitForElementWithTextWithCameraId()
    {
        const string name = "CapsuleInfo";
        string text = altUnityDriver.FindObject(By.NAME, name).GetText();
        var timeStart = DateTime.Now;
        var camera2 = altUnityDriver.FindObject(By.PATH, "//Main Camera");
        var altElement = altUnityDriver.WaitForObjectWithText(By.NAME, name, text, By.ID, camera2.id.ToString());
        var timeEnd = DateTime.Now;
        var time = timeEnd - timeStart;
        Assert.Less(time.TotalSeconds, 20);
        Assert.NotNull(altElement);
        Assert.AreEqual(altElement.GetText(), text);
    }

    [Test]
    public void TestWaitForObjectWhichContainsWithCameraId()
    {
        var camera2 = altUnityDriver.FindObject(By.PATH, "//Main Camera");
        var altElement = altUnityDriver.WaitForObjectWhichContains(By.NAME, "Canva", By.ID, camera2.id.ToString());
        Assert.AreEqual("Canvas", altElement.name);

    }


    [Test]
    public void TestFindObjectWithTag()
    {
        var altButton = altUnityDriver.FindObject(By.PATH, "//Button");
        altButton.Click();
        altButton.Click();
        var altElement = altUnityDriver.FindObject(By.COMPONENT, "CapsuleCollider", By.TAG, "MainCamera");
        Assert.True(altElement.name.Equals("Capsule"));
        var altElement2 = altUnityDriver.FindObject(By.COMPONENT, "CapsuleCollider", By.TAG, "Untagged");
        Assert.AreNotEqual(altElement.getScreenPosition(), altElement2.getScreenPosition());
    }

    [Test]
    public void TestWaitForObjectWithTag()
    {
        var altButton = altUnityDriver.FindObject(By.PATH, "//Button");
        altButton.Click();
        altButton.Click();
        var altElement = altUnityDriver.WaitForObject(By.COMPONENT, "CapsuleCollider", By.TAG, "MainCamera");
        Assert.True(altElement.name.Equals("Capsule"));
        var altElement2 = altUnityDriver.WaitForObject(By.COMPONENT, "CapsuleCollider", By.TAG, "Untagged");
        Assert.AreNotEqual(altElement.getScreenPosition(), altElement2.getScreenPosition());
    }

    [Test]
    public void TestFindObjectsWithTag()
    {
        var altButton = altUnityDriver.FindObject(By.PATH, "//Button");
        altButton.Click();
        altButton.Click();
        var altElement = altUnityDriver.FindObjects(By.NAME, "Capsule", By.TAG, "MainCamera");
        Assert.True(altElement[0].name.Equals("Capsule"));
        var altElement2 = altUnityDriver.FindObjects(By.NAME, "Capsule", By.TAG, "Untagged");
        Assert.AreNotEqual(altElement[0].getScreenPosition(), altElement2[0].getScreenPosition());
    }

    [Test]
    public void TestWaitForObjectNotBePresentWithTag()
    {
        var camera2 = altUnityDriver.FindObject(By.PATH, "//Main Camera");
        altUnityDriver.WaitForObjectNotBePresent(By.NAME, "ObjectDestroyedIn5Secs", By.TAG, "MainCamera");

        var allObjectsInTheScene = altUnityDriver.GetAllElements();
        Assert.IsTrue(!allObjectsInTheScene.Any(obj => obj.name.Equals("ObjectDestroyedIn5Secs")));
    }

    [Test]
    [Obsolete]
    public void TestWaitForElementWithTextWithTag()
    {
        const string name = "CapsuleInfo";
        string text = altUnityDriver.FindObject(By.NAME, name).GetText();
        var timeStart = DateTime.Now;
        var altElement = altUnityDriver.WaitForObjectWithText(By.NAME, name, text, By.TAG, "MainCamera");
        var timeEnd = DateTime.Now;
        var time = timeEnd - timeStart;
        Assert.Less(time.TotalSeconds, 20);
        Assert.NotNull(altElement);
        Assert.AreEqual(altElement.GetText(), text);
    }

    [Test]
    public void TestWaitForObjectWhichContainsWithTag()
    {
        var altElement = altUnityDriver.WaitForObjectWhichContains(By.NAME, "Canva", By.TAG, "MainCamera");
        Assert.AreEqual("Canvas", altElement.name);

    }

    public void TestAcceleration()
    {
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var initialWorldCoordinates = capsule.getWorldPosition();
        altUnityDriver.Tilt(new AltUnityVector3(1, 1, 1), 1);
        Thread.Sleep(1000);
        capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var afterTiltCoordinates = capsule.getWorldPosition();
        Assert.AreNotEqual(initialWorldCoordinates, afterTiltCoordinates);
    }
    [Test]
    public void TestAccelerationAndWait()
    {
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var initialWorldCoordinates = capsule.getWorldPosition();
        altUnityDriver.TiltAndWait(new AltUnityVector3(1, 1, 1), 1);
        Thread.Sleep(1000);
        capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var afterTiltCoordinates = capsule.getWorldPosition();
        Assert.AreNotEqual(initialWorldCoordinates, afterTiltCoordinates);
    }

    [Test]
    public void TestFindObjectByCamera()
    {
        var altButton = altUnityDriver.FindObject(By.PATH, "//Button");
        altButton.Click();
        altButton.Click();
        var altElement = altUnityDriver.FindObject(By.COMPONENT, "CapsuleCollider", By.NAME, "Camera");
        Assert.True(altElement.name.Equals("Capsule"));
        var altElement2 = altUnityDriver.FindObject(By.COMPONENT, "CapsuleCollider", By.NAME, "Main Camera");
        Assert.AreNotEqual(altElement.getScreenPosition(), altElement2.getScreenPosition());
    }

    [Test]
    public void TestWaitForObjectByCamera()
    {
        var altButton = altUnityDriver.FindObject(By.PATH, "//Button");
        altButton.Click();
        altButton.Click();
        var altElement = altUnityDriver.WaitForObject(By.COMPONENT, "CapsuleCollider", By.NAME, "Camera");
        Assert.True(altElement.name.Equals("Capsule"));
        var altElement2 = altUnityDriver.WaitForObject(By.COMPONENT, "CapsuleCollider", By.NAME, "Main Camera");
        Assert.AreNotEqual(altElement.getScreenPosition(), altElement2.getScreenPosition());
    }

    [Test]
    public void TestFindObjectsByCamera()
    {
        var altButton = altUnityDriver.FindObject(By.PATH, "//Button");
        altButton.Click();
        altButton.Click();
        var altElement = altUnityDriver.FindObjects(By.NAME, "Plane", By.NAME, "Camera");
        Assert.True(altElement[0].name.Equals("Plane"));
        var altElement2 = altUnityDriver.FindObjects(By.NAME, "Plane", By.NAME, "Main Camera");
        Assert.AreNotEqual(altElement[0].getScreenPosition(), altElement2[0].getScreenPosition());
    }

    [Test]
    public void TestWaitForObjectNotBePresentByCamera()
    {
        altUnityDriver.WaitForObjectNotBePresent(By.NAME, "ObjectDestroyedIn5Secs", By.NAME, "Main Camera");

        var allObjectsInTheScene = altUnityDriver.GetAllElements();
        Assert.IsTrue(!allObjectsInTheScene.Any(obj => obj.name.Equals("ObjectDestroyedIn5Secs")));
    }

    [Test]
    [Obsolete]
    public void TestWaitForElementWithTextByCamera()
    {
        const string name = "CapsuleInfo";
        string text = altUnityDriver.FindObject(By.NAME, name).GetText();
        var timeStart = DateTime.Now;
        var altElement = altUnityDriver.WaitForObjectWithText(By.NAME, name, text, By.NAME, "Main Camera");
        var timeEnd = DateTime.Now;
        var time = timeEnd - timeStart;
        Assert.Less(time.TotalSeconds, 20);
        Assert.NotNull(altElement);
        Assert.AreEqual(altElement.GetText(), text);
    }

    [Test]
    public void TestWaitForObjectWhichContainsByCamera()
    {
        var altElement = altUnityDriver.WaitForObjectWhichContains(By.NAME, "Canva", By.NAME, "Main Camera");
        Assert.AreEqual("Canvas", altElement.name);

    }
    [Test]
    public void TestLoadAdditiveScenes()
    {
        var initialNumberOfElements = altUnityDriver.GetAllElements();
        altUnityDriver.LoadScene("Scene 2 Draggable Panel", false);
        var finalNumberOfElements = altUnityDriver.GetAllElements();
        Assert.IsTrue(initialNumberOfElements.Count < finalNumberOfElements.Count);
        var scenes = altUnityDriver.GetAllLoadedScenes();
        Assert.IsTrue(scenes.Count == 2);
        altUnityDriver.LoadScene("Scene 2 Draggable Panel", true);
    }
    [Test]
    public void TestGetAltUnityObjectWithCanvasParentButOnlyTransform()
    {
        var altUnityObject = altUnityDriver.FindObject(By.NAME, "UIWithWorldSpace/Plane");
        Assert.NotNull(altUnityObject);
    }

    [Test]
    public void TestGetScreensizeScreenshot()
    {
        var screenWidth = altUnityDriver.CallStaticMethod<short>("UnityEngine.Screen", "get_width", new string[] { }, null, "UnityEngine.CoreModule");
        var screenHeight = altUnityDriver.CallStaticMethod<short>("UnityEngine.Screen", "get_height", new string[] { }, null, "UnityEngine.CoreModule");
        var screenshot = altUnityDriver.GetScreenshot();
        Assert.True(screenshot.textureSize.x == screenWidth);
        Assert.True(screenshot.textureSize.y == screenHeight);

        screenshot = altUnityDriver.GetScreenshot(screenShotQuality: 50);
        Assert.True(screenshot.textureSize.x == screenWidth / 2);
        Assert.True(screenshot.textureSize.y == screenHeight / 2);

        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        screenshot = altUnityDriver.GetScreenshot(capsule.id, new AltUnityColor(1, 0, 0), 1.5f);
        Assert.True(screenshot.textureSize.x == screenWidth);
        Assert.True(screenshot.textureSize.y == screenHeight);

        screenshot = altUnityDriver.GetScreenshot(capsule.id, new AltUnityColor(1, 0, 0), 1.5f, screenShotQuality: 50);
        Assert.True(screenshot.textureSize.x == screenWidth / 2);
        Assert.True(screenshot.textureSize.y == screenHeight / 2);

    }
    [Test]
    public void TestGetComponentPropertyComplexClass()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string propertyName = "AltUnitySampleClass.testInt";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        Assert.NotNull(altElement);
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual("1", propertyValue);
    }
    [Test]
    public void TestGetComponentPropertyComplexClass2()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string propertyName = "listOfSampleClass[1].testString";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        Assert.NotNull(altElement);
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual("test2", propertyValue);
    }

    [Test]
    public void TestSetComponentPropertyComplexClass()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string propertyName = "AltUnitySampleClass.testInt";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        Assert.NotNull(altElement);
        altElement.SetComponentProperty(componentName, propertyName, "2");
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual("2", propertyValue);
    }
    [Test]
    public void TestSetComponentPropertyComplexClass2()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string propertyName = "listOfSampleClass[1].testString";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        Assert.NotNull(altElement);
        altElement.SetComponentProperty(componentName, propertyName, "test3");
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual("test3", propertyValue);
    }
    [Test]
    public void TestClickWithMouse0Capsule()
    {
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var initialCapsulePosition = capsule.getWorldPosition();
        altUnityDriver.MoveMouse(capsule.getScreenPosition(), 0.1f);
        Thread.Sleep(400);
        altUnityDriver.PressKeyAndWait(AltUnityKeyCode.Mouse0, 1, 0.2f);
        capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var finalCapsulePosition = capsule.getWorldPosition();
        Assert.AreNotEqual(initialCapsulePosition, finalCapsulePosition);
    }
    [Test]
    public void TestClickWithMouse1Capsule()
    {
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var initialCapsulePosition = capsule.getWorldPosition();
        altUnityDriver.MoveMouse(capsule.getScreenPosition(), 0.1f);
        Thread.Sleep(400);
        altUnityDriver.PressKeyAndWait(AltUnityKeyCode.Mouse1, 1, 0.2f);

        capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var finalCapsulePosition = capsule.getWorldPosition();
        Assert.True(FastApproximately(initialCapsulePosition.x, finalCapsulePosition.x, 0.01f));
        Assert.True(FastApproximately(initialCapsulePosition.y, finalCapsulePosition.y, 0.01f));
        Assert.True(FastApproximately(initialCapsulePosition.z, finalCapsulePosition.z, 0.01f));
    }
    [Test]
    public void TestClickWithMouse2Capsule()
    {
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var initialCapsulePosition = capsule.getWorldPosition();
        altUnityDriver.MoveMouse(capsule.getScreenPosition(), 0.1f);
        Thread.Sleep(400);
        altUnityDriver.PressKeyAndWait(AltUnityKeyCode.Mouse2, 1, 0.2f);
        capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var finalCapsulePosition = capsule.getWorldPosition();
        Assert.True(FastApproximately(initialCapsulePosition.x, finalCapsulePosition.x, 0.01f));
        Assert.True(FastApproximately(initialCapsulePosition.y, finalCapsulePosition.y, 0.01f));
        Assert.True(FastApproximately(initialCapsulePosition.z, finalCapsulePosition.z, 0.01f));
    }

    [Test]
    public void TestGetVersion()
    {
        Assert.AreEqual(AltUnityDriver.VERSION, altUnityDriver.GetServerVersion());
    }

    [Test]
    public void TestStringIsMarkedAsPrimitive()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "ChineseLetters");
        var componentList = altElement.GetAllComponents();
        var component = componentList.First(componenta =>
            componenta.componentName.Equals("UnityEngine.UI.Text"));
        List<AltUnityProperty> properties = altElement.GetAllProperties(component, AltUnityPropertiesSelections.ALLPROPERTIES);

        AltUnityProperty property = properties.First(prop => prop.name.Equals("text"));
        Assert.NotNull(property);
        Assert.AreEqual(AltUnityType.PRIMITIVE, property.type);
    }
    [Test]
    public void TestKeyPressNumberOfReads()
    {
        var counterElement = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        altUnityDriver.PressKeyAndWait(AltUnityKeyCode.LeftArrow);

        var pressDownCounter = int.Parse(counterElement.GetComponentProperty("AltUnityExampleScriptIncrementOnClick", "keyPressDownCounter"));
        var pressUpCounter = int.Parse(counterElement.GetComponentProperty("AltUnityExampleScriptIncrementOnClick", "keyPressUpCounter"));
        Assert.AreEqual(1, pressDownCounter);
        Assert.AreEqual(1, pressUpCounter);
    }
    [Test]
    [Obsolete]
    public void TestParentId()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule", By.NAME, "Main Camera");
        Assert.AreEqual(altElement.transformParentId, altElement.parentId);
    }
    [Test]
    public void TestSwipeClickWhenMovedButRemainsOnTheSameObject()
    {
        var counterElement = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        var counterButtonText = altUnityDriver.FindObject(By.NAME, "ButtonCounter/Text");
        altUnityDriver.SwipeAndWait(new AltUnityVector2(counterElement.x + 1, counterElement.y + 1), new AltUnityVector2(counterElement.x + 2, counterElement.y + 1), 1);
        Thread.Sleep(500);
        Assert.AreEqual("1", counterButtonText.GetText());
    }
    [Test]
    [Obsolete]
    public void TestCallMethodInsideASubObject_Obsolete()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "AltUnitySampleClass.TestMethod";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, "");
        Assert.AreEqual("\"Test\"", data);
    }

    [Test]
    public void TestCallMethodInsideASubObject()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "AltUnitySampleClass.TestMethod";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod<string>(componentName, methodName, new string[] { });
        Assert.AreEqual("Test", data);
    }



    [Test]
    [Obsolete]
    public void TestCallMethodInsideAListOfSubObject_Obsolete()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "listOfSampleClass[0].TestMethod";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, "");
        Assert.AreEqual("\"Test\"", data);
    }
    [Test]
    public void TestCallMethodInsideAListOfSubObject()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "listOfSampleClass[0].TestMethod";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod<string>(componentName, methodName, new string[] { });
        Assert.AreEqual("Test", data);
    }
    [Test]
    [Obsolete]
    public void TestCallStaticMethodInsideASubObject_Obsolete()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "StaticSampleClass.TestMethod";
        var data = altUnityDriver.CallStaticMethod(componentName, methodName, "");
        Assert.AreEqual("\"Test\"", data);
    }
    [Test]
    public void TestCallStaticMethodInsideASubObject()
    {
        const string componentName = "AltUnityExampleScriptCapsule";
        const string methodName = "StaticSampleClass.TestMethod";
        var data = altUnityDriver.CallStaticMethod<string>(componentName, methodName, new string[] { });
        Assert.AreEqual("Test", data);
    }
    [Test]
    [Obsolete]
    public void TestCallGameObjectMethod_Obsolete()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod("UnityEngine.GameObject", "CompareTag", "Untagged", "System.String", "UnityEngine.CoreModule");
        Assert.AreEqual("true", data);
    }
    [Test]
    public void TestCallGameObjectMethod()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod<bool>("UnityEngine.GameObject", "CompareTag", new[] { "Untagged" }, new[] { "System.String" }, "UnityEngine.CoreModule");
        Assert.IsTrue(data);
    }
    [Test]
    [Obsolete]
    public void TestCallMethodInsideSubObjectOfGameObject_Obsolete()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod("UnityEngine.GameObject", "scene.IsValid", "", "", "UnityEngine.CoreModule");
        Assert.AreEqual("true", data);
    }
    [Test]
    public void TestCallMethodInsideSubObjectOfGameObject()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod<bool>("UnityEngine.GameObject", "scene.IsValid", new string[] { }, null, "UnityEngine.CoreModule");
        Assert.IsTrue(data);
    }

    [Test]
    public void TestFindObjectByAltId()
    {
        var capsule = altUnityDriver.FindObject(By.ID, "2b78431c-2251-4489-8d50-7634304a5630");
        Assert.AreEqual("Capsule", capsule.name);
        var plane = altUnityDriver.FindObject(By.PATH, "//*[@id=eff13b53-66de-4f98-82f3-a140b8949484]");
        Assert.AreEqual("Plane", plane.name);
        var mainCamera = altUnityDriver.FindObject(By.NAME, "Main Camera");
        mainCamera = altUnityDriver.FindObject(By.ID, mainCamera.id.ToString());
        Assert.AreEqual("Main Camera", mainCamera.name);
    }


    [TestCase("/Canvas[0]", "CapsuleInfo", true)]
    [TestCase("/Canvas[1]", "UIButton", true)]
    [TestCase("/Canvas[-1]", "TapClickEventsButtonCollider", true)]
    [TestCase("/Canvas[-2]", "NextScene", true)]
    [TestCase("/Canvas[@layer=UI][5]", "InputField", true)]
    [TestCase("/Canvas[1]/Text", "Text", true)]
    [TestCase("//CanvasPopUp[0]", "Icon", true)]
    [TestCase("//CanvasPopUp[0]", "PopUp", false)]
    [TestCase("//CanvasPopUp[-1]", "Icon", false)]
    [TestCase("//CanvasPopUp[-2]", "PopUp", false)]
    public void TestFindNthChild(string path, string expectedResult, bool enabled)
    {
        var altElement = altUnityDriver.FindObject(By.PATH, path, enabled: enabled);
        Assert.AreEqual(expectedResult, altElement.name);
    }
    [Test]
    public void TestUnloadScene()
    {
        altUnityDriver.LoadScene("Scene 2 Draggable Panel", false);
        Assert.AreEqual(2, altUnityDriver.GetAllLoadedScenes().Count);
        altUnityDriver.UnloadScene("Scene 2 Draggable Panel");
        Assert.AreEqual(1, altUnityDriver.GetAllLoadedScenes().Count);
        Assert.AreEqual("Scene 1 AltUnityDriverTestScene", altUnityDriver.GetAllLoadedScenes()[0]);
    }
    [Test]
    public void TestUnloadOnlyScene()
    {
        Assert.Throws<CouldNotPerformOperationException>(() => altUnityDriver.UnloadScene("Scene 1 AltUnityDriverTestScene"));
        Assert.Throws<CouldNotPerformOperationException>(() => altUnityDriver.UnloadScene("Scene 2 Draggable Panel"));
    }

    [Test]
    public void TestClickButtonInWolrdSpaceCanvas()
    {
        altUnityDriver.LoadScene("Scene6");
        var screenPosition = altUnityDriver.FindObject(By.NAME, "WorldSpaceButton").getScreenPosition();
        altUnityDriver.Tap(screenPosition, 1);
        var worldSpaceButton = altUnityDriver.FindObject(By.NAME, "WorldSpaceButton", enabled: false);
        Assert.IsFalse(worldSpaceButton.enabled);
    }
    [Test]
    public void TestDifferentObjectSelectedClickingOnScreenshot()
    {
        var uiButton = altUnityDriver.FindObject(By.NAME, "UIButton");
        AltUnityObject selectedObject;
        altUnityDriver.GetScreenshot(uiButton.getScreenPosition(), new AltUnityColor(1, 1, 1, 1), 1, out selectedObject);
        Assert.AreEqual("Text", selectedObject.name);
        altUnityDriver.GetScreenshot(uiButton.getScreenPosition(), new AltUnityColor(1, 1, 1, 1), 1, out selectedObject);
        Assert.AreEqual("UIButton", selectedObject.name);
    }
    [Test]
    public void TestFindObjectWithMultipleSelector()
    {
        var capsule = altUnityDriver.FindObject(By.PATH, "//*[@tag=Untagged][@layer=Water]");
        Assert.AreEqual("Capsule", capsule.name);
        var capsuleInfo = altUnityDriver.FindObject(By.PATH, "//*[contains(@name,Capsule)][@layer=UI]");
        Assert.AreEqual("CapsuleInfo", capsuleInfo.name);
        var rotateMainCamera = altUnityDriver.FindObject(By.PATH, "//*[@component=Button][@tag=Untagged][@layer=UI]");
        Assert.AreEqual("UIButton", rotateMainCamera.name);
    }
    [Test]
    public void TestGetAllScenesAndObjectDisableEnableOption()
    {
        var allEnableObjects = altUnityDriver.GetAllLoadedScenesAndObjects(true);
        foreach (var enabledObject in allEnableObjects)
        {
            Assert.IsTrue(enabledObject.enabled);
        }
        var allObjects = altUnityDriver.GetAllLoadedScenesAndObjects(false);
        Assert.IsTrue(allEnableObjects.Count < allObjects.Count);
        Assert.IsTrue(allObjects.Exists(AltUnityObject => AltUnityObject.name.Equals("Cube") && !AltUnityObject.enabled));
    }
    [Test]
    public void TestGetObjectWithNumberAsName()
    {
        var numberObject = altUnityDriver.FindObject(By.NAME, "1234", enabled: false);
        Assert.NotNull(numberObject);
        numberObject = altUnityDriver.FindObject(By.PATH, "//1234", enabled: false);
        Assert.NotNull(numberObject);
    }
    [Test]
    public void TestInvalidPaths()
    {
        Assert.Throws<InvalidPathException>(() => altUnityDriver.FindObject(By.PATH, "//[1]"));
        Assert.Throws<InvalidPathException>(() => altUnityDriver.FindObject(By.PATH, "CapsuleInfo[@tag=UI]"));
        Assert.Throws<InvalidPathException>(() => altUnityDriver.FindObject(By.PATH, "//CapsuleInfo[@tag=UI/Text"));
        Assert.Throws<InvalidPathException>(() => altUnityDriver.FindObject(By.PATH, "//CapsuleInfo[0/Text"));
    }

    [Test]
    [Obsolete]
    public void TestClickEvent()
    {
        var uiButton = altUnityDriver.FindObject(By.NAME, "UIButton");
        uiButton.ClickEvent();
        AltUnityObject capsuleInfo = altUnityDriver.WaitForObject(By.NAME, "CapsuleInfo");

        String text = capsuleInfo.GetText();
        Assert.AreEqual(text, "UIButton clicked to jump capsule!");
    }

    [Test]
    public void TestTap()
    {
        var counterButton = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        var counterButtonText = altUnityDriver.FindObject(By.NAME, "ButtonCounter/Text");
        counterButton.Tap(1);
        altUnityDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=1]");
    }

    [Test]
    public void TestTap_MultipleTaps()
    {
        var counterButton = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        var counterButtonText = altUnityDriver.FindObject(By.NAME, "ButtonCounter/Text");
        counterButton.Tap(2);
        altUnityDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=2]");
    }

    [Test]
    public void TestClick()
    {
        var counterButton = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        var counterButtonText = altUnityDriver.FindObject(By.NAME, "ButtonCounter/Text");
        counterButton.Click(1);
        altUnityDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=1]");
    }

    [Test]
    public void TestClick_MultipleClicks()
    {
        var counterButton = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        var counterButtonText = altUnityDriver.FindObject(By.NAME, "ButtonCounter/Text");
        counterButton.Click(2);
        altUnityDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=2]");
    }


    [Test]
    public void TestClick_MouseDownUp()
    {
        var counterElement = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        counterElement.Click();

        var mouseDownCounter = int.Parse(counterElement.GetComponentProperty("AltUnityExampleScriptIncrementOnClick", "mouseDownCounter"));
        var mouseUpCounter = int.Parse(counterElement.GetComponentProperty("AltUnityExampleScriptIncrementOnClick", "mouseUpCounter"));
        var mousePressedCounter = int.Parse(counterElement.GetComponentProperty("AltUnityExampleScriptIncrementOnClick", "mousePressedCounter"));

        string eventsRaised = counterElement.GetComponentProperty("AltUnityExampleScriptIncrementOnClick", "eventsRaised");
        var eventsRaisedList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(eventsRaised);
        Assert.AreEqual(1, mouseDownCounter);
        Assert.AreEqual(1, mouseUpCounter);
        Assert.AreEqual(1, mousePressedCounter);

        Assert.IsTrue(eventsRaisedList.Contains("OnMouseEnter"));
        Assert.IsTrue(eventsRaisedList.Contains("OnMouseDown"));
        Assert.IsTrue(eventsRaisedList.Contains("OnMouseUp"));
        Assert.IsTrue(eventsRaisedList.Contains("OnMouseUpAsButton"));
        Assert.IsTrue(eventsRaisedList.Contains("OnMouseExit"));
    }

    [Test]
    public void TestMouseOverElement()
    {
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        capsule.Click();
        var mouseOverCounter = int.Parse(capsule.GetComponentProperty("AltUnityExampleScriptCapsule", "mouseOverCounter"));
        Assert.AreEqual(2, mouseOverCounter);
    }
    [Test]
    public void TestMouseOverCoordinates()
    {
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        altUnityDriver.Click(capsule.getScreenPosition());
        var mouseOverCounter = int.Parse(capsule.GetComponentProperty("AltUnityExampleScriptCapsule", "mouseOverCounter"));
        Assert.AreEqual(2, mouseOverCounter);
    }

    [Test]
    public void TestClickCoordinates()
    {
        var counterButton = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        var counterButtonText = altUnityDriver.FindObject(By.NAME, "ButtonCounter/Text");
        altUnityDriver.Click(new AltUnityVector2(80 + counterButton.x, 15 + counterButton.y));
        altUnityDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=1]");
    }

    [Test]
    public void TestClickCoordinates_MultipleClicks()
    {
        var counterButton = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        var counterButtonText = altUnityDriver.FindObject(By.NAME, "ButtonCounter/Text");
        altUnityDriver.Click(new AltUnityVector2(80 + counterButton.x, 15 + counterButton.y), 2);
        altUnityDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=2]");
    }


    [Test]
    public void TestClickCoordinates_MouseDownUp()
    {
        var button2dCollider = altUnityDriver.FindObject(By.NAME, "TapClickEventsButtonCollider");

        altUnityDriver.Click(button2dCollider.getScreenPosition());

        string monoBehaviourEventsRaisedString = button2dCollider.GetComponentProperty("AltUnityTapClickEventsScript", "monoBehaviourEventsRaised");
        var monoBehaviourEventsRaised = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(monoBehaviourEventsRaisedString);
        // Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseEnter")); does not work on mobile
        Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseDown"));
        Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseUp"));
        Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseUpAsButton"));
        // Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseExit")); does not work on mobile

        string eventSystemEventsRaisedString = button2dCollider.GetComponentProperty("AltUnityTapClickEventsScript", "eventSystemEventsRaised");
        var eventSystemEventsRaised = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(eventSystemEventsRaisedString);
        // Assert.IsTrue(eventSystemEventsRaised.Contains("OnPointerEnter")); does not work on mobile
        Assert.IsTrue(eventSystemEventsRaised.Contains("OnPointerDown"));
        Assert.IsTrue(eventSystemEventsRaised.Contains("OnPointerUp"));
        Assert.IsTrue(eventSystemEventsRaised.Contains("OnPointerClick"));
        // Assert.IsTrue(eventSystemEventsRaised.Contains("OnPointerExit")); does not work on mobile
    }

    [Test]
    public void TestClickCoordinates_ColliderNoParent()
    {
        var sphere = altUnityDriver.FindObject(By.PATH, "/Sphere");
        altUnityDriver.Click(sphere.getScreenPosition());

        string monoBehaviourEventsRaisedString = sphere.GetComponentProperty("AltUnitySphereColliderScript", "monoBehaviourEventsRaised");
        var monoBehaviourEventsRaised = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(monoBehaviourEventsRaisedString);
        Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseEnter"));
        Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseDown"));
        Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseUp"));
        Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseUpAsButton"));
        Assert.IsTrue(monoBehaviourEventsRaised.Contains("OnMouseExit"));
    }

    [Test]
    public void TestClickCoordinates_ColliderParentCollider()
    {
        var sphere = altUnityDriver.FindObject(By.PATH, "/Sphere");
        var plane = altUnityDriver.FindObject(By.PATH, "/Sphere/PlaneS");
        altUnityDriver.Click(plane.getScreenPosition());


        string eventsstring = plane.GetComponentProperty("AltUnityPlaneColliderScript", "monoBehaviourEventsRaised");
        var planeEvents = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(eventsstring);
        Assert.IsTrue(planeEvents.Contains("OnMouseEnter"));
        Assert.IsTrue(planeEvents.Contains("OnMouseDown"));
        Assert.IsTrue(planeEvents.Contains("OnMouseUp"));
        Assert.IsTrue(planeEvents.Contains("OnMouseUpAsButton"));
        Assert.IsTrue(planeEvents.Contains("OnMouseExit"));

        eventsstring = sphere.GetComponentProperty("AltUnitySphereColliderScript", "monoBehaviourEventsRaised");
        var sphereevents = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(eventsstring);
        Assert.IsFalse(sphereevents.Contains("OnMouseEnter"));
        Assert.IsFalse(sphereevents.Contains("OnMouseDown"));
        Assert.IsFalse(sphereevents.Contains("OnMouseUp"));
        Assert.IsFalse(sphereevents.Contains("OnMouseUpAsButton"));
        Assert.IsFalse(sphereevents.Contains("OnMouseExit"));
    }
    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
    [Test]
    public void TestClickCoordinatesCheckPressRaycastObject()
    {
        var incrementClick = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        altUnityDriver.Click(incrementClick.getScreenPosition());
        Assert.AreEqual("Text", incrementClick.GetComponentProperty("AltUnityExampleScriptIncrementOnClick", "eventDataPressRaycastObject", "Assembly-CSharp"));
    }

    [Test]
    public void TestPointerPressSet()
    {
        var incrementalClick = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
        var swipeCoordinate = new AltUnityVector2(incrementalClick.x + 10, incrementalClick.y + 10);
        altUnityDriver.SwipeAndWait(swipeCoordinate, swipeCoordinate, 0.2f);
        Assert.AreEqual("(10.0, 10.0)", incrementalClick.GetComponentProperty("AltUnityExampleScriptIncrementOnClick", "pointerPress", "Assembly-CSharp"));
    }

    [Test]
    public void TestKeyDownAndKeyUpMouse0()
    {
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var initialCapsulePosition = capsule.getWorldPosition();
        altUnityDriver.MoveMouse(capsule.getScreenPosition(), 0.1f);
        Thread.Sleep(400);
        altUnityDriver.KeyDown(AltUnityKeyCode.Mouse0);
        altUnityDriver.KeyUp(AltUnityKeyCode.Mouse0);
        capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var finalCapsulePosition = capsule.getWorldPosition();
        Assert.AreNotEqual(initialCapsulePosition, finalCapsulePosition);
    }
    [Test]
    public void TestTouchAreUpdatedWhenMoved()
    {
        var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var id = altUnityDriver.BeginTouch(capsule.getScreenPosition());
        var phase = capsule.GetComponentProperty("AltUnityExampleScriptCapsule", "TouchPhase");
        Assert.AreEqual("0", phase);
        altUnityDriver.MoveTouch(id, capsule.getScreenPosition());
        phase = capsule.GetComponentProperty("AltUnityExampleScriptCapsule", "TouchPhase");
        Assert.AreEqual("1", phase);
        altUnityDriver.EndTouch(id);


    }

}
