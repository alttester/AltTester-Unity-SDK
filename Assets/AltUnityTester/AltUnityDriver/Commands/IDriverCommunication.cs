using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Altom.AltUnityDriver.Logging;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Altom.AltUnityDriver.Commands
{
    public interface IDriverCommunication
    {
        void Send(CommandParams param);
        CommandResponse<T> Recvall<T>(CommandParams param);
        void Close();
    }
    public class DriverWebSocketClient : IDriverCommunication
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        private WebSocket wsClient;
        private Queue<string> messages;

        public DriverWebSocketClient(string tcpIp, int tcpPort, int connectTimeout)
        {
            messages = new Queue<string>();
            connect(tcpIp, tcpPort, connectTimeout);
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


        protected void OnMessage(object sender, MessageEventArgs e)
        {
            messages.Enqueue(e.Data);
        }

        private void connect(string tcpIp, int tcpPort, int connectTimeout)
        {
            int retryPeriod = 5;
            string url = "ws://" + tcpIp + ":" + tcpPort + "/altws";
            wsClient = new WebSocket(url);

            wsClient.OnMessage += OnMessage;
            wsClient.OnError += (sender, args) =>
            {
                logger.Error(args.Message);
                if (args.Exception != null)
                    logger.Error(args.Exception);
            };


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
                    Console.WriteLine(errorMessage);
#if UNITY_EDITOR
                    UnityEngine.Debug.Log(errorMessage);
#endif

                    connectTimeout -= retryPeriod;
                    if (connectTimeout <= 0)
                    {
                        throw new Exception("Could not create connection to " + tcpIp + ":" + tcpPort, ex);
                    }
                    Thread.Sleep(retryPeriod * 1000);
                }
            }
        }
        private void handleErrors(string error, string logs)
        {
            if (string.IsNullOrEmpty(error)) return;
            switch (error)
            {
                case "error:notFound":
                    throw new NotFoundException(logs);
                case "error:propertyNotFound":
                    throw new PropertyNotFoundException(logs);
                case "error:methodNotFound":
                    throw new MethodNotFoundException(logs);
                case "error:componentNotFound":
                    throw new ComponentNotFoundException(logs);
                case "error:assemblyNotFound":
                    throw new AssemblyNotFoundException(logs);
                case "error:couldNotPerformOperation":
                    throw new CouldNotPerformOperationException(logs);
                case "error:methodWithGivenParametersNotFound":
                    throw new MethodWithGivenParametersNotFoundException(logs);
                case "error:failedToParseMethodArguments":
                    throw new FailedToParseArgumentsException(logs);
                case "error:invalidParameterType":
                    throw new InvalidParameterTypeException(logs);
                case "error:objectNotFound":
                    throw new ObjectWasNotFoundException(logs);
                case "error:propertyCannotBeSet":
                    throw new PropertyNotFoundException(logs);
                case "error:nullReferenceException":
                    throw new NullReferenceException(logs);
                case AltUnityErrors.errorUnknownError:
                    throw new UnknownErrorException(logs);
                case AltUnityErrors.errorFormatException:
                    throw new FormatException(logs);
                case AltUnityErrors.errorInvalidPath:
                    throw new InvalidPathException(logs);
                case AltUnityErrors.errorInvalidCommand:
                    throw new InvalidCommandException(logs);
            }
            if (error.StartsWith("error:"))
            {
                logger.Debug(error + " is not handled by driver");
                throw new UnknownErrorException(logs);
            }
        }
    }
}