package ro.altom.altunitytester;

import org.junit.*;

import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParameters;
import ro.altom.altunitytester.Commands.FindObject.AltWaitForObjectsParameters;
import ro.altom.altunitytester.Commands.ObjectCommand.AltTapClickElementParameters;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParameters;
import ro.altom.altunitytester.position.Vector2;

import java.util.Arrays;
import java.util.List;

import static org.junit.Assert.*;

public class TestsSampleScene2 {

    private static AltUnityDriver altUnityDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13000, true);

    }

    @AfterClass
    public static void tearDown() throws Exception {
        altUnityDriver.stop();
        Thread.sleep(1000);
    }

    @Before
    public void loadLevel() throws Exception {
        AltLoadSceneParameters params = new AltLoadSceneParameters.Builder("Scene 2 Draggable Panel").build();
        altUnityDriver.loadScene(params);
    }

    @Test
    public void testResizePanel() throws Exception {

        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Resize Zone").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters1);
        altUnityDriver.swipe(altElement.x, altElement.y, altElement.x + 200, altElement.y + 200, 2);
        Thread.sleep(2000);
        AltUnityObject altElementAfterResize = altUnityDriver.findObject(altFindObjectsParameters1);
        assertNotSame(altElement.x, altElementAfterResize.x);
        assertNotSame(altElement.y, altElementAfterResize.y);
    }

    @Test
    public void testMovePanel() throws Exception {

        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drag Zone").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters1);
        altUnityDriver.swipe(altElement.x, altElement.y, altElement.x + 200, altElement.y + 200, 2);
        Thread.sleep(2000);
        AltUnityObject altElementAfterMove = altUnityDriver.findObject(altFindObjectsParameters1);
        assertNotSame(altElement.x, altElementAfterMove.x);
        assertNotSame(altElement.y, altElementAfterMove.y);
    }

    @Test
    public void testResizePanelWithMultipointSwipe() throws Exception {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Resize Zone").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters1);

        List<Vector2> positions = Arrays.asList(altElement.getScreenPosition(),
                new Vector2(altElement.x + 100, altElement.y + 100),
                new Vector2(altElement.x + 100, altElement.y + 200));

        altUnityDriver.multipointSwipeAndWait(positions, 3);

        AltUnityObject altElementAfterResize = altUnityDriver.findObject(altFindObjectsParameters1);
        assertNotSame(altElement.x, altElementAfterResize.x);
        assertNotSame(altElement.y, altElementAfterResize.y);
    }

    @Test
    public void testClosePanel() throws Exception {

        AltFindObjectsParameters findObjectsParameters = new AltFindObjectsParameters.Builder(By.NAME,
                "Panel Drag Area").build();
        AltWaitForObjectsParameters params = new AltWaitForObjectsParameters.Builder(findObjectsParameters)
                .withInterval(2).build();
        altUnityDriver.waitForObject(params);

        AltFindObjectsParameters findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Panel").build();
        assertTrue(altUnityDriver.findObject(findObjectParams).enabled);

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Close Button").build();
        AltUnityObject altElement = altUnityDriver.findObject(findObjectParams);
        altElement.click(new AltTapClickElementParameters.Builder().build());

        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Button").build();
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Panel").build();
        altElement = altUnityDriver.findObject(altFindObjectsParameters2);
        altElement.click(new AltTapClickElementParameters.Builder().build());
        assertTrue(altUnityDriver.findObject(altFindObjectsParameters1).enabled);
    }

    @Test
    public void testPointerDownFromObject() throws InterruptedException {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Panel").build();
        AltUnityObject panel = altUnityDriver.findObject(altFindObjectsParameters1);
        String color1 = panel.getComponentProperty("AltUnityExampleScriptPanel", "normalColor");
        panel.pointerDown();
        Thread.sleep(1000);
        String color2 = panel.getComponentProperty("AltUnityExampleScriptPanel", "highlightColor");
        assertTrue(color1 != color2);
    }

    @Test
    public void testPointerUpFromObject() throws InterruptedException {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Panel").build();
        AltUnityObject panel = altUnityDriver.findObject(altFindObjectsParameters1);
        String color1 = panel.getComponentProperty("AltUnityExampleScriptPanel", "normalColor");
        panel.pointerDown();
        Thread.sleep(1000);
        panel.pointerUp();
        String color2 = panel.getComponentProperty("AltUnityExampleScriptPanel", "highlightColor");
        assertEquals(color1, color2);
    }

    @Test
    public void testNewTouchCommands() throws InterruptedException {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drag Zone").build();
        AltUnityObject draggableArea = altUnityDriver.findObject(altFindObjectsParameters1);
        Vector2 initialPosition = draggableArea.getScreenPosition();
        int fingerId = altUnityDriver.beginTouch(draggableArea.getScreenPosition());
        Vector2 newPosition = new Vector2(draggableArea.x + 20, draggableArea.y + 10);
        altUnityDriver.moveTouch(fingerId, newPosition);
        altUnityDriver.endTouch(fingerId);
        draggableArea = altUnityDriver.findObject(altFindObjectsParameters1);
        assertNotEquals(initialPosition.x, draggableArea.getScreenPosition().x);
        assertNotEquals(initialPosition.y, draggableArea.getScreenPosition().y);
    }

}
