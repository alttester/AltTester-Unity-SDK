public static void setConnectionData(AppiumDriver appiumDriver,
                                    String host,
                                    String port,
                                    String appName,
                                    int implicitWaitTimeoutSeconds) {
    if (appiumDriver == null) {
        throw new IllegalArgumentException("Appium driver cannot be null");
    }

    // Set a longer implicit wait to ensure elements are found during connection setup
    appiumDriver.manage().timeouts()
            .implicitlyWait(Duration.ofSeconds(implicitWaitTimeoutSeconds));

    try {
        // Update host if provided
        if (host != null && !host.isEmpty()) {
            WebElement hostField = appiumDriver.findElement(AppiumBy.accessibilityId("AltTesterHostInputField"));
            hostField.clear();
            hostField.sendKeys(host);
        }

        // Update port if provided
        if (port != null && !port.isEmpty()) {
            WebElement portField = appiumDriver.findElement(AppiumBy.accessibilityId("AltTesterPortInputField"));
            portField.clear();
            portField.sendKeys(port);
        }

        // Update app name if provided
        if (appName != null && !appName.isEmpty()) {
            WebElement appNameField = appiumDriver.findElement(AppiumBy.accessibilityId("AltTesterAppNameInputField"));
            appNameField.clear();
            appNameField.sendKeys(appName);
        }

        // Press OK button
        WebElement okButton = appiumDriver.findElement(AppiumBy.accessibilityId("AltTesterOkButton"));
        okButton.click();

    } catch (Exception ex) {
        throw new RuntimeException("Error while setting connection data: " + ex.getMessage(), ex);
    }
}