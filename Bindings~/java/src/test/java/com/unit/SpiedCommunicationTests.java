/*
 * Copyright(C) 2024 Altom Consulting
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
 */

// package com.unit;
//
// import org.junit.After;
// import org.junit.AfterClass;
// import org.junit.Assert;
// import org.junit.BeforeClass;
// import org.junit.Test;
// import org.mockito.ArgumentCaptor;
// import org.mockito.Mockito;
// import com.alttester.AltDriver;
//
// public class SpiedCommunicationTests {
// private static final String FIND_OBJECT_BY_NAME_MESSAGE_PATTERN =
// "findObjectByName;%s;%s;true;&";
// private static final String CLOSE_CONNECTION_MESSAGE = "closeConnection;&";
// private static final String LOAD_SCENE_MESSAGE_PATTERN = "loadScene;%s;&";
// private static final int PORT = 15000;
// private static AltDriver spyAltDriver;
// private static ArgumentCaptor<String> captor;
// private static DummyServer dummyServer;
//
// @BeforeClass
// public static void setup() {
// dummyServer = DummyServer.onPort(PORT);
// dummyServer.start();
// spyAltDriver = Mockito.spy(new AltDriver("127.0.0.1", PORT));
// prepareStubbing();
// }
//
// @After
// public void prepareSpy() {
// // Reset mockito verifications
// Mockito.reset(spyAltDriver);
// prepareStubbing();
// }
//
// private static void prepareStubbing() {
// // Don't send anything
// Mockito.doNothing().when(spyAltDriver).send(Mockito.anyString());
// // Don't evaluate response since nothing will be returned
// Mockito.doReturn("").when(spyAltDriver).recvall();
// captor = ArgumentCaptor.forClass(String.class);
// }
//
// @AfterClass
// public static void tearDown() {
// if (spyAltDriver != null) {
// spyAltDriver.stop();
// }
// if (dummyServer != null) {
// dummyServer.stop();
// }
// }
//
// @Test
// public void findElementWithCameraTest() {
// // WHEN
// String elementName = "element";
// String cameraName = "camera";
// spyAltDriver.findElement(elementName, cameraName);
// Mockito.verify(spyAltDriver, Mockito.times(1)).send(captor.capture());
// // THEN
// Assert.assertEquals(String.format(FIND_OBJECT_BY_NAME_MESSAGE_PATTERN,
// elementName, cameraName), captor.getValue());
// }
//
// @Test
// public void closeConnectionTest() {
// // WHEN
// spyAltDriver.stop();
// Mockito.verify(spyAltDriver, Mockito.times(1)).send(captor.capture());
// // THEN
// Assert.assertEquals(CLOSE_CONNECTION_MESSAGE, captor.getValue());
// }
//
// @Test
// public void loadSceneTest() {
// // WHEN
// String sceneToLoad = "sceneToLoad";
// spyAltDriver.loadScene(sceneToLoad);
// Mockito.verify(spyAltDriver, Mockito.times(1)).send(captor.capture());
// // THEN
// Assert.assertEquals(String.format(LOAD_SCENE_MESSAGE_PATTERN, sceneToLoad),
// captor.getValue());
// }
// }
