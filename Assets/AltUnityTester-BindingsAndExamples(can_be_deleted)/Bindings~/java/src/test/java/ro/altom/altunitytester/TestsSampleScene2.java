package ro.altom.altunitytester;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import ro.altom.altunitytester.position.Vector2;

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
}
