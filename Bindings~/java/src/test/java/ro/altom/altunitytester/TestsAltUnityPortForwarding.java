package ro.altom.altunitytester;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.fail;

import java.nio.file.Paths;

import org.junit.Before;
import org.junit.After;
import org.junit.AfterClass;
import org.junit.BeforeClass;
import org.junit.Test;

import javax.websocket.CloseReason;
import javax.websocket.CloseReason.CloseCodes;

import com.github.stefanbirkner.systemlambda.SystemLambda;

public class TestsAltUnityPortForwarding {

    @BeforeClass
    public static void oneTimeSetUp() throws Exception {
    }

    @AfterClass
    public static void oneTimeTearDown() throws Exception {
        AltUnityPortForwarding.forwardAndroid();
    }

    @Before
    public void setUp() {
        AltUnityPortForwarding.removeAllForwardAndroid();
    }

    @After
    public void tearDown() {
        AltUnityPortForwarding.removeAllForwardAndroid();
    }


    @Test
    public void testGetAdbPath() {
        assertEquals("overwrite", AltUnityPortForwarding.getAdbPath("overwrite"));
    }

    @Test
    public void testGetAdbPathAndroidSdk() throws Exception {
        String androidSdkRoot = Paths.get("path", "to", "adb").toString();
        SystemLambda.withEnvironmentVariable("ANDROID_SDK_ROOT", androidSdkRoot)
                .execute(() -> assertEquals(Paths.get(androidSdkRoot, "platform-tools", "adb").toString(),
                        AltUnityPortForwarding.getAdbPath("")));
    }

    @Test
    public void testRemoveForwardAndroid() {
        AltUnityPortForwarding.forwardAndroid();
        AltUnityPortForwarding.removeForwardAndroid(13000);
        try {
            AltUnityDriver altUnityDriver = new AltUnityDriver();
            altUnityDriver.stop(new CloseReason(CloseCodes.getCloseCode(1000), "Could not create connection to [ws://127.0.0.1:13000/altws/]"));
        } catch (Exception ex) {
            assertEquals("Could not create connection to ws://127.0.0.1:13000/altws/", ex.getMessage());
        }
    }

    @Test
    public void testForwardAndroid() {
        AltUnityPortForwarding.forwardAndroid();
        try {
            AltUnityDriver altUnityDriver = new AltUnityDriver();
            altUnityDriver.stop(new CloseReason(CloseCodes.getCloseCode(1000), "ForwardAndroid failed"));
        } catch (Exception ex) {
            ex.printStackTrace();
            fail("ForwardAndroid failed: " + ex.toString());
        }
    }
}