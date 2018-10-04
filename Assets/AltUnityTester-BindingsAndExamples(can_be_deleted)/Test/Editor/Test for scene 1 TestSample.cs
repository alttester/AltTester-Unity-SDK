using System;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using Assets.AltUnityTester.AltUnityDriver;
using NUnit.Framework.Constraints;
using UnityEngine;
[Timeout(5000)]
public class TestForScene1TestSample
{
    private AltUnityDriver altUnityDriver;
    [OneTimeSetUp]
    public void SetUp()
    {
        altUnityDriver=new AltUnityDriver();
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
        var altElement = altUnityDriver.FindElement(name);
        Assert.NotNull(altElement);
        Assert.AreEqual(name,altElement.name);

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
        Assert.True(altElement.name.Contains( name));

    }
    [Test]
    public void TestFindElementsWhereNameContains()
    {
        const string name = "Pla";
        var altElements = altUnityDriver.FindElementsWhereNameContains(name);
        Assert.IsNotEmpty(altElements);
        Assert.True(altElements[0].name.Contains( name));
    }
    [Test]
    public void TestGetAllElements()
    {
       
        var altElements = altUnityDriver.GetAllElements();
        Assert.IsNotEmpty(altElements);
        Assert.IsNotNull(altElements.Where(p=> p.name == "Capsule"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Main Camera"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Directional Light"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Plane"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Canvas"));
        Assert.IsNotNull(altElements.Where(p => p.name == "EventSystem"));
        Assert.IsNotNull(altElements.Where(p => p.name == "AltUnityRunner"));
        Assert.IsNotNull(altElements.Where(p => p.name == "CapsuleInfo"));
        Assert.IsNotNull(altElements.Where(p => p.name == "UIButton"));
        Assert.IsNotNull(altElements.Where(p => p.name == "Text"));
    }

    [Test]
    public void TestWaitForExistingElement()
    {
        const string name = "Capsule";
        var timeStart = DateTime.Now;
        var altElement = altUnityDriver.WaitForElement(name);
        var timeEnd = DateTime.Now;
        var time = timeEnd - timeStart;
        Assert.Less(time.TotalSeconds,20);
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
        var altElement = altUnityDriver.WaitForElementWithText(name,text);
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
        Assert.AreEqual("Capsule",altUnityDriver.FindElementByComponent("Capsule").name);
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
        const string componentName = "AltUnityRunner";
        const string propertyName = "SocketPortNumber";
        var altElement = altUnityDriver.FindElement("AltUnityRunnerPrefab");
        Assert.NotNull(altElement);
        var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual(propertyValue,"13000");
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
            var altElemen2 = altElement.GetComponentProperty(componentName, propertyName);
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
        var propertyValue= altElement.SetComponentProperty(componentName, propertyName, "2");
        Assert.AreEqual("valueSet",propertyValue);
        propertyValue = altElement.GetComponentProperty(componentName, propertyName);
        Assert.AreEqual("2",propertyValue);
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
            var altElemen2 = altElement.SetComponentProperty(componentName, propertyName, "2");
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
        const string methodName="UIButtonClicked";
        var altElement = altUnityDriver.FindElement("Capsule");
        var data = altElement.CallComponentMethod(componentName, methodName, "");
        Assert.AreEqual("null",data);
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
            var altElemen2 = altElement.CallComponentMethod(componentName, methodName, parameters);
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
            var altElemen2 = altElement.CallComponentMethod(componentName, methodName, parameters);
            Assert.Fail();
        }
        catch (IncorrectNumberOfParametersException exception)
        {
            Assert.AreEqual("error:incorrectNumberOfParameters",exception.Message);
        }
    }

    [Test]
    public void TestSetKeyInt()
    {
        altUnityDriver.DeletePlayerPref();
        altUnityDriver.SetKeyPlayerPref("test",1);
        var val = altUnityDriver.GetIntKeyPlayerPref("test");
        Assert.AreEqual(1,val);
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
            var alt=altUnityDriver.GetIntKeyPlayerPref("test");
            Assert.Fail();
        }
        catch (NotFoundException exception)
        {
            Assert.AreEqual("error:notFound",exception.Message);
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
        Vector2 pozOnScreenFromMainCamera = new Vector2(altElement.x, altElement.y);
        Vector2 pozOnScreenFromSecondaryCamera = new Vector2(altElement2.x, altElement2.y);

        Assert.AreNotEqual(pozOnScreenFromSecondaryCamera,pozOnScreenFromMainCamera);
        
    }

    [Test]
    public void TestFindNonExistentObject()
    {
        try
        {
            var altElemen2 = altUnityDriver.FindElement("NonExistent");
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
            var altElemen2 =altUnityDriver.FindElementWhereNameContains("NonExistent");
            Assert.Fail();
        }
        catch (NotFoundException exception)
        {
            Assert.AreEqual(exception.Message, "error:notFound");
        }
        
    }

    [Test]
    public void TestClickOnNothing()
    {
        try
        {
            var altElemen2 = altUnityDriver.TapScreen(0, 0);
            Assert.Fail();
        }
        catch (NullRefferenceException exception)
        {
            Assert.AreEqual(exception.Message, "error:nullRefferenceException");
        }
       
    }

    [Test]
    public void TestButtonClickWithSwipe()
    {
        var button = altUnityDriver.FindElement("UIButton");
        Vector2 vector2 = new Vector2(button.x, button.y);
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
        Assert.AreEqual(name,altElement.name);
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
            var altElement = altUnityDriver.WaitForElement("dlkasldkas", timeout:1, interval:1);
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
           Assert.AreEqual(exception.Message, "Element Capsule still not found after 1 seconds");
       }
   }

    [Test]
    public void TestWaitForObjectWithTextWrongText()
    {
        try
        {
            var altElement = altUnityDriver.WaitForElementWithText("CapsuleInfo", "aaaaa", timeout: 1);
            Assert.Fail();
        }
        catch (WaitTimeOutException exception)
        {
            Assert.AreEqual("Element with text: aaaaa not loaded after 1 seconds",exception.Message);
        }
    }

    [Test]
    public void TestWaitForCurrrentSceneToBeANonExistingScene()
    {
        const string name = "AltUnityDriverTestScenee";
        try
        {
            var altElement = altUnityDriver.WaitForCurrentSceneToBe(name, 1);
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
            var altElement = altUnityDriver.WaitForElementWhereNameContains(name, timeout: 1);
            Assert.Fail();
        }
        catch (WaitTimeOutException exception)
        {
            Assert.AreEqual("Element xyz still not found after 1 seconds", exception.Message);
        }
      
    }

    [Test]
    public void TestCallStaticMethod()
    {

        altUnityDriver.CallStaticMethods("UnityEngine.PlayerPrefs", "SetInt","Test?1");
        int a = Int32.Parse(altUnityDriver.CallStaticMethods("UnityEngine.PlayerPrefs", "GetInt", "Test?2"));
        Assert.AreEqual(1, a);

}
[Test]
    public void TestCallMethodWithMultipleDefinitions() 
    {

    AltUnityObject capsule=altUnityDriver.FindElement("Capsule");
    capsule.CallComponentMethod("Capsule", "Test","2","System.Int32");
    AltUnityObject capsuleInfo=altUnityDriver.FindElement("CapsuleInfo");
    Assert.AreEqual("6",capsuleInfo.GetText());
    }
   




}
