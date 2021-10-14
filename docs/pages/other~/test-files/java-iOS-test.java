import org.junit.AfterClass;
import org.junit.Assert;
import org.junit.BeforeClass;
import org.junit.Test;
import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParameters;
import ro.altom.altunitytester.Commands.FindObject.AltWaitForObjectsParameters;

import java.io.IOException;

public class myFirstTest {

  private static AltUnityDriver altUnityDriver;

  @BeforeClass
  public static void setUp() throws IOException {
    altUnityDriver = new AltUnityDriver();
  }

  @AfterClass
  public static void tearDown() throws Exception {
    altUnityDriver.stop();
  }

  @Test
  public void openClosePanelTest() {

      altUnityDriver.loadScene("Scene 2 Draggable Panel");

      AltFindObjectsParameters altFindObjectsParametersCamera = new AltFindObjectsParameters
              .Builder(AltUnityDriver.By.PATH, "//Main Camera")
              .build();
      AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);

      AltFindObjectsParameters closeButtonObjectsParameters = new AltFindObjectsParameters
              .Builder(AltUnityDriver.By.NAME, "Close Button")
              .withCamera(AltUnityDriver.By.ID, String.valueOf(camera.id))
              .build();
      altUnityDriver.findObject(closeButtonObjectsParameters).tap();

      AltFindObjectsParameters buttonObjectsParameters = new AltFindObjectsParameters
              .Builder(AltUnityDriver.By.NAME, "Button")
              .withCamera(AltUnityDriver.By.ID, String.valueOf(camera.id))
              .build();
      altUnityDriver.findObject(buttonObjectsParameters).tap();

      AltFindObjectsParameters panelObjectsParameters = new AltFindObjectsParameters
              .Builder(AltUnityDriver.By.NAME, "Panel")
              .withCamera(AltUnityDriver.By.ID, String.valueOf(camera.id))
              .build();
      AltWaitForObjectsParameters panelWaitForObjectsParameters = new AltWaitForObjectsParameters
              .Builder(panelObjectsParameters).build();
      AltUnityObject panelElement = altUnityDriver.waitForObject(panelWaitForObjectsParameters);

      Assert.assertTrue(panelElement.isEnabled());
  }
}