package com.alttester;

import org.junit.*;

import com.alttester.AltDriver.By;
import com.alttester.Commands.FindObject.AltFindObjectsParams;
import com.alttester.Commands.FindObject.AltWaitForObjectsParams;
import com.alttester.Commands.InputActions.AltBeginTouchParams;
import com.alttester.Commands.InputActions.AltEndTouchParams;
import com.alttester.Commands.InputActions.AltMoveTouchParams;
import com.alttester.Commands.InputActions.AltMultiPointSwipeParams;
import com.alttester.Commands.InputActions.AltSwipeParams;
import com.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import com.alttester.Commands.ObjectCommand.AltTapClickElementParams;
import com.alttester.Commands.UnityCommand.AltLoadSceneParams;
import com.alttester.position.Vector2;

import java.util.Arrays;
import java.util.List;

import static org.junit.Assert.*;

public class TestsSampleScene2 {

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
        AltLoadSceneParams params = new AltLoadSceneParams.Builder("Scene 2 Draggable Panel").build();
        altDriver.loadScene(params);
    }

    @Test
    public void testResizePanel() throws Exception {

        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Resize Zone").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParameters1);
        Vector2 start = altElement.getScreenPosition();
        Vector2 end = new Vector2(start.x + 200, start.y + 200);
        altDriver.swipe(new AltSwipeParams.Builder(start, end).withDuration(2).withWait(false).build());
        Thread.sleep(2000);
        AltObject altElementAfterResize = altDriver.findObject(altFindObjectsParameters1);
        assertNotSame(altElement.x, altElementAfterResize.x);
        assertNotSame(altElement.y, altElementAfterResize.y);
    }

    @Test
    public void testMovePanel() throws Exception {

        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drag Zone").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParameters1);
        Vector2 start = altElement.getScreenPosition();
        Vector2 end = new Vector2(start.x + 200, start.y + 200);
        altDriver.swipe(new AltSwipeParams.Builder(start, end).withDuration(2).withWait(false).build());
        Thread.sleep(2000);
        AltObject altElementAfterMove = altDriver.findObject(altFindObjectsParameters1);
        assertNotSame(altElement.x, altElementAfterMove.x);
        assertNotSame(altElement.y, altElementAfterMove.y);
    }

    @Test
    public void testResizePanelWithMultipointSwipe() throws Exception {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Resize Zone").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParameters1);

        List<Vector2> positions = Arrays.asList(altElement.getScreenPosition(),
                new Vector2(altElement.x + 100, altElement.y + 100),
                new Vector2(altElement.x + 100, altElement.y + 200));

        altDriver.multipointSwipe(new AltMultiPointSwipeParams.Builder(positions).withDuration(3).build());

        AltObject altElementAfterResize = altDriver.findObject(altFindObjectsParameters1);
        assertNotSame(altElement.x, altElementAfterResize.x);
        assertNotSame(altElement.y, altElementAfterResize.y);
    }

    @Test
    public void testClosePanel() throws Exception {

        AltFindObjectsParams findObjectsParameters = new AltFindObjectsParams.Builder(By.NAME,
                "Panel Drag Area").build();
        AltWaitForObjectsParams params = new AltWaitForObjectsParams.Builder(findObjectsParameters)
                .withInterval(2).build();
        altDriver.waitForObject(params);

        AltFindObjectsParams findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Panel").build();
        assertTrue(altDriver.findObject(findObjectParams).enabled);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Close Button").build();
        AltObject altElement = altDriver.findObject(findObjectParams);
        altElement.click(new AltTapClickElementParams.Builder().build());

        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Button").build();
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Panel").build();
        altElement = altDriver.findObject(altFindObjectsParameters2);
        altElement.click(new AltTapClickElementParams.Builder().build());
        assertTrue(altDriver.findObject(altFindObjectsParameters1).enabled);
    }

    @Test
    public void testPointerDownFromObject() throws InterruptedException {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Panel").build();
        AltObject panel = altDriver.findObject(altFindObjectsParameters1);

        AltColor color1 = panel.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltExampleScriptPanel", "normalColor",
                        "Assembly-CSharp")
                        .build(),
                AltColor.class);
        panel.pointerDown();
        Thread.sleep(1000);
        AltColor color2 = panel.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltExampleScriptPanel", "highlightColor",
                        "Assembly-CSharp")
                        .build(),
                AltColor.class);
        assertTrue(color1.r != color2.r || color1.g != color2.g || color1.b != color2.b
                || color1.a != color2.a);
    }

    @Test
    public void testPointerUpFromObject() throws InterruptedException {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Panel").build();
        AltObject panel = altDriver.findObject(altFindObjectsParameters1);
        AltColor color1 = panel.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltExampleScriptPanel", "normalColor",
                        "Assembly-CSharp")
                        .build(),
                AltColor.class);
        panel.pointerDown();
        Thread.sleep(1000);
        panel.pointerUp();
        AltColor color2 = panel.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltExampleScriptPanel", "highlightColor",
                        "Assembly-CSharp")
                        .build(),
                AltColor.class);
        assertTrue(color1.r == color2.r && color1.g == color2.g && color1.b == color2.b
                && color1.a == color2.a);
    }

    @Test
    public void testNewTouchCommands() throws InterruptedException {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drag Zone").build();
        AltObject draggableArea = altDriver.findObject(altFindObjectsParameters1);
        Vector2 initialPosition = draggableArea.getScreenPosition();
        int fingerId = altDriver.beginTouch(new AltBeginTouchParams.Builder(initialPosition).build());
        Vector2 newPosition = new Vector2(draggableArea.x + 20, draggableArea.y + 10);
        altDriver.moveTouch(new AltMoveTouchParams.Builder(fingerId, newPosition).build());
        altDriver.endTouch(new AltEndTouchParams.Builder(fingerId).build());
        draggableArea = altDriver.findObject(altFindObjectsParameters1);
        assertNotEquals(initialPosition.x, draggableArea.getScreenPosition().x);
        assertNotEquals(initialPosition.y, draggableArea.getScreenPosition().y);
    }

}
