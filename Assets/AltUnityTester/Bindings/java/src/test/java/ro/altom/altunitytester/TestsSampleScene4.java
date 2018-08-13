package ro.altom.altunitytester;

import com.google.gson.Gson;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import java.io.IOException;

import static org.junit.Assert.*;
public class TestsSampleScene4 {
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
        altUnityDriver.loadScene("Scene 4 GameWithMobileSingleStickControls");
    }

    @Test
    public void testPointerDownAndUp() throws Exception {

        AltUnityObject cube = altUnityDriver.findElement("Cube");
        String localScale = cube.getComponentProperty("UnityEngine.Transform", "localScale");
        altUnityDriver.findElement("JumpButton").pointerDown();
        cube = altUnityDriver.findElement("Cube");
        String localScaleClick = cube.getComponentProperty( "UnityEngine.Transform", "localScale");
        assertNotSame(localScale, localScaleClick);
        altUnityDriver.findElement("JumpButton").pointerUp();
        cube = altUnityDriver.findElement("Cube");
        String localScaleRelease = cube.getComponentProperty( "UnityEngine.Transform", "localScale");


        assertNotSame(localScaleRelease, localScaleClick);
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

        assertNotSame(velocityStringDuringDrag, velocityStringStart);
        assertNotSame(Xjoystick, XjoystickDuringDrag);
        assertNotSame(Yjoystick, YjoystickDuringDrag);

        altUnityDriver.findElement("MobileJoystick").pointerUp();
        Thread.sleep(100);

        altElement = altUnityDriver.findElement("MobileJoystick");


        float XJoystickAfterDrop = altElement.x;
        float YJoystickAfterDrop = altElement.y;
        altElement = altUnityDriver.findElement("Cube");
        String velocityStringAfterDrop = altElement.getComponentProperty( "UnityEngine.Rigidbody2D", "velocity");


        assertNotSame(velocityStringDuringDrag, velocityStringAfterDrop);
        assertNotSame(XJoystickAfterDrop, XjoystickDuringDrag);
        assertNotSame(YJoystickAfterDrop, YjoystickDuringDrag);

        assertEquals(velocityStringAfterDrop, velocityStringStart);
        assertEquals(Xjoystick, XJoystickAfterDrop, 0.01);
        assertEquals(Yjoystick, YJoystickAfterDrop, 0.01);
    }


}
