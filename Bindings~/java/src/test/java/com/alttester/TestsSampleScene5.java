package com.alttester;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import com.alttester.position.Vector3;
import com.alttester.AltDriver.By;
import com.alttester.Commands.FindObject.AltFindObjectsParams;
import com.alttester.Commands.InputActions.AltKeyDownParams;
import com.alttester.Commands.InputActions.AltKeyUpParams;
import com.alttester.Commands.InputActions.AltMoveMouseParams;
import com.alttester.Commands.InputActions.AltPressKeyParams;
import com.alttester.Commands.InputActions.AltScrollParams;
import com.alttester.Commands.UnityCommand.AltLoadSceneParams;
import com.alttester.UnityStruct.AltKeyCode;

import java.util.ArrayList;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotEquals;

public class TestsSampleScene5 {

    private static AltDriver altDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        altDriver = new AltDriver(TestsHelper.GetAltDriverHost(), TestsHelper.GetAltDriverPort(),
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
    public void loadLevel() throws Exception {
        altDriver.resetInput();
        AltLoadSceneParams params = new AltLoadSceneParams.Builder("Scene 5 Keyboard Input").build();
        altDriver.loadScene(params);
    }

    @Test
    public void TestMovementCube() throws InterruptedException {

        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Player2").build();
        AltObject cube = altDriver.findObject(altFindObjectsParameters1);
        float cubeInitWorldX = cube.worldX;
        float cubeInitWorldZ = cube.worldZ;

        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Player1").build();
        cube = altDriver.findObject(altFindObjectsParameters2);
        cubeInitWorldX = cube.worldX;
        cubeInitWorldZ = cube.worldZ;

        altDriver.scroll(new AltScrollParams.Builder().withSpeed(30).withDuration(20).withWait(false).build());
        altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.K).withDuration(2).withPower(1)
                .withWait(false).build());
        Thread.sleep(2000);

        altDriver
                .pressKey(new AltPressKeyParams.Builder(AltKeyCode.O).withDuration(1).withPower(1)
                        .build());
        cube = altDriver.findObject(altFindObjectsParameters1);
        float cubeFinalWorldX = cube.worldX;
        float cubeFinalWorldZ = cube.worldZ;

        assertNotEquals(cubeInitWorldX, cubeFinalWorldX);
        assertNotEquals(cubeInitWorldZ, cubeFinalWorldZ);
    }

    @Test
    // Test Keyboard button press
    public void TestCameraMovement() throws InterruptedException {

        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Player1").build();
        AltObject cube = altDriver.findObject(altFindObjectsParameters2);
        float cubeInitWorldZ = cube.worldZ;

        altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.W).withDuration(2).withPower(1)
                .withWait(false).build());
        Thread.sleep(2000);
        cube = altDriver.findObject(altFindObjectsParameters2);
        float cubeFinalWorldZ = cube.worldZ;

        assertNotEquals(cubeInitWorldZ, cubeFinalWorldZ);

    }

    @Test
    // Testing mouse movement and clicking
    public void TestCreatingStars() throws InterruptedException {

        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(By.NAME, "Star")
                .build();
        AltObject[] stars = altDriver.findObjectsWhichContain(altFindObjectsParameters1);
        assertEquals(1, stars.length);

        AltFindObjectsParams params = new AltFindObjectsParams.Builder(By.NAME, "PressingPoint1")
                .withCamera(By.NAME, "Player2").build();
        AltObject pressingPoint1 = altDriver.findObject(params);
        altDriver.moveMouse(
                new AltMoveMouseParams.Builder(pressingPoint1.getScreenPosition()).withWait(false)
                        .build());
        Thread.sleep(1500);

        altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.Mouse0).withWait(false).build());

        params = new AltFindObjectsParams.Builder(AltDriver.By.NAME, "PressingPoint2")
                .withCamera(AltDriver.By.NAME, "Player2").build();
        AltObject pressingPoint2 = altDriver.findObject(params);
        altDriver.moveMouse(new AltMoveMouseParams.Builder(pressingPoint2.getScreenPosition()).build());
        altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.Mouse0).build());

        stars = altDriver.findObjectsWhichContain(altFindObjectsParameters1);
        assertEquals(3, stars.length);
    }

    @Test
    public void TestPowerJoystick() {
        ArrayList<String> ButtonNames = new ArrayList<String>();
        ButtonNames.add("Horizontal");
        ButtonNames.add("Vertical");
        ArrayList<AltKeyCode> KeyToPressForButtons = new ArrayList<AltKeyCode>();
        KeyToPressForButtons.add(AltKeyCode.D);
        KeyToPressForButtons.add(AltKeyCode.W);
        AltLoadSceneParams loadSceneParams = new AltLoadSceneParams.Builder("Scene 5 Keyboard Input").build();
        altDriver.loadScene(loadSceneParams);

        AltFindObjectsParams findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "AxisName").build();
        AltObject axisName = altDriver.findObject(findObjectParams);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME,
                "AxisValue").build();
        AltObject axisValue = altDriver.findObject(findObjectParams);
        int i = 0;
        for (AltKeyCode key : KeyToPressForButtons) {
            altDriver.pressKey(
                    new AltPressKeyParams.Builder(key).withPower(0.5f).withDuration(0.1f).build());
            assertEquals("0.5", axisValue.getText());
            assertEquals(ButtonNames.get(i), axisName.getText());
            i++;
        }
    }

    @Test
    public void TestScroll() throws InterruptedException {
        AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Player2").build();
        AltObject player2 = altDriver.findObject(altFindObjectsParameters);
        Vector3 cubeInitialPostion = new Vector3(player2.worldX, player2.worldY,
                player2.worldY);
        altDriver.scroll(new AltScrollParams.Builder().withSpeed(4).withDuration(2).withWait(false).build());
        Thread.sleep(2000);
        player2 = altDriver.findObject(altFindObjectsParameters);
        Vector3 cubeFinalPosition = new Vector3(player2.worldX, player2.worldY,
                player2.worldY);
        assertNotEquals(cubeInitialPostion, cubeFinalPosition);
    }

    @Test
    public void TestScrollAndWait() throws InterruptedException {
        AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Player2").build();
        AltObject player2 = altDriver.findObject(altFindObjectsParameters);
        Vector3 cubeInitialPostion = new Vector3(player2.worldX, player2.worldY,
                player2.worldY);
        altDriver.scroll(new AltScrollParams.Builder().withSpeed(4).withDuration(2).build());

        player2 = altDriver.findObject(altFindObjectsParameters);
        Vector3 cubeFinalPosition = new Vector3(player2.worldX, player2.worldY,
                player2.worldY);
        assertNotEquals(cubeInitialPostion, cubeFinalPosition);
    }

    @Test
    public void TestKeyDownAndKeyUp() throws Exception {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "LastKeyDownValue").build();
        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "LastKeyUpValue").build();
        AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "LastKeyPressedValue").build();
        AltKeyCode keycode = AltKeyCode.A;
        AltKeyDownParams altKeyParams = new AltKeyDownParams.Builder(keycode).build();

        altDriver.keyDown(altKeyParams);
        Thread.sleep(2000);
        AltObject lastKeyDown = altDriver.findObject(altFindObjectsParameters1);
        AltObject lastKeyPress = altDriver.findObject(altFindObjectsParameters3);
        assertEquals("97", lastKeyDown.getText());
        assertEquals("97", lastKeyPress.getText());

        altDriver.keyUp(new AltKeyUpParams.Builder(keycode).build());
        Thread.sleep(2000);
        AltObject lastKeyUp = altDriver.findObject(altFindObjectsParameters2);
        assertEquals("97", lastKeyUp.getText());
    }
}
