package ro.altom.altunitytester;

import com.google.gson.Gson;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParameters;
import ro.altom.altunitytester.Commands.FindObject.AltGetAllElements;
import ro.altom.altunitytester.Commands.FindObject.AltGetAllElementsParameters;
import ro.altom.altunitytester.Commands.FindObject.AltWaitForObjectWithTextParameters;
import ro.altom.altunitytester.Commands.FindObject.AltWaitForObjectsParameters;
import ro.altom.altunitytester.Commands.InputActions.AltTiltParameters;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParameters;
import ro.altom.altunitytester.altUnityTesterExceptions.*;
import ro.altom.altunitytester.position.Vector3;

import java.io.IOException;
import java.sql.Time;

import static org.junit.Assert.*;

public class TestsSampleScene1 {

    private static AltUnityDriver altUnityDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13000, ";", "&", true);
    }

    @AfterClass
    public static void tearDown() throws Exception {
        altUnityDriver.stop();
        Thread.sleep(1000);
    }

    @Before
    public void loadLevel() throws Exception {
        altUnityDriver.loadScene("Scene 1 AltUnityDriverTestScene");
    }

    @Test
    public void testGetCurrentScene() throws Exception {
        assertEquals("Scene 1 AltUnityDriverTestScene", altUnityDriver.getCurrentScene());
    }

    @Test
    public void testfindElement() throws Exception {
        String name = "Capsule";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                name).build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        assertEquals(name, altElement.name);
    }

    @Test
    public void testfindElements() throws Exception {
        String name = "Plane";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                name).build();
        AltUnityObject[] altElements = altUnityDriver.findObjects(altFindObjectsParameters);
        assertNotNull(altElements);
        assertEquals(altElements[0].name, name);
    }

    @Test
    public void testFindElementWhereNameContains() throws Exception {

        String name = "Cap";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                name).build();
        AltUnityObject altElement = altUnityDriver.findObjectWhichContains(altFindObjectsParameters);
        assertNotNull(altElement);
        assertTrue(altElement.name.contains(name));
    }

    @Test
    public void testFindElementsWhereNameContains() throws Exception {
        String name = "Pla";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                name).build();
        AltUnityObject[] altElements = altUnityDriver.findObjectsWhichContains(altFindObjectsParameters);
        assertNotNull(altElements);
        assertTrue(altElements[0].name.contains(name));
    }

    @Test
    public void testGetAllElements() throws Exception {
        Thread.sleep(1000);
        AltGetAllElementsParameters allElementsParameters = new AltGetAllElementsParameters.Builder().build();
        AltUnityObject[] altElements = altUnityDriver.getAllElements(allElementsParameters);
        assertNotNull(altElements);
        String altElementsString = new Gson().toJson(altElements);
        assertTrue(altElementsString.contains("Capsule"));
        assertTrue(altElementsString.contains("Main Camera"));
        assertTrue(altElementsString.contains("Directional Light"));
        assertTrue(altElementsString.contains("Plane"));
        assertTrue(altElementsString.contains("Canvas"));
        assertTrue(altElementsString.contains("EventSystem"));
        assertTrue(altElementsString.contains("AltUnityRunnerPrefab"));
        assertTrue(altElementsString.contains("CapsuleInfo"));
        assertTrue(altElementsString.contains("UIButton"));
        assertTrue(altElementsString.contains("Text"));
    }

    @Test
    public void testWaitForExistingElement() throws Exception {
        String name = "Capsule";
        long timeStart = System.currentTimeMillis();
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                name).build();
        AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                altFindObjectsParameters).build();
        AltUnityObject altElement = altUnityDriver.waitForObject(altWaitForObjectsParameters);
        long timeEnd = System.currentTimeMillis();
        long time = timeEnd - timeStart;
        assertTrue(time / 1000 < 20);
        assertNotNull(altElement);
        assertEquals(altElement.name, name);
    }

    @Test
    public void testWaitForExistingDisabledElement() throws Exception {
        String name = "Cube";
        long timeStart = System.currentTimeMillis();
        AltUnityObject altElement = altUnityDriver.waitForElement(name, false);
        long timeEnd = System.currentTimeMillis();
        long time = timeEnd - timeStart;
        assertTrue(time / 1000 < 20);
        assertNotNull(altElement);
        assertEquals(altElement.name, name);
    }

    @Test
    public void testWaitForNonExistingElement() {
        String name = "Capsulee";
        try {
            altUnityDriver.waitForElement(name, "", true, 1, 0.5);
            fail();
        } catch (Exception e) {
            assertEquals("Element Capsulee not loaded after 1.0 seconds", e.getMessage());
        }
    }

    @Test
    public void testWaitForCurrentSceneToBe() throws Exception {
        String name = "Scene 1 AltUnityDriverTestScene";
        long timeStart = System.currentTimeMillis();
        String currentScene = altUnityDriver.waitForCurrentSceneToBe(name);
        long timeEnd = System.currentTimeMillis();
        long time = timeEnd - timeStart;
        assertTrue(time / 1000 < 20);
        assertNotNull(currentScene);
        assertEquals("Scene 1 AltUnityDriverTestScene", currentScene);
    }

    @Test
    public void testWaitForCurrrentSceneToBeANonExistingScene() {

        String name = "NonExistentScene";
        try {
            altUnityDriver.waitForCurrentSceneToBe(name, 1, 0.5);
            fail();
        } catch (Exception e) {
            assertEquals(e.getMessage(), "Scene [NonExistentScene] not loaded after 1.0 seconds");
        }
    }

    @Test
    public void testWaitForExistingElementWhereNameContains() throws Exception {
        String name = "Dir";
        long timeStart = System.currentTimeMillis();
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                name).build();
        AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                altFindObjectsParameters).build();
        AltUnityObject altElement = altUnityDriver.waitForObjectWhichContains(altWaitForObjectsParameters);
        long timeEnd = System.currentTimeMillis();
        long time = timeEnd - timeStart;
        assertTrue(time / 1000 < 20);
        assertNotNull(altElement);
        assertEquals(altElement.name, "Directional Light");
    }

    @Test
    public void testWaitForNonExistingElementWhereNameContains() {
        String name = "xyz";
        try {
            altUnityDriver.waitForElementWhereNameContains(name, "", true, 1, 0.5);
            fail();
        } catch (Exception e) {
            assertEquals(e.getMessage(), "Element xyz still not found after 1.0 seconds");
        }
    }

    @Test
    public void testWaitForElementWithText() throws Exception {
        String name = "CapsuleInfo";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                name).build();

        String text = altUnityDriver.findObject(altFindObjectsParameters).getText();
        long timeStart = System.currentTimeMillis();
        AltWaitForObjectWithTextParameters altWaitForObjectsParameters = new AltWaitForObjectWithTextParameters.Builder(
                altFindObjectsParameters, text).build();
        AltUnityObject altElement = altUnityDriver.waitForObjectWithText(altWaitForObjectsParameters);
        long timeEnd = System.currentTimeMillis();
        long time = timeEnd - timeStart;
        assertTrue(time / 1000 < 20);
        assertNotNull(altElement);
        assertEquals(altElement.getText(), text);

    }

    @Test
    public void testWaitForElementWithWrongText() throws Exception {
        String name = "CapsuleInfo";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                name).build();
        String text = altUnityDriver.findObject(altFindObjectsParameters).getText() + "WrongText";
        try {
            altUnityDriver.waitForElementWithText(name, text, "", true, 1, 0.5);
            fail();
        } catch (WaitTimeOutException e) {
            assertEquals(e.getMessage(), "Element with text: Capsule InfoWrongText not loaded after 1.0 seconds");
        }
    }

    @Test
    public void testFindElementByComponent() throws Exception {
        Thread.sleep(1000);
        String componentName = "AltUnityRunner";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.COMPONENT, componentName).build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        assertEquals(altElement.name, "AltUnityRunnerPrefab");
    }

    @Test
    public void testFindElementByComponentWithNamespace() throws Exception {
        Thread.sleep(1000);
        String componentName = "AltUnityTester.AltUnityServer.AltUnityRunner";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.COMPONENT, componentName).build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        assertEquals(altElement.name, "AltUnityRunnerPrefab");
    }

    @Test
    public void testGetComponentProperty() throws Exception {
        Thread.sleep(1000);
        String componentName = "AltUnityRunner";
        String propertyName = "SocketPortNumber";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "AltUnityRunnerPrefab").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        String propertyValue = altElement.getComponentProperty(componentName, propertyName);
        assertEquals(propertyValue, "13000");
    }

    @Test
    public void testGetNonExistingComponentProperty() throws Exception {
        Thread.sleep(1000);
        String componentName = "AltUnityRunner";
        String propertyName = "socketPort";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "AltUnityRunnerPrefab").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        try {
            altElement.getComponentProperty(componentName, propertyName);
            fail();
        } catch (PropertyNotFoundException e) {
            assertEquals(e.getMessage(), "error:propertyNotFound");
        }
    }

    @Test
    public void testGetComponentPropertyArray() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String propertyName = "arrayOfInts";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        String propertyValue = altElement.getComponentProperty(componentName, propertyName);
        assertEquals("[1,2,3]", propertyValue);
    }

    @Test
    public void testGetComponentPropertyUnityEngine() throws Exception {
        String componentName = "UnityEngine.CapsuleCollider";
        String propertyName = "isTrigger";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        String propertyValue = altElement.getComponentProperty(componentName, propertyName);
        assertEquals("false", propertyValue);
    }

    @Test
    public void testSetComponentProperty() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String propertyName = "stringToSetFromTests";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        String propertyValue = altElement.setComponentProperty(componentName, propertyName, "2");
        assertEquals("valueSet", propertyValue);
        propertyValue = altElement.getComponentProperty(componentName, propertyName);
        assertEquals("2", propertyValue);
    }

    @Test
    public void testSetNonExistingComponentProperty() throws Exception {
        String componentName = "AltUnityExampleScriptCapsulee";
        String propertyName = "stringToSetFromTests";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        try {
            altElement.setComponentProperty(componentName, propertyName, "2");
            fail();
        } catch (ComponentNotFoundException e) {
            assertEquals(e.getMessage(), "error:componentNotFound");
        }
    }

    @Test
    public void testCallMethodWithNoParameters() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "UIButtonClicked";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        String data = altElement.callComponentMethod(componentName, methodName, "");
        assertEquals("null", data);
    }

    @Test
    public void testCallMethodWithParameters() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "Jump";
        String parameters = "New Text";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        String data = altElement.callComponentMethod(componentName, methodName, parameters);
        assertEquals("null", data);
    }

    @Test
    public void testCallMethodWithManyParameters() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "TestMethodWithManyParameters";
        String parameters = "1?stringparam?0.5?[1,2,3]";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        String data = altElement.callComponentMethod(componentName, methodName, parameters);
        assertEquals("null", data);
    }

    @Test
    public void testCallMethodWithIncorrectNumberOfParameters() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "TestMethodWithManyParameters";
        String parameters = "1?stringparam?[1,2,3]";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        try {
            altElement.callComponentMethod(componentName, methodName, parameters);
            fail();
        } catch (MethodWithGivenParametersNotFoundException e) {
            assertEquals(e.getMessage(), "error:methodWithGivenParametersNotFound");
        }
    }

    @Test
    public void testCallMethodWithIncorrectTypeOfParameters() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "TestMethodWithManyParameters";
        String parameters = "a?stringparam?[1,2,3]";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        try {
            altElement.callComponentMethod(componentName, methodName, parameters);
            fail();
        } catch (MethodWithGivenParametersNotFoundException e) {
            assertEquals(e.getMessage(), "error:methodWithGivenParametersNotFound");
        }
    }

    @Test
    public void testCallMethodInvalidParameterType()
    {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "TestMethodWithManyParameters";
        String parameters = "1?stringparam?0.5?[1,2,3]";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        try
        {
            altElement.callComponentMethod("", componentName, methodName, parameters, "System.Stringggggg");
            fail();
        }
        catch (InvalidParameterTypeException e)
        {
            assertEquals(e.getMessage(), "error:invalidParameterType");
        }
    }

    @Test
    public void testCallMethodAssmeblyNotFound()
    {
        String componentName = "RandomComponent";
        String methodName = "TestMethodWithManyParameters";
        String parameters = "a?stringparam?0.5?[1,2,3]";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        try
        {
            altElement.callComponentMethod("RandomAssembly", componentName, methodName, parameters, "");
            fail();
        }
        catch (AssemblyNotFoundException e)
        {
            assertEquals(e.getMessage(), "error:assemblyNotFound");
        }
    }


    @Test
    public void testCallMethodWithIncorrectNumberOfParameters2()
    {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "TestMethodWithManyParameters";
        String parameters = "a?stringparam?[1,2,3]";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
        "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        
        try
        {
            altElement.callComponentMethod("", componentName, methodName, parameters, "");
            fail();
        }
        catch (MethodWithGivenParametersNotFoundException e)
        {
            assertEquals(e.getMessage(), "error:methodWithGivenParametersNotFound");
        }
    }


    @Test
    public void testSetKeyInt() throws Exception {
        altUnityDriver.deletePlayerPref();
        altUnityDriver.setKeyPlayerPref("test", 1);
        int val = altUnityDriver.getIntKeyPlayerPref("test");
        assertEquals(1, val);
    }

    @Test
    public void testSetKeyFloat() throws Exception {
        altUnityDriver.deletePlayerPref();
        altUnityDriver.setKeyPlayerPref("test", 1f);
        float val = altUnityDriver.getFloatKeyPlayerPref("test");
        assertEquals(1f, val, 0.01);
    }

    @Test
    public void testSetKeyString() throws Exception {
        altUnityDriver.deletePlayerPref();
        altUnityDriver.setKeyPlayerPref("test", "test");
        String val = altUnityDriver.getStringKeyPlayerPref("test");
        assertEquals("test", val);
    }

    @Test
    public void testDeleteKey() throws Exception {
        altUnityDriver.deletePlayerPref();
        altUnityDriver.setKeyPlayerPref("test", 1);
        int val = altUnityDriver.getIntKeyPlayerPref("test");
        assertEquals(1, val);
        altUnityDriver.deleteKeyPlayerPref("test");
        try {
            altUnityDriver.getIntKeyPlayerPref("test");
            fail();
        } catch (NotFoundException e) {
            assertEquals(e.getMessage(), "error:notFound");
        }
    }

    @Test
    public void testDifferentCamera() throws Exception {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Button").withCamera(By.NAME, "Main Camera").build();
        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Capsule").withCamera(By.NAME, "Main Camera").build();
        AltFindObjectsParameters altFindObjectsParameters3 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Capsule").withCamera(By.NAME, "Camera").build();
        AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParameters1);
        altButton.clickEvent();
        altButton.clickEvent();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters2);
        AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParameters3);
        assertNotSame(altElement.x, altElement2.x);
        assertNotSame(altElement.y, altElement2.y);
    }

    @Test
    public void testFindNonExistentObject() throws Exception {
        try {
            AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                    AltUnityDriver.By.NAME, "NonExistent").build();
            altUnityDriver.findObject(altFindObjectsParameters1);
            fail();
        } catch (NotFoundException e) {
            assertEquals(e.getMessage(), "error:notFound");
        }
    }

    @Test
    public void testFindNonExistentObjectByName() throws Exception {
        try {
            AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                    AltUnityDriver.By.NAME, "NonExistent").build();
            altUnityDriver.findObject(altFindObjectsParameters1);
            fail();
        } catch (NotFoundException e) {
            assertEquals(e.getMessage(), "error:notFound");
        }
    }

    @Test
    public void testButtonClickWithSwipe() throws Exception {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "UIButton").build();
        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "CapsuleInfo").build();
        AltUnityObject button = altUnityDriver.findObject(altFindObjectsParameters1);
        altUnityDriver.holdButtonAndWait(button.x, button.y, 1);
        AltUnityObject capsuleInfo = altUnityDriver.findObject(altFindObjectsParameters2);
        Thread.sleep(2);
        String text = capsuleInfo.getText();
        assertEquals(text, "UIButton clicked to jump capsule!");
    }

    @Test
    public void testClickEvent() throws Exception {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "UIButton").build();
        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "CapsuleInfo").build();
        AltUnityObject button = altUnityDriver.findObject(altFindObjectsParameters1).clickEvent();
        AltUnityObject capsuleInfo = altUnityDriver.findObject(altFindObjectsParameters2);
        Thread.sleep(2);
        String text = capsuleInfo.getText();
        assertEquals(text, "UIButton clicked to jump capsule!");
    }

    @Test
    public void testButtonTap() throws Exception {
        altUnityDriver.findElement("UIButton").tap();
        AltUnityObject capsuleInfo = altUnityDriver.findElement("CapsuleInfo");
        Thread.sleep(2);
        String text = capsuleInfo.getText();
        assertEquals(text, "UIButton clicked to jump capsule!");
    }

    @Test
    public void testCapsuleTap() throws Exception {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Capsule").build();
        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "CapsuleInfo").build();
        altUnityDriver.findObject(altFindObjectsParameters1).tap();
        AltUnityObject capsuleInfo = altUnityDriver.findObject(altFindObjectsParameters2);
        Thread.sleep(2);
        String text = capsuleInfo.getText();
        assertEquals(text, "Capsule was clicked to jump!");
    }

    @Test
    public void testTapScreen() throws Exception {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "UIButton").build();
        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "CapsuleInfo").build();
        AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters1);
        AltUnityObject capsuleInfo = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver.tapScreen(capsule.x, capsule.y);
        Thread.sleep(2);
        String text = capsuleInfo.getText();
        assertEquals(text, "UIButton clicked to jump capsule!");
    }

    @Test
    public void testWaitForObjectWithTextWrongText() throws Exception {
        try {
            AltUnityObject altElement = altUnityDriver.waitForElementWithText("CapsuleInfo", "aaaaa", "", true, 1, 0.5);
            assertEquals(false, true);
        } catch (WaitTimeOutException exception) {
            assertEquals("Element with text: aaaaa not loaded after 1.0 seconds", exception.getMessage());
        }
    }

    @Test
    public void TestCallStaticMethod() throws Exception {

        altUnityDriver.callStaticMethods("UnityEngine.PlayerPrefs", "SetInt", "Test?1");
        // int a=altUnityDriver.getIntKeyPlayerPref("Test");
        int a = Integer.parseInt(altUnityDriver.callStaticMethods("UnityEngine.PlayerPrefs", "GetInt", "Test?2"));
        assertEquals(1, a);

    }

    @Test
    public void TestCallMethodWithMultipleDefinitions() throws Exception {

        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Capsule").build();
        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "CapsuleInfo").build();
        AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters1);
        capsule.callComponentMethod("", "AltUnityExampleScriptCapsule", "Test", "2", "System.Int32");
        AltUnityObject capsuleInfo = altUnityDriver.findObject(altFindObjectsParameters2);
        assertEquals("6", capsuleInfo.getText());
    }

    @Test
    public void TestTapScreenWhereThereIsNoObjects() {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "ButtonCounter").build();
        AltUnityObject counterButton = altUnityDriver.findObject(altFindObjectsParameters1);
        AltUnityObject altObject = altUnityDriver.tapScreen(1, counterButton.y+100);
        assertEquals(null, altObject);
    }

    @Test
    public void TestGetSetTimeScale() {
        altUnityDriver.setTimeScale(0.1f);
        float timeScale = altUnityDriver.getTimeScale();
        assertEquals(0.1f, timeScale, 0);
        altUnityDriver.setTimeScale(1f);
    }

    @Test
    public void TestCallMethodWithAssembly() {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Capsule").build();
        AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters1);
        String initialRotation = capsule.getComponentProperty("UnityEngine.Transform", "rotation");
        capsule.callComponentMethod("UnityEngine.CoreModule", "UnityEngine.Transform", "Rotate", "10?10?10",
                "System.Single?System.Single?System.Single");
        AltUnityObject capsuleAfterRotation = altUnityDriver.findObject(altFindObjectsParameters1);
        String finalRotation = capsuleAfterRotation.getComponentProperty("UnityEngine.Transform", "rotation");
        assertNotEquals(initialRotation, finalRotation);
    }

    @Test
    public void TestWaitForObjectToNotBePresent() {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "ObjectDestroyedIn5Secs").build();
        AltWaitForObjectsParameters altWaitForObjectsParameters1 = new AltWaitForObjectsParameters.Builder(
                altFindObjectsParameters1).build();
        altUnityDriver.waitForObjectToNotBePresent(altWaitForObjectsParameters1);
        try {
            altUnityDriver.findObject(altFindObjectsParameters1);
            assertFalse("Not found exception should be thrown", true);
        } catch (NotFoundException e) {
            assertTrue(e.getMessage().equals("error:notFound"));
        }

        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsulee").build();
        AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                altFindObjectsParameters).build();
        altUnityDriver.waitForObjectToNotBePresent(altWaitForObjectsParameters);
        try {
            altUnityDriver.findObject(altFindObjectsParameters);
            assertFalse("Not found exception should be thrown", true);
        } catch (NotFoundException e) {
            assertTrue(e.getMessage().equals("error:notFound"));
        }
    }

    @Test
    public void TestGetChineseLetters() {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "ChineseLetters").build();
        String text = altUnityDriver.findObject(altFindObjectsParameters1).getText();
        assertEquals("哦伊娜哦", text);
    }

    @Test
    public void TestNonEnglishText() {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "NonEnglishText").build();
        String text = altUnityDriver.findObject(altFindObjectsParameters1).getText();
        assertEquals("BJÖRN'S PASS", text);
    }

    @Test
    public void TestDoubleTap() throws InterruptedException {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "ButtonCounter").build();
        AltUnityObject counterButton = altUnityDriver.findObject(altFindObjectsParameters1);
        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "ButtonCounter/Text").build();
        AltUnityObject counterButtonText = altUnityDriver.findObject(altFindObjectsParameters2);
        counterButton.doubleTap();
        Thread.sleep(500);
        assertEquals("2", counterButtonText.getText());
    }

    @Test
    public void TestCustomTap() throws InterruptedException {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "ButtonCounter").build();
        AltUnityObject counterButton = altUnityDriver.findObject(altFindObjectsParameters1);
        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "ButtonCounter/Text").build();
        AltUnityObject counterButtonText = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver.tapCustom(counterButton.x, counterButton.y, 4);
        Thread.sleep(1000);
        assertEquals("4", counterButtonText.getText());
    }

    @Test
    public void TestPressNextScene() throws InterruptedException {
        String initialScene = altUnityDriver.getCurrentScene();
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "NextScene").build();
        altUnityDriver.findObject(altFindObjectsParameters1).tap();
        String currentScene = altUnityDriver.getCurrentScene();
        assertNotEquals(initialScene, currentScene);
    }

    @Test
    public void TestSetTextFunction() {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "NonEnglishText").build();
        AltUnityObject textObject = altUnityDriver.findObject(altFindObjectsParameters1);
        String originalText = textObject.getText();
        String afterText = textObject.setText("ModifiedText").getText();
        assertNotEquals(originalText, afterText);
    }

    @Test
    public void TestFindParentUsingPath() {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.PATH, "//CapsuleInfo/..").build();
        AltUnityObject parent = altUnityDriver.findObject(altFindObjectsParameters1);
        assertEquals("Canvas", parent.name);
    }

    @Test
    public void TestAcceleration() throws InterruptedException {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Capsule").build();
        AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters1);
        Vector3 initialWorldCoordinates = capsule.getWorldPosition();
        AltTiltParameters altTiltParameters = new AltTiltParameters.Builder(1, 1, 1).withDuration(1).build();
        altUnityDriver.tilt(altTiltParameters);
        Thread.sleep(1000);
        capsule = altUnityDriver.findObject(altFindObjectsParameters1);
        Vector3 afterTiltCoordinates = capsule.getWorldPosition();
        assertNotEquals(initialWorldCoordinates, afterTiltCoordinates);
    }

    @Test
    public void TestAccelerationAndWait() throws InterruptedException {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Capsule").build();
        AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters1);
        Vector3 initialWorldCoordinates = capsule.getWorldPosition();
        AltTiltParameters altTiltParameters = new AltTiltParameters.Builder(1, 1, 1).withDuration(1).build();
        altUnityDriver.tiltAndWait(altTiltParameters);
        capsule = altUnityDriver.findObject(altFindObjectsParameters1);
        Vector3 afterTiltCoordinates = capsule.getWorldPosition();
        assertNotEquals(initialWorldCoordinates, afterTiltCoordinates);
    }
    
    public void TestFindObjectWithCameraId() {
        AltFindObjectsParameters altFindObjectsParametersButton = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.PATH, "//Button").build();
        AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
        altButton.clickEvent();
        altButton.clickEvent();
        AltFindObjectsParameters altFindObjectsParametersCamera = new AltFindObjectsParameters.Builder(By.PATH,
                "//Camera").build();
        AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);
        AltFindObjectsParameters altFindObjectsParametersCampsule = new AltFindObjectsParameters.Builder(By.COMPONENT,
                "CapsuleCollider").withCamera(By.ID, String.valueOf(camera.id)).build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParametersCampsule);

        assertTrue("True", altElement.name.equals("Capsule"));

        altFindObjectsParametersCamera = new AltFindObjectsParameters.Builder(By.PATH, "//Main Camera").build();
        AltUnityObject camera2 = altUnityDriver.findObject(altFindObjectsParametersCamera);
        altFindObjectsParametersCampsule = new AltFindObjectsParameters.Builder(By.COMPONENT, "CapsuleCollider")
                .withCamera(By.ID, String.valueOf(camera2.id)).build();
        AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParametersCampsule);
        assertNotEquals(altElement.getScreenPosition(), altElement2.getScreenPosition());
    }

    @Test
    public void TestWaitForObjectWithCameraId() {
        AltFindObjectsParameters altFindObjectsParametersButton = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.PATH, "//Button").build();
        AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
        altButton.clickEvent();
        altButton.clickEvent();
        AltFindObjectsParameters altFindObjectsParametersCamera = new AltFindObjectsParameters.Builder(By.PATH,
                "//Camera").build();
        AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);
        AltFindObjectsParameters altFindObjectsParametersCapsule = new AltFindObjectsParameters.Builder(By.COMPONENT,
                "CapsuleCollider").withCamera(By.ID, String.valueOf(camera.id)).build();
        AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                altFindObjectsParametersCapsule).build();
        AltUnityObject altElement = altUnityDriver.waitForObject(altWaitForObjectsParameters);

        assertTrue("True", altElement.name.equals("Capsule"));

        altFindObjectsParametersCamera = new AltFindObjectsParameters.Builder(By.PATH, "//Main Camera").build();
        AltUnityObject camera2 = altUnityDriver.findObject(altFindObjectsParametersCamera);
        altFindObjectsParametersCapsule = new AltFindObjectsParameters.Builder(By.COMPONENT, "CapsuleCollider")
                .withCamera(By.ID, String.valueOf(camera2.id)).build();
        altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(altFindObjectsParametersCapsule).build();
        AltUnityObject altElement2 = altUnityDriver.waitForObject(altWaitForObjectsParameters);

        assertNotEquals(altElement.getScreenPosition(), altElement2.getScreenPosition());
    }

    @Test

    public void TestFindObjectsWithCameraId() {
        AltFindObjectsParameters altFindObjectsParametersButton = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.PATH, "//Button").build();
        AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
        altButton.clickEvent();
        altButton.clickEvent();
        AltFindObjectsParameters altFindObjectsParametersCamera = new AltFindObjectsParameters.Builder(By.PATH,
                "//Camera").build();
        AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);
        AltFindObjectsParameters altFindObjectsParametersCapsule = new AltFindObjectsParameters.Builder(By.NAME,
                "Plane").withCamera(By.ID, String.valueOf(camera.id)).build();

        AltUnityObject[] altElement = altUnityDriver.findObjects(altFindObjectsParametersCapsule);

        assertTrue("True", altElement[0].name.equals("Plane"));

        altFindObjectsParametersCamera = new AltFindObjectsParameters.Builder(By.PATH, "//Main Camera").build();
        AltUnityObject camera2 = altUnityDriver.findObject(altFindObjectsParametersCamera);
        altFindObjectsParametersCapsule = new AltFindObjectsParameters.Builder(By.NAME, "Plane")
                .withCamera(By.ID, String.valueOf(camera2.id)).build();
        AltUnityObject[] altElement2 = altUnityDriver.findObjects(altFindObjectsParametersCapsule);

        assertNotEquals(altElement[0].getScreenPosition(), altElement2[0].getScreenPosition());
    }

    @Test

    public void TestWaitForObjectNotBePresentWithCameraId() {
        AltFindObjectsParameters altFindObjectsParametersCamera = new AltFindObjectsParameters.Builder(By.PATH,
                "//Main Camera").build();
        AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);

        AltFindObjectsParameters altFindObjectsParametersObject = new AltFindObjectsParameters.Builder(By.NAME,
                "ObjectDestroyedIn5Secs").withCamera(By.ID, String.valueOf(camera.id)).build();
        AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                altFindObjectsParametersObject).build();
        altUnityDriver.waitForObjectToNotBePresent(altWaitForObjectsParameters);

        AltGetAllElementsParameters allElementsParameters = new AltGetAllElementsParameters.Builder().build();
        AltUnityObject[] allObjectsInTheScene = altUnityDriver.getAllElements(allElementsParameters);

        Boolean searchObjectFound = false;
        for (AltUnityObject altUnityObject : allObjectsInTheScene) {
            if (altUnityObject.name.equals("ObjectDestroyedIn5Secs")) {
                searchObjectFound = true;
                break;
            }
        }
        assertFalse(searchObjectFound);
    }

    @Test

    public void TestWaitForElementWithTextWithCameraId() {
        AltFindObjectsParameters altFindObjectsParametersCamera = new AltFindObjectsParameters.Builder(By.PATH,
                "//Main Camera").build();
        AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);
        String name = "CapsuleInfo";
        AltFindObjectsParameters altFindObjectsParametersObject = new AltFindObjectsParameters.Builder(By.NAME, name)
                .withCamera(By.ID, String.valueOf(camera.id)).build();
        String text = altUnityDriver.findObject(altFindObjectsParametersObject).getText();
        AltWaitForObjectWithTextParameters altWaitForObjectWithTextParameters = new AltWaitForObjectWithTextParameters.Builder(
                altFindObjectsParametersObject, text).build();
        AltUnityObject altElement = altUnityDriver.waitForObjectWithText(altWaitForObjectWithTextParameters);
        assertNotNull(altElement);
        assertEquals(altElement.getText(), text);
    }

    @Test

    public void TestWaitForObjectWhichContainsWithCameraId() {
        AltFindObjectsParameters altFindObjectsParametersCamera = new AltFindObjectsParameters.Builder(By.PATH,
                "//Main Camera").build();
        AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);

        AltFindObjectsParameters altFindObjectsParametersObject = new AltFindObjectsParameters.Builder(By.NAME, "Canva")
                .withCamera(By.ID, String.valueOf(camera.id)).build();
        AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                altFindObjectsParametersObject).build();
        AltUnityObject altElement = altUnityDriver.waitForObjectWhichContains(altWaitForObjectsParameters);
        assertEquals("Canvas", altElement.name);

    }

    @Test
    public void TestFindObjectWithTag() {
        AltFindObjectsParameters altFindObjectsParametersButton = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.PATH, "//Button").build();
        AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
        altButton.clickEvent();
        altButton.clickEvent();
        AltFindObjectsParameters altFindObjectsParametersCampsule = new AltFindObjectsParameters.Builder(By.COMPONENT,
                "CapsuleCollider").withCamera(By.TAG, "MainCamera").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParametersCampsule);

        assertTrue("True", altElement.name.equals("Capsule"));

        altFindObjectsParametersCampsule = new AltFindObjectsParameters.Builder(By.COMPONENT, "CapsuleCollider")
                .withCamera(By.TAG, "Untagged").build();
        AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParametersCampsule);
        assertNotEquals(altElement.getScreenPosition(), altElement2.getScreenPosition());
    }

    @Test
    public void TestWaitForObjectWithTag() {
        AltFindObjectsParameters altFindObjectsParametersButton = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.PATH, "//Button").build();
        AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
        altButton.clickEvent();
        altButton.clickEvent();
        AltFindObjectsParameters altFindObjectsParametersCapsule = new AltFindObjectsParameters.Builder(By.COMPONENT,
                "CapsuleCollider").withCamera(By.TAG, "MainCamera").build();
        AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                altFindObjectsParametersCapsule).build();
        AltUnityObject altElement = altUnityDriver.waitForObject(altWaitForObjectsParameters);

        assertTrue("True", altElement.name.equals("Capsule"));

        altFindObjectsParametersCapsule = new AltFindObjectsParameters.Builder(By.COMPONENT, "CapsuleCollider")
                .withCamera(By.TAG, "Untagged").build();
        altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(altFindObjectsParametersCapsule).build();
        AltUnityObject altElement2 = altUnityDriver.waitForObject(altWaitForObjectsParameters);

        assertNotEquals(altElement.getScreenPosition(), altElement2.getScreenPosition());
    }

    @Test

    public void TestFindObjectsWithTag() {
        AltFindObjectsParameters altFindObjectsParametersButton = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.PATH, "//Button").build();
        AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
        altButton.clickEvent();
        altButton.clickEvent();
        AltFindObjectsParameters altFindObjectsParametersCapsule = new AltFindObjectsParameters.Builder(By.NAME,
                "Plane").withCamera(By.TAG, "MainCamera").build();

        AltUnityObject[] altElement = altUnityDriver.findObjects(altFindObjectsParametersCapsule);

        assertTrue("True", altElement[0].name.equals("Plane"));

        altFindObjectsParametersCapsule = new AltFindObjectsParameters.Builder(By.NAME, "Plane")
                .withCamera(By.TAG, "Untagged").build();
        AltUnityObject[] altElement2 = altUnityDriver.findObjects(altFindObjectsParametersCapsule);

        assertNotEquals(altElement[0].getScreenPosition(), altElement2[0].getScreenPosition());
    }

    @Test

    public void TestWaitForObjectNotBePresentWithTag() {

        AltFindObjectsParameters altFindObjectsParametersObject = new AltFindObjectsParameters.Builder(By.NAME,
                "ObjectDestroyedIn5Secs").withCamera(By.TAG, "MainCamera").build();
        AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                altFindObjectsParametersObject).build();
        altUnityDriver.waitForObjectToNotBePresent(altWaitForObjectsParameters);

        AltGetAllElementsParameters allElementsParameters = new AltGetAllElementsParameters.Builder().build();
        AltUnityObject[] allObjectsInTheScene = altUnityDriver.getAllElements(allElementsParameters);

        Boolean searchObjectFound = false;
        for (AltUnityObject altUnityObject : allObjectsInTheScene) {
            if (altUnityObject.name.equals("ObjectDestroyedIn5Secs")) {
                searchObjectFound = true;
                break;
            }
        }
        assertFalse(searchObjectFound);
    }

    @Test

    public void TestWaitForElementWithTextWithTag() {
        String name = "CapsuleInfo";
        AltFindObjectsParameters altFindObjectsParametersObject = new AltFindObjectsParameters.Builder(By.NAME, name)
                .withCamera(By.TAG, "MainCamera").build();
        String text = altUnityDriver.findObject(altFindObjectsParametersObject).getText();
        AltWaitForObjectWithTextParameters altWaitForObjectWithTextParameters = new AltWaitForObjectWithTextParameters.Builder(
                altFindObjectsParametersObject, text).build();
        AltUnityObject altElement = altUnityDriver.waitForObjectWithText(altWaitForObjectWithTextParameters);
        assertNotNull(altElement);
        assertEquals(altElement.getText(), text);
    }

    @Test

    public void TestWaitForObjectWhichContainsWithTag() {

        AltFindObjectsParameters altFindObjectsParametersObject = new AltFindObjectsParameters.Builder(By.NAME, "Canva")
                .withCamera(By.TAG, "MainCamera").build();
        AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                altFindObjectsParametersObject).build();
        AltUnityObject altElement = altUnityDriver.waitForObjectWhichContains(altWaitForObjectsParameters);
        assertEquals("Canvas", altElement.name);

    }
    
    @Test
    public void TestLoadAdditiveScenes() throws Exception {
        AltGetAllElementsParameters altGetAllElementsParameters=new AltGetAllElementsParameters.Builder().build();
        AltUnityObject[] initialNumberOfElements=altUnityDriver.getAllElements(altGetAllElementsParameters);

        AltLoadSceneParameters altLoadSceneParameters=new AltLoadSceneParameters.Builder("Scene 2 Draggable Panel").loadMode(false).build();
        altUnityDriver.loadScene(altLoadSceneParameters);
        AltUnityObject[] finalNumberOfElements=altUnityDriver.getAllElements(altGetAllElementsParameters);
        
        assertNotEquals(initialNumberOfElements,finalNumberOfElements);
        
        String[] scenes=altUnityDriver.getAllLoadedScenes();
        assertEquals(2,scenes.length);   
    }

}
