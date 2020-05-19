package ro.altom.altunitytester;

import com.google.gson.Gson;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParameters;
import ro.altom.altunitytester.Commands.FindObject.AltWaitForObjectsParameters;
import ro.altom.altunitytester.altUnityTesterExceptions.*;

import java.io.IOException;

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
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME, name);
        assertNotNull(altElement);
        assertEquals(name, altElement.name);
    }

    @Test
    public void testfindElements() throws Exception {
        String name = "Plane";
        AltUnityObject[] altElements = altUnityDriver.findObjects(AltUnityDriver.By.NAME, name);
        assertNotNull(altElements);
        assertEquals(altElements[0].name, name);
    }

    @Test
    public void testFindElementWhereNameContains() throws Exception {

        String name = "Cap";
        AltUnityObject altElement = altUnityDriver.findObjectWhichContains(AltUnityDriver.By.NAME, name);
        assertNotNull(altElement);
        assertTrue(altElement.name.contains(name));
    }

    @Test
    public void testFindElementsWhereNameContains() throws Exception {
        String name = "Pla";
        AltUnityObject[] altElements = altUnityDriver.findObjectsWhichContains(AltUnityDriver.By.NAME, name);
        assertNotNull(altElements);
        assertTrue(altElements[0].name.contains(name));
    }

    @Test
    public void testGetAllElements() throws Exception {
        Thread.sleep(1000);
        AltUnityObject[] altElements = altUnityDriver.getAllElements();
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
        AltUnityObject altElement = altUnityDriver.waitForObject(AltUnityDriver.By.NAME, name);
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
        AltUnityObject altElement = altUnityDriver.waitForObjectWhichContains(AltUnityDriver.By.NAME, name);
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
        String text = altUnityDriver.findObject(AltUnityDriver.By.NAME, name).getText();
        long timeStart = System.currentTimeMillis();
        AltUnityObject altElement = altUnityDriver.waitForObjectWithText(AltUnityDriver.By.NAME, name, text);
        long timeEnd = System.currentTimeMillis();
        long time = timeEnd - timeStart;
        assertTrue(time / 1000 < 20);
        assertNotNull(altElement);
        assertEquals(altElement.getText(), text);

    }

    @Test
    public void testWaitForElementWithWrongText() throws Exception {
        String name = "CapsuleInfo";
        String text = altUnityDriver.findObject(AltUnityDriver.By.NAME, name).getText() + "WrongText";
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
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.COMPONENT, componentName);
        assertNotNull(altElement);
        assertEquals(altElement.name, "AltUnityRunnerPrefab");
    }

    @Test
    public void testGetComponentProperty() throws Exception {
        Thread.sleep(1000);
        String componentName = "AltUnityRunner";
        String propertyName = "SocketPortNumber";
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME, "AltUnityRunnerPrefab");
        assertNotNull(altElement);
        String propertyValue = altElement.getComponentProperty(componentName, propertyName);
        assertEquals(propertyValue, "13000");
    }

    @Test
    public void testGetNonExistingComponentProperty() throws Exception {
        Thread.sleep(1000);
        String componentName = "AltUnityRunner";
        String propertyName = "socketPort";
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME, "AltUnityRunnerPrefab");
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
        String componentName = "Capsule";
        String propertyName = "arrayOfInts";
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule");
        assertNotNull(altElement);
        String propertyValue = altElement.getComponentProperty(componentName, propertyName);
        assertEquals("[1,2,3]", propertyValue);
    }

    @Test
    public void testGetComponentPropertyUnityEngine() throws Exception {
        String componentName = "UnityEngine.CapsuleCollider";
        String propertyName = "isTrigger";
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule");
        assertNotNull(altElement);
        String propertyValue = altElement.getComponentProperty(componentName, propertyName);
        assertEquals("false", propertyValue);
    }

    @Test
    public void testSetComponentProperty() throws Exception {
        String componentName = "Capsule";
        String propertyName = "stringToSetFromTests";
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule");
        assertNotNull(altElement);
        String propertyValue = altElement.setComponentProperty(componentName, propertyName, "2");
        assertEquals("valueSet", propertyValue);
        propertyValue = altElement.getComponentProperty(componentName, propertyName);
        assertEquals("2", propertyValue);
    }

    @Test
    public void testSetNonExistingComponentProperty() throws Exception {
        String componentName = "Capsulee";
        String propertyName = "stringToSetFromTests";
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule");
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
        String componentName = "Capsule";
        String methodName = "UIButtonClicked";
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule");
        String data = altElement.callComponentMethod(componentName, methodName, "");
        assertEquals("null", data);
    }

    @Test
    public void testCallMethodWithParameters() throws Exception {
        String componentName = "Capsule";
        String methodName = "Jump";
        String parameters = "New Text";
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule");
        String data = altElement.callComponentMethod(componentName, methodName, parameters);
        assertEquals("null", data);
    }

    @Test
    public void testCallMethodWithManyParameters() throws Exception {
        String componentName = "Capsule";
        String methodName = "TestMethodWithManyParameters";
        String parameters = "1?stringparam?0.5?[1,2,3]";
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule");
        String data = altElement.callComponentMethod(componentName, methodName, parameters);
        assertEquals("null", data);
    }

    @Test
    public void testCallMethodWithIncorrectNumberOfParameters() throws Exception {
        String componentName = "Capsule";
        String methodName = "TestMethodWithManyParameters";
        String parameters = "1?stringparam?[1,2,3]";
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule");
        try {
            altElement.callComponentMethod(componentName, methodName, parameters);
            fail();
        } catch (IncorrectNumberOfParametersException e) {
            assertEquals(e.getMessage(), "error:incorrectNumberOfParameters");
        }
    }

    @Test
    public void testCallMethodWithIncorrectTypeOfParameters() throws Exception {
        String componentName = "Capsule";
        String methodName = "TestMethodWithManyParameters";
        String parameters = "a?stringparam?[1,2,3]";
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule");
        try {
            altElement.callComponentMethod(componentName, methodName, parameters);
            fail();
        } catch (IncorrectNumberOfParametersException e) {
            assertEquals(e.getMessage(), "error:incorrectNumberOfParameters");
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
        AltUnityObject altButton = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Button", "Main Camera");
        altButton.clickEvent();
        altButton.clickEvent();
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule", "Main Camera");
        AltUnityObject altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule", "Camera");
        assertNotSame(altElement.x, altElement2.x);
        assertNotSame(altElement.y, altElement2.y);
    }

    @Test
    public void testFindNonExistentObject() throws Exception {
        try {
            altUnityDriver.findObject(AltUnityDriver.By.NAME, "NonExistent");
            fail();
        } catch (NotFoundException e) {
            assertEquals(e.getMessage(), "error:notFound");
        }
    }

    @Test
    public void testFindNonExistentObjectByName() throws Exception {
        try {
            altUnityDriver.findObject(AltUnityDriver.By.NAME, "NonExistent");
            fail();
        } catch (NotFoundException e) {
            assertEquals(e.getMessage(), "error:notFound");
        }
    }

    @Test
    public void testButtonClickWithSwipe() throws Exception {
        AltUnityObject button = altUnityDriver.findObject(AltUnityDriver.By.NAME, "UIButton");
        altUnityDriver.holdButtonAndWait(button.x, button.y, 1);
        AltUnityObject capsuleInfo = altUnityDriver.findObject(AltUnityDriver.By.NAME, "CapsuleInfo");
        Thread.sleep(2);
        String text = capsuleInfo.getText();
        assertEquals(text, "UIButton clicked to jump capsule!");
    }

    @Test
    public void testClickEvent() throws Exception {
        AltUnityObject button = altUnityDriver.findObject(AltUnityDriver.By.NAME, "UIButton").clickEvent();
        AltUnityObject capsuleInfo = altUnityDriver.findObject(AltUnityDriver.By.NAME, "CapsuleInfo");
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
        altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule").tap();
        AltUnityObject capsuleInfo = altUnityDriver.findObject(AltUnityDriver.By.NAME, "CapsuleInfo");
        Thread.sleep(2);
        String text = capsuleInfo.getText();
        assertEquals(text, "Capsule was clicked to jump!");
    }

    @Test
    public void testTapScreen() throws Exception {
        AltUnityObject capsule = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule");
        AltUnityObject capsuleInfo = altUnityDriver.findObject(AltUnityDriver.By.NAME, "CapsuleInfo");
        altUnityDriver.tapScreen(capsule.x, capsule.y);
        Thread.sleep(2);
        String text = capsuleInfo.getText();
        assertEquals(text, "Capsule was clicked to jump!");
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

        AltUnityObject capsule = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule");
        capsule.callComponentMethod("", "Capsule", "Test", "2", "System.Int32");
        AltUnityObject capsuleInfo = altUnityDriver.findObject(AltUnityDriver.By.NAME, "CapsuleInfo");
        assertEquals("6", capsuleInfo.getText());
    }

    @Test
    public void TestTapScreenWhereThereIsNoObjects() {
        AltUnityObject altObject = altUnityDriver.tapScreen(1, 1);
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
        AltUnityObject capsule = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule");
        String initialRotation = capsule.getComponentProperty("UnityEngine.Transform", "rotation");
        capsule.callComponentMethod("UnityEngine.CoreModule", "UnityEngine.Transform", "Rotate", "10?10?10",
                "System.Single?System.Single?System.Single");
        AltUnityObject capsuleAfterRotation = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Capsule");
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
    public void TestGetChineseLetters()
    {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
            AltUnityDriver.By.NAME, "ChineseLetters").build();
        String text = altUnityDriver.findObject(altFindObjectsParameters1).getText();
        assertEquals("哦伊娜哦", text);
    }
    @Test
    public void TestNonEnglishText()
    {
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
}
