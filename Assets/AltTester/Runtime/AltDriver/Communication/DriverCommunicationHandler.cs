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
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using AltTester.AltTesterUnitySDK.Driver.Notifications;
using AltWebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AltTester.AltTesterUnitySDK.Driver.Communication
{
    public class DriverCommunicationHandler : IDriverCommunication
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Culture = CultureInfo.InvariantCulture,
            Formatting = Formatting.None
        };

        private DriverWebSocketClient wsClient = null;

        private readonly string host;
        private readonly int port;
        private readonly string appName;
        private readonly int connectTimeout;
        private readonly string platform;
        private readonly string platformVersion;
        private readonly string deviceInstanceId;
        private readonly string appId;
        private readonly string driverType;
        private static readonly object _lock = new object();

        private int commandTimeout = 60;
        private float delayAfterCommand = 0;
        private bool websocketClosedCalled = false;

        private List<string> messageIdTimeouts = new List<string>();

        private Dictionary<string, Queue<CommandResponse>> messages;

        private List<Action<AltLoadSceneNotificationResultParams>> loadSceneCallbacks = new List<Action<AltLoadSceneNotificationResultParams>>();
        private List<Action<String>> unloadSceneCallbacks = new List<Action<String>>();
        private List<Action<AltLogNotificationResultParams>> logCallbacks = new List<Action<AltLogNotificationResultParams>>();
        private List<Action<bool>> applicationPausedCallbacks = new List<Action<bool>>();

        public DriverCommunicationHandler(string host, int port, int connectTimeout, string appName, string platform, string platformVersion, string deviceInstanceId, string appId, string driverType = "SDK")
        {
            this.host = host;
            this.port = port;
            this.appName = appName;
            this.connectTimeout = connectTimeout;
            this.platform = platform;
            this.platformVersion = platformVersion;
            this.deviceInstanceId = deviceInstanceId;
            this.appId = appId;
            this.driverType = driverType;

            this.messages = new Dictionary<string, Queue<CommandResponse>>();
        }

        public void Connect()
        {
            this.wsClient = new DriverWebSocketClient(this.host, this.port, "/altws", this.appName, this.connectTimeout, this.platform, this.platformVersion, this.deviceInstanceId, this.appId, this.driverType);
            this.wsClient.OnMessage += (sender, e) =>
            {
                OnMessage(sender, e.Data);
            };
            this.wsClient.OnCloseEvent += (sender, e) =>
            {
                websocketClosedCalled = true;
            };

            this.wsClient.Connect();
            websocketClosedCalled = false;

        }

        public T Recvall<T>(CommandParams param)
        {
            Stopwatch watch = Stopwatch.StartNew();
            while (true)
            {
                while (!messages.ContainsKey(param.messageId) && commandTimeout >= watch.Elapsed.TotalSeconds && !websocketClosedCalled)
                {
                    Thread.Sleep(10);
                }

                if (websocketClosedCalled)
                {
                    throw new AltException("Driver disconnected");
                }
                if (commandTimeout < watch.Elapsed.TotalSeconds)
                {

                    messageIdTimeouts.Add(param.messageId);
                    throw new CommandResponseTimeoutException();
                }
                Queue<CommandResponse> queue;
                messages.TryGetValue(param.messageId, out queue);
                if (queue == null)
                {
                    throw new AltException(" Could not find the message");
                }
                var message = queue.Dequeue();

                if (queue.Count == 0)
                {
                    messages.Remove(param.messageId);
                }

                if (messageIdTimeouts.Contains(message.messageId))
                {
                    continue;
                }

                if ((message.error == null || message.error.type != AltErrors.errorInvalidCommand) && (message.messageId != param.messageId || message.commandName != param.commandName))
                {
                    throw new AltRecvallMessageIdException(string.Format("Response received does not match command send. Expected {0}:{1}. Got {2}:{3}", param.commandName, param.messageId, message.commandName, message.messageId));
                }

                handleErrors(message.error);
                if (message.data == null)
                {
                    return default(T);
                }

                try
                {
                    return JsonConvert.DeserializeObject<T>(message.data, jsonSerializerSettings);
                }
                catch (JsonReaderException)
                {
                    throw new ResponseFormatException(typeof(T), message.data);
                }
            }
        }

        public void Send(CommandParams param)
        {
            lock (_lock)
            {
                param.messageId = DateTime.UtcNow.Ticks.ToString();
            }
            string message = JsonConvert.SerializeObject(param, jsonSerializerSettings);
            this.wsClient.Send(message);
        }

        public void Close()
        {

            logger.Info(string.Format("Closing connection to AltTester(R) on: {0}", this.wsClient.URI));
            websocketClosedCalled = true;
            this.wsClient.Close();
        }

        public void SetCommandTimeout(int timeout)
        {
            commandTimeout = timeout;
        }

        protected void OnMessage(object sender, string data)
        {
            var message = JsonConvert.DeserializeObject<CommandResponse>(data, jsonSerializerSettings);

            if (message.isNotification)
            {
                handleNotification(message);
            }
            else
            {
                if (messages.ContainsKey(message.messageId))
                {

                    Queue<CommandResponse> queue;
                    messages.TryGetValue(message.messageId, out queue);
                    queue.Enqueue(message);
                }
                else
                {
                    var queue = new Queue<CommandResponse>();
                    queue.Enqueue(message);
                    messages.Add(message.messageId, queue);
                }
            }
        }

        private void handleNotification(CommandResponse message)
        {
            handleErrors(message.error);

            switch (message.commandName)
            {
                case "loadSceneNotification":
                    AltLoadSceneNotificationResultParams data = JsonConvert.DeserializeObject<AltLoadSceneNotificationResultParams>(message.data, jsonSerializerSettings);
                    foreach (var callback in loadSceneCallbacks)
                    {
                        callback(data);
                    }
                    break;
                case "unloadSceneNotification":
                    string sceneName = JsonConvert.DeserializeObject<string>(message.data, jsonSerializerSettings);
                    foreach (var callback in unloadSceneCallbacks)
                    {
                        callback(sceneName);
                    }
                    break;
                case "logNotification":
                    AltLogNotificationResultParams data1 = JsonConvert.DeserializeObject<AltLogNotificationResultParams>(message.data, jsonSerializerSettings);
                    foreach (var callback in logCallbacks)
                    {
                        callback(data1);
                    }
                    break;
                case "applicationPausedNotification":
                    bool data2 = JsonConvert.DeserializeObject<bool>(message.data, jsonSerializerSettings);
                    foreach (var callback in applicationPausedCallbacks)
                    {
                        callback(data2);
                    }
                    break;
                case "driverRegistered":
                    wsClient.DriverRegisteredCalled = true;
                    break;
            }
        }

        private void handleErrors(CommandError error)
        {
            if (error == null)
            {
                return;
            }

            logger.Debug(error.type + ": " + error.message);
            logger.Debug(error.trace);

            switch (error.type)
            {
                case AltErrors.errorNotFound:
                    throw new NotFoundException(error.message);
                case AltErrors.errorSceneNotFound:
                    throw new SceneNotFoundException(error.message);
                case AltErrors.errorPropertyNotFound:
                    throw new PropertyNotFoundException(error.message);
                case AltErrors.errorMethodNotFound:
                    throw new MethodNotFoundException(error.message);
                case AltErrors.errorComponentNotFound:
                    throw new ComponentNotFoundException(error.message);
                case AltErrors.errorAssemblyNotFound:
                    throw new AssemblyNotFoundException(error.message);
                case AltErrors.errorCouldNotPerformOperation:
                    throw new CouldNotPerformOperationException(error.message);
                case AltErrors.errorMethodWithGivenParametersNotFound:
                    throw new MethodWithGivenParametersNotFoundException(error.message);
                case AltErrors.errorFailedToParseArguments:
                    throw new FailedToParseArgumentsException(error.message);
                case AltErrors.errorInvalidParameterType:
                    throw new InvalidParameterTypeException(error.message);
                case AltErrors.errorObjectWasNotFound:
                    throw new ObjectWasNotFoundException(error.message);
                case AltErrors.errorPropertyNotSet:
                    throw new PropertyNotFoundException(error.message);
                case AltErrors.errorNullReference:
                    throw new NullReferenceException(error.message);
                case AltErrors.errorUnknownError:
                    throw new UnknownErrorException(error.message);
                case AltErrors.errorFormatException:
                    throw new FormatException(error.message);
                case AltErrors.errorInvalidPath:
                    throw new InvalidPathException(error.message);
                case AltErrors.errorInvalidCommand:
                    throw new InvalidCommandException("Invalid command exception. You may want to set the Managed Stripping Level to `Minimal` from Player Settings -> Other Settings -> Optimization.");
                case AltErrors.errorInputModule:
                    throw new AltInputModuleException(error.message);
                case AltErrors.errorCameraNotFound:
                    throw new AltCameraNotFoundException(error.message);
            }

            logger.Debug(error.type + " is not handled by driver.");
            throw new UnknownErrorException(error.message);
        }

        public void AddNotificationListener<T>(NotificationType notificationType, Action<T> callback, bool overwrite)
        {
            switch (notificationType)
            {
                case NotificationType.LOADSCENE:
                    if (callback.GetType() != typeof(Action<AltLoadSceneNotificationResultParams>))
                    {
                        throw new InvalidCastException(String.Format("callback must be of type: {0} and not {1}", typeof(Action<AltLoadSceneNotificationResultParams>), callback.GetType()));
                    }
                    if (overwrite)
                        loadSceneCallbacks.Clear();
                    loadSceneCallbacks.Add(callback as Action<AltLoadSceneNotificationResultParams>);
                    break;
                case NotificationType.UNLOADSCENE:
                    if (callback.GetType() != typeof(Action<String>))
                    {
                        throw new InvalidCastException(String.Format("callback must be of type: {0} and not {1}", typeof(Action<String>), callback.GetType()));
                    }
                    if (overwrite)
                        unloadSceneCallbacks.Clear();
                    unloadSceneCallbacks.Add(callback as Action<String>);
                    break;
                case NotificationType.LOG:
                    if (callback.GetType() != typeof(Action<AltLogNotificationResultParams>))
                    {
                        throw new InvalidCastException(String.Format("callback must be of type: {0} and not {1}", typeof(Action<AltLogNotificationResultParams>), callback.GetType()));
                    }
                    if (overwrite)
                        logCallbacks.Clear();
                    logCallbacks.Add(callback as Action<AltLogNotificationResultParams>);
                    break;
                case NotificationType.APPLICATION_PAUSED:
                    if (callback.GetType() != typeof(Action<bool>))
                    {
                        throw new InvalidCastException(String.Format("callback must be of type: {0} and not {1}", typeof(Action<bool>), callback.GetType()));
                    }
                    if (overwrite)
                        applicationPausedCallbacks.Clear();
                    applicationPausedCallbacks.Add(callback as Action<bool>);
                    break;
            }
        }

        public void RemoveNotificationListener(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.LOADSCENE:
                    loadSceneCallbacks.Clear();
                    break;
                case NotificationType.UNLOADSCENE:
                    unloadSceneCallbacks.Clear();
                    break;
                case NotificationType.LOG:
                    logCallbacks.Clear();
                    break;
                case NotificationType.APPLICATION_PAUSED:
                    applicationPausedCallbacks.Clear();
                    break;
            }
        }

        public void SetDelayAfterCommand(float delay)
        {
            delayAfterCommand = delay;
        }

        public float GetDelayAfterCommand()
        {
            return delayAfterCommand;
        }

        public void SleepFor(float time)
        {
            Utils.SleepFor(time);
        }
    }
}