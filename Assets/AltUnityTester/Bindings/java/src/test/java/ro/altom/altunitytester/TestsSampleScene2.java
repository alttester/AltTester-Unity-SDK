package ro.altom.altunitytester;

import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.io.IOException;

import static org.junit.jupiter.api.Assertions.assertNotEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;

public class TestsSampleScene2 {

    private static AltUnityDriver altUnityDriver;

    @BeforeAll
    public static void setUp() throws IOException {
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13000);
    }

    @AfterAll
    public static void tearDown() throws Exception {
        altUnityDriver.stop();
    }

    @BeforeEach
    public void loadLevel() throws Exception {
        altUnityDriver.loadScene("Scene 2 Draggable Panel");
    }


    @Test
    public void testResizePanel() throws Exception {
        AltUnityObject altElement = altUnityDriver.findElement("Resize Zone");
        altUnityDriver.swipe(altElement.x, altElement.y, altElement.x + 200, altElement.y + 200, 2);
        Thread.sleep(2000);
        AltUnityObject altElementAfterResize = altUnityDriver.findElement("Resize Zone");
        assertNotEquals(altElement.x, altElementAfterResize.x);
        assertNotEquals(altElement.y, altElementAfterResize.y);
    }

    @Test
    public void testMovePanel() throws Exception {

        AltUnityObject altElement = altUnityDriver.findElement("Drag Zone");
        altUnityDriver.swipe(altElement.x, altElement.y, altElement.x + 200, altElement.y + 200, 2);
        Thread.sleep(2000);
        AltUnityObject altElementAfterMove = altUnityDriver.findElement("Drag Zone");
        assertNotEquals(altElement.x, altElementAfterMove.x);
        assertNotEquals(altElement.y, altElementAfterMove.y);
    }

    @Test
    public void testClosePanel() throws Exception {
        altUnityDriver.waitForElement("Panel Drag Area", 2, 0.5);
        assertTrue(altUnityDriver.findElement("Panel").enabled);
        AltUnityObject altElement = altUnityDriver.findElement("Close Button");
        altElement.clickEvent();

        altElement = altUnityDriver.findElement("Button");
        altElement.clickEvent();
        assertTrue(altUnityDriver.findElement("Panel").enabled);
    }

}
