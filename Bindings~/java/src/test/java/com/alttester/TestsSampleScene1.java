/*
    Copyright(C) 2025 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

package com.alttester;

import java.io.File;
import java.lang.Void;

import com.alttester.AltDriver.By;
import com.alttester.Commands.AltCallStaticMethodParams;
import com.alttester.Commands.FindObject.AltFindObjectAtCoordinatesParams;
import com.alttester.Commands.FindObject.AltFindObjectsParams;
import com.alttester.Commands.FindObject.AltGetAllElementsParams;
import com.alttester.Commands.FindObject.AltWaitForComponentPropertyParams;
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
import com.alttester.Commands.ObjectCommand.AltSetTextParams;
import com.alttester.Commands.ObjectCommand.AltTapClickElementParams;
import com.alttester.Commands.UnityCommand.AltLoadSceneParams;
import com.alttester.Commands.UnityCommand.AltSetTimeScaleParams;
import com.alttester.Commands.UnityCommand.AltUnloadSceneParams;
import com.alttester.Commands.UnityCommand.AltWaitForCurrentSceneToBeParams;
import com.alttester.UnityStruct.AltKeyCode;
import com.alttester.altTesterExceptions.AssemblyNotFoundException;
import com.alttester.altTesterExceptions.CameraNotFoundException;
import com.alttester.altTesterExceptions.CommandResponseTimeoutException;
import com.alttester.altTesterExceptions.ComponentNotFoundException;
import com.alttester.altTesterExceptions.CouldNotPerformOperationException;
import com.alttester.altTesterExceptions.InvalidParameterTypeException;
import com.alttester.altTesterExceptions.InvalidPathException;
import com.alttester.altTesterExceptions.MethodWithGivenParametersNotFoundException;
import com.alttester.altTesterExceptions.NotFoundException;
import com.alttester.altTesterExceptions.PropertyNotFoundException;
import com.alttester.altTesterExceptions.ResponseFormatException;
import com.alttester.altTesterExceptions.SceneNotFoundException;
import com.alttester.altTesterExceptions.WaitTimeOutException;
import com.alttester.position.Vector2;
import com.alttester.position.Vector3;

import com.google.gson.Gson;
import com.google.gson.JsonElement;
import com.google.gson.JsonParser;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Tag;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertNotSame;
import static org.junit.jupiter.api.Assertions.assertNull;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.junit.jupiter.api.Assertions.fail;

public class TestsSampleScene1 extends BaseTest {

    @BeforeEach
    public void loadLevel() {
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

    @Test
    public void testWaitForNonExistingElement() {
        assertThrows(WaitTimeOutException.class,
                () -> {
                    String name = "Capsulee";
                    AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME,
                            name).build();

                    AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                            altFindObjectsParams).withTimeout(1).build();
                    altDriver.waitForObject(altWaitForObjectsParams);
                });
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

    @Test
    public void testWaitForNonExistingElementWhereNameContains() {
        assertThrows(WaitTimeOutException.class,
                () -> {
                    String name = "xyz";
                    AltFindObjectsParams findObjectsParams = new AltFindObjectsParams.Builder(
                            By.NAME, name).build();
                    AltWaitForObjectsParams params = new AltWaitForObjectsParams.Builder(
                            findObjectsParams)
                            .withTimeout(1)
                            .build();

                    altDriver.waitForObjectWhichContains(params);
                });
    }

    @Test
    public void TestGetApplicationScreenSize() {
        int[] screensize = altDriver.getApplicationScreenSize();
        // We cannot set resolution on iOS so we don't know the exact resolution, we
        // just want to see that it returns a value and is different than 0
        assertNotEquals(0, screensize[0]);
        assertNotEquals(0, screensize[1]);
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
        String componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                AltDriver.By.COMPONENT, componentName).build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        assertEquals(altElement.name, "AltTesterPrefab");
    }

    @Test
    public void testFindElementByComponentWithNamespace() throws InterruptedException {
        Thread.sleep(1000);
        String componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                AltDriver.By.COMPONENT, componentName).build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);
        assertEquals(altElement.name, "AltTesterPrefab");
    }

    @Test
    public void testGetComponentProperty() throws InterruptedException {
        Thread.sleep(1000);
        String componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
        String propertyName = "InstrumentationSettings.ResetConnectionData";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "AltTesterPrefab").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);

        Boolean propertyValue = altElement.getComponentProperty(
                new AltGetComponentPropertyParams.Builder(componentName,
                        propertyName, "AltTester.AltTesterUnitySDK").build(),
                Boolean.class);
        assertTrue(propertyValue);
    }

    @Test
    public void testWaitForComponentProperty() throws InterruptedException {
        Thread.sleep(1000);
        String componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
        String propertyName = "InstrumentationSettings.AppName";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "AltTesterPrefab").build();

        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);

        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                componentName, propertyName, "").build();
        AltWaitForComponentPropertyParams<String> altWaitForComponentPropertyParams = new AltWaitForComponentPropertyParams.Builder<String>(
                altGetComponentPropertyParams).build();
        String propertyValue = altElement.waitForComponentProperty(
                altWaitForComponentPropertyParams,
                "__default__",
                String.class);
        assertEquals("__default__", propertyValue);
    }

    @Test
    public void testWaitForComponentPropertyComponentNotFound() throws InterruptedException {
        Thread.sleep(1000);
        String componentName = "AltRunnerTest";
        String propertyName = "InstrumentationSettings.AltServerPort";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "AltTesterPrefab").build();

        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);

        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                componentName, propertyName, "").build();
        AltWaitForComponentPropertyParams<Boolean> altWaitForComponentPropertyParams = new AltWaitForComponentPropertyParams.Builder<Boolean>(
                altGetComponentPropertyParams).build();
        assertThrows(ComponentNotFoundException.class,
                () -> {
                    Boolean propertyValue = altElement.waitForComponentProperty(
                            altWaitForComponentPropertyParams,
                            false,
                            Boolean.class);
                });

    }

    @Tag("WebGLUnsupported")
    @Test
    public void TestWaitForComponentPropertyMultipleTypes() throws InterruptedException {
        AltObject Canvas = altDriver.waitForObject(new AltWaitForObjectsParams.Builder(
                new AltFindObjectsParams.Builder(AltDriver.By.PATH, "/Canvas").build()).build());
        Canvas.waitForComponentProperty(
                new AltWaitForComponentPropertyParams.Builder<JsonElement>(
                        new AltGetComponentPropertyParams.Builder(
                                "UnityEngine.RectTransform", "rect.x",
                                "UnityEngine.CoreModule").build())
                        .build(),
                new Gson().toJsonTree("-960.0"), true, JsonElement.class);

        Canvas.waitForComponentProperty(
                new AltWaitForComponentPropertyParams.Builder<JsonElement>(
                        new AltGetComponentPropertyParams.Builder(
                                "UnityEngine.RectTransform", "hasChanged",
                                "UnityEngine.CoreModule").build())
                        .build(),
                new Gson().toJsonTree("true"), true, JsonElement.class);

        Canvas.waitForComponentProperty(
                new AltWaitForComponentPropertyParams.Builder<JsonElement>(
                        new AltGetComponentPropertyParams.Builder(
                                "UnityEngine.RectTransform", "tag",
                                "UnityEngine.CoreModule").build())
                        .build(),
                new Gson().toJsonTree("Untagged"), true, JsonElement.class);

        Canvas.waitForComponentProperty(
                new AltWaitForComponentPropertyParams.Builder<JsonElement>(
                        new AltGetComponentPropertyParams.Builder(
                                "UnityEngine.RectTransform", "hideFlags",
                                "UnityEngine.CoreModule").build())
                        .build(),
                new Gson().toJsonTree("0"), true, JsonElement.class);

        Canvas.waitForComponentProperty(
                new AltWaitForComponentPropertyParams.Builder<JsonElement>(
                        new AltGetComponentPropertyParams.Builder("UnityEngine.RectTransform",
                                "reapplyDrivenProperties.Target",
                                "UnityEngine.CoreModule").build())
                        .build(),
                new Gson().toJsonTree("null"), true, JsonElement.class);

        Canvas.waitForComponentProperty(
                new AltWaitForComponentPropertyParams.Builder<JsonElement>(
                        new AltGetComponentPropertyParams.Builder(
                                "UnityEngine.UI.CanvasScaler", "transform",
                                "UnityEngine.UI").build())
                        .build(),
                new Gson().toJsonTree(JsonParser.parseString(
                        "[[],[[]],[[]],[[]],[[]],[[],[],[]],[[[],[],[]]],[],[],[[]],[[]],[[]]]")),
                true,
                JsonElement.class);
    }

    @Test
    public void TestWaitForComponentPropertyNotFound() throws InterruptedException {
        Thread.sleep(1000);
        String componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
        String propertyName = "InstrumentationSettings.AltServerPortTest";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "AltTesterPrefab").build();

        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);

        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                componentName, propertyName, "").build();
        AltWaitForComponentPropertyParams<Boolean> altWaitForComponentPropertyParams = new AltWaitForComponentPropertyParams.Builder<Boolean>(
                altGetComponentPropertyParams).build();
        assertThrows(PropertyNotFoundException.class,
                () -> {
                    Boolean propertyValue = altElement.waitForComponentProperty(
                            altWaitForComponentPropertyParams,
                            false,
                            Boolean.class);
                });
    }

    @Test
    public void TestWaitForComponentPropertyTimeOut() throws InterruptedException {
        Thread.sleep(1000);
        String componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
        String propertyName = "InstrumentationSettings.AltServerPort";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "AltTesterPrefab").build();

        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);

        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                componentName, propertyName, "").build();
        AltWaitForComponentPropertyParams<String> altWaitForComponentPropertyParams = new AltWaitForComponentPropertyParams.Builder<String>(
                altGetComponentPropertyParams).withTimeout(2).build();
        assertThrows(WaitTimeOutException.class,
                () -> {
                    altElement.waitForComponentProperty(
                            altWaitForComponentPropertyParams,
                            "Test",
                            String.class);
                });
    }

    @Tag("WebGLUnsupported") // Fails on WebGL in pipeline, skip until issue #1465 is fixed:
                             // https://github.com/alttester/AltTester-Unity-SDK/issues/1465
    @Test
    public void TestWaitForComponentPropertyAssemblyNotFound() throws InterruptedException {
        Thread.sleep(1000);
        String componentName = "AltExampleScriptCapsule";
        String propertyName = "InstrumentationSettings.AltServerPort";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();

        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        assertNotNull(altElement);

        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                componentName, propertyName, "Assembly-CSharpTest").build();
        AltWaitForComponentPropertyParams<Boolean> altWaitForComponentPropertyParams = new AltWaitForComponentPropertyParams.Builder<Boolean>(
                altGetComponentPropertyParams).build();
        assertThrows(AssemblyNotFoundException.class,
                () -> {
                    altElement.waitForComponentProperty(
                            altWaitForComponentPropertyParams,
                            false,
                            Boolean.class);
                });
    }

    @Test
    public void testGetComponentPropertyInvalidDeserialization() {
        String componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
        String propertyName = "InstrumentationSettings.ResetConnectionData";
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "AltTesterPrefab").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParams);
        try {
            altElement.getComponentProperty(
                    new AltGetComponentPropertyParams.Builder(componentName,
                            propertyName,
                            "AltTester.AltTesterUnitySDK").build(),
                    int.class);
            fail("Expected ResponseFormatException");
        } catch (ResponseFormatException ex) {
            assertEquals("Could not deserialize response data: `true` into int",
                    ex.getMessage());
        }
    }

    @Test
    public void testGetNonExistingComponentProperty() throws InterruptedException {
        assertThrows(PropertyNotFoundException.class,
                () -> {
                    Thread.sleep(1000);
                    String componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
                    String propertyName = "socketPort";
                    AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME,
                            "AltTesterPrefab").build();
                    AltObject altElement = altDriver.findObject(altFindObjectsParams);
                    assertNotNull(altElement);
                    altElement.getComponentProperty(
                            new AltGetComponentPropertyParams.Builder(componentName,
                                    propertyName,
                                    "AltTester.AltTesterUnitySDK").build(),
                            String.class);
                });
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
                    new AltSetComponentPropertyParams.Builder(componentName, propertyName,
                            "AltTester.AltTesterUnitySDK",
                            "2").build());
            fail();
        } catch (ComponentNotFoundException e) {
            assertTrue(e.getMessage().startsWith("Component not found"), e.getMessage());
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
                new AltCallComponentMethodParams.Builder(componentName, methodName, assembly,
                        parameters)
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

    @Test
    public void testCallMethodWithIncorrectNumberOfParameters() throws Exception {
        assertThrows(MethodWithGivenParametersNotFoundException.class,
                () -> {
                    String componentName = "AltExampleScriptCapsule";
                    String methodName = "TestMethodWithManyParameters";
                    String assembly = "Assembly-CSharp";
                    Object[] parameters = new Object[] { 1, "stringparam", new int[] { 1, 2, 3 }
                    };
                    AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME,
                            "Capsule").build();
                    AltObject altElement = altDriver.findObject(altFindObjectsParams);
                    altElement.callComponentMethod(
                            new AltCallComponentMethodParams.Builder(componentName,
                                    methodName,
                                    assembly, parameters)
                                    .build(),
                            Void.class);
                });
    }

    @Test
    public void testCallMethodInvalidParameterType() {
        assertThrows(InvalidParameterTypeException.class,
                () -> {
                    String componentName = "AltExampleScriptCapsule";
                    String methodName = "TestMethodWithManyParameters";
                    Object[] parameters = new Object[] { 1, "stringparam", 0.5,
                            new int[] { 1, 2, 3 } };
                    AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME,
                            "Capsule").build();
                    AltObject altElement = altDriver.findObject(altFindObjectsParams);

                    altElement.callComponentMethod(
                            new AltCallComponentMethodParams.Builder(componentName,
                                    methodName, "", parameters)
                                    .withTypeOfParameters(new String[] {
                                            "System.Stringggggg" })
                                    .build(),
                            Void.class);
                });
    }

    @Test
    public void testCallMethodAssmeblyNotFound() {
        assertThrows(AssemblyNotFoundException.class,
                () -> {
                    String componentName = "RandomComponent";
                    String methodName = "TestMethodWithManyParameters";
                    Object[] parameters = new Object[] { 'a', "stringparam", 0.5, new int[] { 1,
                            2, 3 } };
                    AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME,
                            "Capsule").build();
                    AltObject altElement = altDriver.findObject(altFindObjectsParams);

                    altElement.callComponentMethod(
                            new AltCallComponentMethodParams.Builder(componentName,
                                    methodName,
                                    "RandomAssembly", parameters)
                                    .build(),
                            Void.class);
                });
    }

    @Test
    public void testCallMethodWithIncorrectNumberOfParameters2() {
        assertThrows(MethodWithGivenParametersNotFoundException.class,
                () -> {
                    String componentName = "AltExampleScriptCapsule";
                    String methodName = "TestMethodWithManyParameters";
                    String assembly = "Assembly-CSharp";
                    Object[] parameters = new Object[] { 'a', "stringparam", new int[] { 1, 2, 3
                    } };
                    AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME,
                            "Capsule").build();
                    AltObject altElement = altDriver.findObject(altFindObjectsParams);
                    altElement.callComponentMethod(
                            new AltCallComponentMethodParams.Builder(componentName,
                                    methodName,
                                    assembly, parameters)
                                    .build(),
                            Void.class);
                });
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
            assertTrue(e.getMessage().startsWith("PlayerPrefs key test not found"), e.getMessage());
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
    public void testFindNonExistentObjectByName() {
        AltFindObjectsParams params = new AltFindObjectsParams.Builder(AltDriver.By.NAME, "NonExistent").build();
        assertThrows(NotFoundException.class, () -> altDriver.findObject(params));
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
                new AltCallComponentMethodParams.Builder("AltExampleScriptCapsule", "Test",
                        "Assembly-CSharp",
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
        assertTrue(initialRotation.x != finalRotation.x || initialRotation.y != finalRotation.y
                || initialRotation.z != finalRotation.z
                || initialRotation.w != finalRotation.w, "Rotation should be distinct");
    }

    @Test
    public void TestWaitForObjectToNotBePresent() {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "ObjectDestroyedIn5Secs").build();
        AltWaitForObjectsParams altWaitForObjectsParameters1 = new AltWaitForObjectsParams.Builder(
                altFindObjectsParameters1).build();
        altDriver.waitForObjectToNotBePresent(altWaitForObjectsParameters1);

        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsulee").build();
        AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                altFindObjectsParams).build();
        altDriver.waitForObjectToNotBePresent(altWaitForObjectsParams);
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
        String text = "ModifiedText";
        String afterText = textObject.setText(text).getText();
        assertNotEquals(originalText, afterText);
        assertEquals(text, afterText);

    }

    @Test
    public void TestSetTextWithSubmit() {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "NonEnglishText").build();
        AltObject textObject = altDriver.findObject(altFindObjectsParameters1);
        String originalText = textObject.getText();
        String text = "ModifiedText";
        AltSetTextParams setTextParams = new AltSetTextParams.Builder(text).withSubmit(true).build();
        String afterText = textObject.setText(setTextParams).getText();
        assertNotEquals(originalText, afterText);
        assertEquals(text, afterText);
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

        assertTrue(altElement.name.equals("Capsule"), "True");

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

        assertTrue(altElement.name.equals("Capsule"), "True");

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

        assertTrue(altElement[0].name.equals("Plane"), "True");

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

        assertTrue(altElement.name.equals("Capsule"), "True");

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

        assertTrue(altElement.name.equals("Capsule"), "True");

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

        assertTrue(altElement[0].name.equals("Plane"), "True");

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

    @Test
    public void TestUnloadOnlyScene() {
        assertThrows(CouldNotPerformOperationException.class,
                () -> {
                    altDriver.unloadScene(
                            new AltUnloadSceneParams.Builder("Scene 1 AltDriverTestScene")
                                    .build());
                });
    }

    @Test
    public void TestInvalidPath() {
        assertThrows(InvalidPathException.class,
                () -> {
                    AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                            By.PATH, "//[1]")
                            .build();
                    altDriver.findObject(altFindObjectsParams);
                });
    }

    @Test
    public void TestInvalidPath2() {
        assertThrows(InvalidPathException.class,
                () -> {
                    AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                            By.PATH,
                            "CapsuleInfo[@tag=UI]").build();
                    altDriver.findObject(altFindObjectsParams);
                });
    }

    @Test
    public void TestInvalidPath3() {
        assertThrows(InvalidPathException.class,
                () -> {
                    AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                            By.PATH,
                            "//CapsuleInfo[@tag=UI/Text").build();
                    altDriver.findObject(altFindObjectsParams);
                });
    }

    @Test
    public void TestInvalidPath4() {
        assertThrows(InvalidPathException.class,
                () -> {
                    AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                            By.PATH,
                            "//CapsuleInfo[0/Text").build();
                    altDriver.findObject(altFindObjectsParams);
                });
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

    @Test
    public void TestCameraNotFoundException() {
        assertThrows(CameraNotFoundException.class,
                () -> {
                    AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                            By.NAME, "Capsule")
                            .withCamera(By.NAME, "Camera").build();
                    altDriver.findObject(altFindObjectsParams);
                });
    }

    @Test
    public void testScreenshot() {
        String path = "testJava2.png";
        altDriver.getPNGScreenshot(path);
        assertTrue(new File(path).isFile());
    }

    @Test
    @Tag("WebGLUnsupported")
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
                "AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp",
                expectedValue.toString()).build();
        altDriver.setStaticProperty(altSetComponentPropertyParams);
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp").build();
        Integer value = altDriver.getStaticProperty(altGetComponentPropertyParams,
                Integer.class);
        assertEquals(expectedValue, value);
    }

    @Test
    public void testSetStaticProperty2() {
        Integer newValue = 5;
        Integer[] expectedArray = { 1, 5, 3 };
        AltSetComponentPropertyParams altSetComponentPropertyParams = new AltSetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule", "staticArrayOfInts[1]", "Assembly-CSharp",
                newValue.toString()).build();
        altDriver.setStaticProperty(altSetComponentPropertyParams);
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule", "staticArrayOfInts", "Assembly-CSharp").build();
        Integer[] value = altDriver.getStaticProperty(altGetComponentPropertyParams,
                Integer[].class);
        for (int i = 0; i < expectedArray.length; i++)
            assertEquals(expectedArray[i], value[i]);
    }

    @Test
    public void testSetCommandTimeout() throws Exception {
        altDriver.setCommandResponseTimeout(1);
        try {
            AltTiltParams altTiltParams = new AltTiltParams.Builder(new Vector3(1, 1, 1)).withDuration(2)
                    .withWait(true)
                    .build();
            altDriver.tilt(altTiltParams);
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
        assertEquals("ButtonCounter", element.name);
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

        AltGetComponentPropertyParams pIsPressed = new AltGetComponentPropertyParams.Builder(
                "AltTester.AltTesterUnitySDK.InputModule.NewInputSystem",
                "Keyboard.pKey.isPressed", "AltTester.AltTesterUnitySDK.InputModule").build();
        AltGetComponentPropertyParams count = new AltGetComponentPropertyParams.Builder(
                "Input",
                "_keyCodesPressed.Count", "AltTester.AltTesterUnitySDK.InputModule").build();
        altDriver.keyDown(new AltKeyDownParams.Builder(AltKeyCode.P).build());
        assertTrue(altDriver.findObject(prefab).getComponentProperty(pIsPressed, Boolean.class));
        altDriver.resetInput();
        assertFalse(altDriver.findObject(prefab).getComponentProperty(pIsPressed, Boolean.class));

        int countKeyDown = altDriver.findObject(prefab).getComponentProperty(count, Integer.class);
        assertEquals(0, countKeyDown);
    }

    @Test
    public void testFindObjectFromObject() {
        AltObject parent = altDriver.findObject(new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Canvas").build());

        AltObject child = parent.findObjectFromObject(new AltFindObjectsParams.Builder(
                AltDriver.By.TAG, "Finish").build());
        assertEquals("Button", child.name);
        child = parent.findObjectFromObject(new AltFindObjectsParams.Builder(
                AltDriver.By.LAYER, "ButtonLayer").build());
        assertEquals("Button", child.name);
        child = parent.findObjectFromObject(new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Button").build());
        assertEquals("Button", child.name);
        child = parent.findObjectFromObject(new AltFindObjectsParams.Builder(
                AltDriver.By.COMPONENT, "Button").build());
        assertEquals("UIButton", child.name);
        child = parent.findObjectFromObject(new AltFindObjectsParams.Builder(
                AltDriver.By.PATH, "/Button").build());
        assertEquals("Button", child.name);
        child = parent.findObjectFromObject(new AltFindObjectsParams.Builder(
                AltDriver.By.ID, "049eccc5-b072-468b-83bf-119d868ca311").build());
        assertEquals("Button", child.name);
        child = parent.findObjectFromObject(new AltFindObjectsParams.Builder(
                AltDriver.By.TEXT, "Change Camera Mode").build());
        assertEquals("Text", child.name);
    }
}