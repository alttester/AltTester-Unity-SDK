package ro.altom.altunitytester;

import com.google.gson.Gson;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import java.io.IOException;

import static org.junit.Assert.*;


public class TestsSampleScene2 {

    private static AltUnityDriver altUnityDriver;

    @BeforeClass
    public static void setUp() throws IOException {
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13000);
    }

    @AfterClass
    public static void tearDown() throws Exception {
        altUnityDriver.stop();
    }

    @Before
    public void loadLevel() throws Exception {
        altUnityDriver.loadScene("Scene 2 Draggable Panel");
    }


    @Test
    public void testResizePanel() throws Exception {
        AltUnityObject altElement = altUnityDriver.findElement("Resize Zone");
        altUnityDriver.swipe(altElement.x, altElement.y, altElement.x + 200, altElement.y + 200, 2);
        Thread.sleep(2000);
        AltUnityObject altElementAfterResize = altUnityDriver.findElement("Resize Zone");
        assertNotSame(altElement.x, altElementAfterResize.x);
        assertNotSame(altElement.y, altElementAfterResize.y);
    }

    @Test
    public void testMovePanel() throws Exception {

        AltUnityObject altElement = altUnityDriver.findElement("Drag Zone");
        altUnityDriver.swipe(altElement.x, altElement.y, altElement.x + 200, altElement.y + 200, 2);
        Thread.sleep(2000);
        AltUnityObject altElementAfterMove = altUnityDriver.findElement("Drag Zone");
        assertNotSame(altElement.x, altElementAfterMove.x);
        assertNotSame(altElement.y, altElementAfterMove.y);
    }

    @Test
    public void testClosePanel() throws Exception {
        altUnityDriver.waitForElement("Panel Drag Area","", 2, 0.5);
        assertTrue(altUnityDriver.findElement("Panel").enabled);
        AltUnityObject altElement = altUnityDriver.findElement("Close Button");
        altElement.clickEvent();

        altElement = altUnityDriver.findElement("Button");
        altElement.clickEvent();
        assertTrue(altUnityDriver.findElement("Panel").enabled);
    }

}
