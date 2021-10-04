using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            string url = "ws://" + tcpIp + ":" + tcpPort + "/altws";
            WebSocket wsClient = new WebSocket(url);
            wsClient.OnError += (sender, args) =>
            {
                logger.Error(args.Exception, args.Message);
            };


            logger.Debug("Connecting to: " + url);

            Stopwatch watch = Stopwatch.StartNew();
            int retries = 0;

            while (connectTimeout > watch.Elapsed.TotalSeconds)
            {
                if (retries > 0) logger.Debug(string.Format("Retrying #{0} to {1}", retries, url));
                wsClient.Connect();

                if (wsClient.IsAlive) break;

                retries++;
            }
            if (watch.Elapsed.TotalSeconds > connectTimeout && !wsClient.IsAlive)
                throw new ConnectionTimeoutException("Connection failed because it took too long");

            if (!wsClient.IsAlive)
                throw new Exception("Could not create connection to " + tcpIp + ":" + tcpPort);

            logger.Debug("Connected to: " + url);
            var comm = new DriverCommunicationWebSocket(new AltUnityWebSocketClient(wsClient));
            return comm;
        }

        public CommandResponse<T> Recvall<T>(CommandParams param)
        {

            while (messages.Count == 0 && wsClient.IsAlive())
            {
                Thread.Sleep(10);
            }
            if (!wsClient.IsAlive())
            {
                throw new AltUnityException("Driver disconnected");
            }

            var message = JsonConvert.DeserializeObject<CommandResponse<T>>(messages.Dequeue());

            if (message.error != null && message.error.type != AltUnityErrors.errorInvalidCommand && (message.messageId != param.messageId || message.commandName != param.commandName))
            {
                throw new AltUnityRecvallMessageIdException(string.Format("Response received does not match command send. Expected {0}:{1}. Got {2}:{3}", param.commandName, param.messageId, message.commandName, message.messageId));
            }

            handleErrors(message.error);

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
            logger.Debug("command sent: " + trimLog(message));
        }

        public void Close()
        {
            this.wsClient.Close();
        }

        protected void OnMessage(object sender, string data)
        {
            messages.Enqueue(data);
            logger.Debug("response received: " + trimLog(data));
        }

        private void handleErrors(CommandError error)
        {
            if (error == null)
            {
                return;
            }

            switch (error.type)
            {
                case AltUnityErrors.errorNotFoundMessage:
                    throw new NotFoundException(error.message);
                case AltUnityErrors.errorPropertyNotFoundMessage:
                    throw new PropertyNotFoundException(error.message);
                case AltUnityErrors.errorMethodNotFoundMessage:
                    throw new MethodNotFoundException(error.message);
                case AltUnityErrors.errorComponentNotFoundMessage:
                    throw new ComponentNotFoundException(error.message);
                case AltUnityErrors.errorAssemblyNotFoundMessage:
                    throw new AssemblyNotFoundException(error.message);
                case AltUnityErrors.errorCouldNotPerformOperationMessage:
                    throw new CouldNotPerformOperationException(error.message);
                case AltUnityErrors.errorMethodWithGivenParametersNotFound:
                    throw new MethodWithGivenParametersNotFoundException(error.message);
                case AltUnityErrors.errorFailedToParseArguments:
                    throw new FailedToParseArgumentsException(error.message);
                case AltUnityErrors.errorInvalidParameterType:
                    throw new InvalidParameterTypeException(error.message);
                case AltUnityErrors.errorObjectWasNotFound:
                    throw new ObjectWasNotFoundException(error.message);
                case AltUnityErrors.errorPropertyNotSet:
                    throw new PropertyNotFoundException(error.message);
                case AltUnityErrors.errorNullReferenceMessage:
                    throw new NullReferenceException(error.message);
                case AltUnityErrors.errorUnknownError:
                    throw new UnknownErrorException(error.message);
                case AltUnityErrors.errorFormatException:
                    throw new FormatException(error.message);
                case AltUnityErrors.errorInvalidPath:
                    throw new InvalidPathException(error.message);
                case AltUnityErrors.errorInvalidCommand:
                    throw new InvalidCommandException(error.message);
                case AltUnityErrors.errorInputModule:
                    throw new AltUnityInputModuleException(error.message);
                case AltUnityErrors.errorCameraNotFound:
                    throw new AltUnityCameraNotFoundException(error.message);
            }

            logger.Debug(error.type + " is not handled by driver.");
            throw new UnknownErrorException(error.message);
        }
        private string trimLog(string log, int maxLogLength = 1000)
        {
            if (string.IsNullOrEmpty(log)) return log;
            if (log.Length <= maxLogLength) return log;
            return log.Substring(0, maxLogLength) + "[...]";
        }
    }
}
