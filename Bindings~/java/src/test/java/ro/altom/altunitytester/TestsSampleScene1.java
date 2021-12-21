package ro.altom.altunitytester;

import com.google.gson.Gson;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.AltCallStaticMethodParameters;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParameters;
import ro.altom.altunitytester.Commands.FindObject.AltGetAllElementsParameters;
import ro.altom.altunitytester.Commands.FindObject.AltWaitForObjectsParameters;
import ro.altom.altunitytester.Commands.InputActions.AltHoldParameters;
import ro.altom.altunitytester.Commands.InputActions.AltKeyParameters;
import ro.altom.altunitytester.Commands.InputActions.AltMoveMouseParameters;
import ro.altom.altunitytester.Commands.InputActions.AltTapClickCoordinatesParameters;
import ro.altom.altunitytester.Commands.InputActions.AltTiltParameters;
import ro.altom.altunitytester.Commands.ObjectCommand.AltGetComponentPropertyParameters;
import ro.altom.altunitytester.Commands.ObjectCommand.AltSetComponentPropertyParameters;
import ro.altom.altunitytester.Commands.ObjectCommand.AltTapClickElementParameters;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParameters;
import ro.altom.altunitytester.Commands.UnityCommand.AltWaitForCurrentSceneToBeParameters;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;
import ro.altom.altunitytester.altUnityTesterExceptions.*;
import ro.altom.altunitytester.position.Vector2;
import ro.altom.altunitytester.position.Vector3;

import static junit.framework.TestCase.*;
import static org.junit.Assert.assertNotEquals;

import java.lang.Void;

import java.io.File;

public class TestsSampleScene1 {

    private static AltUnityDriver altUnityDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13010, true);
    }

    @AfterClass
    public static void tearDown() throws Exception {
        if (altUnityDriver != null) {
            altUnityDriver.stop();
        }
        Thread.sleep(1000);
    }

    @Before
    public void loadLevel() throws Exception {
        altUnityDriver.loadScene(new AltLoadSceneParameters.Builder("Scene 1 AltUnityDriverTestScene").build());
    }

    @Test
    public void testLodeNonExistentScene() throws Exception {
        try {
            altUnityDriver.loadScene(new AltLoadSceneParameters.Builder("Scene 0").build());
            assertTrue(false);
        } catch (SceneNotFoundException e) {
            assertTrue(true);
        }
    }

    @Test
    public void testGetCurrentScene() throws Exception {
        assertEquals("Scene 1 AltUnityDriverTestScene", altUnityDriver.getCurrentScene());
    }

    @Test
    public void testFindElement() throws Exception {
        String name = "Capsule";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                name).build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        assertEquals(name, altElement.name);
    }

    @Test
    public void testFindElements() throws Exception {
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
        AltUnityObject[] altElements = altUnityDriver.findObjectsWhichContain(altFindObjectsParameters);
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
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                name).build();
        altFindObjectsParameters.setEnabled(false);
        AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                altFindObjectsParameters).build();
        AltUnityObject altElement = altUnityDriver.waitForObject(altWaitForObjectsParameters);
        long timeEnd = System.currentTimeMillis();
        long time = timeEnd - timeStart;
        assertTrue(time / 1000 < 20);
        assertNotNull(altElement);
        assertEquals(altElement.name, name);
    }

    @Test(expected = WaitTimeOutException.class)
    public void testWaitForNonExistingElement() {
        String name = "Capsulee";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                name).build();

        AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                altFindObjectsParameters).withTimeout(1).build();
        altUnityDriver.waitForObject(altWaitForObjectsParameters);
    }

    @Test
    public void testWaitForCurrentSceneToBe() throws Exception {
        String name = "Scene 1 AltUnityDriverTestScene";
        long timeStart = System.currentTimeMillis();
        AltWaitForCurrentSceneToBeParameters params = new AltWaitForCurrentSceneToBeParameters.Builder(name).build();
        String currentScene = altUnityDriver.waitForCurrentSceneToBe(params);
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
            AltWaitForCurrentSceneToBeParameters params = new AltWaitForCurrentSceneToBeParameters.Builder(name)
                    .withTimeout(1).build();
            altUnityDriver.waitForCurrentSceneToBe(params);
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

    @Test(expected = WaitTimeOutException.class)
    public void testWaitForNonExistingElementWhereNameContains() {
        String name = "xyz";
        AltFindObjectsParameters findObjectsParams = new AltFindObjectsParameters.Builder(By.NAME, name).build();
        AltWaitForObjectsParameters params = new AltWaitForObjectsParameters.Builder(findObjectsParams).withTimeout(1)
                .build();

        altUnityDriver.waitForObjectWhichContains(params);
    }

    @Test
    public void testFindElementWithText() throws Exception {
        String name = "CapsuleInfo";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                name).build();

        String text = altUnityDriver.findObject(altFindObjectsParameters).getText();

        altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.TEXT, text).build();

        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);

        assertNotNull(altElement);
        assertEquals(altElement.getText(), text);
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
        String componentName = "Altom.AltUnityTester.AltUnityRunner";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.COMPONENT, componentName).build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        assertEquals(altElement.name, "AltUnityRunnerPrefab");
    }

    @Test
    public void testGetComponentProperty() throws Exception {
        Thread.sleep(1000);
        String componentName = "Altom.AltUnityTester.AltUnityRunner";
        String propertyName = "InstrumentationSettings.ProxyPort";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "AltUnityRunnerPrefab").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);

        String propertyValue = altElement.getComponentProperty(
                new AltGetComponentPropertyParameters.Builder(componentName,
                        propertyName).build(),
                String.class);
        assertEquals(propertyValue, "13010");
    }

    @Test
    public void testGetComponentPropertyInvalidDeserialization() {
        String componentName = "Altom.AltUnityTester.AltUnityRunner";
        String propertyName = "InstrumentationSettings.ShowPopUp";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "AltUnityRunnerPrefab").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        try {
            altElement.getComponentProperty(
                    new AltGetComponentPropertyParameters.Builder(componentName,
                            propertyName).build(),
                    int.class);
            fail("Expected ResponseFormatException");
        } catch (ResponseFormatException ex) {
            assertEquals("Could not deserialize response data: `true` into int",
                    ex.getMessage());
        }
    }

    @Test(expected = PropertyNotFoundException.class)
    public void testGetNonExistingComponentProperty() throws Exception {
        Thread.sleep(1000);
        String componentName = "Altom.AltUnityTester.AltUnityRunner";
        String propertyName = "socketPort";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "AltUnityRunnerPrefab").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        altElement.getComponentProperty(
                new AltGetComponentPropertyParameters.Builder(componentName,
                        propertyName).build(),
                String.class);
    }

    @Test
    public void testGetComponentPropertyArray() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String propertyName = "arrayOfInts";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        int[] propertyValue = altElement.getComponentProperty(
                new AltGetComponentPropertyParameters.Builder(componentName,
                        propertyName).build(),
                int[].class);
        assertEquals(3, propertyValue.length);
        assertEquals(1, propertyValue[0]);
        assertEquals(2, propertyValue[1]);
        assertEquals(3, propertyValue[2]);
    }

    @Test
    public void testGetComponentPropertyUnityEngine() throws Exception {
        String componentName = "UnityEngine.CapsuleCollider";
        String propertyName = "isTrigger";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        boolean propertyValue = altElement.getComponentProperty(
                new AltGetComponentPropertyParameters.Builder(componentName,
                        propertyName).build(),
                Boolean.class);
        assertEquals(false, propertyValue);
    }

    @Test
    public void testSetComponentProperty() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String propertyName = "stringToSetFromTests";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        altElement.setComponentProperty(
                new AltSetComponentPropertyParameters.Builder(componentName, propertyName,
                        "2").build());
        int propertyValue = altElement.getComponentProperty(
                new AltGetComponentPropertyParameters.Builder(componentName,
                        propertyName).build(),
                int.class);
        assertEquals(2, propertyValue);
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
            altElement.setComponentProperty(
                    new AltSetComponentPropertyParameters.Builder(componentName, propertyName,
                            "2").build());
            fail();
        } catch (ComponentNotFoundException e) {
            assertTrue(e.getMessage(), e.getMessage().startsWith("Component not found"));
        }
    }

    @Test
    public void testCallMethodWithNoParameters() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "UIButtonClicked";
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNull(altElement.callComponentMethod(componentName, methodName, new Object[] {}, Void.class));
    }

    @Test
    public void testCallMethodWithParameters() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "Jump";
        String[] parameters = new String[] { "New Text" };
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNull(altElement.callComponentMethod(componentName, methodName,
                parameters, Void.class));
    }

    @Test
    public void testCallMethodWithManyParameters() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "TestMethodWithManyParameters";
        Object[] parameters = new Object[] { 1, "stringparam", 0.5, new int[] { 1, 2,
                3 } };
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNull(altElement.callComponentMethod(componentName, methodName,
                parameters, Void.class));
    }

    @Test(expected = MethodWithGivenParametersNotFoundException.class)
    public void testCallMethodWithIncorrectNumberOfParameters() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "TestMethodWithManyParameters";
        Object[] parameters = new Object[] { 1, "stringparam", new int[] { 1, 2, 3 }
        };
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        altElement.callComponentMethod(componentName, methodName, parameters,
                Void.class);
    }

    @Test(expected = InvalidParameterTypeException.class)
    public void testCallMethodInvalidParameterType() {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "TestMethodWithManyParameters";
        Object[] parameters = new Object[] { 1, "stringparam", 0.5, new int[] { 1, 2,
                3 } };
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        altElement.callComponentMethod("", componentName, methodName, parameters, new String[] { "System.Stringggggg" },
                Void.class);
    }

    @Test(expected = AssemblyNotFoundException.class)
    public void testCallMethodAssmeblyNotFound() {
        String componentName = "RandomComponent";
        String methodName = "TestMethodWithManyParameters";
        Object[] parameters = new Object[] { 'a', "stringparam", 0.5, new int[] { 1,
                2, 3 } };
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        altElement.callComponentMethod("RandomAssembly", componentName, methodName,
                parameters, new String[] {},
                Void.class);
    }

    @Test(expected = MethodWithGivenParametersNotFoundException.class)
    public void testCallMethodWithIncorrectNumberOfParameters2() {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "TestMethodWithManyParameters";
        Object[] parameters = new Object[] { 'a', "stringparam", new int[] { 1, 2, 3
        } };
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        altElement.callComponentMethod("", componentName, methodName, parameters, new String[] {}, Void.class);
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
            assertTrue(e.getMessage(), e.getMessage().startsWith("PlayerPrefs key test not found"));
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
        altButton.click(new AltTapClickElementParameters.Builder().build());
        altButton.click(new AltTapClickElementParameters.Builder().build());
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
            assertTrue(e.getMessage(), e.getMessage().startsWith("Object //NonExistent not found"));
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
            assertTrue(e.getMessage(), e.getMessage().startsWith("Object //NonExistent not found"));
        }
    }

    @Test
    public void testButtonClickWithSwipe() throws Exception {
        AltUnityObject button = altUnityDriver
                .findObject(new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                        "UIButton").build());
        altUnityDriver.holdButton(new AltHoldParameters.Builder(button.getScreenPosition()).withDuration(1).build());
        AltUnityObject capsuleInfo = altUnityDriver
                .findObject(new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                        "CapsuleInfo").build());
        String text = capsuleInfo.getText();
        assertEquals(text, "UIButton clicked to jump capsule!");
    }

    @Test
    public void testButtonTap() throws Exception {
        AltFindObjectsParameters params = new AltFindObjectsParameters.Builder(By.NAME, "UIButton").build();
        AltTapClickElementParameters param2 = new AltTapClickElementParameters.Builder().build();
        altUnityDriver.findObject(params).tap(param2);

        params = new AltFindObjectsParameters.Builder(By.NAME,
                "CapsuleInfo").build();
        AltUnityObject capsuleInfo = altUnityDriver.findObject(params);

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
    public void TestCallStaticMethod() throws Exception {
        altUnityDriver.callStaticMethod(new AltCallStaticMethodParameters.Builder("UnityEngine.PlayerPrefs", "SetInt",
                new Object[] { "Test", "1" }).build(), String.class);
        int a = altUnityDriver.callStaticMethod(new AltCallStaticMethodParameters.Builder("UnityEngine.PlayerPrefs",
                "GetInt", new Object[] { "Test", "2" }).build(), Integer.class);
        assertEquals(1, a);
    }

    @Test
    public void TestCallMethodWithMultipleDefinitions() throws Exception {

        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Capsule").build();
        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "CapsuleInfo").build();
        AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters1);
        capsule.callComponentMethod("", "AltUnityExampleScriptCapsule", "Test", new Object[] { "2" },
                new String[] { "System.Int32" }, Void.class);
        AltUnityObject capsuleInfo = altUnityDriver.findObject(altFindObjectsParameters2);
        assertEquals("6", capsuleInfo.getText());
    }

    @Test
    public void TestGetSetTimeScale() throws InterruptedException {
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
        AltUnityRotation initialRotation = capsule.getComponentProperty(
                new AltGetComponentPropertyParameters.Builder("UnityEngine.Transform",
                        "rotation").build(),
                AltUnityRotation.class);

        capsule.callComponentMethod("UnityEngine.CoreModule",
                "UnityEngine.Transform", "Rotate",
                new Object[] { 10, 10, 10 }, new String[] {}, Void.class);
        AltUnityObject capsuleAfterRotation = altUnityDriver.findObject(altFindObjectsParameters1);
        AltUnityRotation finalRotation = capsuleAfterRotation.getComponentProperty(
                new AltGetComponentPropertyParameters.Builder("UnityEngine.Transform",
                        "rotation").build(),
                AltUnityRotation.class);
        assertTrue("Rotation should be distinct",
                initialRotation.x != finalRotation.x || initialRotation.y != finalRotation.y
                        || initialRotation.z != finalRotation.z || initialRotation.w != finalRotation.w);
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
            assertTrue(e.getMessage(), e.getMessage().startsWith("Object //ObjectDestroyedIn5Secs not found"));
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
            assertTrue(e.getMessage(), e.getMessage().startsWith("Object //Capsulee not found"));
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
        altUnityDriver
                .tilt(new AltTiltParameters.Builder(new Vector3(1, 1,
                        1)).withDuration(1).withWait(false).build());
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
        altUnityDriver.tilt(new AltTiltParameters.Builder(new Vector3(1, 1,
                1)).withDuration(1).build());
        capsule = altUnityDriver.findObject(altFindObjectsParameters1);
        Vector3 afterTiltCoordinates = capsule.getWorldPosition();
        assertNotEquals(initialWorldCoordinates, afterTiltCoordinates);
    }

    public void TestFindObjectWithCameraId() {
        AltFindObjectsParameters altFindObjectsParametersButton = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.PATH, "//Button").build();
        AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
        altButton.click(new AltTapClickElementParameters.Builder().build());
        altButton.click(new AltTapClickElementParameters.Builder().build());
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
        assertNotEquals(altElement.getScreenPosition(),
                altElement2.getScreenPosition());
    }

    @Test
    public void TestWaitForObjectWithCameraId() {
        AltFindObjectsParameters altFindObjectsParametersButton = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.PATH, "//Button").build();
        AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
        altButton.click(new AltTapClickElementParameters.Builder().build());
        altButton.click(new AltTapClickElementParameters.Builder().build());
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

        assertNotEquals(altElement.getScreenPosition(),
                altElement2.getScreenPosition());
    }

    @Test
    public void TestFindObjectsWithCameraId() {
        AltFindObjectsParameters altFindObjectsParametersButton = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.PATH, "//Button").build();
        AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
        altButton.click(new AltTapClickElementParameters.Builder().build());
        altButton.click(new AltTapClickElementParameters.Builder().build());
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

        assertNotEquals(altElement[0].getScreenPosition(),
                altElement2[0].getScreenPosition());
    }

    @Test
    public void TestWaitForObjectNotBePresentWithCameraId() {
        AltFindObjectsParameters altFindObjectsParametersCamera = new AltFindObjectsParameters.Builder(By.PATH,
                "//Main Camera").build();
        AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);

        AltFindObjectsParameters altFindObjectsParametersObject = new AltFindObjectsParameters.Builder(By.NAME,
                "ObjectDestroyedIn5Secs").withCamera(By.ID,
                        String.valueOf(camera.id)).build();
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
        altButton.click(new AltTapClickElementParameters.Builder().build());
        altButton.click(new AltTapClickElementParameters.Builder().build());
        AltFindObjectsParameters altFindObjectsParametersCampsule = new AltFindObjectsParameters.Builder(By.COMPONENT,
                "CapsuleCollider").withCamera(By.TAG, "MainCamera").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParametersCampsule);

        assertTrue("True", altElement.name.equals("Capsule"));

        altFindObjectsParametersCampsule = new AltFindObjectsParameters.Builder(By.COMPONENT, "CapsuleCollider")
                .withCamera(By.TAG, "Untagged").build();
        AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParametersCampsule);
        assertNotEquals(altElement.getScreenPosition(),
                altElement2.getScreenPosition());
    }

    @Test
    public void TestWaitForObjectWithTag() {
        AltFindObjectsParameters altFindObjectsParametersButton = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.PATH, "//Button").build();
        AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
        altButton.click(new AltTapClickElementParameters.Builder().build());
        altButton.click(new AltTapClickElementParameters.Builder().build());
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

        assertNotEquals(altElement.getScreenPosition(),
                altElement2.getScreenPosition());
    }

    @Test
    public void TestFindObjectsWithTag() {
        AltFindObjectsParameters altFindObjectsParametersButton = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.PATH, "//Button").build();
        AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
        altButton.click(new AltTapClickElementParameters.Builder().build());
        altButton.click(new AltTapClickElementParameters.Builder().build());
        AltFindObjectsParameters altFindObjectsParametersCapsule = new AltFindObjectsParameters.Builder(By.NAME,
                "Plane").withCamera(By.TAG, "MainCamera").build();

        AltUnityObject[] altElement = altUnityDriver.findObjects(altFindObjectsParametersCapsule);

        assertTrue("True", altElement[0].name.equals("Plane"));

        altFindObjectsParametersCapsule = new AltFindObjectsParameters.Builder(By.NAME, "Plane")
                .withCamera(By.TAG, "Untagged").build();
        AltUnityObject[] altElement2 = altUnityDriver.findObjects(altFindObjectsParametersCapsule);

        assertNotEquals(altElement[0].getScreenPosition(),
                altElement2[0].getScreenPosition());
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
        AltGetAllElementsParameters altGetAllElementsParameters = new AltGetAllElementsParameters.Builder().build();
        AltUnityObject[] initialNumberOfElements = altUnityDriver.getAllElements(altGetAllElementsParameters);

        AltLoadSceneParameters altLoadSceneParameters = new AltLoadSceneParameters.Builder("Scene 2 Draggable Panel")
                .loadMode(false).build();
        altUnityDriver.loadScene(altLoadSceneParameters);
        AltUnityObject[] finalNumberOfElements = altUnityDriver.getAllElements(altGetAllElementsParameters);

        assertNotEquals(initialNumberOfElements, finalNumberOfElements);

        String[] scenes = altUnityDriver.getAllLoadedScenes();
        assertEquals(2, scenes.length);
    }

    @Test
    public void TestGetComponentPropertyComplexClass() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String propertyName = "AltUnitySampleClass.testInt";
        AltGetComponentPropertyParameters altGetComponentPropertyParameters = new AltGetComponentPropertyParameters.Builder(
                componentName, propertyName).build();
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(By.NAME, "Capsule")
                .build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        int propertyValue = altElement.getComponentProperty(altGetComponentPropertyParameters,
                int.class);
        assertEquals(1, propertyValue);
    }

    @Test
    public void TestGetComponentPropertyComplexClass2() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String propertyName = "listOfSampleClass[1].testString";
        AltGetComponentPropertyParameters altGetComponentPropertyParameters = new AltGetComponentPropertyParameters.Builder(
                componentName, propertyName).build();
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(By.NAME, "Capsule")
                .build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        String propertyValue = altElement.getComponentProperty(altGetComponentPropertyParameters,
                String.class);
        assertEquals("test2", propertyValue);
    }

    @Test
    public void TestSetComponentPropertyComplexClass() {
        String componentName = "AltUnityExampleScriptCapsule";
        String propertyName = "AltUnitySampleClass.testInt";
        AltGetComponentPropertyParameters altGetComponentPropertyParameters = new AltGetComponentPropertyParameters.Builder(
                componentName, propertyName).withMaxDepth(1).build();
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(By.NAME, "Capsule")
                .build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        AltSetComponentPropertyParameters altSetComponentPropertyParameters = new AltSetComponentPropertyParameters.Builder(
                componentName, propertyName, 2).build();
        altElement.setComponentProperty(altSetComponentPropertyParameters);
        int propertyValue = altElement.getComponentProperty(altGetComponentPropertyParameters,
                int.class);
        assertEquals(2, propertyValue);
    }

    @Test
    public void TestSetComponentPropertyComplexClass2() {

        String componentName = "AltUnityExampleScriptCapsule";
        String propertyName = "listOfSampleClass[1].testString";
        AltGetComponentPropertyParameters altGetComponentPropertyParameters = new AltGetComponentPropertyParameters.Builder(
                componentName, propertyName).withMaxDepth(1).build();
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(By.NAME, "Capsule")
                .build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        assertNotNull(altElement);
        AltSetComponentPropertyParameters altSetComponentPropertyParameters = new AltSetComponentPropertyParameters.Builder(
                componentName, propertyName, "test3").build();
        altElement.setComponentProperty(altSetComponentPropertyParameters);
        String propertyValue = altElement.getComponentProperty(altGetComponentPropertyParameters,
                String.class);
        assertEquals("test3", propertyValue);
    }

    @Test
    public void TestGetServerVersion() {
        String serverVersion = altUnityDriver.getServerVersion();
        assertEquals(serverVersion, AltUnityDriver.VERSION);
    }

    @Test
    public void TestGetParent() {
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(By.NAME, "CapsuleInfo")
                .build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        AltUnityObject altElementParent = altElement.getParent();
        assertEquals("Canvas", altElementParent.name);
    }

    @Test
    public void TestUnloadScene() {
        AltLoadSceneParameters altLoadSceneParameters = new AltLoadSceneParameters.Builder("Scene 2 Draggable Panel")
                .loadMode(false).build();
        altUnityDriver.loadScene(altLoadSceneParameters);
        assertEquals(2, altUnityDriver.getAllLoadedScenes().length);
        altUnityDriver.unloadScene("Scene 2 Draggable Panel");
        assertEquals(1, altUnityDriver.getAllLoadedScenes().length);
        assertEquals("Scene 1 AltUnityDriverTestScene",
                altUnityDriver.getAllLoadedScenes()[0]);
    }

    @Test(expected = CouldNotPerformOperationException.class)
    public void TestUnloadOnlyScene() {
        altUnityDriver.unloadScene("Scene 1 AltUnityDriverTestScene");
    }

    @Test(expected = InvalidPathException.class)
    public void TestInvalidPath() {
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(By.PATH, "//[1]")
                .build();
        altUnityDriver.findObject(altFindObjectsParameters);
    }

    @Test(expected = InvalidPathException.class)
    public void TestInvalidPath2() {
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(By.PATH,
                "CapsuleInfo[@tag=UI]").build();
        altUnityDriver.findObject(altFindObjectsParameters);
    }

    @Test(expected = InvalidPathException.class)
    public void TestInvalidPath3() {
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(By.PATH,
                "//CapsuleInfo[@tag=UI/Text").build();
        altUnityDriver.findObject(altFindObjectsParameters);
    }

    @Test(expected = InvalidPathException.class)
    public void TestInvalidPath4() {
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(By.PATH,
                "//CapsuleInfo[0/Text").build();
        altUnityDriver.findObject(altFindObjectsParameters);
    }

    @Test()
    public void TestTapCoordinates() {
        AltFindObjectsParameters findCapsuleParameters = new AltFindObjectsParameters.Builder(By.NAME, "Capsule")
                .build();
        AltUnityObject capsule = altUnityDriver.findObject(findCapsuleParameters);
        AltTapClickCoordinatesParameters tapParameters = new AltTapClickCoordinatesParameters.Builder(
                capsule.getScreenPosition()).build();
        altUnityDriver.tap(tapParameters);

        AltFindObjectsParameters findCapsuleInfoParameters = new AltFindObjectsParameters.Builder(By.PATH,
                "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
        AltWaitForObjectsParameters waitParams = new AltWaitForObjectsParameters.Builder(findCapsuleInfoParameters)
                .build();
        altUnityDriver.waitForObject(waitParams);
    }

    @Test()
    public void TestClickCoordinates() {
        AltFindObjectsParameters findCapsuleParameters = new AltFindObjectsParameters.Builder(By.NAME, "Capsule")
                .build();
        AltUnityObject capsule = altUnityDriver.findObject(findCapsuleParameters);
        AltTapClickCoordinatesParameters clickParameters = new AltTapClickCoordinatesParameters.Builder(
                capsule.getScreenPosition()).build();
        altUnityDriver.click(clickParameters);

        AltFindObjectsParameters findCapsuleInfoParameters = new AltFindObjectsParameters.Builder(By.PATH,
                "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
        AltWaitForObjectsParameters waitParams = new AltWaitForObjectsParameters.Builder(findCapsuleInfoParameters)
                .build();
        altUnityDriver.waitForObject(waitParams);
    }

    @Test()
    public void TestTapElement() {
        AltFindObjectsParameters findCapsuleParameters = new AltFindObjectsParameters.Builder(By.NAME, "Capsule")
                .build();
        AltUnityObject capsule = altUnityDriver.findObject(findCapsuleParameters);

        AltTapClickElementParameters tapParameters = new AltTapClickElementParameters.Builder().build();
        capsule.tap(tapParameters);

        AltFindObjectsParameters findCapsuleInfoParameters = new AltFindObjectsParameters.Builder(By.PATH,
                "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
        AltWaitForObjectsParameters waitParams = new AltWaitForObjectsParameters.Builder(findCapsuleInfoParameters)
                .build();
        altUnityDriver.waitForObject(waitParams);
    }

    @Test()
    public void TestClickElement() {
        AltFindObjectsParameters findCapsuleParameters = new AltFindObjectsParameters.Builder(By.NAME, "Capsule")
                .build();
        AltUnityObject capsule = altUnityDriver.findObject(findCapsuleParameters);

        AltTapClickElementParameters tapParameters = new AltTapClickElementParameters.Builder().build();
        capsule.click(tapParameters);

        AltFindObjectsParameters findCapsuleInfoParameters = new AltFindObjectsParameters.Builder(By.PATH,
                "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
        AltWaitForObjectsParameters waitParams = new AltWaitForObjectsParameters.Builder(findCapsuleInfoParameters)
                .build();
        altUnityDriver.waitForObject(waitParams);
    }

    @Test()
    public void TestKeyDownAndKeyUpMouse0() throws InterruptedException {
        AltFindObjectsParameters findCapsuleParameters = new AltFindObjectsParameters.Builder(By.NAME, "Capsule")
                .build();
        AltUnityObject capsule = altUnityDriver.findObject(findCapsuleParameters);
        Vector2 initialCapsPos = capsule.getWorldPosition();
        AltMoveMouseParameters altMoveMouseParameters = new AltMoveMouseParameters.Builder(capsule.getScreenPosition())
                .withDuration(0.1f).build();
        altUnityDriver.moveMouse(altMoveMouseParameters);
        Thread.sleep(1000);
        AltKeyParameters altKeyParameters = new AltKeyParameters.Builder(AltUnityKeyCode.Mouse0).build();
        altUnityDriver.keyDown(altKeyParameters);
        altUnityDriver.keyUp(AltUnityKeyCode.Mouse0);
        capsule = altUnityDriver.findObject(findCapsuleParameters);
        Vector2 finalCapsPos = capsule.getWorldPosition();
        assertNotEquals(initialCapsPos, finalCapsPos);
    }

    @Test(expected = CameraNotFoundException.class)
    public void TestCameraNotFoundException() {
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(By.NAME, "Capsule")
                .withCamera(By.NAME, "Camera").build();
        altUnityDriver.findObject(altFindObjectsParameters);

    }

    @Test
    public void testScreenshot() {
        String path = "testJava2.png";
        altUnityDriver.getPNGScreenshot(path);
        assertTrue(new File(path).isFile());
    }

    @Test
    public void testGetStaticProperty() {
        AltCallStaticMethodParameters altCallStaticMethodParameters = new AltCallStaticMethodParameters.Builder(
                "UnityEngine.Screen", "SetResolution", new Object[] { "1920", "1080", "True"
                })
                        .withTypeOfParameters(new String[] { "System.Int32", "System.Int32",
                                "System.Boolean" })
                        .withAssembly("UnityEngine.CoreModule").build();
        altUnityDriver.callStaticMethod(altCallStaticMethodParameters,
                Integer.class);
        AltGetComponentPropertyParameters altGetComponentPropertyParameters = new AltGetComponentPropertyParameters.Builder(
                "UnityEngine.Screen",
                "currentResolution.width").withAssembly("UnityEngine.CoreModule").build();
        int width = altUnityDriver.GetStaticProperty(altGetComponentPropertyParameters,
                Integer.class);
        assertEquals(width, 1920);
    }

    @Test
    public void testGetStaticPropertyInstanceNull() {
        AltCallStaticMethodParameters altCallStaticMethodParameters = new AltCallStaticMethodParameters.Builder(
                "UnityEngine.Screen", "get_width", new Object[] {}).withAssembly("UnityEngine.CoreModule").build();
        int screenWidth = altUnityDriver.callStaticMethod(altCallStaticMethodParameters,
                Integer.class);
        AltGetComponentPropertyParameters altGetComponentPropertyParameters = new AltGetComponentPropertyParameters.Builder(
                "UnityEngine.Screen",
                "width").withAssembly("UnityEngine.CoreModule").build();
        int width = altUnityDriver.GetStaticProperty(altGetComponentPropertyParameters,
                Integer.class);
        assertEquals(screenWidth, width);
    }

    @Test
    public void testSetCommandTimeout() throws Exception {
        String componentName = "AltUnityExampleScriptCapsule";
        String methodName = "JumpWithDelay";
        Object[] parameters = new Object[] {};
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME,
                "Capsule").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
        altUnityDriver.setCommandResponseTimeout(1);
        try {
            altElement.callComponentMethod(componentName, methodName, parameters, Void.class);
            fail("Expected CommandResponseTimeoutException");
        } catch (CommandResponseTimeoutException ex) {

        } finally {
            altUnityDriver.setCommandResponseTimeout(60);
        }
    }
}
