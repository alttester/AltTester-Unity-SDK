import org.junit.AfterClass;
import org.junit.Assert;
import org.junit.BeforeClass;
import org.junit.Test;
import com.alttester.AltReversePortForwarding;
import com.alttester.AltDriver;
import com.alttester.AltObject;
import com.alttester.Commands.FindObject.AltFindObjectsParameters;
import com.alttester.Commands.FindObject.AltWaitForObjectsParameters;

import java.io.IOException;

public class MyFirstTest {

    private static AltDriver altDriver;

    @BeforeAll
    public static void setUp() throws IOException {
            AltReversePortForwarding.reversePortForwardingAndroid();
            altDriver = new AltDriver();
    }

    @AfterAll
    public static void tearDown() throws Exception {
            altDriver.stop();
            AltReversePortForwarding.removeReversePortForwardingAndroid();
    }

    @Test
    public void openClosePanelTest() {
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 2 Draggable Panel").build());

            AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(
                            AltDriver.By.PATH, "//Main Camera")
                            .build();
            AltObject camera = altDriver.findObject(altFindObjectsParametersCamera);

            AltFindObjectsParams closeButtonObjectsParameters = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME, "Close Button")
                            .withCamera(AltDriver.By.ID, String.valueOf(camera.id))
                            .build();
            altDriver.findObject(closeButtonObjectsParameters).tap();

            AltFindObjectsParams buttonObjectsParameters = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME, "Button")
                            .withCamera(AltDriver.By.ID, String.valueOf(camera.id))
                            .build();
            altDriver.findObject(buttonObjectsParameters).tap();

            AltFindObjectsParams panelObjectsParameters = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME, "Panel")
                            .withCamera(AltDriver.By.ID, String.valueOf(camera.id))
                            .build();
            AltWaitForObjectsParams panelWaitForObjectsParameters = new AltWaitForObjectsParams.Builder(
                            panelObjectsParameters).build();
            AltObject panelElement = altDriver.waitForObject(panelWaitForObjectsParameters);

            Assertions.assertTrue(panelElement.isEnabled());
    }
}