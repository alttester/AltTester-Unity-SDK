import org.junit.AfterClass;
import org.junit.Assert;
import org.junit.BeforeClass;
import org.junit.Test;
import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.AltUnityObject;

import java.io.IOException;


public class myFirstTest {

    private static AltUnityDriver altdriver;

    @BeforeClass
    public static void setUp() throws IOException {
        AltUnityDriver.setupPortForwarding("android", "", 13000, 13000);
        altdriver = new AltUnityDriver("127.0.0.1", 13000, ";", "&", true);
    }

    @Test
    public void openClosePanelTest() {
        altdriver.loadScene("Scene 2 Draggable Panel");

        AltUnityObject closePanelButton = altdriver.findObject(AltUnityDriver.By.NAME, "Close Button").tap();
        AltUnityObject togglePanelButton = altdriver.findObject(AltUnityDriver.By.NAME, "Button").tap();
        AltUnityObject panelElement = altdriver.findObject(AltUnityDriver.By.NAME, "Panel");
        
        Assert.assertTrue(altdriver.waitForObject(AltUnityDriver.By.NAME, "Panel").isEnabled());
    }

    @AfterClass
    public static void tearDown() throws Exception {
        AltUnityDriver.removePortForwarding();
        altdriver.stop();
        Thread.sleep(1000);
    }

}