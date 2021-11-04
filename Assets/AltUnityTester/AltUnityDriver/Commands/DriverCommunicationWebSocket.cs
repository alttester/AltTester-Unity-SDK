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
        private IWebSocketClient wsClient = null;
        private readonly string _host;
        private readonly int _port;
        private readonly string _uri;
        private readonly int _connectTimeout;
        private Queue<string> messages;

        public DriverCommunicationWebSocket(string host, int port, int connectTimeout)
        {
            _host = host;
            _port = port;
            _uri = "ws://" + host + ":" + port + "/altws";
            _connectTimeout = connectTimeout;

            messages = new Queue<string>();
        }

        public void Connect()
        {
            int delay = 100;

            logger.Info("Connecting to host: {0} port: {1}.", _host, _port);

            WebSocket wsClient = new WebSocket(_uri);
            wsClient.OnError += (sender, args) =>
            {
                logger.Error(args.Exception, args.Message);
            };

            Stopwatch watch = Stopwatch.StartNew();
            int retries = 0;

            while (_connectTimeout > watch.Elapsed.TotalSeconds)
            {
                if (retries > 0) logger.Debug(string.Format("Retrying #{0} to host: {1} port: {2}.", retries, _host, _port));
                wsClient.Connect();

                if (wsClient.IsAlive) break;

                retries++;

                Thread.Sleep(delay); // delay between retries
            }
            if (watch.Elapsed.TotalSeconds > _connectTimeout && !wsClient.IsAlive)
                throw new ConnectionTimeoutException(string.Format("Failed to connect to AltUnity on host: {0} port: {1}.", _host, _port));

            if (!wsClient.IsAlive)
                throw new ConnectionException(string.Format("Failed to connect to AltUnity on host: {0} port: {1}.", _host, _port));

            logger.Debug("Connected to: " + _uri);

            this.wsClient = new AltUnityWebSocketClient(wsClient);
            this.wsClient.OnMessage += OnMessage;
            this.wsClient.OnError += (sender, args) =>
            {
                logger.Error(args.Message);
                if (args.Exception != null)
                    logger.Error(args.Exception);
            };
            this.wsClient.OnClose += (sender, args) =>
            {
                logger.Debug("Connection to AltUnity closed: [Code:{0}, Reason:{1}]", args.Code, args.Reason);
            };
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
            logger.Info(string.Format("Closing connection to AltUnity on: {0}", _uri));
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
                case AltUnityErrors.errorNotFound:
                    throw new NotFoundException(error.message);
                case AltUnityErrors.errorSceneNotFound:
                    throw new SceneNotFoundException(error.message);
                case AltUnityErrors.errorPropertyNotFound:
                    throw new PropertyNotFoundException(error.message);
                case AltUnityErrors.errorMethodNotFound:
                    throw new MethodNotFoundException(error.message);
                case AltUnityErrors.errorComponentNotFound:
                    throw new ComponentNotFoundException(error.message);
                case AltUnityErrors.errorAssemblyNotFound:
                    throw new AssemblyNotFoundException(error.message);
                case AltUnityErrors.errorCouldNotPerformOperation:
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
                case AltUnityErrors.errorNullReference:
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
