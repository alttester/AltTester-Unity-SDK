using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public class AltBaseCommand
{
    private static int BUFFER_SIZE = 1024;
    protected SocketSettings SocketSettings;
    protected System.Net.Sockets.TcpClient Socket;
    public AltBaseCommand(SocketSettings socketSettings)
    {
        this.SocketSettings = socketSettings;
        Socket=SocketSettings.socket;
    }
    public string Recvall()
    {
        string data = "";
        string previousPart = "";
        while (true)
        {
            var bytesReceived = new byte[BUFFER_SIZE];
            SocketSettings.socket.Client.Receive(bytesReceived);
            string part = fromBytes(bytesReceived);
            string partToSeeAltEnd = previousPart + part;
            data += part;
            if (partToSeeAltEnd.Contains("::altend"))
                break;
            previousPart = part;
        }
        try
        {
            string[] start = new string[] { "altstart::" };
            string[] end = new string[] { "::altend" };
            string[] startLogMessage = new string[] { "::altLog::" };
            data = data.Split(start, System.StringSplitOptions.None)[1].Split(end, System.StringSplitOptions.None)[0];
            var splittedString = data.Split(startLogMessage, System.StringSplitOptions.None);
            var response = splittedString[0];
            data = response;
            var logMessage = splittedString[1];
            if (SocketSettings.logFlag)
            {
                WriteInLogFile(logMessage);
                WriteInLogFile(System.DateTime.Now + ": response received: " + response);
            }

        }
        catch (System.Exception)
        {
            System.Diagnostics.Debug.WriteLine("Data received from socket doesn't have correct start and end control strings");
        }

        return data;
    }
    protected void WriteInLogFile(string logMessage)
    {
        var FileWriter = new System.IO.StreamWriter(@"AltUnityTesterLog.txt", true);
        FileWriter.WriteLine(logMessage);
        FileWriter.Close();
    }

    public string CreateCommand(params string[] arguments)
    {
        string command = "";
        foreach (var argument in arguments)
        {
            command += argument + SocketSettings.requestSeparator;
        }
        command += SocketSettings.requestEnding;
        return command;
    }
    protected byte[] toBytes(string text)
    {
        return System.Text.Encoding.UTF8.GetBytes(text);
    }

    protected string fromBytes(byte[] text)
    {
        return System.Text.Encoding.UTF8.GetString(text);
    }

    public static void HandleErrors(string data)
    {

        var typeOfException = data.Split(';')[0];
        switch (typeOfException)
        {
            case "error:notFound":
                throw new Assets.AltUnityTester.AltUnityDriver.NotFoundException(data);
            case "error:propertyNotFound":
                throw new Assets.AltUnityTester.AltUnityDriver.PropertyNotFoundException(data);
            case "error:methodNotFound":
                throw new Assets.AltUnityTester.AltUnityDriver.MethodNotFoundException(data);
            case "error:componentNotFound":
                throw new Assets.AltUnityTester.AltUnityDriver.ComponentNotFoundException(data);
            case "error:couldNotPerformOperation":
                throw new Assets.AltUnityTester.AltUnityDriver.CouldNotPerformOperationException(data);
            case "error:couldNotParseJsonString":
                throw new Assets.AltUnityTester.AltUnityDriver.CouldNotParseJsonStringException(data);
            case "error:incorrectNumberOfParameters":
                throw new Assets.AltUnityTester.AltUnityDriver.IncorrectNumberOfParametersException(data);
            case "error:failedToParseMethodArguments":
                throw new Assets.AltUnityTester.AltUnityDriver.FailedToParseArgumentsException(data);
            case "error:objectNotFound":
                throw new Assets.AltUnityTester.AltUnityDriver.ObjectWasNotFoundException(data);
            case "error:propertyCannotBeSet":
                throw new Assets.AltUnityTester.AltUnityDriver.PropertyNotFoundException(data);
            case "error:nullReferenceException":
                throw new Assets.AltUnityTester.AltUnityDriver.NullReferenceException(data);
            case "error:unknownError":
                throw new Assets.AltUnityTester.AltUnityDriver.UnknownErrorException(data);
            case "error:formatException":
                throw new Assets.AltUnityTester.AltUnityDriver.FormatException(data);
        }
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
            throw new System.Exception("Out of order operations");
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
        var LongLength = Newtonsoft.Json.JsonConvert.DeserializeObject<long>(length);
        var textureFormatString = screenshotInfo[2];
        var textureFormat = (Assets.AltUnityTester.AltUnityDriver.UnityStruct.AltUnityTextureFormat)System.Enum.Parse(typeof(Assets.AltUnityTester.AltUnityDriver.UnityStruct.AltUnityTextureFormat), textureFormatString);
        var textSizeString = screenshotInfo[3];
        var textSizeVector3 = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityVector3>(textSizeString);

        System.Byte[] imageCompressed = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Byte[]>(screenshotInfo[4], new Newtonsoft.Json.JsonSerializerSettings
        {
            StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeNonAscii
        });

        System.Byte[] imageDecompressed = DeCompressScreenshot(imageCompressed);

        return new AltUnityTextureInformation(imageDecompressed, Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityVector2>(scaleDifference), textSizeVector3, textureFormat);
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
