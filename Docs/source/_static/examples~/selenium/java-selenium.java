import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.support.ui.WebDriverWait;
import org.openqa.selenium.support.ui.ExpectedConditions;
import com.alttester.AltDriver;
import java.time.Duration;

public class ConnectionSettingsPopupTest {
    private static WebDriver driver;
    private static AltDriver altDriver;
    
    public static void setUp() {
        driver = new ChromeDriver();
        driver.navigate().to("http://localhost:8080/index.html");
        
        // Set connection data in the app
        String appName = "my_app";
        String altServerHost = "127.0.0.1";
        String altServerPort = "13005";
        
        setConnectionData(altServerHost, altServerPort, appName);
        
        // Initialize AltDriver
        altDriver = new AltDriver(appName, Integer.parseInt(altServerPort));
    }
    
    public static void setConnectionData(String host, String port, String appName) {
        setConnectionData(host, port, appName, 60);
    }
    
    public static void setConnectionData(String host, String port, String appName, int implicitWaitTimeout) {
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
            
            // Press OK button
            WebElement okButton = driver.findElement(By.id("AltTesterOkButton"));
            okButton.click();
            
        } catch (Exception ex) {
            throw new RuntimeException("Error while setting connection data: " + ex.getMessage(), ex);
        }
    }
    
    public static void tearDown() {
        if (altDriver != null) {
            altDriver.stop();
        }
        if (driver != null) {
            driver.quit();
        }
    }
}