using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using Assets.AltUnityTester.AltUnityDriver;
using NUnit.Framework.Constraints;
using NullReferenceException = Assets.AltUnityTester.AltUnityDriver.NullReferenceException;
using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

[Timeout(5000)]
#pragma warning disable CS0618
public class TestForScene1WithOldSearch
{
    private AltUnityDriver altUnityDriver;
    [OneTimeSetUp]
    public void SetUp()
    {
        altUnityDriver = new AltUnityDriver();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        altUnityDriver.Stop();
    }

    [SetUp]
    public void LoadLevel()
    {
        altUnityDriver.LoadScene("Scene 1 AltUnityDriverTestScene");
    }
    

    [Test]
    public void TestFindElement()
    {

        const string name = "Capsule";
        var altElement = altUnityDriver.FindElement(name);
        Assert.NotNull(altElement);
        Assert.AreEqual(name, altElement.name);

    }


    [Test]
    public void TestFindElements()
    {

        const string name = "Plane";
        var altElements = altUnityDriver.FindElements(name);
        Assert.IsNotEmpty(altElements);
        Assert.AreEqual(altElements[0].name, name);

    }

    [Test]
    public void TestFindElementWhereNameContains()
    {

        const string name = "Cap";
        var altElement = altUnityDriver.FindElementWhereNameContains(name);
        Assert.NotNull(altElement);
        Assert.True(altElement.name.Contains(name));

    }
    [Test]
    public void TestFindElementsWhereNameContains()
    {
        const string name = "Pla";
        var altElements = altUnityDriver.FindElementsWhereNameContains(name);
        Assert.IsNotEmpty(altElements);
        Assert.True(altElements[0].name.Contains(name));
    }
    

    [Test]
    public void TestWaitForExistingElement()
    {
        const string name = "Capsule";
        var timeStart = DateTime.Now;
        var altElement = altUnityDriver.WaitForElement(name);
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
        var altElement = altUnityDriver.WaitForElement(name, enabled: false);
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
        var altElement = altUnityDriver.WaitForElementWhereNameContains(name);
        var timeEnd = DateTime.Now;
        var time = timeEnd - timeStart;
        Assert.Less(time.TotalSeconds, 20);
        Assert.NotNull(altElement);
        Assert.AreEqual(altElement.name, "Directional Light");
    }


    [Test]
    public void TestWaitForElementWithText()
    {
        const string name = "CapsuleInfo";
        string text = altUnityDriver.FindElement(name).GetText();
        var timeStart = DateTime.Now;
        var altElement = altUnityDriver.WaitForElementWithText(name, text);
        var timeEnd = DateTime.Now;
        var time = timeEnd - timeStart;
        Assert.Less(time.TotalSeconds, 20);
        Assert.NotNull(altElement);
        Assert.AreEqual(altElement.GetText(), text);

    }

    [Test]
    public void TestFindElementByComponent()
    {
        const string componentName = "AltUnityRunner";
        var altElement = altUnityDriver.FindElementByComponent(componentName);
        Assert.NotNull(altElement);
        Assert.AreEqual(altElement.name, "AltUnityRunnerPrefab");
    }

    [Test]
    public void TestFindElementByComponent2()
    {
        Assert.AreEqual("Capsule", altUnityDriver.FindElementByComponent("Capsule").name);
    }

    [Test]
    public void TestFindElemetsByComponent()
    {
        var a = altUnityDriver.FindElementsByComponent("UnityEngine.MeshFilter");
        Assert.AreEqual(3, a.Count);
    }

    [Test]
    public void TestGetComponentProperty()
    {
        Thread.Sleep(1000);

        const string componentName = "AltUnityRunner";
        const string propertyName = "SocketPortNumber";
        var altElement = altUnityDriver.FindElement("AltUnityRunnerPrefab");
        Assert.NotNull(altElement);
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual(propertyValue, "13000");
    }

    [Test]
    public void TestGetNonExistingComponentProperty()
    {
        const string componentName = "AltUnityRunner";
        const string propertyName = "socketPort";
        var altElement = altUnityDriver.FindElement("AltUnityRunnerPrefab");
        Assert.NotNull(altElement);
        try
        {
            altElement.GetComponentProperty(componentName, propertyName);
            Assert.Fail();
        }
        catch (PropertyNotFoundException exception)
        {
            Assert.AreEqual(exception.Message, "error:propertyNotFound");
        }

    }
    [Test]
    public void TestGetComponentPropertyArray()
    {
        const string componentName = "Capsule";
        const string propertyName = "arrayOfInts";
        var altElement = altUnityDriver.FindElement("Capsule");
        Assert.NotNull(altElement);
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual("[1,2,3]", propertyValue);
    }

    //#if !UNITY_IOS
    [Test]
    public void TestGetComponentPropertyUnityEngine()
    {
        const string componentName = "UnityEngine.CapsuleCollider";
        const string propertyName = "isTrigger";
        var altElement = altUnityDriver.FindElement("Capsule");
        Assert.NotNull(altElement);
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual("false", propertyValue);
    }
    //#endif 

    [Test]
    public void TestSetComponentProperty()
    {
        const string componentName = "Capsule";
        const string propertyName = "stringToSetFromTests";
        var altElement = altUnityDriver.FindElement("Capsule");
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
        var altElement = altUnityDriver.FindElement("Capsule");
        Assert.NotNull(altElement);
        try
        {
            altElement.SetComponentProperty(componentName, propertyName, "2");
            Assert.Fail();
        }
        catch (ComponentNotFoundException exception)
        {
            Assert.AreEqual(exception.Message, "error:componentNotFound");
        }
    }

    [Test]
    public void TestCallMethodWithNoParameters()
    {
        const string componentName = "Capsule";
        const string methodName = "UIButtonClicked";
        var altElement = altUnityDriver.FindElement("Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, "");
        Assert.AreEqual("null", data);
    }

    [Test]
    public void TestCallMethodWithParameters()
    {
        const string componentName = "Capsule";
        const string methodName = "Jump";
        const string parameters = "New Text";
        var altElement = altUnityDriver.FindElement("Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, parameters);
        Assert.AreEqual("null", data);
    }

    [Test]
    public void TestCallMethodWithManyParameters()
    {
        const string componentName = "Capsule";
        const string methodName = "TestMethodWithManyParameters";
        const string parameters = "1?stringparam?0.5?[1,2,3]";
        var altElement = altUnityDriver.FindElement("Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, parameters);
        Assert.AreEqual("null", data);
    }

    [Test]
    public void TestCallMethodWithIncorrectNumberOfParameters()
    {
        const string componentName = "Capsule";
        const string methodName = "TestMethodWithManyParameters";
        const string parameters = "1?stringparam?[1,2,3]";
        var altElement = altUnityDriver.FindElement("Capsule");
        try
        {
            altElement.CallComponentMethod(componentName, methodName, parameters);
            Assert.Fail();
        }
        catch (IncorrectNumberOfParametersException exception)
        {
            Assert.AreEqual(exception.Message, "error:incorrectNumberOfParameters");
        }

    }

    [Test]
    public void TestCallMethodWithIncorrectTypeOfParameters()
    {
        const string componentName = "Capsule";
        const string methodName = "TestMethodWithManyParameters";
        const string parameters = "a?stringparam?[1,2,3]";
        var altElement = altUnityDriver.FindElement("Capsule");
        try
        {
            altElement.CallComponentMethod(componentName, methodName, parameters);
            Assert.Fail();
        }
        catch (IncorrectNumberOfParametersException exception)
        {
            Assert.AreEqual("error:incorrectNumberOfParameters", exception.Message);
        }
    }

    
    [Test]
    public void TestDifferentCamera()
    {
        var altButton = altUnityDriver.FindElement("Button", "Main Camera");
        altButton.ClickEvent();
        altButton.ClickEvent();
        var altElement = altUnityDriver.FindElement("Capsule", "Main Camera");
        var altElement2 = altUnityDriver.FindElement("Capsule", "Camera");
        AltUnityVector2 pozOnScreenFromMainCamera = new AltUnityVector2(altElement.x, altElement.y);
        AltUnityVector2 pozOnScreenFromSecondaryCamera = new AltUnityVector2(altElement2.x, altElement2.y);

        Assert.AreNotEqual(pozOnScreenFromSecondaryCamera, pozOnScreenFromMainCamera);

    }

    [Test]
    public void TestFindNonExistentObject()
    {
        try
        {
            altUnityDriver.FindElement("NonExistent");
            Assert.Fail();
        }
        catch (NotFoundException exception)
        {
            Assert.AreEqual(exception.Message, "error:notFound");
        }

    }

    [Test]
    public void TestFindNonExistentObjectByName()
    {
        try
        {
            altUnityDriver.FindElementWhereNameContains("NonExistent");
            Assert.Fail();
        }
        catch (NotFoundException exception)
        {
            Assert.AreEqual(exception.Message, "error:notFound");
        }

    }

    [Test]
    public void TestButtonClickWithSwipe()
    {
        var button = altUnityDriver.FindElement("UIButton");
        AltUnityVector2 vector2 = new AltUnityVector2(button.x, button.y);
        altUnityDriver.HoldButtonAndWait(vector2, 1);
        var capsuleInfo = altUnityDriver.FindElement("CapsuleInfo");
        Thread.Sleep(1400);
        var text = capsuleInfo.GetText();
        Assert.AreEqual(text, "UIButton clicked to jump capsule!");
    }

    [Test]
    public void TestClickElement()
    {
        const string name = "Capsule";
        var altElement = altUnityDriver.FindElement(name).Tap();
        Assert.AreEqual(name, altElement.name);
        altUnityDriver.WaitForElementWithText("CapsuleInfo", "Capsule was clicked to jump!");
    }

    [Test]
    public void TestClickScreen()
    {
        const string name = "UIButton";
        var altElement2 = altUnityDriver.FindElement(name);
        var altElement = altUnityDriver.TapScreen(altElement2.x, altElement2.y);
        Assert.AreEqual(name, altElement.name);
        altUnityDriver.WaitForElementWithText("CapsuleInfo", "UIButton clicked to jump capsule!");
    }

    [Test]
    public void TestWaitForNonExistingObject()
    {
        try
        {
            altUnityDriver.WaitForElement("dlkasldkas", timeout: 1, interval: 1);
            Assert.Fail();
        }
        catch (WaitTimeOutException exception)
        {
            Assert.AreEqual(exception.Message, "Element dlkasldkas not loaded after 1 seconds");
        }
    }
    [Test]
    public void TestWaitForObjectToNotExistFail()
    {
        try
        {
            altUnityDriver.WaitForElementToNotBePresent("Capsule", timeout: 1, interval: 0.5f);
            Assert.Fail();
        }
        catch (WaitTimeOutException exception)
        {
            Assert.AreEqual("Element Capsule still found after 1 seconds", exception.Message);
        }
    }

    [Test]
    public void TestWaitForObjectWithTextWrongText()
    {
        try
        {
            altUnityDriver.WaitForElementWithText("CapsuleInfo", "aaaaa", timeout: 1);
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
            altUnityDriver.WaitForElementWhereNameContains(name, timeout: 1);
            Assert.Fail();
        }
        catch (WaitTimeOutException exception)
        {
            Assert.AreEqual("Element xyz still not found after 1 seconds", exception.Message);
        }

    }

    [Test]
    public void TestCallMethodWithMultipleDefinitions()
    {

        AltUnityObject capsule = altUnityDriver.FindElement("Capsule");
        capsule.CallComponentMethod("Capsule", "Test", "2", "System.Int32");
        AltUnityObject capsuleInfo = altUnityDriver.FindElement("CapsuleInfo");
        Assert.AreEqual("6", capsuleInfo.GetText());
    }

    [Test]
    public void TestGetAllComponents()
    {
        List<AltUnityComponent> components = altUnityDriver.FindElement("Canvas").GetAllComponents();
        Assert.AreEqual(4, components.Count);
        Assert.AreEqual("UnityEngine.RectTransform", components[0].componentName);
        Assert.AreEqual("UnityEngine.CoreModule", components[0].assemblyName);
    }

    [Test]
    public void TestGetAllMethods()
    {
        var altElement = altUnityDriver.FindElement("Capsule");
        List<String> methods = altElement.GetAllMethods(altElement.GetAllComponents().First(component => component.componentName.Equals("Capsule")));
        Assert.IsTrue(methods.Contains("Void UIButtonClicked()"));
    }

    [Test]
    public void TestGetAllFields()
    {
        var altElement = altUnityDriver.FindElement("Capsule");
        var componentList = altElement.GetAllComponents();
        var component = componentList.First(componenta =>
            componenta.componentName.Equals("Capsule") && componenta.assemblyName.Equals("Assembly-CSharp"));
        List<AltUnityProperty> properties = altElement.GetAllProperties(component);
        AltUnityProperty field = properties.First(prop => prop.name.Equals("stringToSetFromTests"));
        Assert.NotNull(field);
        Assert.AreEqual(field.value, "intialValue");
    }



    [Test]
    public void TestInactiveObject()
    {
        AltUnityObject cube = altUnityDriver.FindElement("Cube", enabled: false);
        Assert.AreEqual(false, cube.enabled);

    }
#pragma warning restore CS0618


}