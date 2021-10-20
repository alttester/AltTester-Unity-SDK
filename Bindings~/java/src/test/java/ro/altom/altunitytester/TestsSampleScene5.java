package ro.altom.altunitytester;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.altunitytester.position.Vector3;
import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParameters;
import ro.altom.altunitytester.Commands.InputActions.AltKeyParameters;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParameters;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;

import java.util.ArrayList;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotEquals;

public class TestsSampleScene5 {

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
        AltLoadSceneParameters params = new AltLoadSceneParameters.Builder("Scene 5 Keyboard Input").build();
        altUnityDriver.loadScene(params);
    }

    @Test
    public void TestMovementCube() throws InterruptedException {

        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Player2").build();
        AltUnityObject cube = altUnityDriver.findObject(altFindObjectsParameters1);
        float cubeInitWorldX = cube.worldX;
        float cubeInitWorldZ = cube.worldZ;

        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Player1").build();
        cube = altUnityDriver.findObject(altFindObjectsParameters2);
        cubeInitWorldX = cube.worldX;
        cubeInitWorldZ = cube.worldZ;

        altUnityDriver.scrollMouse(30, 20);
        altUnityDriver.pressKey(AltUnityKeyCode.K, 1, 2);
        Thread.sleep(2000);

        altUnityDriver.pressKeyAndWait(AltUnityKeyCode.O, 1, 1);
        cube = altUnityDriver.findObject(altFindObjectsParameters1);
        float cubeFinalWorldX = cube.worldX;
        float cubeFinalWorldZ = cube.worldZ;

        assertNotEquals(cubeInitWorldX, cubeFinalWorldX);
        assertNotEquals(cubeInitWorldZ, cubeFinalWorldZ);
    }

    @Test
    // Test Keyboard button press
    public void TestCameraMovement() throws InterruptedException {

        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Player1").build();
        AltUnityObject cube = altUnityDriver.findObject(altFindObjectsParameters2);
        float cubeInitWorldZ = cube.worldZ;

        altUnityDriver.pressKey(AltUnityKeyCode.W, 1, 2);
        Thread.sleep(2000);
        cube = altUnityDriver.findObject(altFindObjectsParameters2);
        float cubeFinalWorldZ = cube.worldZ;

        assertNotEquals(cubeInitWorldZ, cubeFinalWorldZ);

    }

    @Test
    // Testing mouse movement and clicking
    public void TestCreatingStars() throws InterruptedException {

        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(By.NAME, "Star")
                .build();
        AltUnityObject[] stars = altUnityDriver.findObjectsWhichContain(altFindObjectsParameters1);
        assertEquals(1, stars.length);

        AltFindObjectsParameters params = new AltFindObjectsParameters.Builder(By.NAME, "PressingPoint1")
                .withCamera(By.NAME, "Player2").build();
        AltUnityObject pressingPoint1 = altUnityDriver.findObject(params);
        altUnityDriver.moveMouse(pressingPoint1.x, pressingPoint1.y, 1);
        Thread.sleep(1500);

        altUnityDriver.pressKey(AltUnityKeyCode.Mouse0, 1, 1);

        params = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, "PressingPoint2")
                .withCamera(AltUnityDriver.By.NAME, "Player2").build();
        AltUnityObject pressingPoint2 = altUnityDriver.findObject(params);
        altUnityDriver.moveMouseAndWait(pressingPoint2.x, pressingPoint2.y, 1);
        altUnityDriver.pressKeyAndWait(AltUnityKeyCode.Mouse0, 1, 1);

        stars = altUnityDriver.findObjectsWhichContain(altFindObjectsParameters1);
        assertEquals(3, stars.length);

    }

    @Test
    public void TestPowerJoystick() {
        ArrayList<String> ButtonNames = new ArrayList<String>();
        ButtonNames.add("Horizontal");
        ButtonNames.add("Vertical");
        ArrayList<AltUnityKeyCode> KeyToPressForButtons = new ArrayList<AltUnityKeyCode>();
        KeyToPressForButtons.add(AltUnityKeyCode.D);
        KeyToPressForButtons.add(AltUnityKeyCode.W);
        AltLoadSceneParameters loadSceneParams = new AltLoadSceneParameters.Builder("Scene 5 Keyboard Input").build();
        altUnityDriver.loadScene(loadSceneParams);

        AltFindObjectsParameters findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "AxisName").build();
        AltUnityObject axisName = altUnityDriver.findObject(findObjectParams);

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "AxisValue").build();
        AltUnityObject axisValue = altUnityDriver.findObject(findObjectParams);
        int i = 0;
        for (AltUnityKeyCode key : KeyToPressForButtons) {
            altUnityDriver.pressKeyAndWait(key, 0.5f, 0.1f);
            assertEquals("0.5", axisValue.getText());
            assertEquals(ButtonNames.get(i), axisName.getText());
            i++;
        }
    }

    @Test
    public void TestScroll() throws InterruptedException {
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Player2").build();
        AltUnityObject player2 = altUnityDriver.findObject(altFindObjectsParameters);
        Vector3 cubeInitialPostion = new Vector3(player2.worldX, player2.worldY, player2.worldY);
        altUnityDriver.scrollMouse(4, 2);
        Thread.sleep(2000);
        player2 = altUnityDriver.findObject(altFindObjectsParameters);
        Vector3 cubeFinalPosition = new Vector3(player2.worldX, player2.worldY, player2.worldY);
        assertNotEquals(cubeInitialPostion, cubeFinalPosition);
    }

    @Test
    public void TestScrollAndWait() throws InterruptedException {

        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                "Player2").build();
        AltUnityObject player2 = altUnityDriver.findObject(altFindObjectsParameters);
        Vector3 cubeInitialPostion = new Vector3(player2.worldX, player2.worldY, player2.worldY);
        altUnityDriver.scrollMouse(4, 2);
        Thread.sleep(2000);
        player2 = altUnityDriver.findObject(altFindObjectsParameters);
        Vector3 cubeFinalPosition = new Vector3(player2.worldX, player2.worldY, player2.worldY);
        assertNotEquals(cubeInitialPostion, cubeFinalPosition);
    }

    @Test
    public void TestKeyDownAndKeyUp() throws Exception {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "LastKeyDownValue").build();
        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "LastKeyUpValue").build();
        AltFindObjectsParameters altFindObjectsParameters3 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "LastKeyPressedValue").build();
        AltUnityKeyCode kcode = AltUnityKeyCode.A;
        AltKeyParameters altKeyParams = new AltKeyParameters.Builder(kcode).build();

        altUnityDriver.KeyDown(altKeyParams);
        Thread.sleep(2000);
        AltUnityObject lastKeyDown = altUnityDriver.findObject(altFindObjectsParameters1);
        AltUnityObject lastKeyPress = altUnityDriver.findObject(altFindObjectsParameters3);
        assertEquals("A", AltUnityKeyCode.valueOf(lastKeyDown.getText()).name());
        assertEquals("A", AltUnityKeyCode.valueOf(lastKeyPress.getText()).name());

        altUnityDriver.KeyUp(kcode);
        Thread.sleep(2000);
        AltUnityObject lastKeyUp = altUnityDriver.findObject(altFindObjectsParameters2);
        assertEquals("A", AltUnityKeyCode.valueOf(lastKeyUp.getText()).name());
    }
}
