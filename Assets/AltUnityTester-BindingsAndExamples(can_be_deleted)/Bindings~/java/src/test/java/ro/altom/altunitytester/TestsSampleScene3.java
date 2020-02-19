package ro.altom.altunitytester;

import com.google.gson.Gson;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import ro.altom.altunitytester.position.Vector2;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import static org.junit.Assert.*;

public class TestsSampleScene3 {
    private static AltUnityDriver altUnityDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13000,";","&",false);
    }

    @AfterClass
    public static void tearDown() throws Exception {
        altUnityDriver.stop();
        Thread.sleep(1000);
    }

    @Before
    public void loadLevel() throws Exception {
        altUnityDriver.loadScene("Scene 3 Drag And Drop");
    }
    @Test
    public void testScreenshot(){
        String path="testJava2.png";
        altUnityDriver.getPNGScreeshot(path);
        assertTrue(new File(path).isFile());
    }
   
    @Test
    public void testMultipleDragAndDrop() throws Exception {
        AltUnityObject altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1");
        AltUnityObject altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
        altUnityDriver.swipe(altElement1.x, altElement1.y,altElement2.x, altElement2.y, 2);

        altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image2");
        altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box2");
        altUnityDriver.swipe(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 2);

        altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image3");
        altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
        altUnityDriver.swipe(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 3);


        altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1");
        altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
        altUnityDriver.swipe(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 5);

        Thread.sleep(6000);

        String imageSource = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1").getComponentProperty("UnityEngine.UI.Image", "sprite");
        String imageSourceDropZone= altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Image").getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotSame(imageSource, imageSourceDropZone);

        imageSource = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image2").getComponentProperty("UnityEngine.UI.Image", "sprite");
        imageSourceDropZone = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop").getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotSame(imageSource, imageSourceDropZone);

    }
    @Test
    public void testMultipleDragAndDropWait() throws Exception {
        AltUnityObject altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1");
        AltUnityObject altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
        altUnityDriver.swipeAndWait(altElement1.x, altElement1.y,altElement2.x, altElement2.y, 2);

        altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image2");
        altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box2");
        altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 2);

        altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image3");
        altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
        altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 3);


        altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1");
        altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
        altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 1);
        String imageSource = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1").getComponentProperty("UnityEngine.UI.Image", "sprite");
        String imageSourceDropZone = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Image").getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotSame(imageSource, imageSourceDropZone);

        imageSource = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image2").getComponentProperty("UnityEngine.UI.Image", "sprite");
        imageSourceDropZone = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop").getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotSame(imageSource, imageSourceDropZone);

    }

    @Test
    public void testTestPointerEnterAndExit() throws Exception {
        AltUnityObject altElement = altUnityDriver.findElement("Drop Image");
        String color1 = altElement.getComponentProperty("DropMe", "highlightColor");
        altUnityDriver.findElement("Drop Image").pointerEnter();
        String color2 = altElement.getComponentProperty("DropMe", "highlightColor");
        assertNotSame(color1,color2);
        altUnityDriver.findElement("Drop Image").pointerExit();
        String color3 = altElement.getComponentProperty("DropMe", "highlightColor");
        assertNotSame(color3, color2);
        assertEquals(color1,color3);

    }
    
    @Test
    public void testMultipleDragAndDropWaitWithMultipointSwipe() throws InterruptedException {
        AltUnityObject altElement1 = altUnityDriver.findElement("Drag Image1");
        AltUnityObject altElement2 = altUnityDriver.findElement("Drop Box1");
        List<Vector2> positions = Arrays.asList(
            new Vector2(altElement1.x, altElement1.y),
            new Vector2(altElement2.x, altElement2.y)
        );
        
        altUnityDriver.multipointSwipe(positions, 2);
        
        Thread.sleep(2000);
        altElement1 = altUnityDriver.findElement("Drag Image1");
        altElement2 = altUnityDriver.findElement("Drop Box1");
        AltUnityObject altElement3 = altUnityDriver.findElement("Drop Box2");

        List<Vector2> positions2 = Arrays.asList(
            new Vector2(altElement1.x, altElement1.y),
            new Vector2(altElement2.x, altElement2.y),
            new Vector2(altElement3.x, altElement3.y)
        );
        altUnityDriver.multipointSwipeAndWait(positions2, 3);

        String imageSource = altUnityDriver.findElement("Drag Image1").getComponentProperty("UnityEngine.UI.Image", "sprite");
        String imageSourceDropZone = altUnityDriver.findElement("Drop Image").getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotEquals(imageSource,imageSourceDropZone);

        imageSource = altUnityDriver.findElement("Drag Image2").getComponentProperty("UnityEngine.UI.Image", "sprite");
        imageSourceDropZone = altUnityDriver.findElement("Drop").getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotEquals(imageSource,imageSourceDropZone);
    }
}
