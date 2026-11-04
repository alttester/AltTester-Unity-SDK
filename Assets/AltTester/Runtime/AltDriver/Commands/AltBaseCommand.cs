/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using AltTester.AltTesterSDK.Driver.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AltTester.AltTesterSDK.Driver.Commands
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
