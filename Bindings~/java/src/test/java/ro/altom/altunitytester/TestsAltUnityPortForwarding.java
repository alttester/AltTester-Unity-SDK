package ro.altom.altunitytester;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.fail;

import java.nio.file.Paths;

import org.junit.Before;
import org.junit.After;
import org.junit.AfterClass;
import org.junit.BeforeClass;
import org.junit.Test;

import static com.github.stefanbirkner.systemlambda.SystemLambda.withEnvironmentVariable;

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
        withEnvironmentVariable("ANDROID_SDK_ROOT", androidSdkRoot)
                .execute(() -> assertEquals(Paths.get(androidSdkRoot, "platform-tools", "adb").toString(),
                        AltUnityPortForwarding.getAdbPath("")));
    }

    @Test
    public void testRemoveForwardAndroid() {
        AltUnityPortForwarding.forwardAndroid();
        AltUnityPortForwarding.removeForwardAndroid(13000);
        try {
            AltUnityDriverParams params = new AltUnityDriverParams();
            params.connectTimeout = 2;
            AltUnityDriver driver = new AltUnityDriver(params);
            driver.stop();
        } catch (Exception ex) {
            assertEquals("Could not create connection to 127.0.0.1:13000", ex.getMessage());
        }
    }

    @Test
    public void testForwardAndroid() {
        AltUnityPortForwarding.forwardAndroid();

        try {
            AltUnityDriverParams params = new AltUnityDriverParams();
            params.connectTimeout = 2;
            AltUnityDriver driver = new AltUnityDriver(params);
            driver.stop();
        } catch (Exception ex) {
            ex.printStackTrace();
            fail("ForwardAndroid failed: " + ex.toString());
        }
    }
}