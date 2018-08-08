package ro.altom.altunitytester;

import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.io.IOException;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotEquals;

public class TestsSampleScene4 {
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
        altUnityDriver.loadScene("Scene 4 GameWithMobileSingleStickControls");
    }

    @Test
    public void testPointerDownAndUp() throws Exception {

        AltUnityObject cube = altUnityDriver.findElement("Cube");
        String localScale = cube.getComponentProperty("UnityEngine.Transform", "localScale");
        altUnityDriver.findElement("JumpButton").pointerDown();
        cube = altUnityDriver.findElement("Cube");
        String localScaleClick = cube.getComponentProperty( "UnityEngine.Transform", "localScale");
        assertNotEquals(localScale, localScaleClick);
        altUnityDriver.findElement("JumpButton").pointerUp();
        cube = altUnityDriver.findElement("Cube");
        String localScaleRelease = cube.getComponentProperty( "UnityEngine.Transform", "localScale");


        assertNotEquals(localScaleRelease, localScaleClick);
        assertEquals(localScale, localScaleRelease);
    }

    @Test
    public void testDragAndRelease() throws Exception {
        AltUnityObject altElement = altUnityDriver.findElement("Cube");
        String velocityStringStart = altElement.getComponentProperty( "UnityEngine.Rigidbody2D", "velocity");
        altElement = altUnityDriver.findElement("MobileJoystick");
        float Xjoystick = altElement.x;
        float Yjoystick = altElement.y;



        altUnityDriver.findElement("MobileJoystick").drag(200, 200);
        Thread.sleep(100);

        altElement = altUnityDriver.findElement("MobileJoystick");

        float XjoystickDuringDrag = altElement.x;
        float YjoystickDuringDrag = altElement.y;

        altElement = altUnityDriver.findElement("Cube");
        String velocityStringDuringDrag = altElement.getComponentProperty("UnityEngine.Rigidbody2D", "velocity");

        assertNotEquals(velocityStringDuringDrag, velocityStringStart);
        assertNotEquals(Xjoystick, XjoystickDuringDrag);
        assertNotEquals(Yjoystick, YjoystickDuringDrag);

        altUnityDriver.findElement("MobileJoystick").pointerUp();
        Thread.sleep(100);

        altElement = altUnityDriver.findElement("MobileJoystick");


        float XJoystickAfterDrop = altElement.x;
        float YJoystickAfterDrop = altElement.y;
        altElement = altUnityDriver.findElement("Cube");
        String velocityStringAfterDrop = altElement.getComponentProperty( "UnityEngine.Rigidbody2D", "velocity");


        assertNotEquals(velocityStringDuringDrag, velocityStringAfterDrop);
        assertNotEquals(XJoystickAfterDrop, XjoystickDuringDrag);
        assertNotEquals(YJoystickAfterDrop, YjoystickDuringDrag);

        assertEquals(velocityStringAfterDrop, velocityStringStart);
        assertEquals(Xjoystick, XJoystickAfterDrop);
        assertEquals(Yjoystick, YJoystickAfterDrop);
    }


}
