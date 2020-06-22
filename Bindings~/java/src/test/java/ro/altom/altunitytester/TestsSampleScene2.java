package ro.altom.altunitytester;

import org.junit.*;
import ro.altom.altunitytester.position.Vector2;
import ro.altom.altunitytester.position.Vector3;

import java.io.IOException;
import java.util.Arrays;
import java.util.List;

import static org.junit.Assert.*;


public class TestsSampleScene2 {

    private static AltUnityDriver altUnityDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13000,";","&",true);

    }

    @AfterClass
    public static void tearDown() throws Exception {
        altUnityDriver.stop();
        Thread.sleep(1000);
    }

    @Before
    public void loadLevel() throws Exception {
        altUnityDriver.loadScene("Scene 2 Draggable Panel");
    }


    @Test
    public void testResizePanel() throws Exception {
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Resize Zone");
        altUnityDriver.swipe(altElement.x, altElement.y, altElement.x + 200, altElement.y + 200, 2);
        Thread.sleep(2000);
        AltUnityObject altElementAfterResize = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Resize Zone");
        assertNotSame(altElement.x, altElementAfterResize.x);
        assertNotSame(altElement.y, altElementAfterResize.y);
    }

    @Test
    public void testMovePanel() throws Exception {

        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Zone");
        altUnityDriver.swipe(altElement.x, altElement.y, altElement.x + 200, altElement.y + 200, 2);
        Thread.sleep(2000);
        AltUnityObject altElementAfterMove = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Zone");
        assertNotSame(altElement.x, altElementAfterMove.x);
        assertNotSame(altElement.y, altElementAfterMove.y);
    }

    @Test
    public void testResizePanelWithMultipointSwipe() throws Exception {
        AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Resize Zone");

        List<Vector2> positions = Arrays.asList(
            altElement.getScreenPosition(), 
            new Vector2(altElement.x + 100, altElement.y + 100),
            new Vector2(altElement.x + 100, altElement.y + 200));
        
        altUnityDriver.multipointSwipeAndWait(positions, 3);

        AltUnityObject altElementAfterResize = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Resize Zone");
        assertNotSame(altElement.x, altElementAfterResize.x);
        assertNotSame(altElement.y, altElementAfterResize.y);
    }
    
    @Test
    public void testClosePanel() throws Exception {
        altUnityDriver.waitForElement("Panel Drag Area","",true, 2, 0.5);
        assertTrue(altUnityDriver.findElement("Panel").enabled);
        AltUnityObject altElement = altUnityDriver.findElement("Close Button");
        altElement.clickEvent();

        altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Button");
        altElement.clickEvent();
        assertTrue(altUnityDriver.findObject(AltUnityDriver.By.NAME,"Panel").enabled);
    }

    @Test
    public void testDragObject() throws InterruptedException {
        AltUnityObject dragPanel = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Drag Zone");
        Vector3 initPosition = new Vector3(dragPanel.x, dragPanel.y, dragPanel.z);
        dragPanel.drag(200,200);
        Thread.sleep(1000);
        dragPanel = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Drag Zone");
        Vector3 finalPosition = new Vector3(dragPanel.x, dragPanel.y, dragPanel.z);
        assertTrue(initPosition != finalPosition);
    }

    @Test
    public void testDropObject() throws InterruptedException {
        AltUnityObject dragPanel = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Drag Zone");
        Vector3 initPosition = new Vector3(dragPanel.x, dragPanel.y, dragPanel.z);
        dragPanel.drop(200,200);
        Thread.sleep(1000);
        dragPanel = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Drag Zone");
        Vector3 finalPosition = new Vector3(dragPanel.x, dragPanel.y, dragPanel.z);
        assertTrue(initPosition != finalPosition);
    }
    
    @Test
    public void testPointerDownFromObject() throws InterruptedException {
        AltUnityObject panel = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Panel");
        String color1 = panel.getComponentProperty("PanelScript", "normalColor");
        panel.pointerDown();
        Thread.sleep(1000);
        String color2 = panel.getComponentProperty("PanelScript", "highlightColor");
        assertTrue(color1 != color2);
    }

    @Test
    public void testPointerUpFromObject() throws InterruptedException {
        AltUnityObject panel = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Panel");
        String color1 = panel.getComponentProperty("PanelScript", "normalColor");
        panel.pointerDown();
        Thread.sleep(1000);
        panel.pointerUp();
        String color2 = panel.getComponentProperty("PanelScript", "highlightColor");
        assertEquals(color1, color2);
    }
}
