/*
    Copyright(C) 2023 Altom Consulting

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

using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using AltTester.AltTesterUnitySDK.Driver.Tests;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Threading;
using System;
using System.IO;


public class TestBase
{
    protected AltDriver altDriver;
    protected string sceneName;
    private static bool setupExecuted = false;
    protected RemoteWebDriver driver;   
    [OneTimeSetUp] 
    public void SetUp()
    {
        if (!setupExecuted && (Environment.GetEnvironmentVariable("RUN_WEBGL_IN_BROWSERSTACK") == "true")){
        DriverOptions capability = new OpenQA.Selenium.Chrome.ChromeOptions();
            capability.BrowserVersion = "latest";
            capability.AddAdditionalOption("bstack:options", capability);
            driver = new RemoteWebDriver(
            capability
            );
            driver.Navigate().GoToUrl("http://localhost:8360/index.html");
            setupExecuted = true;
        }
        altDriver = TestsHelper.GetAltDriver();
        DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Info);
        DriverLogManager.SetMinLogLevel(AltLogger.Unity, AltLogLevel.Info);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        altDriver.Stop();
    }

    [SetUp]
    protected void LoadLevel()
    {   
        driver.Manage().Window.Maximize();
        altDriver.ResetInput();
        altDriver.SetCommandResponseTimeout(60);
        altDriver.LoadScene(this.sceneName, true);
    }
}
