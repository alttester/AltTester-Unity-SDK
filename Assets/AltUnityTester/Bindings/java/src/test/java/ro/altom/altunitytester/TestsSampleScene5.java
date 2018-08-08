package ro.altom.altunitytester;

import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.io.IOException;

import static org.junit.jupiter.api.Assertions.assertNotEquals;

public class TestsSampleScene5 {
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
        altUnityDriver.loadScene("Scene 5 GameWithCarTiltControls");
    }

    @Test
    public void testSwipe() throws Exception {
        AltUnityObject camera = altUnityDriver.findElement("Main Camera");
        String rotation = camera.getComponentProperty("UnityEngine.Transform", "rotation");

        AltUnityObject altUnityObject = altUnityDriver.findElement("LookUpAndDownTouchpad");

        altUnityDriver.swipe(altUnityObject.x, altUnityObject.mobileY, altUnityObject.x + 100, altUnityObject.mobileY, 2);

        camera = altUnityDriver.findElement("Main Camera");
        Thread.sleep(1000);
        String rotationAfter = camera.getComponentProperty( "UnityEngine.Transform", "rotation");

        assertNotEquals(rotation, rotationAfter);
    }

    @Test
    public void testTilt() throws Exception {

        AltUnityObject cube = altUnityDriver.findElement("Cube");
        String position = cube.getComponentProperty("UnityEngine.Transform", "position");

        altUnityDriver.tilt(2, 2, 2);
        Thread.sleep(1000);
        altUnityDriver.tilt(0, 0, 0);
        cube = altUnityDriver.findElement("Cube");

        String positionAfter = cube.getComponentProperty( "UnityEngine.Transform", "position");

        assertNotEquals(position, positionAfter);
    }

    @Test
    public void testAcceleration() throws Exception {

        AltUnityObject cube = altUnityDriver.findElement("Cube");
        String position = cube.getComponentProperty("UnityEngine.Transform", "position");


        altUnityDriver.findElement("Accelerator").pointerDown();
        Thread.sleep(1000);
        altUnityDriver.findElement("Accelerator").pointerUp();
        cube = altUnityDriver.findElement("Cube");
        String positionAfter = cube.getComponentProperty("UnityEngine.Transform", "position");
        assertNotEquals(position, positionAfter);
    }

    @Test
    public void testBrake() throws Exception
    {
        AltUnityObject cube = altUnityDriver.findElement("Cube");
        String position = cube.getComponentProperty("UnityEngine.Transform", "position");
        altUnityDriver.findElement("Brake").pointerDown();
        Thread.sleep(1000);
        altUnityDriver.findElement("Brake").pointerUp();
        cube = altUnityDriver.findElement("Cube");
        String positionAfter = cube.getComponentProperty("UnityEngine.Transform", "position");
        assertNotEquals(position, positionAfter);
    }

}
