package ro.altom.altunitytester;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.altunitytester.position.Vector3;
import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParams;
import ro.altom.altunitytester.Commands.InputActions.AltKeyDownParams;
import ro.altom.altunitytester.Commands.InputActions.AltKeyUpParams;
import ro.altom.altunitytester.Commands.InputActions.AltMoveMouseParams;
import ro.altom.altunitytester.Commands.InputActions.AltPressKeyParams;
import ro.altom.altunitytester.Commands.InputActions.AltScrollParams;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParams;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;

import java.util.ArrayList;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotEquals;

public class TestsSampleScene5 {

    private static AltUnityDriver altUnityDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        altUnityDriver = new AltUnityDriver(TestsHelper.GetAltUnityDriverHost(), TestsHelper.GetAltUnityDriverPort(),
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
    public void loadLevel() throws Exception {
        AltLoadSceneParams params = new AltLoadSceneParams.Builder("Scene 5 Keyboard Input").build();
        altUnityDriver.loadScene(params);
    }

    @Test
    public void TestMovementCube() throws InterruptedException {

        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Player2").build();
        AltUnityObject cube = altUnityDriver.findObject(altFindObjectsParameters1);
        float cubeInitWorldX = cube.worldX;
        float cubeInitWorldZ = cube.worldZ;

        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Player1").build();
        cube = altUnityDriver.findObject(altFindObjectsParameters2);
        cubeInitWorldX = cube.worldX;
        cubeInitWorldZ = cube.worldZ;

        altUnityDriver.scroll(new AltScrollParams.Builder().withSpeed(30).withDuration(20).withWait(false).build());
        altUnityDriver.pressKey(new AltPressKeyParams.Builder(AltUnityKeyCode.K).withDuration(2).withPower(1)
                .withWait(false).build());
        Thread.sleep(2000);

        altUnityDriver
                .pressKey(new AltPressKeyParams.Builder(AltUnityKeyCode.O).withDuration(1).withPower(1).build());
        cube = altUnityDriver.findObject(altFindObjectsParameters1);
        float cubeFinalWorldX = cube.worldX;
        float cubeFinalWorldZ = cube.worldZ;

        assertNotEquals(cubeInitWorldX, cubeFinalWorldX);
        assertNotEquals(cubeInitWorldZ, cubeFinalWorldZ);
    }

    @Test
    // Test Keyboard button press
    public void TestCameraMovement() throws InterruptedException {

        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Player1").build();
        AltUnityObject cube = altUnityDriver.findObject(altFindObjectsParameters2);
        float cubeInitWorldZ = cube.worldZ;

        altUnityDriver.pressKey(new AltPressKeyParams.Builder(AltUnityKeyCode.W).withDuration(2).withPower(1)
                .withWait(false).build());
        Thread.sleep(2000);
        cube = altUnityDriver.findObject(altFindObjectsParameters2);
        float cubeFinalWorldZ = cube.worldZ;

        assertNotEquals(cubeInitWorldZ, cubeFinalWorldZ);

    }

    @Test
    // Testing mouse movement and clicking
    public void TestCreatingStars() throws InterruptedException {

        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(By.NAME, "Star")
                .build();
        AltUnityObject[] stars = altUnityDriver.findObjectsWhichContain(altFindObjectsParameters1);
        assertEquals(1, stars.length);

        AltFindObjectsParams params = new AltFindObjectsParams.Builder(By.NAME, "PressingPoint1")
                .withCamera(By.NAME, "Player2").build();
        AltUnityObject pressingPoint1 = altUnityDriver.findObject(params);
        altUnityDriver.moveMouse(
                new AltMoveMouseParams.Builder(pressingPoint1.getScreenPosition()).withWait(false).build());
        Thread.sleep(1500);

        altUnityDriver.pressKey(new AltPressKeyParams.Builder(AltUnityKeyCode.Mouse0).withWait(false).build());

        params = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME, "PressingPoint2")
                .withCamera(AltUnityDriver.By.NAME, "Player2").build();
        AltUnityObject pressingPoint2 = altUnityDriver.findObject(params);
        altUnityDriver.moveMouse(new AltMoveMouseParams.Builder(pressingPoint2.getScreenPosition()).build());
        altUnityDriver.pressKey(new AltPressKeyParams.Builder(AltUnityKeyCode.Mouse0).build());

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
        AltLoadSceneParams loadSceneParams = new AltLoadSceneParams.Builder("Scene 5 Keyboard Input").build();
        altUnityDriver.loadScene(loadSceneParams);

        AltFindObjectsParams findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "AxisName").build();
        AltUnityObject axisName = altUnityDriver.findObject(findObjectParams);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "AxisValue").build();
        AltUnityObject axisValue = altUnityDriver.findObject(findObjectParams);
        int i = 0;
        for (AltUnityKeyCode key : KeyToPressForButtons) {
            altUnityDriver.pressKey(new AltPressKeyParams.Builder(key).withPower(0.5f).withDuration(0.1f).build());
            assertEquals("0.5", axisValue.getText());
            assertEquals(ButtonNames.get(i), axisName.getText());
            i++;
        }
    }

    @Test
    public void TestScroll() throws InterruptedException {
        AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                "Player2").build();
        AltUnityObject player2 = altUnityDriver.findObject(altFindObjectsParameters);
        Vector3 cubeInitialPostion = new Vector3(player2.worldX, player2.worldY, player2.worldY);
        altUnityDriver.scroll(new AltScrollParams.Builder().withSpeed(4).withDuration(2).withWait(false).build());
        Thread.sleep(2000);
        player2 = altUnityDriver.findObject(altFindObjectsParameters);
        Vector3 cubeFinalPosition = new Vector3(player2.worldX, player2.worldY, player2.worldY);
        assertNotEquals(cubeInitialPostion, cubeFinalPosition);
    }

    @Test
    public void TestScrollAndWait() throws InterruptedException {
        AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                "Player2").build();
        AltUnityObject player2 = altUnityDriver.findObject(altFindObjectsParameters);
        Vector3 cubeInitialPostion = new Vector3(player2.worldX, player2.worldY, player2.worldY);
        altUnityDriver.scroll(new AltScrollParams.Builder().withSpeed(4).withDuration(2).build());

        player2 = altUnityDriver.findObject(altFindObjectsParameters);
        Vector3 cubeFinalPosition = new Vector3(player2.worldX, player2.worldY, player2.worldY);
        assertNotEquals(cubeInitialPostion, cubeFinalPosition);
    }

    @Test
    public void TestKeyDownAndKeyUp() throws Exception {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "LastKeyDownValue").build();
        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "LastKeyUpValue").build();
        AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "LastKeyPressedValue").build();
        AltUnityKeyCode keycode = AltUnityKeyCode.A;
        AltKeyDownParams altKeyParams = new AltKeyDownParams.Builder(keycode).build();

        altUnityDriver.keyDown(altKeyParams);
        Thread.sleep(2000);
        AltUnityObject lastKeyDown = altUnityDriver.findObject(altFindObjectsParameters1);
        AltUnityObject lastKeyPress = altUnityDriver.findObject(altFindObjectsParameters3);
        assertEquals("97", lastKeyDown.getText());
        assertEquals("97", lastKeyPress.getText());

        altUnityDriver.keyUp(new AltKeyUpParams.Builder(keycode).build());
        Thread.sleep(2000);
        AltUnityObject lastKeyUp = altUnityDriver.findObject(altFindObjectsParameters2);
        assertEquals("97", lastKeyUp.getText());
    }
}
