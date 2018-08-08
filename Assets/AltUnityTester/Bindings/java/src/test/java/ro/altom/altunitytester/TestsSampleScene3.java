package ro.altom.altunitytester;

import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.io.IOException;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotEquals;

public class TestsSampleScene3 {
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
        altUnityDriver.loadScene("Scene 3 Drag And Drop");
    }

     @Test
    public void testMultipleDragAndDrop() throws Exception {
        AltUnityObject altElement1 = altUnityDriver.findElement("Drag Image1");
        AltUnityObject altElement2 = altUnityDriver.findElement("Drop Box1");
        altUnityDriver.swipe(altElement1.x, altElement1.y,altElement2.x, altElement2.y, 2);

        altElement1 = altUnityDriver.findElement("Drag Image2");
        altElement2 = altUnityDriver.findElement("Drop Box2");
        altUnityDriver.swipe(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 2);

        altElement1 = altUnityDriver.findElement("Drag Image3");
        altElement2 = altUnityDriver.findElement("Drop Box1");
        altUnityDriver.swipe(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 3);


        altElement1 = altUnityDriver.findElement("Drag Image1");
        altElement2 = altUnityDriver.findElement("Drop Box1");
        altUnityDriver.swipe(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 5);

        Thread.sleep(6000);

        String imageSource = altUnityDriver.findElement("Drag Image1").getComponentProperty("UnityEngine.UI.Image", "sprite");
        String imageSourceDropZone= altUnityDriver.findElement("Drop Image").getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotEquals(imageSource, imageSourceDropZone);

        imageSource = altUnityDriver.findElement("Drag Image2").getComponentProperty("UnityEngine.UI.Image", "sprite");
        imageSourceDropZone = altUnityDriver.findElement("Drop").getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotEquals(imageSource, imageSourceDropZone);

    }
    @Test
    public void testMultipleDragAndDropWait() throws Exception {
        AltUnityObject altElement1 = altUnityDriver.findElement("Drag Image1");
        AltUnityObject altElement2 = altUnityDriver.findElement("Drop Box1");
        altUnityDriver.swipeAndWait(altElement1.x, altElement1.y,altElement2.x, altElement2.y, 2);

        altElement1 = altUnityDriver.findElement("Drag Image2");
        altElement2 = altUnityDriver.findElement("Drop Box2");
        altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 2);

        altElement1 = altUnityDriver.findElement("Drag Image3");
        altElement2 = altUnityDriver.findElement("Drop Box1");
        altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 3);


        altElement1 = altUnityDriver.findElement("Drag Image1");
        altElement2 = altUnityDriver.findElement("Drop Box1");
        altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 1);
        String imageSource = altUnityDriver.findElement("Drag Image1").getComponentProperty("UnityEngine.UI.Image", "sprite");
        String imageSourceDropZone = altUnityDriver.findElement("Drop Image").getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotEquals(imageSource, imageSourceDropZone);

        imageSource = altUnityDriver.findElement("Drag Image2").getComponentProperty("UnityEngine.UI.Image", "sprite");
        imageSourceDropZone = altUnityDriver.findElement("Drop").getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotEquals(imageSource, imageSourceDropZone);

    }

    @Test
    public void testTestPointerEnterAndExit() throws Exception {
        AltUnityObject altElement = altUnityDriver.findElement("Drop Image");
        String color1 = altElement.getComponentProperty("DropMe", "highlightColor");
        altUnityDriver.findElement("Drop Image").pointerEnter();
        String color2 = altElement.getComponentProperty("DropMe", "highlightColor");
        assertNotEquals(color1,color2);
        altUnityDriver.findElement("Drop Image").pointerExit();
        String color3 = altElement.getComponentProperty("DropMe", "highlightColor");
        assertNotEquals(color3, color2);
        assertEquals(color1,color3);

    }
}
