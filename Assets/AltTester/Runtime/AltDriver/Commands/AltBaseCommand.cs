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

using System;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class AltBaseCommand
    {
        readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        protected readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Culture = System.Globalization.CultureInfo.InvariantCulture
        };
        protected IDriverCommunication CommHandler;

        public AltBaseCommand(IDriverCommunication commHandler)
        {
            this.CommHandler = commHandler;
        }

        protected void ValidateResponse(string expected, string received)
        {
            if (!expected.Equals(received, StringComparison.InvariantCulture))
            {
                throw new AltInvalidServerResponse(expected, received);
            }
        }

    }
}
