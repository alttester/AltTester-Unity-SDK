package ro.altom.altunitytester;

import com.google.gson.Gson;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import ro.altom.altunitytester.altUnityTesterExceptions.*;

import java.io.IOException;

import static org.junit.Assert.*;

public class TestsSampleScene1 {

    private static AltUnityDriver altUnityDriver;

    @BeforeClass
    public static void setUp() throws IOException {
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13000);
    }

    @AfterClass
    public static void tearDown() throws Exception {
        altUnityDriver.stop();
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
        AltUnityObject altElement = altUnityDriver.findElement(name);
        assertNotNull(altElement);
        assertEquals(name, altElement.name);
    }

    @Test
    public void testfindElements() throws Exception {
        String name = "Plane";
        AltUnityObject[] altElements = altUnityDriver.findElements(name);
        assertNotNull(altElements);
        assertEquals(altElements[0].name, name);
    }


    @Test
    public void testFindElementWhereNameContains() throws Exception {

        String name = "Cap";
        AltUnityObject altElement = altUnityDriver.findElementWhereNameContains(name);
        assertNotNull(altElement);
        assertTrue(altElement.name.contains(name));
    }

    @Test
    public void testFindElementsWhereNameContains() throws Exception {
        String name = "Pla";
        AltUnityObject[] altElements = altUnityDriver.findElementsWhereNameContains(name);
        assertNotNull(altElements);
        assertTrue(altElements[0].name.contains(name));
    }

    @Test
    public void testGetAllElements() throws Exception {
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
        AltUnityObject altElement = altUnityDriver.waitForElement(name);
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
            altUnityDriver.waitForElement(name,"", 1, 0.5);
            fail();
        } catch (Exception e) {
            assertEquals(e.getMessage(), "Element Capsulee not loaded after 1.0 seconds");
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
            assertEquals(e.getMessage(), "Scene NonExistentScene not loaded after 1.0 seconds");
        }
    }

    @Test
    public void testWaitForExistingElementWhereNameContains() throws Exception {
        String name = "Dir";
        long timeStart = System.currentTimeMillis();
        AltUnityObject altElement = altUnityDriver.waitForElementWhereNameContains(name);
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
            altUnityDriver.waitForElementWhereNameContains(name,"", 1, 0.5);
            fail();
        } catch (Exception e) {
            assertEquals(e.getMessage(), "Element xyz still not found after 1.0 seconds");
        }
    }

    @Test
    public void testWaitForElementWithText() throws Exception {
        String name = "CapsuleInfo";
        String text = altUnityDriver.findElement(name).getText();
        long timeStart = System.currentTimeMillis();
        AltUnityObject altElement = altUnityDriver.waitForElementWithText(name, text);
        long timeEnd = System.currentTimeMillis();
        long time = timeEnd - timeStart;
        assertTrue(time / 1000 < 20);
        assertNotNull(altElement);
        assertEquals(altElement.getText(), text);

    }

    @Test
    public void testWaitForElementWithWrongText() throws Exception {
        String name = "CapsuleInfo";
        String text = altUnityDriver.findElement(name).getText() + "WrongText";
        try {
            altUnityDriver.waitForElementWithText(name, text,"", 1, 0.5);
            fail();
        } catch (WaitTimeOutException e) {
            assertEquals(e.getMessage(), "Element with text: Capsule InfoWrongText not loaded after 1.0 seconds");
        }
    }

    @Test
    public void testFindElementByComponent() throws Exception {
        String componentName = "AltUnityRunner";
        AltUnityObject altElement = altUnityDriver.findElementByComponent(componentName);
        assertNotNull(altElement);
        assertEquals(altElement.name, "AltUnityRunnerPrefab");
    }

    @Test
    public void testGetComponentProperty() throws Exception {
        String componentName = "AltUnityRunner";
        String propertyName = "SocketPortNumber";
        AltUnityObject altElement = altUnityDriver.findElement("AltUnityRunnerPrefab");
        assertNotNull(altElement);
        String propertyValue = altElement.getComponentProperty(componentName, propertyName);
        assertEquals(propertyValue, "13000");
    }

    @Test
    public void testGetNonExistingComponentProperty() throws Exception {
        String componentName = "AltUnityRunner";
        String propertyName = "socketPort";
        AltUnityObject altElement = altUnityDriver.findElement("AltUnityRunnerPrefab");
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
        AltUnityObject altElement = altUnityDriver.findElement("Capsule");
        assertNotNull(altElement);
        String propertyValue = altElement.getComponentProperty(componentName, propertyName);
        assertEquals("[1,2,3]", propertyValue);
    }

    @Test
    public void testGetComponentPropertyUnityEngine() throws Exception {
        String componentName = "UnityEngine.CapsuleCollider";
        String propertyName = "isTrigger";
        AltUnityObject altElement = altUnityDriver.findElement("Capsule");
        assertNotNull(altElement);
        String propertyValue = altElement.getComponentProperty(componentName, propertyName);
        assertEquals("false", propertyValue);
    }

    @Test
    public void testSetComponentProperty() throws Exception {
        String componentName = "Capsule";
        String propertyName = "stringToSetFromTests";
        AltUnityObject altElement = altUnityDriver.findElement("Capsule");
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
        AltUnityObject altElement = altUnityDriver.findElement("Capsule");
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
        AltUnityObject altElement = altUnityDriver.findElement("Capsule");
        String data = altElement.callComponentMethod(componentName, methodName, "");
        assertEquals("null", data);
    }


    @Test
    public void testCallMethodWithParameters() throws Exception {
        String componentName = "Capsule";
        String methodName = "Jump";
        String parameters = "New Text";
        AltUnityObject altElement = altUnityDriver.findElement("Capsule");
        String data = altElement.callComponentMethod(componentName, methodName, parameters);
        assertEquals("null", data);
    }

    @Test
    public void testCallMethodWithManyParameters() throws Exception {
        String componentName = "Capsule";
        String methodName = "TestMethodWithManyParameters";
        String parameters = "1?stringparam?0.5?[1,2,3]";
        AltUnityObject altElement = altUnityDriver.findElement("Capsule");
        String data = altElement.callComponentMethod(componentName, methodName, parameters);
        assertEquals("null", data);
    }

    @Test
    public void testCallMethodWithIncorrectNumberOfParameters() throws Exception {
        String componentName = "Capsule";
        String methodName = "TestMethodWithManyParameters";
        String parameters = "1?stringparam?[1,2,3]";
        AltUnityObject altElement = altUnityDriver.findElement("Capsule");
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
        AltUnityObject altElement = altUnityDriver.findElement("Capsule");
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
        AltUnityObject altButton = altUnityDriver.findElement("Button", "Main Camera");
        altButton.clickEvent();
        altButton.clickEvent();
        AltUnityObject altElement = altUnityDriver.findElement("Capsule", "Main Camera");
        AltUnityObject altElement2 = altUnityDriver.findElement("Capsule", "Camera");
        assertNotSame(altElement.x, altElement2.x);
        assertNotSame(altElement.y, altElement2.y);
    }

    @Test
    public void testFindNonExistentObject() throws Exception {
        try {
            altUnityDriver.findElement("NonExistent");
            fail();
        } catch (NotFoundException e) {
            assertEquals(e.getMessage(), "error:notFound");
        }
    }

    @Test
    public void testFindNonExistentObjectByName() throws Exception {
        try {
            altUnityDriver.findElementWhereNameContains("NonExistent");
            fail();
        } catch (NotFoundException e) {
            assertEquals(e.getMessage(), "error:notFound");
        }
    }

    @Test
    public void testClickOnNothing() throws Exception {
        try {
            altUnityDriver.clickScreen(0,0);
            fail();
        } catch (CouldNotPerformOperationException e) {
            assertEquals(e.getMessage(), "error:couldNotPerformOperation");
        }
    }

    @Test
    public void testButtonClickWithSwipe() throws Exception {
        AltUnityObject button = altUnityDriver.findElement("UIButton");
        altUnityDriver.swipeAndWait(button.x, button.y, button.x, button.y, 1);
        AltUnityObject capsuleInfo = altUnityDriver.findElement("CapsuleInfo");
        Thread.sleep(2);
        String text = capsuleInfo.getText();
        assertEquals(text, "UIButton clicked to jump capsule!");
    }

    @Test
    public void testClickEvent() throws Exception {
        AltUnityObject button = altUnityDriver.findElement("UIButton").clickEvent();
        AltUnityObject capsuleInfo = altUnityDriver.findElement("CapsuleInfo");
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
        altUnityDriver.findElement("Capsule").tap();
        AltUnityObject capsuleInfo = altUnityDriver.findElement("CapsuleInfo");
        Thread.sleep(2);
        String text = capsuleInfo.getText();
        assertEquals(text, "Capsule was clicked to jump!");
    }

    @Test
    public void testTapScreen() throws Exception {
        AltUnityObject capsule = altUnityDriver.findElement("Capsule");
        AltUnityObject capsuleInfo = altUnityDriver.findElement("CapsuleInfo");
        altUnityDriver.tapScreen(capsule.x, capsule.y);
        Thread.sleep(2);
        String text = capsuleInfo.getText();
        assertEquals(text, "Capsule was clicked to jump!");
    }
    @Test
    public void testWaitForObjectWithTextWrongText() throws Exception {
        try
        {
            AltUnityObject altElement = altUnityDriver.waitForElementWithText("CapsuleInfo", "aaaaa","", 1,0.5);
            assertEquals(false,true);
        }
        catch (WaitTimeOutException exception)
        {
            assertEquals("Element with text: aaaaa not loaded after 1.0 seconds",exception.getMessage());
        }
    }

@Test
public void TestCallStaticMethod() throws Exception {

    altUnityDriver.callStaticMethods("UnityEngine.PlayerPrefs", "SetInt","Test?1");
//    int a=altUnityDriver.getIntKeyPlayerPref("Test");
    int a=Integer.parseInt(altUnityDriver.callStaticMethods("UnityEngine.PlayerPrefs", "GetInt", "Test?2"));
    assertEquals(1,a);

}
    @Test
    public void TestCallMethodWithMultipleDefinitions() throws Exception {

        AltUnityObject capsule=altUnityDriver.findElement("Capsule");
        capsule.callComponentMethod("Capsule", "Test","2","System.Int32","");
        AltUnityObject capsuleInfo=altUnityDriver.findElement("CapsuleInfo");
        assertEquals("6",capsuleInfo.getText());
    }

}
