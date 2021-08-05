
using System;
using System.Collections.Generic;
using System.Linq;
using Altom.AltUnityDriver.Logging;
using Newtonsoft.Json;
using NLog;

namespace Altom.AltUnityDriver.Commands
{
    public class AltBaseCommand
    {
        readonly Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();

        private const int BUFFER_SIZE = 1024;
        protected SocketSettings SocketSettings;
        private string messageId;

        private string _remaining = "";
        public AltBaseCommand(SocketSettings socketSettings)
        {
            this.SocketSettings = socketSettings;
        }
        public string Recvall()
        {
            string data = "";

            if (_remaining.Contains("::altend"))
            {
                data = _remaining;
            }
            else
            {
                string previousPart = _remaining;
                List<byte[]> byteArray = new List<byte[]>();
                int received = 0;
                int receivedZeroBytesCounter = 0;
                int receivedZeroBytesCounterLimit = 2;
                do
                {
                    var bytesReceived = new byte[BUFFER_SIZE];
                    received = SocketSettings.Socket.Receive(bytesReceived);
                    if (received == 0)
                    {
                        if (receivedZeroBytesCounter < receivedZeroBytesCounterLimit)
                        {
                            receivedZeroBytesCounter++;
                            continue;
                        }
                        else
                        {
                            throw new Exception("Socket not connected yet");
                        }
                    }
                    var newBytesReceived = new byte[received];
                    for (int i = 0; i < received; i++)
                    {
                        newBytesReceived[i] = bytesReceived[i];
                    }
                    byteArray.Add(newBytesReceived);
                    string part = fromBytes(newBytesReceived);
                    string partToSeeAltEnd = previousPart + part;
                    if (partToSeeAltEnd.Contains("::altend"))
                        break;
                    previousPart = part;

                } while (true);
                data = fromBytes(byteArray.SelectMany(x => x).ToArray());
            }
            logger.Trace(data);

            _remaining = "";
            int index = data.IndexOf("::altendaltstart::");
            if (index >= 0)
            {
                _remaining = data.Substring(index + 8);
                data = data.Substring(0, index + 8);
            }

            var parts = data.Split(new[] { "altstart::", "::response::", "::altLog::", "::altend" }, StringSplitOptions.None);

            if (parts.Length != 5 || !string.IsNullOrEmpty(parts[0]) || !string.IsNullOrEmpty(parts[4]))
                throw new AltUnityRecvallMessageFormatException(string.Format("Data received from socket doesn't have correct start and end control strings.\nGot:\n{0}", data));

            if (parts[1] != messageId)
            {
                throw new AltUnityRecvallMessageIdException(string.Format("Response received does not match command send. Expected message id: {0}. Got {1}", messageId, parts[1]));
            }

            data = parts[2];
            string log = parts[3];

            logger.Debug("response: " + trimLogData(data));
            if (!string.IsNullOrEmpty(log))
                logger.Debug(log);


            handleErrors(data, log);

            return data;
        }

        public AltUnityTextureInformation ReceiveImage()
        {
            var data = Recvall();
            if (data == "Ok")
            {
                data = Recvall();
            }
            else
            {
                throw new AltUnityRecvallException("Out of order operations");
            }
            string[] screenshotInfo = JsonConvert.DeserializeObject<string[]>(data);

            // Some workaround this: https://stackoverflow.com/questions/710853/base64-string-throwing-invalid-character-error
            var screenshotParts = screenshotInfo[4].Split('\0');
            screenshotInfo[4] = "";
            for (int i = 0; i < screenshotParts.Length; i++)
            {
                screenshotInfo[4] += screenshotParts[i];
            }

            var scaleDifference = screenshotInfo[0];
            // screenshotInfo[1] contains the length, but it is not used;
            var textureFormatString = screenshotInfo[2];
            var textureFormat = (AltUnityTextureFormat)Enum.Parse(typeof(AltUnityTextureFormat), textureFormatString);
            var textSizeString = screenshotInfo[3];
            var textSizeVector3 = JsonConvert.DeserializeObject<AltUnityVector3>(textSizeString);

            byte[] imageCompressed = JsonConvert.DeserializeObject<byte[]>(screenshotInfo[4], new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            });

            byte[] imageDecompressed = deCompressScreenshot(imageCompressed);

            return new AltUnityTextureInformation(imageDecompressed, JsonConvert.DeserializeObject<AltUnityVector2>(scaleDifference), textSizeVector3, textureFormat);
        }

        protected void ValidateResponse(string expected, string received, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            if (!expected.Equals(received, stringComparison))
            {
                throw new AltUnityInvalidServerResponse(expected, received);
            }
        }

        protected void SendCommand(params string[] arguments)
        {
            var command = createCommand(arguments);
            var bytes = toBytes(command);
            SocketSettings.Socket.Send(bytes);

            logger.Debug("sent: " + command);
        }
        protected string PositionToJson(float x, float y)
        {
            return PositionToJson(new AltUnityVector2(x, y));
        }
        protected string PositionToJson(AltUnityVector2 position)
        {
            return JsonConvert.SerializeObject(position, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        private static byte[] deCompressScreenshot(byte[] screenshotCompressed)
        {
            using (var memoryStreamInput = new System.IO.MemoryStream(screenshotCompressed))
            using (var memoryStreamOutput = new System.IO.MemoryStream())
            {
                using (var gs = new System.IO.Compression.GZipStream(memoryStreamInput, System.IO.Compression.CompressionMode.Decompress))
                {
                    copyTo(gs, memoryStreamOutput);
                }

                return memoryStreamOutput.ToArray();
            }
        }

        private static void copyTo(System.IO.Stream src, System.IO.Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        private string createCommand(string[] arguments)
        {
            messageId = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            var args = arguments.ToList();
            args.Insert(0, messageId);

            return string.Join(SocketSettings.RequestSeparator, args) + SocketSettings.RequestEnding;
        }

        private byte[] toBytes(string text)
        {
            return System.Text.Encoding.UTF8.GetBytes(text);
        }
        private string fromBytes(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        private string trimLogData(string data, int maxSize = 10 * 1024)
        {
            if (data.Length > maxSize)
                return data.Substring(0, maxSize) + "[...]";
            return data;
        }

        private void handleErrors(string data, string log)
        {
            var typeOfException = data.Split(';')[0];
            if (!string.IsNullOrEmpty(log))
                data = data + "\n" + log + "\n";
            switch (typeOfException)
            {
                case "error:notFound":
                    throw new NotFoundException(data);
                case "error:propertyNotFound":
                    throw new PropertyNotFoundException(data);
                case "error:methodNotFound":
                    throw new MethodNotFoundException(data);
                case "error:componentNotFound":
                    throw new ComponentNotFoundException(data);
                case "error:assemblyNotFound":
                    throw new AssemblyNotFoundException(data);
                case "error:couldNotPerformOperation":
                    throw new CouldNotPerformOperationException(data);
                case "error:couldNotParseJsonString":
                    throw new CouldNotParseJsonStringException(data);
                case "error:methodWithGivenParametersNotFound":
                    throw new MethodWithGivenParametersNotFoundException(data);
                case "error:failedToParseMethodArguments":
                    throw new FailedToParseArgumentsException(data);
                case "error:invalidParameterType":
                    throw new InvalidParameterTypeException(data);
                case "error:objectNotFound":
                    throw new ObjectWasNotFoundException(data);
                case "error:propertyCannotBeSet":
                    throw new PropertyNotFoundException(data);
                case "error:nullReferenceException":
                    throw new NullReferenceException(data);
                case "error:unknownError":
                    throw new UnknownErrorException(data);
                case "error:formatException":
                    throw new FormatException(data);
                case "error:invalidPath":
                    throw new InvalidPathException(data);
                case "error:ALTUNITYTESTERNotAddedAsDefineVariable":
                    throw new AltUnityInputModuleException(data);
                case "error:cameraNotFound":
                    throw new AltUnityCameraNotFoundException(data);

            }
        }
    }
}