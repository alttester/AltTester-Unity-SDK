/*
    Copyright(C) 2025 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AltTester.AltTesterSDK.Driver
{
    /// <summary>
    /// API to interact with the native popup dialog (implemented by AltTester) using Appium
    /// </summary>
    public class AltAppiumHelper
    {
        /// <summary>
        /// Sets connection data in the native popup dialog
        /// </summary>
        /// <param name="appiumDriver">The Appium driver instance</param>
        /// <param name="platform">The platform (Android or iOS)</param>
        /// <param name="host">The host value to set. If not provided, the host field won't be updated</param>
        /// <param name="port">The port value to set. If not provided, the port field won't be updated</param>
        /// <param name="appName">The app name value to set. If not provided, the app name field won't be updated</param>
        /// <param name="timeout">Timeout in seconds for waiting for elements (default: 60)</param>
        /// <exception cref="ArgumentNullException">Thrown when appiumDriver is null</exception>
        /// <exception cref="ArgumentException">Thrown when no fields are provided for update</exception>
        /// <exception cref="ArgumentException">Thrown when host or port is invalid</exception>
        /// <exception cref="AppiumHelperException">Thrown when an error occurs while interacting with the popup</exception>
        public static void SetConnectionData(AppiumDriver appiumDriver, string platform, string host = null, string port = null, string appName = null, int timeout = 60)
        {
            if (appiumDriver == null)
            {
                throw new ArgumentNullException(nameof(appiumDriver), "Appium driver cannot be null");
            }

            // Check that at least one field is provided
            if (string.IsNullOrEmpty(host) && string.IsNullOrEmpty(port) && string.IsNullOrEmpty(appName))
            {
                throw new ArgumentException("At least one of 'host', 'port', or 'appName' must be provided");
            }

            // Validate connection data
            if (!string.IsNullOrEmpty(host) && Uri.CheckHostName(host) == UriHostNameType.Unknown)
            {
                throw new ArgumentException($"Invalid host: {host}. The host should be a valid host.");
            }

            if (!string.IsNullOrEmpty(port))
            {
                if (!int.TryParse(port, out int portInt) || portInt <= 0 || portInt > 65535)
                {
                    throw new ArgumentException($"Invalid port: {port}. The port number should be between 1 and 65535.");
                }
            }

            if (!platform.Equals("Android", StringComparison.OrdinalIgnoreCase) &&
                !platform.Equals("iOS", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Unsupported platform: {platform}. Supported platforms are 'Android' and 'iOS'.");
            }
            
            try
            {
                // Set XPath based on platform
                string hostXPath = platform.Equals("iOS", StringComparison.OrdinalIgnoreCase)
                    ? "//XCUIElementTypeTextField[@value=\"Host\"]"
                    : "//android.widget.EditText[@text=\"Host\"]";

                string portXPath = platform.Equals("iOS", StringComparison.OrdinalIgnoreCase)
                    ? "//XCUIElementTypeTextField[@value=\"Port\"]"
                    : "//android.widget.EditText[@text=\"Port\"]";

                string appNameXPath = platform.Equals("iOS", StringComparison.OrdinalIgnoreCase)
                    ? "//XCUIElementTypeTextField[@value=\"App Name\"]"
                    : "//android.widget.EditText[@text=\"App Name\"]";

                string okButtonXPath = platform.Equals("iOS", StringComparison.OrdinalIgnoreCase)
                    ? "//XCUIElementTypeButton[@name=\"OK\"]"
                    : "//android.widget.Button[@resource-id=\"android:id/button1\"]";

                // Wait for the connection dialog to be present
                var wait = new WebDriverWait(appiumDriver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.ElementExists(OpenQA.Selenium.By.XPath(hostXPath)));

                // Update host if provided
                if (!string.IsNullOrEmpty(host))
                {
                    var hostField = appiumDriver.FindElement(OpenQA.Selenium.By.XPath(hostXPath));
                    hostField.Clear();
                    hostField.SendKeys(host);
                }

                // Update port if provided
                if (!string.IsNullOrEmpty(port))
                {
                    var portField = appiumDriver.FindElement(OpenQA.Selenium.By.XPath(portXPath));
                    portField.Clear();
                    portField.SendKeys(port);
                }

                // Update app name if provided
                if (!string.IsNullOrEmpty(appName))
                {
                    var appNameField = appiumDriver.FindElement(OpenQA.Selenium.By.XPath(appNameXPath));
                    appNameField.Clear();
                    appNameField.SendKeys(appName);
                }

                // Press OK button
                var okButton = appiumDriver.FindElement(OpenQA.Selenium.By.XPath(okButtonXPath));
                okButton.Click();
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppiumHelperException($"Error while setting connection data on {platform} platform: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sets connection data in the native popup dialog with integer port
        /// </summary>
        /// <param name="appiumDriver">The Appium driver instance</param>
        /// <param name="platform">The platform (Android or iOS)</param>
        /// <param name="host">The host value to set. If not provided, the host field won't be updated</param>
        /// <param name="port">The port value to set. If not provided, the port field won't be updated</param>
        /// <param name="appName">The app name value to set. If not provided, the app name field won't be updated</param>
        /// <param name="timeout">Timeout in seconds for waiting for elements (default: 60)</param>
        public static void SetConnectionData(AppiumDriver appiumDriver, string platform, string host = null, int? port = null, string appName = null, int timeout = 60)
        {
            SetConnectionData(appiumDriver, platform, host, port?.ToString(), appName, timeout);
        }
    }

    /// <summary>
    /// Exception thrown when an error occurs while using AltAppiumHelper
    /// </summary>
    public class AppiumHelperException : Exception
    {
        public AppiumHelperException(string message) : base(message)
        {
        }

        public AppiumHelperException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
