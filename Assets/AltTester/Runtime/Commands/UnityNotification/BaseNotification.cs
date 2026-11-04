/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Globalization;
using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AltTester.AltTesterUnitySDK.Notification
{
    public class BaseNotification
    {
        private static ICommandHandler commandHandler;
        private static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Culture = CultureInfo.InvariantCulture
        };

        public BaseNotification(ICommandHandler commandHandlerParam)
        {
            commandHandler = commandHandlerParam;
        }

        protected static void SendNotification<T>(T data, string commandName)
        {
            var cmdResponse = new CommandResponse
            {
                commandName = commandName,
                messageId = null,
                data = JsonConvert.SerializeObject(data, jsonSerializerSettings),
                error = null,
                isNotification = true
            };

            var notification = JsonConvert.SerializeObject(cmdResponse, jsonSerializerSettings);
            commandHandler.Send(notification);

        }
    }
}
