
using System.Linq;
namespace Altom.AltUnityDriver.Commands
{
    public class AltBaseCommand
    {
        private static int BUFFER_SIZE = 1024;
        protected SocketSettings SocketSettings;
        private string messageId;
        public AltBaseCommand(SocketSettings socketSettings)
        {
            this.SocketSettings = socketSettings;
        }
        public string Recvall()
        {
            string data = "";
            string previousPart = "";
            System.Collections.Generic.List<byte[]> byteArray = new System.Collections.Generic.List<byte[]>();
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
                        throw new System.Exception("Socket not connected yet");
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



            var parts = data.Split(new[] { "altstart::", "::response::", "::altLog::", "::altend" }, System.StringSplitOptions.None);
            if (parts.Length != 5 || !string.IsNullOrEmpty(parts[0]) || !string.IsNullOrEmpty(parts[4]))
                throw new AltUnityRecvallMessageFormatException("Data received from socket doesn't have correct start and end control strings");

            if (parts[1] != messageId)
            {
                throw new AltUnityRecvallMessageIdException("Response received does not match command send. Expected message id: " + messageId + ". Got " + parts[1]);
            }

            data = parts[2];
            string log = parts[3];
            if (SocketSettings.LogFlag)
            {
                writeInLogFile(System.DateTime.Now + ": response received: " + trimLogData(data));
                writeInLogFile(log);
            }

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
            string[] screenshotInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(data);

            // Some workaround this: https://stackoverflow.com/questions/710853/base64-string-throwing-invalid-character-error
            var screenshotParts = screenshotInfo[4].Split('\0');
            screenshotInfo[4] = "";
            for (int i = 0; i < screenshotParts.Length; i++)
            {
                screenshotInfo[4] += screenshotParts[i];
            }

            var scaleDifference = screenshotInfo[0];

            var length = screenshotInfo[1];
            var textureFormatString = screenshotInfo[2];
            var textureFormat = (AltUnityTextureFormat)System.Enum.Parse(typeof(AltUnityTextureFormat), textureFormatString);
            var textSizeString = screenshotInfo[3];
            var textSizeVector3 = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityVector3>(textSizeString);

            System.Byte[] imageCompressed = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Byte[]>(screenshotInfo[4], new Newtonsoft.Json.JsonSerializerSettings
            {
                StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeNonAscii
            });

            System.Byte[] imageDecompressed = DeCompressScreenshot(imageCompressed);

            return new AltUnityTextureInformation(imageDecompressed, Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityVector2>(scaleDifference), textSizeVector3, textureFormat);
        }

        protected void SendCommand(params string[] arguments)
        {
            var command = createCommand(arguments);
            var bytes = toBytes(command);
            SocketSettings.Socket.Send(bytes);
        }
        protected string PositionToJson(float x, float y)
        {
            return PositionToJson(new AltUnityVector2(x, y));
        }
        protected string PositionToJson(AltUnityVector2 position)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(position, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
        }

        private string createCommand(string[] arguments)
        {
            messageId = System.DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
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

        private void writeInLogFile(string logMessage)
        {
            var FileWriter = new System.IO.StreamWriter(@"AltUnityTesterLog.txt", true);
            FileWriter.WriteLine(logMessage);
            FileWriter.Close();
        }
        private string trimLogData(string data, int maxSize = 10 * 1024)
        {
            if (data.Length > maxSize)
                return data.Substring(0, maxSize) + "[...]";
            return data;
        }

        public static void HandleErrors(string data)
        {
            var typeOfException = data.Split(';')[0];
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
            }
        }

        public static byte[] DeCompressScreenshot(byte[] screenshotCompressed)
        {

            using (var memoryStreamInput = new System.IO.MemoryStream(screenshotCompressed))
            using (var memoryStreamOutput = new System.IO.MemoryStream())
            {
                using (var gs = new System.IO.Compression.GZipStream(memoryStreamInput, System.IO.Compression.CompressionMode.Decompress))
                {
                    CopyTo(gs, memoryStreamOutput);
                }

                return memoryStreamOutput.ToArray();
            }
        }
        public static T[] SubArray<T>(T[] data, int index, long length)
        {
            T[] result = new T[length];
            System.Array.Copy(data, index, result, 0, length);
            return result;
        }
        public static void CopyTo(System.IO.Stream src, System.IO.Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
    }
}