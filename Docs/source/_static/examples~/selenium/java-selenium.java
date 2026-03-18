import com.alttester.AltDriver;

import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.Assumptions;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.support.ui.WebDriverWait;
import org.openqa.selenium.support.ui.ExpectedConditions;
import java.time.Duration;

public class MyFirstTest {
    private static WebDriver driver;
    private static AltDriver altDriver;
    
    @BeforeAll
    public static void setUp() {
        try {
            driver = new ChromeDriver();
            driver.navigate().to("http://localhost:8000");

            // Set connection data in the app
            String appName = "__default__";
            String altServerHost = "127.0.0.1";
            String altServerPort = "13000";

            setConnectionData(altServerHost, altServerPort, appName);

            // Initialize AltDriver
            altDriver = new AltDriver(altServerHost, Integer.parseInt(altServerPort));
        } catch (Exception ex) {
            Assumptions.assumeTrue(false, "Test environment not ready (ChromeDriver/AltTester): " + ex.getMessage());
        }
    }
    
    public static void setConnectionData(String host, String port, String appName) {
        setConnectionData(host, port, appName, false, 60);
    }

    public static void setConnectionData(String host, String port, String appName, boolean dontShowThisAgain, int implicitWaitTimeout) {
        if (driver == null) {
            throw new IllegalArgumentException("Selenium driver cannot be null");
        }
        
        // Set implicit wait to ensure elements are found during connection setup
        driver.manage().timeouts().implicitlyWait(Duration.ofSeconds(implicitWaitTimeout));
        
        try {
            // Update host if provided
            if (host != null) {
                WebElement hostField = driver.findElement(By.id("AltTesterHostInputField"));
                hostField.clear();
                hostField.sendKeys(host);
            }
            
            // Update port if provided
            if (port != null) {
                WebElement portField = driver.findElement(By.id("AltTesterPortInputField"));
                portField.clear();
                portField.sendKeys(port);
            }
            
            // Update app_name if provided
            if (appName != null) {
                WebElement appNameField = driver.findElement(By.id("AltTesterAppNameInputField"));
                appNameField.clear();
                appNameField.sendKeys(appName);
            }
            
            // Set "Don't show this again" if specified
            if (dontShowThisAgain) {
                WebElement dontShowAgainCheckbox = driver.findElement(By.id("AltTesterDontShowAgainCheckbox"));
                if (!dontShowAgainCheckbox.isSelected()) {
                    dontShowAgainCheckbox.click();
                }
            }

            // Press OK button
            WebElement okButton = driver.findElement(By.id("AltTesterOkButton"));
            okButton.click();
            
        } catch (Exception ex) {
            throw new RuntimeException("Error while setting connection data: " + ex.getMessage(), ex);
        }
    }

    @AfterAll
    public static void tearDown() throws Exception {
        if (altDriver != null) {
            altDriver.stop();
        }
        if (driver != null) {
            driver.quit();
        }
    }
}