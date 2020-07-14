import org.junit.AfterClass;
import org.junit.Assert;
import org.junit.BeforeClass;
import org.junit.Test;
import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.AltUnityObject;

import java.io.IOException;

public class MyFirstTest {
    private static AltUnityDriver altdriver;

    @BeforeClass
    public static void setUp() throws IOException {
        altdriver = new AltUnityDriver();
    }

    @AfterClass
    public static void tearDown() throws Exception {
        altdriver.stop();
    }

    @Test
    public void openClosePanelTest() {
        altdriver.loadScene("Scene 2 Draggable Panel");

        altdriver.findObject(AltUnityDriver.By.NAME, "Close Button").tap();
        altdriver.findObject(AltUnityDriver.By.NAME, "Button").tap();

        AltUnityObject panelElement = altdriver.waitForObject(AltUnityDriver.By.NAME, "Panel");
        Assert.assertTrue(panelElement.isEnabled());
    }
}