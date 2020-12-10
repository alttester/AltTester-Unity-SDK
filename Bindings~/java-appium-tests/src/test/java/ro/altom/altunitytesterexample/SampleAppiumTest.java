package ro.altom.altunitytesterexample;

import io.appium.java_client.MobileElement;
import io.appium.java_client.TouchAction;
import io.appium.java_client.android.AndroidDriver;
import io.appium.java_client.touch.offset.PointOption;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import org.openqa.selenium.remote.CapabilityType;
import org.openqa.selenium.remote.DesiredCapabilities;
import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParameters;
import ro.altom.altunitytester.Commands.FindObject.AltWaitForObjectWithTextParameters;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParameters;

import java.io.File;
import java.io.IOException;
import java.net.URL;
import java.util.concurrent.TimeUnit;

import static org.junit.Assert.*;

public class SampleAppiumTest {

    private static AltUnityDriver altUnityDriver;
    private static AndroidDriver appiumDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        AltUnityDriver.setupPortForwarding("android", "", 13000, 13000);
        File app = new File("../../sampleGame.apk");
        DesiredCapabilities capabilities = new DesiredCapabilities();
        capabilities.setCapability(CapabilityType.BROWSER_NAME, "");
        capabilities.setCapability("deviceName", "Android");
        capabilities.setCapability("platformName", "Android");
        capabilities.setCapability("app", app.getAbsolutePath());
        AltUnityDriver.setupPortForwarding("android", "", 13000, 13000);
        appiumDriver = new AndroidDriver<MobileElement>(new URL("http://127.0.0.1:4723/wd/hub"), capabilities);
        appiumDriver.manage().timeouts().implicitlyWait(80, TimeUnit.SECONDS);
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13000);
    }

    @AfterClass
    public static void tearDown() throws Exception {
        altUnityDriver.stop();
        appiumDriver.quit();
        AltUnityDriver.removePortForwarding();
    }

    @Before
    public void loadLevel() throws Exception {
        AltLoadSceneParameters params = new AltLoadSceneParameters.Builder("Scene 1 AltUnityDriverTestScene").build();
        altUnityDriver.loadScene(params);
    }

    @Test
    public void testTapOnButton() throws Exception {
        assertEquals("Scene 1 AltUnityDriverTestScene", altUnityDriver.getCurrentScene());

        AltFindObjectsParameters altFindObjectsParameters = 
            new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,"Capsule").build();
        AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters);
        TouchAction tapButton = new TouchAction(appiumDriver);
        tapButton.tap(new PointOption().withCoordinates(capsule.x, capsule.mobileY)).perform();

        AltFindObjectsParameters altFindObjectsParameters2 = 
            new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, "CapsuleInfo").build();
        AltWaitForObjectWithTextParameters altWaitForObjectsParameters = 
            new AltWaitForObjectWithTextParameters.Builder(altFindObjectsParameters2, "Capsule was clicked to jump!").build();
        String text = altUnityDriver.waitForObjectWithText(altWaitForObjectsParameters).getText();
        assertEquals("Capsule was clicked to jump!", text);
    }
}
