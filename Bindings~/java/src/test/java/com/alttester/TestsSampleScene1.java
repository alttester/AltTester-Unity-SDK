package com.alttester;

import com.google.gson.Gson;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import com.alttester.AltDriver.By;
import com.alttester.Commands.AltCallStaticMethodParams;
import com.alttester.Commands.FindObject.AltFindObjectAtCoordinatesParams;
import com.alttester.Commands.FindObject.AltFindObjectsParams;
import com.alttester.Commands.FindObject.AltGetAllElementsParams;
import com.alttester.Commands.FindObject.AltWaitForObjectsParams;
import com.alttester.Commands.InputActions.AltHoldParams;
import com.alttester.Commands.InputActions.AltKeyDownParams;
import com.alttester.Commands.InputActions.AltKeyUpParams;
import com.alttester.Commands.InputActions.AltKeysDownParams;
import com.alttester.Commands.InputActions.AltKeysUpParams;
import com.alttester.Commands.InputActions.AltMoveMouseParams;
import com.alttester.Commands.InputActions.AltPressKeysParams;
import com.alttester.Commands.InputActions.AltTapClickCoordinatesParams;
import com.alttester.Commands.InputActions.AltTiltParams;
import com.alttester.Commands.ObjectCommand.AltCallComponentMethodParams;
import com.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import com.alttester.Commands.ObjectCommand.AltSetComponentPropertyParams;
import com.alttester.Commands.ObjectCommand.AltTapClickElementParams;
import com.alttester.Commands.ObjectCommand.AltSetTextParams;
import com.alttester.Commands.UnityCommand.AltLoadSceneParams;
import com.alttester.Commands.UnityCommand.AltSetTimeScaleParams;
import com.alttester.Commands.UnityCommand.AltUnloadSceneParams;
import com.alttester.Commands.UnityCommand.AltWaitForCurrentSceneToBeParams;
import com.alttester.UnityStruct.AltKeyCode;
import com.alttester.altTesterExceptions.*;
import com.alttester.position.Vector2;
import com.alttester.position.Vector3;

import static junit.framework.TestCase.*;
import static org.junit.Assert.assertNotEquals;

import java.lang.Void;

import java.io.File;

public class TestsSampleScene1 {

    private static AltDriver altDriver;

    @BeforeClass
    public static void setUp() {
        altDriver = new AltDriver(TestsHelper.GetAltDriverHost(),
                TestsHelper.GetAltDriverPort(),
                true);
    }

    @AfterClass
    public static void tearDown() throws Exception {
        if (altDriver != null) {
            altDriver.stop();
        }
        Thread.sleep(1000);
    }

    @Before
    public void loadLevel() {
        altDriver.resetInput();
        altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 1 AltDriverTestScene").build());
    }

    @Test
    public void testLodeNonExistentScene() {
        try {
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 0").build());
            assertTrue(false);
        } catch (SceneNotFoundException e) {
            assertTrue(true);
        }
    }

    @Test
    public void testGetCurrentScene() {
        assertEquals("Scene 1 AltDriverTestScene", altDriver.getCurrentScene());
    }

    @Test
    public void testFindElement() {
        String name = "Capsule";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                name).build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        assertEquals(name, altElement.name);
    }

    @Test
    public void testFindElements() {
        String name = "Plane";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                name).build();
        AltObject[] altElements = altDriver.findObjects(altFindObjectsParams);
        assertNotNull(altElements);
        assertEquals(altElements[0].name, name);
    }

    @Test
    public void testFindElementWhereNameContains() {

        String name = "Cap";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                name).build();
        AltObject altElement = altDriver.findObjectWhichContains(altFindObjectsParams);
        assertNotNull(altElement);
        assertTrue(altElement.name.contains(name));
    }

    @Test
    public void testFindElementsWhereNameContains() {
        String name = "Pla";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                name).build();
        AltObject[] altElements = altDriver.findObjectsWhichContain(altFindObjectsParams);
        assertNotNull(altElements);
        assertTrue(altElements[0].name.contains(name));
    }

    @Test
    public void testGetAllElements() throws Exception {
        Thread.sleep(1000);
        AltGetAllElementsParams allElementsParams = new AltGetAllElementsParams.Builder().build();
        AltObject[] altElements = altDriver.getAllElements(allElementsParams);
        assertNotNull(altElements);
        String altElementsString = new Gson().toJson(altElements);
        assertTrue(altElementsString.contains("Capsule"));
        assertTrue(altElementsString.contains("Main Camera"));
        assertTrue(altElementsString.contains("Directional Light"));
        assertTrue(altElementsString.contains("Plane"));
        assertTrue(altElementsString.contains("Canvas"));
        assertTrue(altElementsString.contains("EventSystem"));
        assertTrue(altElementsString.contains("AltTesterPrefab"));
        assertTrue(altElementsString.contains("CapsuleInfo"));
        assertTrue(altElementsString.contains("UIButton"));
        assertTrue(altElementsString.contains("Text"));
    }

    @Test
    public void testWaitForExistingElement() {
        String name = "Capsule";
        long timeStart = System.currentTimeMillis();
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                name).build();
        AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                altFindObjectsParams).build();
        AltObject altElement = altDriver.waitForObject(altWaitForObjectsParams);
        long timeEnd = System.currentTimeMillis();
        long time = timeEnd - timeStart;
        assertTrue(time / 1000 < 20);
        assertNotNull(altElement);
        assertEquals(altElement.name, name);
    }

    @Test
    public void testWaitForExistingDisabledElement() {
        String name = "Cube";
        long timeStart = System.currentTimeMillis();
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                name).build();
        altFindObjectsParams.setEnabled(false);
        AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                altFindObjectsParams).build();
        AltObject altElement = altDriver.waitForObject(altWaitForObjectsParams);
        long timeEnd = System.currentTimeMillis();
        long time = timeEnd - timeStart;
        assertTrue(time / 1000 < 20);
        assertNotNull(altElement);
        assertEquals(altElement.name, name);
    }

    @Test(expected = WaitTimeOutException.class)
    public void testWaitForNonExistingElement() {
        String name = "Capsulee";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                name).build();

        AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                altFindObjectsParams).withTimeout(1).build();
        altDriver.waitForObject(altWaitForObjectsParams);
    }

    @Test
    public void testWaitForCurrentSceneToBe() {
        String name = "Scene 1 AltDriverTestScene";
        long timeStart = System.currentTimeMillis();
        AltWaitForCurrentSceneToBeParams params = new AltWaitForCurrentSceneToBeParams.Builder(name).build();
        altDriver.waitForCurrentSceneToBe(params);
        long timeEnd = System.currentTimeMillis();
        long time = timeEnd - timeStart;
        assertTrue(time / 1000 < 20);

        String currentScene = altDriver.getCurrentScene();
        assertEquals(name, currentScene);
    }

    @Test
    public void testWaitForCurrentSceneToBeANonExistingScene() {

        String name = "NonExistentScene";
        try {
            AltWaitForCurrentSceneToBeParams params = new AltWaitForCurrentSceneToBeParams.Builder(name)
                    .withTimeout(1).build();
            altDriver.waitForCurrentSceneToBe(params);
            fail();
        } catch (Exception e) {
            assertEquals(e.getMessage(), "Scene [NonExistentScene] not loaded after 1.0 seconds");
        }
    }

    @Test
    public void testWaitForExistingElementWhereNameContains() {
        String name = "Dir";
        long timeStart = System.currentTimeMillis();
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                name).build();
        AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                altFindObjectsParams).build();
        AltObject altElement = altDriver.waitForObjectWhichContains(altWaitForObjectsParams);
        long timeEnd = System.currentTimeMillis();
        long time = timeEnd - timeStart;
        assertTrue(time / 1000 < 20);
        assertNotNull(altElement);
        assertEquals(altElement.name, "Directional Light");
    }

    @Test(expected = WaitTimeOutException.class)
    public void testWaitForNonExistingElementWhereNameContains() {
        String name = "xyz";
        AltFindObjectsParams findObjectsParams = new AltFindObjectsParams.Builder(By.NAME, name).build();
        AltWaitForObjectsParams params = new AltWaitForObjectsParams.Builder(findObjectsParams).withTimeout(1)
                .build();

        altDriver.waitForObjectWhichContains(params);
    }

    @Test
    public void testFindElementWithText() {
        String name = "CapsuleInfo";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                name).build();

        String text = altDriver.findObject(altFindObjectsParams).getText();

        altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.TEXT, text).build();

        AltObject altElement = altDriver.findObject(altFindObjectsParams);

        assertNotNull(altElement);
        assertEquals(altElement.getText(), text);
    }

    @Test
    public void testFindElementByComponent() throws InterruptedException {
        Thread.sleep(1000);
        String componentName = "AltRunner";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                AltDriver.By.COMPONENT, componentName).build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        assertEquals(altElement.name, "AltTesterPrefab");
    }

    @Test
    public void testFindElementByComponentWithNamespace() throws InterruptedException {
        Thread.sleep(1000);
        String componentName = "Altom.AltTester.AltRunner";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                AltDriver.By.COMPONENT, componentName).build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        assertEquals(altElement.name, "AltTesterPrefab");
    }

    @Test
    public void testGetComponentProperty() throws InterruptedException {
        Thread.sleep(1000);
        String componentName = "Altom.AltTester.AltRunner";
        String propertyName = "InstrumentationSettings.ShowPopUp";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "AltTesterPrefab").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);

        Boolean propertyValue = altElement.getComponentProperty(
                new AltGetComponentPropertyParams.Builder(componentName,
                        propertyName, "AltTester").build(),
                Boolean.class);
        assertTrue(propertyValue);
    }

    @Test
    public void testGetComponentPropertyInvalidDeserialization() {
        String componentName = "Altom.AltTester.AltRunner";
        String propertyName = "InstrumentationSettings.ShowPopUp";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "AltTesterPrefab").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        try {
            altElement.getComponentProperty(
                    new AltGetComponentPropertyParams.Builder(componentName,
                            propertyName,
                            "AltTester").build(),
                    int.class);
            fail("Expected ResponseFormatException");
        } catch (ResponseFormatException ex) {
            assertEquals("Could not deserialize response data: `true` into int",
                    ex.getMessage());
        }
    }

    @Test(expected = PropertyNotFoundException.class)
    public void testGetNonExistingComponentProperty() throws InterruptedException {
        Thread.sleep(1000);
        String componentName = "Altom.AltTester.AltRunner";
        String propertyName = "socketPort";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "AltTesterPrefab").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        altElement.getComponentProperty(
                new AltGetComponentPropertyParams.Builder(componentName,
                        propertyName,
                        "AltTester").build(),
                String.class);
    }

    @Test
    public void testGetComponentPropertyArray() {
        String componentName = "AltExampleScriptCapsule";
        String propertyName = "arrayOfInts";
        String assembly = "Assembly-CSharp";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        int[] propertyValue = altElement.getComponentProperty(
                new AltGetComponentPropertyParams.Builder(componentName,
                        propertyName,
                        assembly).build(),
                int[].class);
        assertEquals(3, propertyValue.length);
        assertEquals(1, propertyValue[0]);
        assertEquals(2, propertyValue[1]);
        assertEquals(3, propertyValue[2]);
    }

    @Test
    public void testGetComponentPropertyUnityEngine() {
        String componentName = "UnityEngine.CapsuleCollider";
        String propertyName = "isTrigger";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        boolean propertyValue = altElement.getComponentProperty(
                new AltGetComponentPropertyParams.Builder(componentName,
                        propertyName, "").build(),
                Boolean.class);
        assertEquals(false, propertyValue);
    }

    @Test
    public void testSetComponentProperty() {
        String componentName = "AltExampleScriptCapsule";
        String propertyName = "stringToSetFromTests";
        String assembly = "Assembly-CSharp";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        altElement.setComponentProperty(
                new AltSetComponentPropertyParams.Builder(componentName, propertyName,
                        assembly, "2").build());
        int propertyValue = altElement.getComponentProperty(
                new AltGetComponentPropertyParams.Builder(componentName,
                        propertyName,
                        assembly).build(),
                int.class);
        assertEquals(2, propertyValue);
    }

    @Test
    public void testSetNonExistingComponentProperty() {
        String componentName = "AltExampleScriptCapsuleNotFound";
        String propertyName = "stringToSetFromTests";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        try {
            altElement.setComponentProperty(
                    new AltSetComponentPropertyParams.Builder(componentName, propertyName, "AltTester",
                            "2").build());
            fail();
        } catch (ComponentNotFoundException e) {
            assertTrue(e.getMessage(), e.getMessage().startsWith("Component not found"));
        }
    }

    @Test
    public void testCallMethodWithNoParameters() {
        String componentName = "AltExampleScriptCapsule";
        String methodName = "UIButtonClicked";
        String assembly = "Assembly-CSharp";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);

        assertNull(altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodName,
                        assembly, new Object[] {})
                        .build(),
                Void.class));
    }

    @Test
    public void testGetTextCallMethodWithNoParameters() {

        String componentName = "UnityEngine.UI.Text";
        String methodName = "get_text";
        String assembly = "UnityEngine.UI";
        String expected_text = "Change Camera Mode";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.PATH,
                "/Canvas/Button/Text").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);

        assertEquals(expected_text, altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodName,
                        assembly, new Object[] {})
                        .build(),
                String.class));
    }

    @Test
    public void TestSetFontSizeCallMethodWithParameters() throws Exception {

        String componentName = "UnityEngine.UI.Text";
        String methodName = "set_fontSize";
        String methodExpectedName = "get_fontSize";
        String assembly = "UnityEngine.UI";
        String[] parameters = new String[] { "16" };
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.PATH,
                "/Canvas/UnityUIInputField/Text").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);

        altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodName, assembly, parameters)
                        .build(),
                Void.class);
        Integer fontSize = altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodExpectedName,
                        assembly,
                        new Object[] {})
                        .build(),
                Integer.class);

        assert (16 == fontSize);
    }

    @Test
    public void testCallMethodWithParameters() throws Exception {
        String componentName = "AltExampleScriptCapsule";
        String methodName = "Jump";
        String assembly = "Assembly-CSharp";
        String[] parameters = new String[] { "New Text" };
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);

        assertNull(altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodName,
                        assembly, parameters)
                        .build(),
                Void.class));
    }

    @Test
    public void testCallMethodWithManyParameters() throws Exception {
        String componentName = "AltExampleScriptCapsule";
        String methodName = "TestMethodWithManyParameters";
        String assembly = "Assembly-CSharp";
        Object[] parameters = new Object[] { 1, "stringparam", 0.5, new int[] { 1, 2,
                3 } };
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNull(altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodName,
                        assembly, parameters)
                        .build(),
                Void.class));
    }

    @Test(expected = MethodWithGivenParametersNotFoundException.class)
    public void testCallMethodWithIncorrectNumberOfParameters() throws Exception {
        String componentName = "AltExampleScriptCapsule";
        String methodName = "TestMethodWithManyParameters";
        String assembly = "Assembly-CSharp";
        Object[] parameters = new Object[] { 1, "stringparam", new int[] { 1, 2, 3 }
        };
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodName,
                        assembly, parameters)
                        .build(),
                Void.class);
    }

    @Test(expected = InvalidParameterTypeException.class)
    public void testCallMethodInvalidParameterType() {
        String componentName = "AltExampleScriptCapsule";
        String methodName = "TestMethodWithManyParameters";
        Object[] parameters = new Object[] { 1, "stringparam", 0.5, new int[] { 1, 2, 3 } };
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);

        altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodName, "", parameters)
                        .withTypeOfParameters(new String[] { "System.Stringggggg" }).build(),
                Void.class);
    }

    @Test(expected = AssemblyNotFoundException.class)
    public void testCallMethodAssmeblyNotFound() {
        String componentName = "RandomComponent";
        String methodName = "TestMethodWithManyParameters";
        Object[] parameters = new Object[] { 'a', "stringparam", 0.5, new int[] { 1,
                2, 3 } };
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);

        altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodName,
                        "RandomAssembly", parameters)
                        .build(),
                Void.class);
    }

    @Test(expected = MethodWithGivenParametersNotFoundException.class)
    public void testCallMethodWithIncorrectNumberOfParameters2() {
        String componentName = "AltExampleScriptCapsule";
        String methodName = "TestMethodWithManyParameters";
        String assembly = "Assembly-CSharp";
        Object[] parameters = new Object[] { 'a', "stringparam", new int[] { 1, 2, 3
        } };
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodName,
                        assembly, parameters)
                        .build(),
                Void.class);
    }

    @Test
    public void testSetKeyInt() throws Exception {
        altDriver.deletePlayerPref();
        altDriver.setKeyPlayerPref("test", 1);
        int val = altDriver.getIntKeyPlayerPref("test");
        assertEquals(1, val);
    }

    @Test
    public void testSetKeyFloat() throws Exception {
        altDriver.deletePlayerPref();
        altDriver.setKeyPlayerPref("test", 1f);
        float val = altDriver.getFloatKeyPlayerPref("test");
        assertEquals(1f, val, 0.01);
    }

    @Test
    public void testSetKeyString() throws Exception {
        altDriver.deletePlayerPref();
        altDriver.setKeyPlayerPref("test", "test");
        String val = altDriver.getStringKeyPlayerPref("test");
        assertEquals("test", val);
    }

    @Test
    public void testDeleteKey() throws Exception {
        altDriver.deletePlayerPref();
        altDriver.setKeyPlayerPref("test", 1);
        int val = altDriver.getIntKeyPlayerPref("test");
        assertEquals(1, val);
        altDriver.deleteKeyPlayerPref("test");
        try {
            altDriver.getIntKeyPlayerPref("test");
            fail();
        } catch (NotFoundException e) {
            assertTrue(e.getMessage(), e.getMessage().startsWith("PlayerPrefs key test not found"));
        }
    }

    @Test
    public void testDifferentCamera() throws Exception {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Button").withCamera(By.NAME, "Main Camera").build();
        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Capsule").withCamera(By.NAME, "Main Camera").build();
        AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Capsule").withCamera(By.NAME, "Camera").build();
        AltObject altButton = altDriver.findObject(altFindObjectsParameters1);
        altButton.click(new AltTapClickElementParams.Builder().build());
        altButton.click(new AltTapClickElementParams.Builder().build());
        AltObject altElement = altDriver.findObject(altFindObjectsParameters2);
        AltObject altElement2 = altDriver.findObject(altFindObjectsParameters3);
        assertNotSame(altElement.x, altElement2.x);
        assertNotSame(altElement.y, altElement2.y);
    }

    @Test
    public void testFindNonExistentObject() throws Exception {
        try {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "NonExistent").build();
            altDriver.findObject(altFindObjectsParameters1);
            fail();
        } catch (NotFoundException e) {
            assertTrue(e.getMessage(), e.getMessage().startsWith("Object //NonExistent not found"));
        }
    }

    @Test
    public void testFindNonExistentObjectByName() throws Exception {
        try {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "NonExistent").build();
            altDriver.findObject(altFindObjectsParameters1);
            fail();
        } catch (NotFoundException e) {
            assertTrue(e.getMessage(), e.getMessage().startsWith("Object //NonExistent not found"));
        }
    }

    @Test
    public void testButtonClickWithSwipe() throws Exception {
        AltObject button = altDriver
                .findObject(new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                        "UIButton").build());
        altDriver.holdButton(
                new AltHoldParams.Builder(button.getScreenPosition()).withDuration(1).build());
        AltObject capsuleInfo = altDriver
                .findObject(new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                        "CapsuleInfo").build());
        String text = capsuleInfo.getText();
        assertEquals(text, "UIButton clicked to jump capsule!");
    }

    @Test
    public void testButtonTap() throws Exception {
        AltFindObjectsParams params = new AltFindObjectsParams.Builder(By.NAME, "UIButton").build();
        AltTapClickElementParams param2 = new AltTapClickElementParams.Builder().build();
        altDriver.findObject(params).tap(param2);

        params = new AltFindObjectsParams.Builder(By.NAME,
                "CapsuleInfo").build();
        AltObject capsuleInfo = altDriver.findObject(params);

        Thread.sleep(2);
        String text = capsuleInfo.getText();
        assertEquals(text, "UIButton clicked to jump capsule!");
    }

    @Test
    public void testCapsuleTap() throws Exception {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Capsule").build();
        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "CapsuleInfo").build();
        altDriver.findObject(altFindObjectsParameters1).tap();
        AltObject capsuleInfo = altDriver.findObject(altFindObjectsParameters2);
        Thread.sleep(2);
        String text = capsuleInfo.getText();
        assertEquals(text, "Capsule was clicked to jump!");
    }

    @Test
    public void TestCallStaticMethod() throws Exception {
        altDriver.callStaticMethod(
                new AltCallStaticMethodParams.Builder("UnityEngine.PlayerPrefs", "SetInt", "",
                        new Object[] { "Test", "1" }).build(),
                String.class);
        int a = altDriver.callStaticMethod(new AltCallStaticMethodParams.Builder("UnityEngine.PlayerPrefs",
                "GetInt", "", new Object[] { "Test", "2" }).build(), Integer.class);
        assertEquals(1, a);
    }

    @Test
    public void TestCallMethodWithMultipleDefinitions() throws Exception {

        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Capsule").build();
        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "CapsuleInfo").build();
        AltObject capsule = altDriver.findObject(altFindObjectsParameters1);
        capsule.callComponentMethod(
                new AltCallComponentMethodParams.Builder("AltExampleScriptCapsule", "Test", "Assembly-CSharp",
                        new Object[] { 2 })
                        .withTypeOfParameters(new String[] { "System.Int32" })
                        .build(),
                Void.class);
        AltObject capsuleInfo = altDriver.findObject(altFindObjectsParameters2);
        assertEquals("6", capsuleInfo.getText());
    }

    @Test
    public void TestGetSetTimeScale() {
        float timescale = 0.1f;
        AltSetTimeScaleParams.Builder builder = new AltSetTimeScaleParams.Builder(timescale);

        altDriver.setTimeScale(builder.build());
        float timeScale = altDriver.getTimeScale();
        assertEquals(0.1f, timeScale, 0);
        altDriver.setTimeScale(new AltSetTimeScaleParams.Builder(1f).build());
    }

    @Test
    public void TestCallMethodWithAssembly() {

        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Capsule").build();
        AltObject capsule = altDriver.findObject(altFindObjectsParameters1);
        AltRotation initialRotation = capsule.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("UnityEngine.Transform",
                        "rotation",
                        "UnityEngine.CoreModule").build(),
                AltRotation.class);

        capsule.callComponentMethod(new AltCallComponentMethodParams.Builder(
                "UnityEngine.Transform", "Rotate",
                "UnityEngine.CoreModule",
                new Object[] { 10, 10, 10 })
                .withTypeOfParameters(new String[] {}).build(),
                Void.class);
        AltObject capsuleAfterRotation = altDriver.findObject(altFindObjectsParameters1);
        AltRotation finalRotation = capsuleAfterRotation.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("UnityEngine.Transform",
                        "rotation", "UnityEngine.CoreModule").build(),
                AltRotation.class);
        assertTrue("Rotation should be distinct",
                initialRotation.x != finalRotation.x || initialRotation.y != finalRotation.y
                        || initialRotation.z != finalRotation.z
                        || initialRotation.w != finalRotation.w);
    }

    @Test
    public void TestWaitForObjectToNotBePresent() {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "ObjectDestroyedIn5Secs").build();
        AltWaitForObjectsParams altWaitForObjectsParameters1 = new AltWaitForObjectsParams.Builder(
                altFindObjectsParameters1).build();
        altDriver.waitForObjectToNotBePresent(altWaitForObjectsParameters1);
        try {
            altDriver.findObject(altFindObjectsParameters1);
            assertFalse("Not found exception should be thrown", true);
        } catch (NotFoundException e) {
            assertTrue(e.getMessage(),
                    e.getMessage().startsWith("Object //ObjectDestroyedIn5Secs not found"));
        }

        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsulee").build();
        AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                altFindObjectsParams).build();
        altDriver.waitForObjectToNotBePresent(altWaitForObjectsParams);
        try {
            altDriver.findObject(altFindObjectsParams);
            assertFalse("Not found exception should be thrown", true);
        } catch (NotFoundException e) {
            assertTrue(e.getMessage(), e.getMessage().startsWith("Object //Capsulee not found"));
        }
    }

    @Test
    public void TestGetChineseLetters() {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "ChineseLetters").build();
        String text = altDriver.findObject(altFindObjectsParameters1).getText();
        assertEquals("哦伊娜哦", text);
    }

    @Test
    public void TestNonEnglishText() {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "NonEnglishText").build();
        String text = altDriver.findObject(altFindObjectsParameters1).getText();
        assertEquals("BJÖRN'S PASS", text);
    }

    @Test
    public void TestPressNextScene() throws InterruptedException {
        String initialScene = altDriver.getCurrentScene();
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "NextScene").build();
        altDriver.findObject(altFindObjectsParameters1).tap();
        String currentScene = altDriver.getCurrentScene();
        assertNotEquals(initialScene, currentScene);
    }

    @Test
    public void TestSetText() {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "NonEnglishText").build();
        AltObject textObject = altDriver.findObject(altFindObjectsParameters1);
        String originalText = textObject.getText();
        String afterText = textObject.setText("ModifiedText").getText();
        assertNotEquals(originalText, afterText);
    }

    @Test
    public void TestSetTextWithSubmit() {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "NonEnglishText").build();
        AltObject textObject = altDriver.findObject(altFindObjectsParameters1);
        String originalText = textObject.getText();

        AltSetTextParams setTextParams = new AltSetTextParams.Builder("ModifiedText").withSubmit(true).build();
        String afterText = textObject.setText(setTextParams).getText();
        assertNotEquals(originalText, afterText);
    }

    @Test
    public void TestFindParentUsingPath() {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.PATH, "//CapsuleInfo/..").build();
        AltObject parent = altDriver.findObject(altFindObjectsParameters1);
        assertEquals("Canvas", parent.name);
    }

    @Test
    public void TestAcceleration() throws InterruptedException {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Capsule").build();
        AltObject capsule = altDriver.findObject(altFindObjectsParameters1);
        Vector3 initialWorldCoordinates = capsule.getWorldPosition();
        altDriver
                .tilt(new AltTiltParams.Builder(new Vector3(1, 1,
                        1)).withDuration(1).withWait(false).build());
        Thread.sleep(1000);
        capsule = altDriver.findObject(altFindObjectsParameters1);
        Vector3 afterTiltCoordinates = capsule.getWorldPosition();
        assertNotEquals(initialWorldCoordinates, afterTiltCoordinates);
    }

    @Test
    public void TestAccelerationAndWait() throws InterruptedException {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Capsule").build();
        AltObject capsule = altDriver.findObject(altFindObjectsParameters1);
        Vector3 initialWorldCoordinates = capsule.getWorldPosition();
        altDriver.tilt(new AltTiltParams.Builder(new Vector3(1, 1,
                1)).withDuration(1).build());
        capsule = altDriver.findObject(altFindObjectsParameters1);
        Vector3 afterTiltCoordinates = capsule.getWorldPosition();
        assertNotEquals(initialWorldCoordinates, afterTiltCoordinates);
    }

    public void TestFindObjectWithCameraId() {
        AltFindObjectsParams altFindObjectsParametersButton = new AltFindObjectsParams.Builder(
                AltDriver.By.PATH, "//Button").build();
        AltObject altButton = altDriver.findObject(altFindObjectsParametersButton);
        altButton.click(new AltTapClickElementParams.Builder().build());
        altButton.click(new AltTapClickElementParams.Builder().build());
        AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH,
                "//Camera").build();
        AltObject camera = altDriver.findObject(altFindObjectsParametersCamera);
        AltFindObjectsParams altFindObjectsParametersCampsule = new AltFindObjectsParams.Builder(By.COMPONENT,
                "CapsuleCollider").withCamera(By.ID, String.valueOf(camera.id)).build();
        AltObject altElement = altDriver.findObject(altFindObjectsParametersCampsule);

        assertTrue("True", altElement.name.equals("Capsule"));

        altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH, "//Main Camera").build();
        AltObject camera2 = altDriver.findObject(altFindObjectsParametersCamera);
        altFindObjectsParametersCampsule = new AltFindObjectsParams.Builder(By.COMPONENT, "CapsuleCollider")
                .withCamera(By.ID, String.valueOf(camera2.id)).build();
        AltObject altElement2 = altDriver.findObject(altFindObjectsParametersCampsule);
        assertNotEquals(altElement.getScreenPosition(),
                altElement2.getScreenPosition());
    }

    @Test
    public void TestWaitForObjectWithCameraId() {
        AltFindObjectsParams altFindObjectsParametersButton = new AltFindObjectsParams.Builder(
                AltDriver.By.PATH, "//Button").build();
        AltObject altButton = altDriver.findObject(altFindObjectsParametersButton);
        altButton.click(new AltTapClickElementParams.Builder().build());
        altButton.click(new AltTapClickElementParams.Builder().build());
        AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH,
                "//Camera").build();
        AltObject camera = altDriver.findObject(altFindObjectsParametersCamera);
        AltFindObjectsParams altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.COMPONENT,
                "CapsuleCollider").withCamera(By.ID, String.valueOf(camera.id)).build();
        AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                altFindObjectsParametersCapsule).build();
        AltObject altElement = altDriver.waitForObject(altWaitForObjectsParams);

        assertTrue("True", altElement.name.equals("Capsule"));

        altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH, "//Main Camera").build();
        AltObject camera2 = altDriver.findObject(altFindObjectsParametersCamera);
        altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.COMPONENT, "CapsuleCollider")
                .withCamera(By.ID, String.valueOf(camera2.id)).build();
        altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(altFindObjectsParametersCapsule).build();
        AltObject altElement2 = altDriver.waitForObject(altWaitForObjectsParams);

        assertNotEquals(altElement.getScreenPosition(), altElement2.getScreenPosition());
    }

    @Test
    public void TestFindObjectsWithCameraId() {
        AltFindObjectsParams altFindObjectsParametersButton = new AltFindObjectsParams.Builder(
                AltDriver.By.PATH, "//Button").build();
        AltObject altButton = altDriver.findObject(altFindObjectsParametersButton);
        altButton.click(new AltTapClickElementParams.Builder().build());
        altButton.click(new AltTapClickElementParams.Builder().build());
        AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH,
                "//Camera").build();
        AltObject camera = altDriver.findObject(altFindObjectsParametersCamera);
        AltFindObjectsParams altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.NAME,
                "Plane").withCamera(By.ID, String.valueOf(camera.id)).build();

        AltObject[] altElement = altDriver.findObjects(altFindObjectsParametersCapsule);

        assertTrue("True", altElement[0].name.equals("Plane"));

        altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH, "//Main Camera").build();
        AltObject camera2 = altDriver.findObject(altFindObjectsParametersCamera);
        altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.NAME, "Plane")
                .withCamera(By.ID, String.valueOf(camera2.id)).build();
        AltObject[] altElement2 = altDriver.findObjects(altFindObjectsParametersCapsule);

        assertNotEquals(altElement[0].getScreenPosition(), altElement2[0].getScreenPosition());
    }

    @Test
    public void TestWaitForObjectNotBePresentWithCameraId() {
        AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH,
                "//Main Camera").build();
        AltObject camera = altDriver.findObject(altFindObjectsParametersCamera);

        AltFindObjectsParams altFindObjectsParametersObject = new AltFindObjectsParams.Builder(By.NAME,
                "ObjectDestroyedIn5Secs").withCamera(By.ID, String.valueOf(camera.id)).build();
        AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                altFindObjectsParametersObject).build();
        altDriver.waitForObjectToNotBePresent(altWaitForObjectsParams);

        AltGetAllElementsParams allElementsParams = new AltGetAllElementsParams.Builder().build();
        AltObject[] allObjectsInTheScene = altDriver.getAllElements(allElementsParams);

        Boolean searchObjectFound = false;
        for (AltObject altObject : allObjectsInTheScene) {
            if (altObject.name.equals("ObjectDestroyedIn5Secs")) {
                searchObjectFound = true;
                break;
            }
        }
        assertFalse(searchObjectFound);
    }

    @Test
    public void TestWaitForObjectWhichContainsWithCameraId() {
        AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH,
                "//Main Camera").build();
        AltObject camera = altDriver.findObject(altFindObjectsParametersCamera);

        AltFindObjectsParams altFindObjectsParametersObject = new AltFindObjectsParams.Builder(By.NAME, "Canva")
                .withCamera(By.ID, String.valueOf(camera.id)).build();
        AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                altFindObjectsParametersObject).build();
        AltObject altElement = altDriver.waitForObjectWhichContains(altWaitForObjectsParams);
        assertEquals("Canvas", altElement.name);

    }

    @Test
    public void TestFindObjectWithTag() {
        AltFindObjectsParams altFindObjectsParametersButton = new AltFindObjectsParams.Builder(
                AltDriver.By.PATH, "//Button").build();
        AltObject altButton = altDriver.findObject(altFindObjectsParametersButton);
        altButton.click(new AltTapClickElementParams.Builder().build());
        altButton.click(new AltTapClickElementParams.Builder().build());
        AltFindObjectsParams altFindObjectsParametersCampsule = new AltFindObjectsParams.Builder(By.COMPONENT,
                "CapsuleCollider").withCamera(By.TAG, "MainCamera").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParametersCampsule);

        assertTrue("True", altElement.name.equals("Capsule"));

        altFindObjectsParametersCampsule = new AltFindObjectsParams.Builder(By.COMPONENT, "CapsuleCollider")
                .withCamera(By.TAG, "Untagged").build();
        AltObject altElement2 = altDriver.findObject(altFindObjectsParametersCampsule);
        assertNotEquals(altElement.getScreenPosition(), altElement2.getScreenPosition());
    }

    @Test
    public void TestWaitForObjectWithTag() {
        AltFindObjectsParams altFindObjectsParametersButton = new AltFindObjectsParams.Builder(
                AltDriver.By.PATH, "//Button").build();
        AltObject altButton = altDriver.findObject(altFindObjectsParametersButton);
        altButton.click(new AltTapClickElementParams.Builder().build());
        altButton.click(new AltTapClickElementParams.Builder().build());
        AltFindObjectsParams altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.COMPONENT,
                "CapsuleCollider").withCamera(By.TAG, "MainCamera").build();
        AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                altFindObjectsParametersCapsule).build();
        AltObject altElement = altDriver.waitForObject(altWaitForObjectsParams);

        assertTrue("True", altElement.name.equals("Capsule"));

        altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.COMPONENT, "CapsuleCollider")
                .withCamera(By.TAG, "Untagged").build();
        altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(altFindObjectsParametersCapsule).build();
        AltObject altElement2 = altDriver.waitForObject(altWaitForObjectsParams);

        assertNotEquals(altElement.getScreenPosition(), altElement2.getScreenPosition());
    }

    @Test
    public void TestFindObjectsWithTag() {
        AltFindObjectsParams altFindObjectsParametersButton = new AltFindObjectsParams.Builder(
                AltDriver.By.PATH, "//Button").build();
        AltObject altButton = altDriver.findObject(altFindObjectsParametersButton);
        altButton.click(new AltTapClickElementParams.Builder().build());
        altButton.click(new AltTapClickElementParams.Builder().build());
        AltFindObjectsParams altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.NAME,
                "Plane").withCamera(By.TAG, "MainCamera").build();

        AltObject[] altElement = altDriver.findObjects(altFindObjectsParametersCapsule);

        assertTrue("True", altElement[0].name.equals("Plane"));

        altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.NAME, "Plane")
                .withCamera(By.TAG, "Untagged").build();
        AltObject[] altElement2 = altDriver.findObjects(altFindObjectsParametersCapsule);

        assertNotEquals(altElement[0].getScreenPosition(), altElement2[0].getScreenPosition());
    }

    @Test
    public void TestWaitForObjectNotBePresentWithTag() {

        AltFindObjectsParams altFindObjectsParametersObject = new AltFindObjectsParams.Builder(By.NAME,
                "ObjectDestroyedIn5Secs").withCamera(By.TAG, "MainCamera").build();
        AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                altFindObjectsParametersObject).build();
        altDriver.waitForObjectToNotBePresent(altWaitForObjectsParams);

        AltGetAllElementsParams allElementsParams = new AltGetAllElementsParams.Builder().build();
        AltObject[] allObjectsInTheScene = altDriver.getAllElements(allElementsParams);

        Boolean searchObjectFound = false;
        for (AltObject altObject : allObjectsInTheScene) {
            if (altObject.name.equals("ObjectDestroyedIn5Secs")) {
                searchObjectFound = true;
                break;
            }
        }
        assertFalse(searchObjectFound);
    }

    @Test
    public void TestWaitForObjectWhichContainsWithTag() {

        AltFindObjectsParams altFindObjectsParametersObject = new AltFindObjectsParams.Builder(By.NAME, "Canva")
                .withCamera(By.TAG, "MainCamera").build();
        AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                altFindObjectsParametersObject).build();
        AltObject altElement = altDriver.waitForObjectWhichContains(altWaitForObjectsParams);
        assertEquals("Canvas", altElement.name);

    }

    @Test
    public void TestLoadAdditiveScenes() throws Exception {
        AltGetAllElementsParams altGetAllElementsParams = new AltGetAllElementsParams.Builder().build();
        AltObject[] initialNumberOfElements = altDriver.getAllElements(altGetAllElementsParams);

        AltLoadSceneParams altLoadSceneParams = new AltLoadSceneParams.Builder("Scene 2 Draggable Panel")
                .loadSingle(false).build();
        altDriver.loadScene(altLoadSceneParams);
        AltObject[] finalNumberOfElements = altDriver.getAllElements(altGetAllElementsParams);

        assertNotEquals(initialNumberOfElements, finalNumberOfElements);

        String[] scenes = altDriver.getAllLoadedScenes();
        assertEquals(2, scenes.length);
    }

    @Test
    public void TestGetComponentPropertyComplexClass() throws Exception {
        String componentName = "AltExampleScriptCapsule";
        String propertyName = "AltSampleClass.testInt";
        String assembly = "Assembly-CSharp";
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                componentName, propertyName, assembly).build();
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                .build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        int propertyValue = altElement.getComponentProperty(altGetComponentPropertyParams, int.class);
        assertEquals(1, propertyValue);
    }

    @Test
    public void TestGetComponentPropertyComplexClass2() throws Exception {
        String componentName = "AltExampleScriptCapsule";
        String propertyName = "listOfSampleClass[1].testString";
        String assembly = "Assembly-CSharp";
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                componentName, propertyName, assembly).build();
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                .build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        String propertyValue = altElement.getComponentProperty(altGetComponentPropertyParams, String.class);
        assertEquals("test2", propertyValue);
    }

    @Test
    public void TestSetComponentPropertyComplexClass() {
        String componentName = "AltExampleScriptCapsule";
        String propertyName = "AltSampleClass.testInt";
        String assembly = "Assembly-CSharp";
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                componentName, propertyName, assembly).withMaxDepth(1).build();
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                .build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        AltSetComponentPropertyParams altSetComponentPropertyParams = new AltSetComponentPropertyParams.Builder(
                componentName, propertyName, assembly, 2).build();
        altElement.setComponentProperty(altSetComponentPropertyParams);
        int propertyValue = altElement.getComponentProperty(altGetComponentPropertyParams, int.class);
        assertEquals(2, propertyValue);
    }

    @Test
    public void TestSetComponentPropertyComplexClass2() {

        String componentName = "AltExampleScriptCapsule";
        String propertyName = "listOfSampleClass[1].testString";
        String assembly = "Assembly-CSharp";
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                componentName, propertyName, assembly).withMaxDepth(1).build();
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                .build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        AltSetComponentPropertyParams altSetComponentPropertyParams = new AltSetComponentPropertyParams.Builder(
                componentName, propertyName, assembly, "test3").build();
        altElement.setComponentProperty(altSetComponentPropertyParams);
        String propertyValue = altElement.getComponentProperty(altGetComponentPropertyParams, String.class);
        assertEquals("test3", propertyValue);
    }

    @Test
    public void TestGetServerVersion() {
        String serverVersion = altDriver.getServerVersion();
        assertEquals(serverVersion, AltDriver.VERSION);
    }

    @Test
    public void TestGetParent() {
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.NAME, "CapsuleInfo")
                .build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        AltObject altElementParent = altElement.getParent();
        assertEquals("Canvas", altElementParent.name);
    }

    @Test
    public void TestUnloadScene() {
        AltLoadSceneParams altLoadSceneParams = new AltLoadSceneParams.Builder("Scene 2 Draggable Panel")
                .loadSingle(false).build();
        altDriver.loadScene(altLoadSceneParams);
        assertEquals(2, altDriver.getAllLoadedScenes().length);
        altDriver.unloadScene(new AltUnloadSceneParams.Builder("Scene 2 Draggable Panel").build());

        assertEquals(1, altDriver.getAllLoadedScenes().length);
        assertEquals("Scene 1 AltDriverTestScene", altDriver.getAllLoadedScenes()[0]);
    }

    @Test(expected = CouldNotPerformOperationException.class)
    public void TestUnloadOnlyScene() {
        altDriver.unloadScene(new AltUnloadSceneParams.Builder("Scene 1 AltDriverTestScene").build());
    }

    @Test(expected = InvalidPathException.class)
    public void TestInvalidPath() {
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.PATH, "//[1]")
                .build();
        altDriver.findObject(altFindObjectsParams);
    }

    @Test(expected = InvalidPathException.class)
    public void TestInvalidPath2() {
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.PATH,
                "CapsuleInfo[@tag=UI]").build();
        altDriver.findObject(altFindObjectsParams);
    }

    @Test(expected = InvalidPathException.class)
    public void TestInvalidPath3() {
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.PATH,
                "//CapsuleInfo[@tag=UI/Text").build();
        altDriver.findObject(altFindObjectsParams);
    }

    @Test(expected = InvalidPathException.class)
    public void TestInvalidPath4() {
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.PATH,
                "//CapsuleInfo[0/Text").build();
        altDriver.findObject(altFindObjectsParams);
    }

    @Test()
    public void TestTapCoordinates() {
        AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                .build();
        AltObject capsule = altDriver.findObject(findCapsuleParams);
        AltTapClickCoordinatesParams tapParams = new AltTapClickCoordinatesParams.Builder(
                capsule.getScreenPosition()).build();
        altDriver.tap(tapParams);

        AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
        AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                .build();
        altDriver.waitForObject(waitParams);
    }

    @Test()
    public void TestClickCoordinates() {
        AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                .build();
        AltObject capsule = altDriver.findObject(findCapsuleParams);
        AltTapClickCoordinatesParams clickParams = new AltTapClickCoordinatesParams.Builder(
                capsule.getScreenPosition()).build();
        altDriver.click(clickParams);

        AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
        AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                .build();
        altDriver.waitForObject(waitParams);
    }

    @Test()
    public void TestTapElement() {
        AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                .build();
        AltObject capsule = altDriver.findObject(findCapsuleParams);

        AltTapClickElementParams tapParams = new AltTapClickElementParams.Builder().build();
        capsule.tap(tapParams);

        AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
        AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                .build();
        altDriver.waitForObject(waitParams);
    }

    @Test()
    public void TestClickElement() {
        AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                .build();
        AltObject capsule = altDriver.findObject(findCapsuleParams);

        AltTapClickElementParams tapParams = new AltTapClickElementParams.Builder().build();
        capsule.click(tapParams);

        AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
        AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                .build();
        altDriver.waitForObject(waitParams);
    }

    @Test()
    public void TestKeyDownAndKeyUpMouse0() throws InterruptedException {
        AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                .build();
        AltObject capsule = altDriver.findObject(findCapsuleParams);
        Vector2 initialCapsPos = capsule.getWorldPosition();
        AltMoveMouseParams altMoveMouseParams = new AltMoveMouseParams.Builder(capsule.getScreenPosition())
                .withDuration(0.1f).build();
        altDriver.moveMouse(altMoveMouseParams);
        Thread.sleep(1000);
        altDriver.keyDown(new AltKeyDownParams.Builder(AltKeyCode.Mouse0).build());
        altDriver.keyUp(new AltKeyUpParams.Builder(AltKeyCode.Mouse0).build());
        capsule = altDriver.findObject(findCapsuleParams);
        Vector2 finalCapsPos = capsule.getWorldPosition();
        assertNotEquals(initialCapsPos, finalCapsPos);
    }

    @Test(expected = CameraNotFoundException.class)
    public void TestCameraNotFoundException() {
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                .withCamera(By.NAME, "Camera").build();
        altDriver.findObject(altFindObjectsParams);

    }

    @Test
    public void testScreenshot() {
        String path = "testJava2.png";
        altDriver.getPNGScreenshot(path);
        assertTrue(new File(path).isFile());
    }

    @Test
    public void testGetStaticProperty() {
        AltCallStaticMethodParams altCallStaticMethodParams = new AltCallStaticMethodParams.Builder(
                "UnityEngine.Screen", "SetResolution",
                "UnityEngine.CoreModule", new Object[] { "1920", "1080", "True"
                })
                .withTypeOfParameters(new String[] { "System.Int32", "System.Int32",
                        "System.Boolean" })
                .build();
        altDriver.callStaticMethod(altCallStaticMethodParams,
                Void.class);
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "UnityEngine.Screen",
                "currentResolution.width", "UnityEngine.CoreModule").build();
        int width = altDriver.getStaticProperty(altGetComponentPropertyParams,
                Integer.class);
        assertEquals(width, 1920);
    }

    @Test
    public void testGetStaticPropertyInstanceNull() {
        AltCallStaticMethodParams altCallStaticMethodParams = new AltCallStaticMethodParams.Builder(
                "UnityEngine.Screen", "get_width",
                "UnityEngine.CoreModule", new Object[] {})
                .build();
        int screenWidth = altDriver.callStaticMethod(altCallStaticMethodParams,
                Integer.class);
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "UnityEngine.Screen",
                "width", "UnityEngine.CoreModule").build();
        int width = altDriver.getStaticProperty(altGetComponentPropertyParams,
                Integer.class);

        assertEquals(screenWidth, width);
    }
    
    @Test
    public void testSetStaticProperty() {
        final Integer expectedValue = 5;
        AltSetComponentPropertyParams altSetComponentPropertyParams = new AltSetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp", expectedValue.toString()).build();
        altDriver.setStaticProperty(altSetComponentPropertyParams);
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp").build();
        Integer value = altDriver.getStaticProperty(altGetComponentPropertyParams,
                Integer.class);
        assertEquals(expectedValue, value);
    }

    @Test
    public void testSetStaticProperty2()
    {
        Integer newValue = 5;
        Integer[] expectedArray = { 1, 5, 3 };
        AltSetComponentPropertyParams altSetComponentPropertyParams = new AltSetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule", "staticArrayOfInts[1]", "Assembly-CSharp", newValue.toString()).build();
        altDriver.setStaticProperty(altSetComponentPropertyParams);
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule", "staticArrayOfInts", "Assembly-CSharp").build();
        Integer[] value = altDriver.getStaticProperty(altGetComponentPropertyParams,
                Integer[].class);
        for(int i=0; i<expectedArray.length; i++)
                assertEquals(expectedArray[i], value[i]);
    }

    @Test
    public void testSetCommandTimeout() throws Exception {
        String componentName = "AltExampleScriptCapsule";
        String methodName = "JumpWithDelay";
        String assembly = "Assembly-CSharp";
        Object[] parameters = new Object[] {};
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME,
                "Capsule").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        altDriver.setCommandResponseTimeout(1);
        try {
            altElement.callComponentMethod(
                    new AltCallComponentMethodParams.Builder(componentName, methodName,
                            assembly, parameters)
                            .build(),
                    Void.class);
            fail("Expected CommandResponseTimeoutException");
        } catch (CommandResponseTimeoutException ex) {

        } finally {
            altDriver.setCommandResponseTimeout(60);
        }
    }

    @Test
    public void testKeysDown() {
        AltKeyCode[] keys = { AltKeyCode.K, AltKeyCode.L };

        altDriver.keysDown(new AltKeysDownParams.Builder(keys).build());
        altDriver.keysUp(new AltKeysUpParams.Builder(keys).build());

        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Capsule").build();
        AltObject altObject = altDriver.findObject(altFindObjectsParams);

        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule",
                "stringToSetFromTests", "Assembly-CSharp").build();
        String finalPropertyValue = altObject.getComponentProperty(altGetComponentPropertyParams,
                String.class);

        assertEquals(finalPropertyValue, "multiple keys pressed");
    }

    @Test
    public void testPressKeys() {
        AltKeyCode[] keys = { AltKeyCode.K, AltKeyCode.L };

        altDriver.pressKeys(new AltPressKeysParams.Builder(keys).build());

        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Capsule").build();
        AltObject altObject = altDriver.findObject(altFindObjectsParams);

        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule",
                "stringToSetFromTests", "Assembly-CSharp").build();
        String finalPropertyValue = altObject.getComponentProperty(altGetComponentPropertyParams,
                String.class);

        assertEquals(finalPropertyValue, "multiple keys pressed");
    }

    @Test
    public void testFindElementAtCoordinates() {
        AltObject counterButton = altDriver.findObject(new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "ButtonCounter").build());

        AltObject element = altDriver.findObjectAtCoordinates(
                new AltFindObjectAtCoordinatesParams.Builder(
                        new Vector2(80 + counterButton.x, 15 + counterButton.y))
                        .build());
        assertEquals("Text", element.name);
    }

    @Test
    public void testFindElementAtCoordinates_NoElement() {
        AltObject element = altDriver.findObjectAtCoordinates(
                new AltFindObjectAtCoordinatesParams.Builder(new Vector2(-1, -1))
                        .build());
        assertNull(element);
    }

    @Test
    public void testCallPrivateMethod() {
        AltObject altObject = altDriver.findObject(new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Capsule").build());
        altObject.callComponentMethod(
                new AltCallComponentMethodParams.Builder("AltExampleScriptCapsule", "callJump",
                        "Assembly-CSharp",
                        new Object[] {})
                        .build(),
                Void.class);
        AltObject capsuleInfo = altDriver.findObject(new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "CapsuleInfo").build());
        String text = capsuleInfo.getText();
        assertEquals("Capsule jumps!", text);
    }

    @Test
    public void testResetInput() throws InterruptedException {
        AltFindObjectsParams prefab = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "AltTesterPrefab").build();

        AltGetComponentPropertyParams deviceID = new AltGetComponentPropertyParams.Builder(
                "Altom.AltTester.NewInputSystem",
                "Keyboard.deviceId", "Assembly-CSharp").build();
        AltGetComponentPropertyParams count = new AltGetComponentPropertyParams.Builder(
                "Input",
                "_keyCodesPressed.Count", "Assembly-CSharp").build();
        altDriver.keyDown(new AltKeyDownParams.Builder(AltKeyCode.Alpha1).build());
        int oldId = altDriver.findObject(prefab).getComponentProperty(deviceID, Integer.class);
        altDriver.resetInput();
        int newId = altDriver.findObject(prefab).getComponentProperty(deviceID, Integer.class);

        int countKeyDown = altDriver.findObject(prefab).getComponentProperty(count, Integer.class);
        assertEquals(0, countKeyDown);
        assertNotEquals(newId, oldId);
    }
}
