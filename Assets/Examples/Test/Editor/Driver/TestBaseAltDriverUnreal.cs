/*
    Copyright(C) 2024 Altom Consulting

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

using System.Threading;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using AltTester.AltTesterUnitySDK.Driver.Tests;
using NUnit.Framework;

public class TestBaseAltDriverUnreal
{
    protected AltDriverUnreal altDriver;
    protected string sceneName;
    private int defaultCommandResponseTimeout = 5;

    [OneTimeSetUp]
    public void SetUp()
    {
        altDriver = TestsHelper.GetAltDriverUnreal();
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
        if (!string.IsNullOrEmpty(sceneName)) // Ensure sceneName is not null or empty
        {
            // altDriver.ResetInput();
            altDriver.SetCommandResponseTimeout(defaultCommandResponseTimeout);
            altDriver.LoadLevel(sceneName);
        }
    }
}
