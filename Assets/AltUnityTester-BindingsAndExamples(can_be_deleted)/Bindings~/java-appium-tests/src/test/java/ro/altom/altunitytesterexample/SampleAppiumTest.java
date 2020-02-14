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

import java.io.File;
import java.io.IOException;
import java.net.URL;
import java.util.concurrent.TimeUnit;

import static org.junit.Assert.*;

public class SampleAppiumTest {

    private static AltUnityDriver altUnityDriver;
    private static AndroidDriver appiumDriver;

    @BeforeClass
    public static void setUp() throws Exception{
        AltUnityDriver.setupPortForwarding("android", "", 13000, 13000);
        File app = new File("../../../../sampleGame.apk");
        DesiredCapabilities capabilities = new DesiredCapabilities();
        capabilities.setCapability(CapabilityType.BROWSER_NAME, "");
        capabilities.setCapability("deviceName", "Android");
        capabilities.setCapability("platformName", "Android");
        capabilities.setCapability("app", app.getAbsolutePath());
        appiumDriver = new AndroidDriver<MobileElement>(new URL("http://127.0.0.1:4723/wd/hub"), capabilities);
        appiumDriver.manage().timeouts().implicitlyWait(80, TimeUnit.SECONDS);
        Thread.sleep(10000);
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13000);
    }

    @AfterClass
    public static void tearDown() throws Exception {
        altUnityDriver.stop();
        appiumDriver.quit();
    }

    @Before
    public void loadLevel() throws Exception {
        altUnityDriver.loadScene("Scene 1 AltUnityDriverTestScene");
    }

    @Test
    public void testTapOnButton() throws Exception {
        assertEquals("Scene 1 AltUnityDriverTestScene", altUnityDriver.getCurrentScene());
        AltUnityObject jumpButton = altUnityDriver.findObject(AltUnityDriver.By.NAME,"UIButton");
        TouchAction tapButton = new TouchAction(appiumDriver);
        tapButton.tap(new PointOption().withCoordinates(jumpButton.x, jumpButton.mobileY)).perform();
        String text=altUnityDriver.waitForObjectWithText(AltUnityDriver.By.NAME,"CapsuleInfo", "UIButton clicked to jump capsule!").getText();
        assertEquals("UIButton clicked to jump capsule!",text);
    }
}

