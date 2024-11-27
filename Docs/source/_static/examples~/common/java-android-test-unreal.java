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
            altDriver.loadScene(new AltLoadSceneParams.Builder("MainMenu").build());

            AltFindObjectsParams closeButtonObjectsParameters = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME, "Close Button")
                            .build();
            altDriver.findObject(closeButtonObjectsParameters).Click();

            AltFindObjectsParams buttonObjectsParameters = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME, "Button")
                            .build();
            altDriver.findObject(buttonObjectsParameters).Click();

            AltFindObjectsParams panelObjectsParameters = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME, "Panel")
                            .build();
            AltWaitForObjectsParams panelWaitForObjectsParameters = new AltWaitForObjectsParams.Builder(
                            panelObjectsParameters).build();
            AltObject panelElement = altDriver.waitForObject(panelWaitForObjectsParameters);

            Assertions.assertTrue(panelElement.isEnabled());
    }
}