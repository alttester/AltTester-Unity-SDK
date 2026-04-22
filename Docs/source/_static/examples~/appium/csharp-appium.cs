public static void SetConnectionData(AppiumDriver appiumDriver, string? host = null, string? port = null, string? appName = null, bool dontShowThisAgain = false, int timeout = 60)
{
    if (appiumDriver == null)
    {
        throw new ArgumentNullException(nameof(appiumDriver), "Appium driver cannot be null");
    }

    // Set a longer implicit wait to ensure elements are found during connection setup
    appiumDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeout);

    try
    {
        // Update host if provided
        if (!string.IsNullOrEmpty(host))
        {
            var hostField = appiumDriver.FindElement(MobileBy.AccessibilityId("AltTesterHostInputField"));
            hostField.Clear();
            hostField.SendKeys(host);
        }

        // Update port if provided
        if (!string.IsNullOrEmpty(port))
        {
            var portField = appiumDriver.FindElement(MobileBy.AccessibilityId("AltTesterPortInputField"));
            portField.Clear();
            portField.SendKeys(port);
        }

        // Update app name if provided
        if (!string.IsNullOrEmpty(appName))
        {
            var appNameField = appiumDriver.FindElement(MobileBy.AccessibilityId("AltTesterAppNameInputField"));
            appNameField.Clear();
            appNameField.SendKeys(appName);
        }

        // Set "Don't show this again" if specified
        if (dontShowThisAgain)
        {
            var dontShowAgainCheckbox = appiumDriver.FindElement(MobileBy.AccessibilityId("AltTesterDontShowAgainCheckbox"));
            if (!dontShowAgainCheckbox.Selected)
            {
                dontShowAgainCheckbox.Click();
            }
        }

        // Press OK button
        var okButton = appiumDriver.FindElement(MobileBy.AccessibilityId("AltTesterOkButton"));
        okButton.Click();
    }
    catch (ArgumentException)
    {
        throw;
    }
    catch (Exception ex)
    {
        throw new AppiumHelperException($"Error while setting connection data: {ex.Message}", ex);
    }
}