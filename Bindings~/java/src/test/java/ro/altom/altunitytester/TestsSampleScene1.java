package ro.altom.altunitytester;

import com.google.gson.Gson;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.AltCallStaticMethodParams;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectAtCoordinatesParams;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParams;
import ro.altom.altunitytester.Commands.FindObject.AltGetAllElementsParams;
import ro.altom.altunitytester.Commands.FindObject.AltWaitForObjectsParams;
import ro.altom.altunitytester.Commands.InputActions.AltHoldParams;
import ro.altom.altunitytester.Commands.InputActions.AltKeyDownParams;
import ro.altom.altunitytester.Commands.InputActions.AltKeyUpParams;
import ro.altom.altunitytester.Commands.InputActions.AltKeysDownParams;
import ro.altom.altunitytester.Commands.InputActions.AltKeysUpParams;
import ro.altom.altunitytester.Commands.InputActions.AltMoveMouseParams;
import ro.altom.altunitytester.Commands.InputActions.AltPressKeysParams;
import ro.altom.altunitytester.Commands.InputActions.AltTapClickCoordinatesParams;
import ro.altom.altunitytester.Commands.InputActions.AltTiltParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltCallComponentMethodParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltSetComponentPropertyParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltTapClickElementParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltSetTextParams;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParams;
import ro.altom.altunitytester.Commands.UnityCommand.AltSetTimeScaleParams;
import ro.altom.altunitytester.Commands.UnityCommand.AltUnloadSceneParams;
import ro.altom.altunitytester.Commands.UnityCommand.AltWaitForCurrentSceneToBeParams;
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
	public static void setUp() {
		altUnityDriver = new AltUnityDriver(TestsHelper.GetAltUnityDriverHost(),
				TestsHelper.GetAltUnityDriverPort(),
				true);
	}

	@AfterClass
	public static void tearDown() throws Exception {
		if (altUnityDriver != null) {
			altUnityDriver.stop();
		}
		Thread.sleep(1000);
	}

	@Before
	public void loadLevel() {
		altUnityDriver.loadScene(new AltLoadSceneParams.Builder("Scene 1 AltUnityDriverTestScene").build());
	}

	@Test
	public void testLodeNonExistentScene() {
		try {
			altUnityDriver.loadScene(new AltLoadSceneParams.Builder("Scene 0").build());
			assertTrue(false);
		} catch (SceneNotFoundException e) {
			assertTrue(true);
		}
	}

	@Test
	public void testGetCurrentScene() {
		assertEquals("Scene 1 AltUnityDriverTestScene", altUnityDriver.getCurrentScene());
	}

	@Test
	public void testFindElement() {
		String name = "Capsule";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				name).build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNotNull(altElement);
		assertEquals(name, altElement.name);
	}

	@Test
	public void testFindElements() {
		String name = "Plane";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				name).build();
		AltUnityObject[] altElements = altUnityDriver.findObjects(altFindObjectsParams);
		assertNotNull(altElements);
		assertEquals(altElements[0].name, name);
	}

	@Test
	public void testFindElementWhereNameContains() {

		String name = "Cap";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				name).build();
		AltUnityObject altElement = altUnityDriver.findObjectWhichContains(altFindObjectsParams);
		assertNotNull(altElement);
		assertTrue(altElement.name.contains(name));
	}

	@Test
	public void testFindElementsWhereNameContains() {
		String name = "Pla";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				name).build();
		AltUnityObject[] altElements = altUnityDriver.findObjectsWhichContain(altFindObjectsParams);
		assertNotNull(altElements);
		assertTrue(altElements[0].name.contains(name));
	}

	@Test
	public void testGetAllElements() throws Exception {
		Thread.sleep(1000);
		AltGetAllElementsParams allElementsParams = new AltGetAllElementsParams.Builder().build();
		AltUnityObject[] altElements = altUnityDriver.getAllElements(allElementsParams);
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
	public void testWaitForExistingElement() {
		String name = "Capsule";
		long timeStart = System.currentTimeMillis();
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				name).build();
		AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
				altFindObjectsParams).build();
		AltUnityObject altElement = altUnityDriver.waitForObject(altWaitForObjectsParams);
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
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				name).build();
		altFindObjectsParams.setEnabled(false);
		AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
				altFindObjectsParams).build();
		AltUnityObject altElement = altUnityDriver.waitForObject(altWaitForObjectsParams);
		long timeEnd = System.currentTimeMillis();
		long time = timeEnd - timeStart;
		assertTrue(time / 1000 < 20);
		assertNotNull(altElement);
		assertEquals(altElement.name, name);
	}

	@Test(expected = WaitTimeOutException.class)
	public void testWaitForNonExistingElement() {
		String name = "Capsulee";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				name).build();

		AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
				altFindObjectsParams).withTimeout(1).build();
		altUnityDriver.waitForObject(altWaitForObjectsParams);
	}

	@Test
	public void testWaitForCurrentSceneToBe() {
		String name = "Scene 1 AltUnityDriverTestScene";
		long timeStart = System.currentTimeMillis();
		AltWaitForCurrentSceneToBeParams params = new AltWaitForCurrentSceneToBeParams.Builder(name).build();
		altUnityDriver.waitForCurrentSceneToBe(params);
		long timeEnd = System.currentTimeMillis();
		long time = timeEnd - timeStart;
		assertTrue(time / 1000 < 20);

		String currentScene = altUnityDriver.getCurrentScene();
		assertEquals(name, currentScene);
	}

	@Test
	public void testWaitForCurrentSceneToBeANonExistingScene() {

		String name = "NonExistentScene";
		try {
			AltWaitForCurrentSceneToBeParams params = new AltWaitForCurrentSceneToBeParams.Builder(name)
					.withTimeout(1).build();
			altUnityDriver.waitForCurrentSceneToBe(params);
			fail();
		} catch (Exception e) {
			assertEquals(e.getMessage(), "Scene [NonExistentScene] not loaded after 1.0 seconds");
		}
	}

	@Test
	public void testWaitForExistingElementWhereNameContains() {
		String name = "Dir";
		long timeStart = System.currentTimeMillis();
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				name).build();
		AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
				altFindObjectsParams).build();
		AltUnityObject altElement = altUnityDriver.waitForObjectWhichContains(altWaitForObjectsParams);
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

		altUnityDriver.waitForObjectWhichContains(params);
	}

	@Test
	public void testFindElementWithText() {
		String name = "CapsuleInfo";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				name).build();

		String text = altUnityDriver.findObject(altFindObjectsParams).getText();

		altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.TEXT, text).build();

		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);

		assertNotNull(altElement);
		assertEquals(altElement.getText(), text);
	}

	@Test
	public void testFindElementByComponent() throws InterruptedException {
		Thread.sleep(1000);
		String componentName = "AltUnityRunner";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.COMPONENT, componentName).build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNotNull(altElement);
		assertEquals(altElement.name, "AltUnityRunnerPrefab");
	}

	@Test
	public void testFindElementByComponentWithNamespace() throws InterruptedException {
		Thread.sleep(1000);
		String componentName = "Altom.AltUnityTester.AltUnityRunner";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.COMPONENT, componentName).build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNotNull(altElement);
		assertEquals(altElement.name, "AltUnityRunnerPrefab");
	}

	@Test
	public void testGetComponentProperty() throws InterruptedException {
		Thread.sleep(1000);
		String componentName = "Altom.AltUnityTester.AltUnityRunner";
		String propertyName = "InstrumentationSettings.ShowPopUp";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"AltUnityRunnerPrefab").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNotNull(altElement);

		Boolean propertyValue = altElement.getComponentProperty(
				new AltGetComponentPropertyParams.Builder(componentName,
						propertyName).build(),
				Boolean.class);
		assertTrue(propertyValue);
	}

	@Test
	public void testGetComponentPropertyInvalidDeserialization() {
		String componentName = "Altom.AltUnityTester.AltUnityRunner";
		String propertyName = "InstrumentationSettings.ShowPopUp";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"AltUnityRunnerPrefab").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		try {
			altElement.getComponentProperty(
					new AltGetComponentPropertyParams.Builder(componentName,
							propertyName).build(),
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
		String componentName = "Altom.AltUnityTester.AltUnityRunner";
		String propertyName = "socketPort";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"AltUnityRunnerPrefab").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNotNull(altElement);
		altElement.getComponentProperty(
				new AltGetComponentPropertyParams.Builder(componentName,
						propertyName).build(),
				String.class);
	}

	@Test
	public void testGetComponentPropertyArray() {
		String componentName = "AltUnityExampleScriptCapsule";
		String propertyName = "arrayOfInts";
		String assembly = "Assembly-CSharp";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"Capsule").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNotNull(altElement);
		int[] propertyValue = altElement.getComponentProperty(
				new AltGetComponentPropertyParams.Builder(componentName,
						propertyName).withAssembly(assembly).build(),
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
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"Capsule").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNotNull(altElement);
		boolean propertyValue = altElement.getComponentProperty(
				new AltGetComponentPropertyParams.Builder(componentName,
						propertyName).build(),
				Boolean.class);
		assertEquals(false, propertyValue);
	}

	@Test
	public void testSetComponentProperty() {
		String componentName = "AltUnityExampleScriptCapsule";
		String propertyName = "stringToSetFromTests";
		String assembly = "Assembly-CSharp";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"Capsule").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNotNull(altElement);
		altElement.setComponentProperty(
				new AltSetComponentPropertyParams.Builder(componentName, propertyName,
						"2").withAssembly(assembly).build());
		int propertyValue = altElement.getComponentProperty(
				new AltGetComponentPropertyParams.Builder(componentName,
						propertyName).withAssembly(assembly).build(),
				int.class);
		assertEquals(2, propertyValue);
	}

	@Test
	public void testSetNonExistingComponentProperty() {
		String componentName = "AltUnityExampleScriptCapsuleNotFound";
		String propertyName = "stringToSetFromTests";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"Capsule").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNotNull(altElement);
		try {
			altElement.setComponentProperty(
					new AltSetComponentPropertyParams.Builder(componentName, propertyName,
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
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"Capsule").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);

		assertNull(altElement.callComponentMethod(
				new AltCallComponentMethodParams.Builder(componentName, methodName, new Object[] {})
						.withAssembly(assembly).build(),
				Void.class));
	}

	@Test
	public void testGetTextCallMethodWithNoParameters() 
	{

		String componentName = "UnityEngine.UI.Text";
		String methodName = "get_text";
		String assembly = "UnityEngine.UI";
		String expected_text = "Change Camera Mode";
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.PATH,
			"/Canvas/Button/Text").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);

		assertEquals(expected_text, altElement.callComponentMethod(
			new AltCallComponentMethodParams.Builder(componentName, methodName, new Object[] {})
					.withAssembly(assembly).build(),
			String.class));
	}

	@Test
	public void TestSetFontSizeCallMethodWithParameters() throws Exception 
	{

		String componentName = "UnityEngine.UI.Text";
		String methodName = "set_fontSize";
		String methodExpectedName = "get_fontSize";
		String assembly = "UnityEngine.UI";
		String[] parameters = new String[] { "16"};
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.PATH,
			"/Canvas/UnityUIInputField/Text").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);

		altElement.callComponentMethod(
			new AltCallComponentMethodParams.Builder(componentName, methodName, parameters)
					.withAssembly(assembly)
					.build(),
			Void.class);
		Integer fontSize = altElement.callComponentMethod(
			new AltCallComponentMethodParams.Builder(componentName, methodExpectedName, new Object[] {})
					.withAssembly(assembly)
					.build(),
			Integer.class);

		assert(16==fontSize);
	}

	@Test
	public void testCallMethodWithParameters() throws Exception {
		String componentName = "AltUnityExampleScriptCapsule";
		String methodName = "Jump";
		String assembly = "Assembly-CSharp";
		String[] parameters = new String[] { "New Text" };
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"Capsule").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);

		assertNull(altElement.callComponentMethod(
				new AltCallComponentMethodParams.Builder(componentName, methodName, parameters)
						.withAssembly(assembly)
						.build(),
				Void.class));
	}

	@Test
	public void testCallMethodWithManyParameters() throws Exception {
		String componentName = "AltUnityExampleScriptCapsule";
		String methodName = "TestMethodWithManyParameters";
		String assembly = "Assembly-CSharp";
		Object[] parameters = new Object[] { 1, "stringparam", 0.5, new int[] { 1, 2,
				3 } };
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"Capsule").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNull(altElement.callComponentMethod(
				new AltCallComponentMethodParams.Builder(componentName, methodName, parameters)
						.withAssembly(assembly)
						.build(),
				Void.class));
	}

	@Test(expected = MethodWithGivenParametersNotFoundException.class)
	public void testCallMethodWithIncorrectNumberOfParameters() throws Exception {
		String componentName = "AltUnityExampleScriptCapsule";
		String methodName = "TestMethodWithManyParameters";
		String assembly = "Assembly-CSharp";
		Object[] parameters = new Object[] { 1, "stringparam", new int[] { 1, 2, 3 }
		};
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"Capsule").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		altElement.callComponentMethod(
				new AltCallComponentMethodParams.Builder(componentName, methodName, parameters)
						.withAssembly(assembly)
						.build(),
				Void.class);
	}

	
	@Test(expected = InvalidParameterTypeException.class)
	public void testCallMethodInvalidParameterType() {
		String componentName = "AltUnityExampleScriptCapsule";
		String methodName = "TestMethodWithManyParameters";
		Object[] parameters = new Object[] { 1, "stringparam", 0.5, new int[] { 1, 2, 3 } };
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"Capsule").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);

		altElement.callComponentMethod(
				new AltCallComponentMethodParams.Builder(componentName, methodName, parameters)
						.withTypeOfParameters(new String[] { "System.Stringggggg" }).build(),
				Void.class);
	}

	@Test(expected = AssemblyNotFoundException.class)
	public void testCallMethodAssmeblyNotFound() {
		String componentName = "RandomComponent";
		String methodName = "TestMethodWithManyParameters";
		Object[] parameters = new Object[] { 'a', "stringparam", 0.5, new int[] { 1,
				2, 3 } };
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"Capsule").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);

		altElement.callComponentMethod(
				new AltCallComponentMethodParams.Builder(componentName, methodName, parameters)
						.withAssembly("RandomAssembly").build(),
				Void.class);
	}

	@Test(expected = MethodWithGivenParametersNotFoundException.class)
	public void testCallMethodWithIncorrectNumberOfParameters2() {
		String componentName = "AltUnityExampleScriptCapsule";
		String methodName = "TestMethodWithManyParameters";
		String assembly = "Assembly-CSharp";
		Object[] parameters = new Object[] { 'a', "stringparam", new int[] { 1, 2, 3
		} };
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"Capsule").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		altElement.callComponentMethod(
				new AltCallComponentMethodParams.Builder(componentName, methodName, parameters)
						.withAssembly(assembly)
						.build(),
				Void.class);
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
		AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "Button").withCamera(By.NAME, "Main Camera").build();
		AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "Capsule").withCamera(By.NAME, "Main Camera").build();
		AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "Capsule").withCamera(By.NAME, "Camera").build();
		AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParameters1);
		altButton.click(new AltTapClickElementParams.Builder().build());
		altButton.click(new AltTapClickElementParams.Builder().build());
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters2);
		AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParameters3);
		assertNotSame(altElement.x, altElement2.x);
		assertNotSame(altElement.y, altElement2.y);
	}

	@Test
	public void testFindNonExistentObject() throws Exception {
		try {
			AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
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
			AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
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
				.findObject(new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
						"UIButton").build());
		altUnityDriver.holdButton(
				new AltHoldParams.Builder(button.getScreenPosition()).withDuration(1).build());
		AltUnityObject capsuleInfo = altUnityDriver
				.findObject(new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
						"CapsuleInfo").build());
		String text = capsuleInfo.getText();
		assertEquals(text, "UIButton clicked to jump capsule!");
	}

	@Test
	public void testButtonTap() throws Exception {
		AltFindObjectsParams params = new AltFindObjectsParams.Builder(By.NAME, "UIButton").build();
		AltTapClickElementParams param2 = new AltTapClickElementParams.Builder().build();
		altUnityDriver.findObject(params).tap(param2);

		params = new AltFindObjectsParams.Builder(By.NAME,
				"CapsuleInfo").build();
		AltUnityObject capsuleInfo = altUnityDriver.findObject(params);

		Thread.sleep(2);
		String text = capsuleInfo.getText();
		assertEquals(text, "UIButton clicked to jump capsule!");
	}

	@Test
	public void testCapsuleTap() throws Exception {
		AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "Capsule").build();
		AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "CapsuleInfo").build();
		altUnityDriver.findObject(altFindObjectsParameters1).tap();
		AltUnityObject capsuleInfo = altUnityDriver.findObject(altFindObjectsParameters2);
		Thread.sleep(2);
		String text = capsuleInfo.getText();
		assertEquals(text, "Capsule was clicked to jump!");
	}

	@Test
	public void TestCallStaticMethod() throws Exception {
		altUnityDriver.callStaticMethod(
				new AltCallStaticMethodParams.Builder("UnityEngine.PlayerPrefs", "SetInt",
						new Object[] { "Test", "1" }).build(),
				String.class);
		int a = altUnityDriver.callStaticMethod(new AltCallStaticMethodParams.Builder("UnityEngine.PlayerPrefs",
				"GetInt", new Object[] { "Test", "2" }).build(), Integer.class);
		assertEquals(1, a);
	}

	@Test
	public void TestCallMethodWithMultipleDefinitions() throws Exception {

		AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "Capsule").build();
		AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "CapsuleInfo").build();
		AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters1);
		capsule.callComponentMethod(
				new AltCallComponentMethodParams.Builder("AltUnityExampleScriptCapsule", "Test",
						new Object[] { 2 })
						.withTypeOfParameters(new String[] { "System.Int32" })
						.withAssembly("Assembly-CSharp").build(),
				Void.class);
		AltUnityObject capsuleInfo = altUnityDriver.findObject(altFindObjectsParameters2);
		assertEquals("6", capsuleInfo.getText());
	}

	@Test
	public void TestGetSetTimeScale() {
		float timescale = 0.1f;
		AltSetTimeScaleParams.Builder builder = new AltSetTimeScaleParams.Builder(timescale);

		altUnityDriver.setTimeScale(builder.build());
		float timeScale = altUnityDriver.getTimeScale();
		assertEquals(0.1f, timeScale, 0);
		altUnityDriver.setTimeScale(new AltSetTimeScaleParams.Builder(1f).build());
	}

	@Test
	public void TestCallMethodWithAssembly() {

		AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "Capsule").build();
		AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters1);
		AltUnityRotation initialRotation = capsule.getComponentProperty(
				new AltGetComponentPropertyParams.Builder("UnityEngine.Transform",
						"rotation").build(),
				AltUnityRotation.class);

		capsule.callComponentMethod(new AltCallComponentMethodParams.Builder(
				"UnityEngine.Transform", "Rotate",
				new Object[] { 10, 10, 10 }).withAssembly("UnityEngine.CoreModule")
				.withTypeOfParameters(new String[] {}).build(),
				Void.class);
		AltUnityObject capsuleAfterRotation = altUnityDriver.findObject(altFindObjectsParameters1);
		AltUnityRotation finalRotation = capsuleAfterRotation.getComponentProperty(
				new AltGetComponentPropertyParams.Builder("UnityEngine.Transform",
						"rotation").build(),
				AltUnityRotation.class);
		assertTrue("Rotation should be distinct",
				initialRotation.x != finalRotation.x || initialRotation.y != finalRotation.y
						|| initialRotation.z != finalRotation.z
						|| initialRotation.w != finalRotation.w);
	}

	@Test
	public void TestWaitForObjectToNotBePresent() {
		AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "ObjectDestroyedIn5Secs").build();
		AltWaitForObjectsParams altWaitForObjectsParameters1 = new AltWaitForObjectsParams.Builder(
				altFindObjectsParameters1).build();
		altUnityDriver.waitForObjectToNotBePresent(altWaitForObjectsParameters1);
		try {
			altUnityDriver.findObject(altFindObjectsParameters1);
			assertFalse("Not found exception should be thrown", true);
		} catch (NotFoundException e) {
			assertTrue(e.getMessage(),
					e.getMessage().startsWith("Object //ObjectDestroyedIn5Secs not found"));
		}

		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
				"Capsulee").build();
		AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
				altFindObjectsParams).build();
		altUnityDriver.waitForObjectToNotBePresent(altWaitForObjectsParams);
		try {
			altUnityDriver.findObject(altFindObjectsParams);
			assertFalse("Not found exception should be thrown", true);
		} catch (NotFoundException e) {
			assertTrue(e.getMessage(), e.getMessage().startsWith("Object //Capsulee not found"));
		}
	}

	@Test
	public void TestGetChineseLetters() {
		AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "ChineseLetters").build();
		String text = altUnityDriver.findObject(altFindObjectsParameters1).getText();
		assertEquals("哦伊娜哦", text);
	}

	@Test
	public void TestNonEnglishText() {
		AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "NonEnglishText").build();
		String text = altUnityDriver.findObject(altFindObjectsParameters1).getText();
		assertEquals("BJÖRN'S PASS", text);
	}

	@Test
	public void TestPressNextScene() throws InterruptedException {
		String initialScene = altUnityDriver.getCurrentScene();
		AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "NextScene").build();
		altUnityDriver.findObject(altFindObjectsParameters1).tap();
		String currentScene = altUnityDriver.getCurrentScene();
		assertNotEquals(initialScene, currentScene);
	}

	@Test
	public void TestSetText() {
		AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "NonEnglishText").build();
		AltUnityObject textObject = altUnityDriver.findObject(altFindObjectsParameters1);
		String originalText = textObject.getText();
		String afterText = textObject.setText("ModifiedText").getText();
		assertNotEquals(originalText, afterText);
	}

	@Test
	public void TestSetTextWithSubmit() {
		AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "NonEnglishText").build();
		AltUnityObject textObject = altUnityDriver.findObject(altFindObjectsParameters1);
		String originalText = textObject.getText();

		AltSetTextParams setTextParams = new AltSetTextParams.Builder("ModifiedText").withSubmit(true).build();
		String afterText = textObject.setText(setTextParams).getText();
		assertNotEquals(originalText, afterText);
	}

	@Test
	public void TestFindParentUsingPath() {
		AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.PATH, "//CapsuleInfo/..").build();
		AltUnityObject parent = altUnityDriver.findObject(altFindObjectsParameters1);
		assertEquals("Canvas", parent.name);
	}

	@Test
	public void TestAcceleration() throws InterruptedException {
		AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "Capsule").build();
		AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters1);
		Vector3 initialWorldCoordinates = capsule.getWorldPosition();
		altUnityDriver
				.tilt(new AltTiltParams.Builder(new Vector3(1, 1,
						1)).withDuration(1).withWait(false).build());
		Thread.sleep(1000);
		capsule = altUnityDriver.findObject(altFindObjectsParameters1);
		Vector3 afterTiltCoordinates = capsule.getWorldPosition();
		assertNotEquals(initialWorldCoordinates, afterTiltCoordinates);
	}

	@Test
	public void TestAccelerationAndWait() throws InterruptedException {
		AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "Capsule").build();
		AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters1);
		Vector3 initialWorldCoordinates = capsule.getWorldPosition();
		altUnityDriver.tilt(new AltTiltParams.Builder(new Vector3(1, 1,
				1)).withDuration(1).build());
		capsule = altUnityDriver.findObject(altFindObjectsParameters1);
		Vector3 afterTiltCoordinates = capsule.getWorldPosition();
		assertNotEquals(initialWorldCoordinates, afterTiltCoordinates);
	}

	public void TestFindObjectWithCameraId() {
		AltFindObjectsParams altFindObjectsParametersButton = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.PATH, "//Button").build();
		AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
		altButton.click(new AltTapClickElementParams.Builder().build());
		altButton.click(new AltTapClickElementParams.Builder().build());
		AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH,
				"//Camera").build();
		AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);
		AltFindObjectsParams altFindObjectsParametersCampsule = new AltFindObjectsParams.Builder(By.COMPONENT,
				"CapsuleCollider").withCamera(By.ID, String.valueOf(camera.id)).build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParametersCampsule);

		assertTrue("True", altElement.name.equals("Capsule"));

		altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH, "//Main Camera").build();
		AltUnityObject camera2 = altUnityDriver.findObject(altFindObjectsParametersCamera);
		altFindObjectsParametersCampsule = new AltFindObjectsParams.Builder(By.COMPONENT, "CapsuleCollider")
				.withCamera(By.ID, String.valueOf(camera2.id)).build();
		AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParametersCampsule);
		assertNotEquals(altElement.getScreenPosition(),
				altElement2.getScreenPosition());
	}

	@Test
	public void TestWaitForObjectWithCameraId() {
		AltFindObjectsParams altFindObjectsParametersButton = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.PATH, "//Button").build();
		AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
		altButton.click(new AltTapClickElementParams.Builder().build());
		altButton.click(new AltTapClickElementParams.Builder().build());
		AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH,
				"//Camera").build();
		AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);
		AltFindObjectsParams altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.COMPONENT,
				"CapsuleCollider").withCamera(By.ID, String.valueOf(camera.id)).build();
		AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
				altFindObjectsParametersCapsule).build();
		AltUnityObject altElement = altUnityDriver.waitForObject(altWaitForObjectsParams);

		assertTrue("True", altElement.name.equals("Capsule"));

		altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH, "//Main Camera").build();
		AltUnityObject camera2 = altUnityDriver.findObject(altFindObjectsParametersCamera);
		altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.COMPONENT, "CapsuleCollider")
				.withCamera(By.ID, String.valueOf(camera2.id)).build();
		altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(altFindObjectsParametersCapsule).build();
		AltUnityObject altElement2 = altUnityDriver.waitForObject(altWaitForObjectsParams);

		assertNotEquals(altElement.getScreenPosition(), altElement2.getScreenPosition());
	}

	@Test
	public void TestFindObjectsWithCameraId() {
		AltFindObjectsParams altFindObjectsParametersButton = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.PATH, "//Button").build();
		AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
		altButton.click(new AltTapClickElementParams.Builder().build());
		altButton.click(new AltTapClickElementParams.Builder().build());
		AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH,
				"//Camera").build();
		AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);
		AltFindObjectsParams altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.NAME,
				"Plane").withCamera(By.ID, String.valueOf(camera.id)).build();

		AltUnityObject[] altElement = altUnityDriver.findObjects(altFindObjectsParametersCapsule);

		assertTrue("True", altElement[0].name.equals("Plane"));

		altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH, "//Main Camera").build();
		AltUnityObject camera2 = altUnityDriver.findObject(altFindObjectsParametersCamera);
		altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.NAME, "Plane")
				.withCamera(By.ID, String.valueOf(camera2.id)).build();
		AltUnityObject[] altElement2 = altUnityDriver.findObjects(altFindObjectsParametersCapsule);

		assertNotEquals(altElement[0].getScreenPosition(), altElement2[0].getScreenPosition());
	}

	@Test
	public void TestWaitForObjectNotBePresentWithCameraId() {
		AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH,
				"//Main Camera").build();
		AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);

		AltFindObjectsParams altFindObjectsParametersObject = new AltFindObjectsParams.Builder(By.NAME,
				"ObjectDestroyedIn5Secs").withCamera(By.ID, String.valueOf(camera.id)).build();
		AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
				altFindObjectsParametersObject).build();
		altUnityDriver.waitForObjectToNotBePresent(altWaitForObjectsParams);

		AltGetAllElementsParams allElementsParams = new AltGetAllElementsParams.Builder().build();
		AltUnityObject[] allObjectsInTheScene = altUnityDriver.getAllElements(allElementsParams);

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
		AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH,
				"//Main Camera").build();
		AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);

		AltFindObjectsParams altFindObjectsParametersObject = new AltFindObjectsParams.Builder(By.NAME, "Canva")
				.withCamera(By.ID, String.valueOf(camera.id)).build();
		AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
				altFindObjectsParametersObject).build();
		AltUnityObject altElement = altUnityDriver.waitForObjectWhichContains(altWaitForObjectsParams);
		assertEquals("Canvas", altElement.name);

	}

	@Test
	public void TestFindObjectWithTag() {
		AltFindObjectsParams altFindObjectsParametersButton = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.PATH, "//Button").build();
		AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
		altButton.click(new AltTapClickElementParams.Builder().build());
		altButton.click(new AltTapClickElementParams.Builder().build());
		AltFindObjectsParams altFindObjectsParametersCampsule = new AltFindObjectsParams.Builder(By.COMPONENT,
				"CapsuleCollider").withCamera(By.TAG, "MainCamera").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParametersCampsule);

		assertTrue("True", altElement.name.equals("Capsule"));

		altFindObjectsParametersCampsule = new AltFindObjectsParams.Builder(By.COMPONENT, "CapsuleCollider")
				.withCamera(By.TAG, "Untagged").build();
		AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParametersCampsule);
		assertNotEquals(altElement.getScreenPosition(), altElement2.getScreenPosition());
	}

	@Test
	public void TestWaitForObjectWithTag() {
		AltFindObjectsParams altFindObjectsParametersButton = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.PATH, "//Button").build();
		AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
		altButton.click(new AltTapClickElementParams.Builder().build());
		altButton.click(new AltTapClickElementParams.Builder().build());
		AltFindObjectsParams altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.COMPONENT,
				"CapsuleCollider").withCamera(By.TAG, "MainCamera").build();
		AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
				altFindObjectsParametersCapsule).build();
		AltUnityObject altElement = altUnityDriver.waitForObject(altWaitForObjectsParams);

		assertTrue("True", altElement.name.equals("Capsule"));

		altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.COMPONENT, "CapsuleCollider")
				.withCamera(By.TAG, "Untagged").build();
		altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(altFindObjectsParametersCapsule).build();
		AltUnityObject altElement2 = altUnityDriver.waitForObject(altWaitForObjectsParams);

		assertNotEquals(altElement.getScreenPosition(), altElement2.getScreenPosition());
	}

	@Test
	public void TestFindObjectsWithTag() {
		AltFindObjectsParams altFindObjectsParametersButton = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.PATH, "//Button").build();
		AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
		altButton.click(new AltTapClickElementParams.Builder().build());
		altButton.click(new AltTapClickElementParams.Builder().build());
		AltFindObjectsParams altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.NAME,
				"Plane").withCamera(By.TAG, "MainCamera").build();

		AltUnityObject[] altElement = altUnityDriver.findObjects(altFindObjectsParametersCapsule);

		assertTrue("True", altElement[0].name.equals("Plane"));

		altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.NAME, "Plane")
				.withCamera(By.TAG, "Untagged").build();
		AltUnityObject[] altElement2 = altUnityDriver.findObjects(altFindObjectsParametersCapsule);

		assertNotEquals(altElement[0].getScreenPosition(), altElement2[0].getScreenPosition());
	}

	@Test
	public void TestWaitForObjectNotBePresentWithTag() {

		AltFindObjectsParams altFindObjectsParametersObject = new AltFindObjectsParams.Builder(By.NAME,
				"ObjectDestroyedIn5Secs").withCamera(By.TAG, "MainCamera").build();
		AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
				altFindObjectsParametersObject).build();
		altUnityDriver.waitForObjectToNotBePresent(altWaitForObjectsParams);

		AltGetAllElementsParams allElementsParams = new AltGetAllElementsParams.Builder().build();
		AltUnityObject[] allObjectsInTheScene = altUnityDriver.getAllElements(allElementsParams);

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

		AltFindObjectsParams altFindObjectsParametersObject = new AltFindObjectsParams.Builder(By.NAME, "Canva")
				.withCamera(By.TAG, "MainCamera").build();
		AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
				altFindObjectsParametersObject).build();
		AltUnityObject altElement = altUnityDriver.waitForObjectWhichContains(altWaitForObjectsParams);
		assertEquals("Canvas", altElement.name);

	}

	@Test
	public void TestLoadAdditiveScenes() throws Exception {
		AltGetAllElementsParams altGetAllElementsParams = new AltGetAllElementsParams.Builder().build();
		AltUnityObject[] initialNumberOfElements = altUnityDriver.getAllElements(altGetAllElementsParams);

		AltLoadSceneParams altLoadSceneParams = new AltLoadSceneParams.Builder("Scene 2 Draggable Panel")
				.loadSingle(false).build();
		altUnityDriver.loadScene(altLoadSceneParams);
		AltUnityObject[] finalNumberOfElements = altUnityDriver.getAllElements(altGetAllElementsParams);

		assertNotEquals(initialNumberOfElements, finalNumberOfElements);

		String[] scenes = altUnityDriver.getAllLoadedScenes();
		assertEquals(2, scenes.length);
	}

	@Test
	public void TestGetComponentPropertyComplexClass() throws Exception {
		String componentName = "AltUnityExampleScriptCapsule";
		String propertyName = "AltUnitySampleClass.testInt";
		String assembly = "Assembly-CSharp";
		AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
				componentName, propertyName).withAssembly(assembly).build();
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
				.build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNotNull(altElement);
		int propertyValue = altElement.getComponentProperty(altGetComponentPropertyParams, int.class);
		assertEquals(1, propertyValue);
	}

	@Test
	public void TestGetComponentPropertyComplexClass2() throws Exception {
		String componentName = "AltUnityExampleScriptCapsule";
		String propertyName = "listOfSampleClass[1].testString";
		String assembly = "Assembly-CSharp";
		AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
				componentName, propertyName).withAssembly(assembly).build();
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
				.build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNotNull(altElement);
		String propertyValue = altElement.getComponentProperty(altGetComponentPropertyParams, String.class);
		assertEquals("test2", propertyValue);
	}

	@Test
	public void TestSetComponentPropertyComplexClass() {
		String componentName = "AltUnityExampleScriptCapsule";
		String propertyName = "AltUnitySampleClass.testInt";
		String assembly = "Assembly-CSharp";
		AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
				componentName, propertyName).withAssembly(assembly).withMaxDepth(1).build();
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
				.build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNotNull(altElement);
		AltSetComponentPropertyParams altSetComponentPropertyParams = new AltSetComponentPropertyParams.Builder(
				componentName, propertyName, 2).withAssembly(assembly).build();
		altElement.setComponentProperty(altSetComponentPropertyParams);
		int propertyValue = altElement.getComponentProperty(altGetComponentPropertyParams, int.class);
		assertEquals(2, propertyValue);
	}

	@Test
	public void TestSetComponentPropertyComplexClass2() {

		String componentName = "AltUnityExampleScriptCapsule";
		String propertyName = "listOfSampleClass[1].testString";
		String assembly = "Assembly-CSharp";
		AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
				componentName, propertyName).withAssembly(assembly).withMaxDepth(1).build();
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
				.build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		assertNotNull(altElement);
		AltSetComponentPropertyParams altSetComponentPropertyParams = new AltSetComponentPropertyParams.Builder(
				componentName, propertyName, "test3").withAssembly(assembly).build();
		altElement.setComponentProperty(altSetComponentPropertyParams);
		String propertyValue = altElement.getComponentProperty(altGetComponentPropertyParams, String.class);
		assertEquals("test3", propertyValue);
	}

	@Test
	public void TestGetServerVersion() {
		String serverVersion = altUnityDriver.getServerVersion();
		assertEquals(serverVersion, AltUnityDriver.VERSION);
	}

	@Test
	public void TestGetParent() {
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.NAME, "CapsuleInfo")
				.build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		AltUnityObject altElementParent = altElement.getParent();
		assertEquals("Canvas", altElementParent.name);
	}

	@Test
	public void TestUnloadScene() {
		AltLoadSceneParams altLoadSceneParams = new AltLoadSceneParams.Builder("Scene 2 Draggable Panel")
				.loadSingle(false).build();
		altUnityDriver.loadScene(altLoadSceneParams);
		assertEquals(2, altUnityDriver.getAllLoadedScenes().length);
		altUnityDriver.unloadScene(new AltUnloadSceneParams.Builder("Scene 2 Draggable Panel").build());

		assertEquals(1, altUnityDriver.getAllLoadedScenes().length);
		assertEquals("Scene 1 AltUnityDriverTestScene", altUnityDriver.getAllLoadedScenes()[0]);
	}

	@Test(expected = CouldNotPerformOperationException.class)
	public void TestUnloadOnlyScene() {
		altUnityDriver.unloadScene(new AltUnloadSceneParams.Builder("Scene 1 AltUnityDriverTestScene").build());
	}

	@Test(expected = InvalidPathException.class)
	public void TestInvalidPath() {
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.PATH, "//[1]")
				.build();
		altUnityDriver.findObject(altFindObjectsParams);
	}

	@Test(expected = InvalidPathException.class)
	public void TestInvalidPath2() {
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.PATH,
				"CapsuleInfo[@tag=UI]").build();
		altUnityDriver.findObject(altFindObjectsParams);
	}

	@Test(expected = InvalidPathException.class)
	public void TestInvalidPath3() {
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.PATH,
				"//CapsuleInfo[@tag=UI/Text").build();
		altUnityDriver.findObject(altFindObjectsParams);
	}

	@Test(expected = InvalidPathException.class)
	public void TestInvalidPath4() {
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.PATH,
				"//CapsuleInfo[0/Text").build();
		altUnityDriver.findObject(altFindObjectsParams);
	}

	@Test()
	public void TestTapCoordinates() {
		AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
				.build();
		AltUnityObject capsule = altUnityDriver.findObject(findCapsuleParams);
		AltTapClickCoordinatesParams tapParams = new AltTapClickCoordinatesParams.Builder(
				capsule.getScreenPosition()).build();
		altUnityDriver.tap(tapParams);

		AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
				"//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
		AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
				.build();
		altUnityDriver.waitForObject(waitParams);
	}

	@Test()
	public void TestClickCoordinates() {
		AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
				.build();
		AltUnityObject capsule = altUnityDriver.findObject(findCapsuleParams);
		AltTapClickCoordinatesParams clickParams = new AltTapClickCoordinatesParams.Builder(
				capsule.getScreenPosition()).build();
		altUnityDriver.click(clickParams);

		AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
				"//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
		AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
				.build();
		altUnityDriver.waitForObject(waitParams);
	}

	@Test()
	public void TestTapElement() {
		AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
				.build();
		AltUnityObject capsule = altUnityDriver.findObject(findCapsuleParams);

		AltTapClickElementParams tapParams = new AltTapClickElementParams.Builder().build();
		capsule.tap(tapParams);

		AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
				"//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
		AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
				.build();
		altUnityDriver.waitForObject(waitParams);
	}

	@Test()
	public void TestClickElement() {
		AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
				.build();
		AltUnityObject capsule = altUnityDriver.findObject(findCapsuleParams);

		AltTapClickElementParams tapParams = new AltTapClickElementParams.Builder().build();
		capsule.click(tapParams);

		AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
				"//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
		AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
				.build();
		altUnityDriver.waitForObject(waitParams);
	}

	@Test()
	public void TestKeyDownAndKeyUpMouse0() throws InterruptedException {
		AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
				.build();
		AltUnityObject capsule = altUnityDriver.findObject(findCapsuleParams);
		Vector2 initialCapsPos = capsule.getWorldPosition();
		AltMoveMouseParams altMoveMouseParams = new AltMoveMouseParams.Builder(capsule.getScreenPosition())
				.withDuration(0.1f).build();
		altUnityDriver.moveMouse(altMoveMouseParams);
		Thread.sleep(1000);
		altUnityDriver.keyDown(new AltKeyDownParams.Builder(AltUnityKeyCode.Mouse0).build());
		altUnityDriver.keyUp(new AltKeyUpParams.Builder(AltUnityKeyCode.Mouse0).build());
		capsule = altUnityDriver.findObject(findCapsuleParams);
		Vector2 finalCapsPos = capsule.getWorldPosition();
		assertNotEquals(initialCapsPos, finalCapsPos);
	}

	@Test(expected = CameraNotFoundException.class)
	public void TestCameraNotFoundException() {
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
				.withCamera(By.NAME, "Camera").build();
		altUnityDriver.findObject(altFindObjectsParams);

	}

	@Test
	public void testScreenshot() {
		String path = "testJava2.png";
		altUnityDriver.getPNGScreenshot(path);
		assertTrue(new File(path).isFile());
	}

	@Test
	public void testGetStaticProperty() {
		AltCallStaticMethodParams altCallStaticMethodParams = new AltCallStaticMethodParams.Builder(
				"UnityEngine.Screen", "SetResolution", new Object[] { "1920", "1080", "True"
				})
				.withTypeOfParameters(new String[] { "System.Int32", "System.Int32",
						"System.Boolean" })
				.withAssembly("UnityEngine.CoreModule").build();
		altUnityDriver.callStaticMethod(altCallStaticMethodParams,
				Void.class);
		AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
				"UnityEngine.Screen",
				"currentResolution.width").withAssembly("UnityEngine.CoreModule").build();
		int width = altUnityDriver.getStaticProperty(altGetComponentPropertyParams,
				Integer.class);
		assertEquals(width, 1920);
	}

	@Test
	public void testGetStaticPropertyInstanceNull() {
		AltCallStaticMethodParams altCallStaticMethodParams = new AltCallStaticMethodParams.Builder(
				"UnityEngine.Screen", "get_width", new Object[] {})
				.withAssembly("UnityEngine.CoreModule").build();
		int screenWidth = altUnityDriver.callStaticMethod(altCallStaticMethodParams,
				Integer.class);
		AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
				"UnityEngine.Screen",
				"width").withAssembly("UnityEngine.CoreModule").build();
		int width = altUnityDriver.getStaticProperty(altGetComponentPropertyParams,
				Integer.class);

		assertEquals(screenWidth, width);
	}

	@Test
	public void testSetCommandTimeout() throws Exception {
		String componentName = "AltUnityExampleScriptCapsule";
		String methodName = "JumpWithDelay";
		String assembly = "Assembly-CSharp";
		Object[] parameters = new Object[] {};
		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME,
				"Capsule").build();
		AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
		altUnityDriver.setCommandResponseTimeout(1);
		try {
			altElement.callComponentMethod(
					new AltCallComponentMethodParams.Builder(componentName, methodName, parameters)
							.withAssembly(assembly).build(),
					Void.class);
			fail("Expected CommandResponseTimeoutException");
		} catch (CommandResponseTimeoutException ex) {

		} finally {
			altUnityDriver.setCommandResponseTimeout(60);
		}
	}

	@Test
	public void testKeysDown() {
		AltUnityKeyCode[] keys = { AltUnityKeyCode.K, AltUnityKeyCode.L };

		altUnityDriver.keysDown(new AltKeysDownParams.Builder(keys).build());
		altUnityDriver.keysUp(new AltKeysUpParams.Builder(keys).build());

		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "Capsule").build();
		AltUnityObject altUnityObject = altUnityDriver.findObject(altFindObjectsParams);

		AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
				"AltUnityExampleScriptCapsule",
				"stringToSetFromTests").withAssembly("Assembly-CSharp").build();
		String finalPropertyValue = altUnityObject.getComponentProperty(altGetComponentPropertyParams,
				String.class);

		assertEquals(finalPropertyValue, "multiple keys pressed");
	}

	@Test
	public void testPressKeys() {
		AltUnityKeyCode[] keys = { AltUnityKeyCode.K, AltUnityKeyCode.L };

		altUnityDriver.pressKeys(new AltPressKeysParams.Builder(keys).build());

		AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "Capsule").build();
		AltUnityObject altUnityObject = altUnityDriver.findObject(altFindObjectsParams);

		AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
				"AltUnityExampleScriptCapsule",
				"stringToSetFromTests").withAssembly("Assembly-CSharp").build();
		String finalPropertyValue = altUnityObject.getComponentProperty(altGetComponentPropertyParams,
				String.class);

		assertEquals(finalPropertyValue, "multiple keys pressed");
	}

	@Test
	public void testFindElementAtCoordinates() {
		AltUnityObject counterButton = altUnityDriver.findObject(new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "ButtonCounter").build());

		AltUnityObject element = altUnityDriver.findObjectAtCoordinates(
				new AltFindObjectAtCoordinatesParams.Builder(
						new Vector2(80 + counterButton.x, 15 + counterButton.y))
						.build());
		assertEquals("Text", element.name);
	}

	@Test
	public void testFindElementAtCoordinates_NoElement() {
		AltUnityObject element = altUnityDriver.findObjectAtCoordinates(
				new AltFindObjectAtCoordinatesParams.Builder(new Vector2(-1, -1))
						.build());
		assertNull(element);
	}

	@Test
	public void testCallPrivateMethod() {
		AltUnityObject altUnityObject = altUnityDriver.findObject(new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "Capsule").build());
		altUnityObject.callComponentMethod(
				(new AltCallComponentMethodParams.Builder("AltUnityExampleScriptCapsule", "callJump",
						new Object[] {})
						.withAssembly("Assembly-CSharp"))
						.build(),
				Void.class);
		AltUnityObject capsuleInfo = altUnityDriver.findObject(new AltFindObjectsParams.Builder(
				AltUnityDriver.By.NAME, "CapsuleInfo").build());
		String text = capsuleInfo.getText();
		assertEquals("Capsule jumps!", text);
	}
}
