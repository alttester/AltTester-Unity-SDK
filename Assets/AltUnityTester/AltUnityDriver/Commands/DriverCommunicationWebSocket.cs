using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Altom.AltUnityDriver.Logging;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Altom.AltUnityDriver.Commands
{
    public class DriverCommunicationWebSocket : IDriverCommunication
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        private IWebSocketClient wsClient;
        private Queue<string> messages;

        public DriverCommunicationWebSocket(IWebSocketClient wsClient)
        {
            messages = new Queue<string>();
            this.wsClient = wsClient;
            this.wsClient.OnMessage += OnMessage;
            this.wsClient.OnError += (sender, args) =>
            {
                logger.Error(args.Message);
                if (args.Exception != null)
                    logger.Error(args.Exception);
            };
        }

        public static DriverCommunicationWebSocket Connect(string tcpIp, int tcpPort, int connectTimeout)
        {
            int retryPeriod = 5;
            string url = "ws://" + tcpIp + ":" + tcpPort + "/altws";
            WebSocket wsClient = new WebSocket(url);
            var comm = new DriverCommunicationWebSocket(new AltUnityWebSocketClient(wsClient));

            while (connectTimeout > 0)
            {
                try
                {
                    logger.Debug("Trying to connect to: " + url);
                    wsClient.Connect();

                    if (!wsClient.IsAlive)
                    {
                        throw new Exception("Could not connect to: " + url);
                    }

                    logger.Debug("Connected to: " + url);

                    break;
                }
                catch (Exception ex)
                {
                    string errorMessage = "Trying to reach AltUnity Server at port" + tcpPort + ",retrying in " + retryPeriod + " (timing out in " + connectTimeout + " secs)...";
                    logger.Error(errorMessage);

                    connectTimeout -= retryPeriod;
                    if (connectTimeout <= 0)
                    {
                        throw new Exception("Could not create connection to " + tcpIp + ":" + tcpPort, ex);
                    }
                    Thread.Sleep(retryPeriod * 1000);
                }
            }
            return comm;
        }

        public CommandResponse<T> Recvall<T>(CommandParams param)
        {
            //TODO: set timeout
            while (messages.Count == 0)
            {
                Thread.Sleep(10);
            }

            var message = JsonConvert.DeserializeObject<CommandResponse<T>>(messages.Dequeue());


            if (message.error != AltUnityErrors.errorInvalidCommand && (message.messageId != param.messageId || message.commandName != param.commandName))
                throw new AltUnityRecvallMessageIdException(string.Format("Response received does not match command send. Expected {0}:{1}. Got {2}:{3}", param.commandName, param.messageId, message.commandName, message.messageId));

            handleErrors(message.error, message.logs);

            return message;
        }

        public void Send(CommandParams param)
        {
            param.messageId = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            string message = JsonConvert.SerializeObject(param, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Culture = CultureInfo.InvariantCulture
            });
            this.wsClient.Send(message);
        }
        public void Close()
        {
            this.wsClient.Close();
        }


        protected void OnMessage(object sender, string data)
        {
            messages.Enqueue(data);
        }
        private void handleErrors(string error, string logs)
        {
            if (string.IsNullOrEmpty(error)) return;
            switch (error)
            {
                case AltUnityErrors.errorNotFoundMessage:
                    throw new NotFoundException(logs);
                case AltUnityErrors.errorPropertyNotFoundMessage:
                    throw new PropertyNotFoundException(logs);
                case AltUnityErrors.errorMethodNotFoundMessage:
                    throw new MethodNotFoundException(logs);
                case AltUnityErrors.errorComponentNotFoundMessage:
                    throw new ComponentNotFoundException(logs);
                case AltUnityErrors.errorAssemblyNotFoundMessage:
                    throw new AssemblyNotFoundException(logs);
                case AltUnityErrors.errorCouldNotPerformOperationMessage:
                    throw new CouldNotPerformOperationException(logs);
                case AltUnityErrors.errorMethodWithGivenParametersNotFound:
                    throw new MethodWithGivenParametersNotFoundException(logs);
                case AltUnityErrors.errorFailedToParseArguments:
                    throw new FailedToParseArgumentsException(logs);
                case AltUnityErrors.errorInvalidParameterType:
                    throw new InvalidParameterTypeException(logs);
                case AltUnityErrors.errorObjectWasNotFound:
                    throw new ObjectWasNotFoundException(logs);
                case AltUnityErrors.errorPropertyNotSet:
                    throw new PropertyNotFoundException(logs);
                case AltUnityErrors.errorNullRefferenceMessage:
                    throw new NullReferenceException(logs);
                case AltUnityErrors.errorUnknownError:
                    throw new UnknownErrorException(logs);
                case AltUnityErrors.errorFormatException:
                    throw new FormatException(logs);
                case AltUnityErrors.errorInvalidPath:
                    throw new InvalidPathException(logs);
                case AltUnityErrors.errorInvalidCommand:
                    throw new InvalidCommandException(logs);

                case AltUnityErrors.errorInputModule:
                    throw new AltUnityInputModuleException(logs);
                case AltUnityErrors.errorCameraNotFound:
                    throw new AltUnityCameraNotFoundException(logs);
            }
            if (error.StartsWith("error:"))
            {
                logger.Debug(error + " is not handled by driver");
                throw new UnknownErrorException(logs);
            }
        }
    }
}