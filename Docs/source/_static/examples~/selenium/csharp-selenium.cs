using AltTester.AltTesterSDK.Driver;
using OpenQA.Selenium.Chrome;

public class MyFirstTest
{
    private static ChromeDriver driver;
    private static AltDriver altDriver;

    [OneTimeSetUp]
    public void SetUp()
    {
        driver = new ChromeDriver();
        driver.Navigate().GoToUrl("http://localhost:8000");

        // Set connection data in the app
        string appName = "__default__";
        string altServerHost = "127.0.0.1";
        string altServerPort = "13000";

        SetConnectionData(altServerHost, altServerPort, appName);

        // Initialize AltDriver
        altDriver = new AltDriver(host: altServerHost, port: int.Parse(altServerPort), appName: appName);
    }

    private void SetConnectionData(string? host = null, string? port = null, string? appName = null, bool dontShowThisAgain = false, int implicitWaitTimeout = 60)
    {
        if (driver == null)
        {
            throw new ArgumentNullException(nameof(driver), "Selenium driver cannot be null");
        }

        // Set a longer implicit wait to ensure elements are found during connection setup
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(implicitWaitTimeout);

        try
        {
            // Update host if provided
            if (host != null)
            {
                var hostField = driver.FindElement(OpenQA.Selenium.By.Id("AltTesterHostInputField"));
                hostField.Clear();
                hostField.SendKeys(host);
            }

            // Update port if provided
            if (port != null)
            {
                var portField = driver.FindElement(OpenQA.Selenium.By.Id("AltTesterPortInputField"));
                portField.Clear();
                portField.SendKeys(port);
            }

            // Update app_name if provided
            if (appName != null)
            {
                var appNameField = driver.FindElement(OpenQA.Selenium.By.Id("AltTesterAppNameInputField"));
                appNameField.Clear();
                appNameField.SendKeys(appName);
            }

            // Set "Don't show this again" if specified
            if (dontShowThisAgain)
            {
                var dontShowAgainCheckbox = driver.FindElement(OpenQA.Selenium.By.Id("AltTesterDontShowAgainCheckbox"));
                if (!dontShowAgainCheckbox.Selected)
                {
                    dontShowAgainCheckbox.Click();
                }
            }

            // Press OK button
            var okButton = driver.FindElement(OpenQA.Selenium.By.Id("AltTesterOkButton"));
            okButton.Click();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while setting connection data: {ex.Message}", ex);
        }
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        altDriver?.Stop();
        driver?.Dispose();
    }
}