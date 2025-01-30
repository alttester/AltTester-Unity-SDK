/*
    Copyright(C) 2025 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using NUnit.Framework;


namespace AltTester.AltTesterUnitySDK.Driver.Tests
{
    public class AltBaseCommandImpl : AltBaseCommand
    {
        public AltBaseCommandImpl(IDriverCommunication comm) : base(comm)
        {
        }

        public void Validate(string expected, string received)
        {
            base.ValidateResponse(expected, received);
        }
    }

    [Timeout(1000)]
    public class TestAltBaseCommand
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DriverLogManager.SetMinLogLevel(AltLogger.Console, AltLogLevel.Debug);
        }

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void TestValidateResponse()
        {
            AltBaseCommandImpl cmd = new AltBaseCommandImpl(null);

            cmd.Validate("aa", "aa");
            try
            {
                cmd.Validate("aa", "bb");
                Assert.Fail();
            }

            catch (AltInvalidServerResponse ex)
            {
                Assert.AreEqual(ex.Message, string.Format("Expected to get response '{0}'; Got  '{1}'", "aa", "bb"));
            }
        }
    }
}