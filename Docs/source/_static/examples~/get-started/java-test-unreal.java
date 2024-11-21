import org.junit.AfterClass;
import org.junit.Assert;
import org.junit.BeforeClass;
import org.junit.Test;
import com.alttester.AltDriver;
import com.alttester.AltObject;
import com.alttester.Commands.FindObject.AltFindObjectsParameters;
import com.alttester.Commands.FindObject.AltWaitForObjectsParameters;

import java.io.IOException;

public class myFirstTest {

        private static AltDriver altDriver;

        @BeforeClass
        public static void setUp() throws IOException {
                altDriver = new AltDriver();
        }

        @AfterClass
        public static void tearDown() throws Exception {
                altDriver.stop();
        }

        @Test
        public void openClosePanelTest() {
                altDriver.loadScene("MainMenu");

                AltFindObjectsParameters closeButtonObjectsParameters = new AltFindObjectsParameters.Builder(
                                AltDriver.By.NAME, "Close Button")
                                .build();
                altDriver.findObject(closeButtonObjectsParameters).Click();

                AltFindObjectsParameters buttonObjectsParameters = new AltFindObjectsParameters.Builder(
                                AltDriver.By.NAME, "Button")
                                .build();
                altDriver.findObject(buttonObjectsParameters).Click();

                AltFindObjectsParameters panelObjectsParameters = new AltFindObjectsParameters.Builder(
                                AltDriver.By.NAME, "Panel")
                                .build();
                AltWaitForObjectsParameters panelWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                                panelObjectsParameters).build();
                AltObject panelElement = altDriver.waitForObject(panelWaitForObjectsParameters);

                Assert.assertTrue(panelElement.isEnabled());
        }
}