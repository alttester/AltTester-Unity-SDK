using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using Assets.AltUnityTester.AltUnityDriver;
using NUnit.Framework.Constraints;
using System.Diagnostics;
using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

[Timeout(5000)]
public class TestForScene1TestSample
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
    public void TestFindElementByComponent()
    {
        const string componentName = "AltUnityRunner";
        var altElement = altUnityDriver.FindObject(By.COMPONENT, componentName);
        Assert.NotNull(altElement);
        Assert.AreEqual(altElement.name, "AltUnityRunnerPrefab");
    }

    [Test]
    public void TestFindElementByComponent2()
    {
        Assert.AreEqual("Capsule", altUnityDriver.FindObject(By.COMPONENT, "Capsule").name);
    }

    [Test]
    public void TestFindElemetsByComponent()
    {
        var a = altUnityDriver.FindObjects(By.COMPONENT, "MeshFilter");
        Assert.AreEqual(3, a.Count);
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
    public void TestGetNonExistingComponentProperty()
    {
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
            Assert.AreEqual(exception.Message, "error:propertyNotFound");
        }

    }
    [Test]
    public void TestGetComponentPropertyArray()
    {
        const string componentName = "Capsule";
        const string propertyName = "arrayOfInts";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
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
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
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
        catch (Assets.AltUnityTester.AltUnityDriver.ComponentNotFoundException exception)
        {
            Assert.AreEqual(exception.Message, "error:componentNotFound");
        }
    }

    [Test]
    public void TestCallMethodWithNoParameters()
    {
        const string componentName = "Capsule";
        const string methodName = "UIButtonClicked";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, "");
        Assert.AreEqual("null", data);
    }

    [Test]
    public void TestCallMethodWithParameters()
    {
        const string componentName = "Capsule";
        const string methodName = "Jump";
        const string parameters = "New Text";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, parameters);
        Assert.AreEqual("null", data);
    }

    [Test]
    public void TestCallMethodWithManyParameters()
    {
        const string componentName = "Capsule";
        const string methodName = "TestMethodWithManyParameters";
        const string parameters = "1?stringparam?0.5?[1,2,3]";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, parameters);
        Assert.AreEqual("null", data);
    }

    [Test]
    public void TestCallMethodWithIncorrectNumberOfParameters()
    {
        const string componentName = "Capsule";
        const string methodName = "TestMethodWithManyParameters";
        const string parameters = "1?stringparam?[1,2,3]";
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
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
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
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
            Assert.AreEqual("error:notFound", exception.Message);
        }

    }





    [Test]
    public void TestDifferentCamera()
    {
        var altButton = altUnityDriver.FindObject(By.NAME, "Button", "Main Camera");
        altButton.ClickEvent();
        altButton.ClickEvent();
        var altElement = altUnityDriver.FindObject(By.NAME,"Capsule", "Main Camera");
        var altElement2 = altUnityDriver.FindObject(By.NAME,"Capsule", "Camera");
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
            Assert.AreEqual(exception.Message, "error:notFound");
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
            Assert.AreEqual(exception.Message, "error:notFound");
        }

    }

    [Test]
    public void TestButtonClickWithSwipe()
    {
        var button = altUnityDriver.FindObject(By.NAME,"UIButton");
        AltUnityVector2 vector2 = new AltUnityVector2(button.x, button.y);
        altUnityDriver.HoldButtonAndWait(vector2, 1);
        var capsuleInfo = altUnityDriver.FindObject(By.NAME, "CapsuleInfo");
        Thread.Sleep(1400);
        var text = capsuleInfo.GetText();
        Assert.AreEqual(text, "UIButton clicked to jump capsule!");
    }

    [Test]
    public void TestClickElement()
    {
        const string name = "Capsule";
        var altElement = altUnityDriver.FindObject(By.NAME, name).Tap();
        Assert.AreEqual(name, altElement.name);
        altUnityDriver.WaitForObjectWithText(By.NAME, "CapsuleInfo", "Capsule was clicked to jump!");
    }

    [Test]
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
            Assert.AreEqual("Element dlkasldkas not loaded after 1 seconds", exception.Message);
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
    public void TestCallStaticMethod()
    {

        altUnityDriver.CallStaticMethods("UnityEngine.PlayerPrefs", "SetInt", "Test?1");
        int a = Int32.Parse(altUnityDriver.CallStaticMethods("UnityEngine.PlayerPrefs", "GetInt", "Test?2"));
        Assert.AreEqual(1, a);

    }
    [Test]
    public void TestCallMethodWithMultipleDefinitions()
    {

        AltUnityObject capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        capsule.CallComponentMethod("Capsule", "Test", "2", "System.Int32");
        AltUnityObject capsuleInfo = altUnityDriver.FindObject(By.NAME, "CapsuleInfo");
        Assert.AreEqual("6", capsuleInfo.GetText());
    }
    [Test]
    public void TestCallMethodWithAssembly()
    {
        AltUnityObject capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
        var initialRotation = capsule.GetComponentProperty("UnityEngine.Transform", "rotation");
        capsule.CallComponentMethod("UnityEngine.Transform", "Rotate", "10?10?10", "System.Single?System.Single?System.Single", "UnityEngine.CoreModule");
        AltUnityObject capsuleAfterRotation = altUnityDriver.FindObject(By.NAME, "Capsule");
        var finalRotation = capsuleAfterRotation.GetComponentProperty("UnityEngine.Transform", "rotation");
        Assert.AreNotEqual(initialRotation, finalRotation);
    }

    [Test]
    public void TestGetAllComponents()
    {
        List<AltUnityComponent> components = altUnityDriver.FindObject(By.NAME, "Canvas").GetAllComponents();
        Assert.AreEqual(4, components.Count);
        Assert.AreEqual("UnityEngine.RectTransform", components[0].componentName);
        Assert.AreEqual("UnityEngine.CoreModule", components[0].assemblyName);
    }

    [Test]
    public void TestGetAllMethods()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        List<String> methods = altElement.GetAllMethods(altElement.GetAllComponents().First(component => component.componentName.Equals("Capsule")));
        Assert.IsTrue(methods.Contains("Void UIButtonClicked()"));
    }

    [Test]
    public void TestGetAllFields()
    {
        var altElement = altUnityDriver.FindObject(By.NAME, "Capsule");
        var componentList = altElement.GetAllComponents();
        var component = componentList.First(componenta =>
            componenta.componentName.Equals("Capsule") && componenta.assemblyName.Equals("Assembly-CSharp"));
        List<AltUnityProperty> properties = altElement.GetAllProperties(component);
        AltUnityProperty field = properties.First(prop => prop.name.Equals("stringToSetFromTests"));
        Assert.NotNull(field);
        Assert.AreEqual(field.value, "intialValue");
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
    public void TestFindObjectByComponent()
    {
        var altElement = altUnityDriver.FindObject(By.COMPONENT, "Capsule");
        Assert.True(altElement.name.Equals("Capsule"));
    }

    [Test]
    public void TestFindChild()
    {
        var altElement = altUnityDriver.FindObject(By.PATH, "//UIButton/*");
        Assert.True(altElement.name.Equals("Text"));
    }
    [Test]
    public void TestFindingDifferentObjects()
    {
        var altElement = altUnityDriver.FindObject(By.PATH, "//*[contains(@name,Cub)]", enabled: false);
        Assert.True(altElement.name.Equals("Cube"));

        altElement = altUnityDriver.FindObject(By.PATH, "//RotateMainCameraButton/../*[contains(@name,Seconda)]/Text");
        Assert.True(altElement.name.Equals("Text"));

        altElement = altUnityDriver.FindObject(By.PATH, "//*[@component=BoxCollider]", enabled: false);
        Assert.True(altElement.name.Equals("Cube"));


    }
    [Test]
    public void TestFindObjectWithDifferentCameras()
    {
        var changeCameraButton = altUnityDriver.FindObject(By.NAME, "Button");
        changeCameraButton.Tap().Tap();
        var altElement1 = altUnityDriver.FindObject(By.NAME, "Capsule", cameraName: "Main Camera", enabled: false);
        var altElement2 = altUnityDriver.FindObject(By.NAME, "Capsule", cameraName: "Camera", enabled: false);
        Assert.AreNotEqual(altElement1.y, altElement2.y);
        Assert.AreNotEqual(altElement1.x, altElement2.x);
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
        var altElements = altUnityDriver.FindObjects(By.PATH, "//*[contains(@name,Ca)]");
        Assert.AreEqual(9, altElements.Count);
        foreach (var altElement in altElements)
        {
            Assert.True(altElement.name.Contains("Ca"));
        }
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
    public void TestTapScreenWhereThereIsNoObjects()
    {
        AltUnityObject altObject = altUnityDriver.TapScreen(1, 1);
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
        var altElement = altUnityDriver.FindObjectWhichContains(By.NAME, "Event");
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
            Assert.AreEqual(exception.Message, "error:notFound");
        }
    }
    [Test]
    public void TestGetAllCameras()
    {
        var cameras = altUnityDriver.GetAllCameras();
        Assert.AreEqual(2, cameras.Count);
    }

    [Test]
    public void TestFindObjectScene1()
    {
        var altElements = altUnityDriver.FindObjects(By.PATH, "//Canvas/*/Text");
        Assert.AreEqual(5, altElements.Count);
    }

    [Test]
    public void TestFindObjectScene6()
    {
        altUnityDriver.LoadScene("Scene6");

        Thread.Sleep(1000);
        altUnityDriver.WaitForCurrentSceneToBe("Scene6");
        var altElements = altUnityDriver.FindObjects(By.PATH, "//Canvas/*/Text");
        Assert.AreEqual(2, altElements.Count);
        altElements = altUnityDriver.FindObjects(By.PATH, "/*/*/Text");
        Assert.AreEqual(2, altElements.Count);
        altElements = altUnityDriver.FindObjects(By.PATH, "/*/Text");
        Assert.AreEqual(1, altElements.Count);
        altElements = altUnityDriver.FindObjects(By.PATH, "//Text");
        Assert.AreEqual(4, altElements.Count);
        altElements = altUnityDriver.FindObjects(By.PATH, "//Canvas/*//Text");
        Assert.AreEqual(3, altElements.Count);
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



}
