import org.junit.AfterClass;
import org.junit.Assert;
import org.junit.BeforeClass;
import org.junit.Test;
import ro.altom.altunitytester.AltUnityPortForwarding;
import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParameters;
import ro.altom.altunitytester.Commands.FindObject.AltWaitForObjectsParameters;

import java.io.IOException;

public class myFirstTest {

  private static AltUnityDriver altdriver;

  @BeforeClass
  public static void setUp() throws IOException {
    AltUnityPortForwarding.forwardIos();
    altdriver = new AltUnityDriver();
  }

  @AfterClass
  public static void tearDown() throws Exception {
    altdriver.stop();
    AltUnityPortForwarding.killAllIproxyProcess();
  }

  @Test
  public void openClosePanelTest() {

      altdriver.loadScene("Scene 2 Draggable Panel");

      AltFindObjectsParameters altFindObjectsParametersCamera = new AltFindObjectsParameters
              .Builder(AltUnityDriver.By.PATH, "//Main Camera")
              .build();
      AltUnityObject camera = altdriver.findObject(altFindObjectsParametersCamera);

      AltFindObjectsParameters closeButtonObjectsParameters = new AltFindObjectsParameters
              .Builder(AltUnityDriver.By.NAME, "Close Button")
              .withCamera(AltUnityDriver.By.ID, String.valueOf(camera.id))
              .build();
      altdriver.findObject(closeButtonObjectsParameters).tap();

      AltFindObjectsParameters buttonObjectsParameters = new AltFindObjectsParameters
              .Builder(AltUnityDriver.By.NAME, "Button")
              .withCamera(AltUnityDriver.By.ID, String.valueOf(camera.id))
              .build();
      altdriver.findObject(buttonObjectsParameters).tap();

      AltFindObjectsParameters panelObjectsParameters = new AltFindObjectsParameters
              .Builder(AltUnityDriver.By.NAME, "Panel")
              .withCamera(AltUnityDriver.By.ID, String.valueOf(camera.id))
              .build();
      AltWaitForObjectsParameters panelWaitForObjectsParameters = new AltWaitForObjectsParameters
              .Builder(panelObjectsParameters).build();
      AltUnityObject panelElement = altdriver.waitForObject(panelWaitForObjectsParameters);

      Assert.assertTrue(panelElement.isEnabled());
  }
}