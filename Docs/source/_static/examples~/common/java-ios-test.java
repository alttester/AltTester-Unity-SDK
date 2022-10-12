import org.junit.AfterClass;
import org.junit.Assert;
import org.junit.BeforeClass;
import org.junit.Test;
import ro.altom.alttester.AltPortForwarding;
import ro.altom.alttester.AltDriver;
import ro.altom.alttester.AltObject;
import ro.altom.alttester.Commands.FindObject.AltFindObjectsParameters;
import ro.altom.alttester.Commands.FindObject.AltWaitForObjectsParameters;

import java.io.IOException;

public class myFirstTest {

    private static AltDriver altDriver;

    @BeforeClass
    public static void setUp() throws IOException {
        AltPortForwarding.forwardIos();
        altDriver = new AltDriver();
    }

    @AfterClass
    public static void tearDown() throws Exception {
        altDriver.stop();
        AltPortForwarding.killAllIproxyProcess();
    }

    @Test
    public void openClosePanelTest() {
        altDriver.loadScene("Scene 2 Draggable Panel");

        AltFindObjectsParameters altFindObjectsParametersCamera = new AltFindObjectsParameters
                .Builder(AltDriver.By.PATH, "//Main Camera")
                .build();
        AltObject camera = altDriver.findObject(altFindObjectsParametersCamera);

        AltFindObjectsParameters closeButtonObjectsParameters = new AltFindObjectsParameters
                .Builder(AltDriver.By.NAME, "Close Button")
                .withCamera(AltDriver.By.ID, String.valueOf(camera.id))
                .build();
        altDriver.findObject(closeButtonObjectsParameters).tap();

        AltFindObjectsParameters buttonObjectsParameters = new AltFindObjectsParameters
                .Builder(AltDriver.By.NAME, "Button")
                .withCamera(AltDriver.By.ID, String.valueOf(camera.id))
                .build();
        altDriver.findObject(buttonObjectsParameters).tap();

        AltFindObjectsParameters panelObjectsParameters = new AltFindObjectsParameters
                .Builder(AltDriver.By.NAME, "Panel")
                .withCamera(AltDriver.By.ID, String.valueOf(camera.id))
                .build();
        AltWaitForObjectsParameters panelWaitForObjectsParameters = new AltWaitForObjectsParameters
                .Builder(panelObjectsParameters).build();
        AltObject panelElement = altDriver.waitForObject(panelWaitForObjectsParameters);

        Assert.assertTrue(panelElement.isEnabled());
    }
}