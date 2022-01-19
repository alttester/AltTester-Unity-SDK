package ro.altom.altunitytester;

import org.junit.*;

import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParams;
import ro.altom.altunitytester.Commands.FindObject.AltWaitForObjectsParams;
import ro.altom.altunitytester.Commands.InputActions.AltBeginTouchParams;
import ro.altom.altunitytester.Commands.InputActions.AltEndTouchParams;
import ro.altom.altunitytester.Commands.InputActions.AltMoveTouchParams;
import ro.altom.altunitytester.Commands.InputActions.AltMultiPointSwipeParams;
import ro.altom.altunitytester.Commands.InputActions.AltSwipeParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltTapClickElementParams;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParams;
import ro.altom.altunitytester.position.Vector2;

import java.util.Arrays;
import java.util.List;

import static org.junit.Assert.*;

public class TestsSampleScene2 {

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
        AltLoadSceneParams params = new AltLoadSceneParams.Builder("Scene 2 Draggable Panel").build();
        altUnityDriver.loadScene(params);
    }

    @Test
    public void testResizePanel() throws Exception {

        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Resize Zone").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters1);
        Vector2 start = altElement.getScreenPosition();
        Vector2 end = new Vector2(start.x + 200, start.y + 200);
        altUnityDriver.swipe(new AltSwipeParams.Builder(start, end).withDuration(2).withWait(false).build());
        Thread.sleep(2000);
        AltUnityObject altElementAfterResize = altUnityDriver.findObject(altFindObjectsParameters1);
        assertNotSame(altElement.x, altElementAfterResize.x);
        assertNotSame(altElement.y, altElementAfterResize.y);
    }

    @Test
    public void testMovePanel() throws Exception {

        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drag Zone").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters1);
        Vector2 start = altElement.getScreenPosition();
        Vector2 end = new Vector2(start.x + 200, start.y + 200);
        altUnityDriver.swipe(new AltSwipeParams.Builder(start, end).withDuration(2).withWait(false).build());
        Thread.sleep(2000);
        AltUnityObject altElementAfterMove = altUnityDriver.findObject(altFindObjectsParameters1);
        assertNotSame(altElement.x, altElementAfterMove.x);
        assertNotSame(altElement.y, altElementAfterMove.y);
    }

    @Test
    public void testResizePanelWithMultipointSwipe() throws Exception {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Resize Zone").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters1);

        List<Vector2> positions = Arrays.asList(altElement.getScreenPosition(),
                new Vector2(altElement.x + 100, altElement.y + 100),
                new Vector2(altElement.x + 100, altElement.y + 200));

        altUnityDriver.multipointSwipe(new AltMultiPointSwipeParams.Builder(positions).withDuration(3).build());

        AltUnityObject altElementAfterResize = altUnityDriver.findObject(altFindObjectsParameters1);
        assertNotSame(altElement.x, altElementAfterResize.x);
        assertNotSame(altElement.y, altElementAfterResize.y);
    }

    @Test
    public void testClosePanel() throws Exception {

        AltFindObjectsParams findObjectsParameters = new AltFindObjectsParams.Builder(By.NAME,
                "Panel Drag Area").build();
        AltWaitForObjectsParams params = new AltWaitForObjectsParams.Builder(findObjectsParameters)
                .withInterval(2).build();
        altUnityDriver.waitForObject(params);

        AltFindObjectsParams findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Panel").build();
        assertTrue(altUnityDriver.findObject(findObjectParams).enabled);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Close Button").build();
        AltUnityObject altElement = altUnityDriver.findObject(findObjectParams);
        altElement.click(new AltTapClickElementParams.Builder().build());

        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Button").build();
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Panel").build();
        altElement = altUnityDriver.findObject(altFindObjectsParameters2);
        altElement.click(new AltTapClickElementParams.Builder().build());
        assertTrue(altUnityDriver.findObject(altFindObjectsParameters1).enabled);
    }

    @Test
    public void testPointerDownFromObject() throws InterruptedException {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Panel").build();
        AltUnityObject panel = altUnityDriver.findObject(altFindObjectsParameters1);

        AltUnityColor color1 = panel.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltUnityExampleScriptPanel", "normalColor").build(),
                AltUnityColor.class);
        panel.pointerDown();
        Thread.sleep(1000);
        AltUnityColor color2 = panel.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltUnityExampleScriptPanel", "highlightColor").build(),
                AltUnityColor.class);
        assertTrue(color1.r != color2.r || color1.g != color2.g || color1.b != color2.b || color1.a != color2.a);
    }

    @Test
    public void testPointerUpFromObject() throws InterruptedException {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Panel").build();
        AltUnityObject panel = altUnityDriver.findObject(altFindObjectsParameters1);
        AltUnityColor color1 = panel.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltUnityExampleScriptPanel", "normalColor").build(),
                AltUnityColor.class);
        panel.pointerDown();
        Thread.sleep(1000);
        panel.pointerUp();
        AltUnityColor color2 = panel.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltUnityExampleScriptPanel", "highlightColor").build(),
                AltUnityColor.class);
        assertTrue(color1.r == color2.r && color1.g == color2.g && color1.b == color2.b && color1.a == color2.a);
    }

    @Test
    public void testNewTouchCommands() throws InterruptedException {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drag Zone").build();
        AltUnityObject draggableArea = altUnityDriver.findObject(altFindObjectsParameters1);
        Vector2 initialPosition = draggableArea.getScreenPosition();
        int fingerId = altUnityDriver.beginTouch(new AltBeginTouchParams.Builder(initialPosition).build());
        Vector2 newPosition = new Vector2(draggableArea.x + 20, draggableArea.y + 10);
        altUnityDriver.moveTouch(new AltMoveTouchParams.Builder(fingerId, newPosition).build());
        altUnityDriver.endTouch(new AltEndTouchParams.Builder(fingerId).build());
        draggableArea = altUnityDriver.findObject(altFindObjectsParameters1);
        assertNotEquals(initialPosition.x, draggableArea.getScreenPosition().x);
        assertNotEquals(initialPosition.y, draggableArea.getScreenPosition().y);
    }

}
