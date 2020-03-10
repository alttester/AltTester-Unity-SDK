public class AltUnityCheckServerVersion : AltBaseCommand
{
    public AltUnityCheckServerVersion(SocketSettings socketSettings) : base(socketSettings)
    {
    }
    public string Execute()
    {
        string serverVersion;
        Socket.Client.Send(toBytes(CreateCommand("getServerVersion")));
        serverVersion = Recvall();
        if (serverVersion.Equals("error:unknownError"))
        {
            WriteWarning(true);
            return "Version mismatch";
        }
        if (serverVersion.Contains("error:"))
        {
            HandleErrors(serverVersion);
        }

        if (!AltUnityDriver.VERSION.Equals(serverVersion))
        {
            WriteWarning(false, serverVersion);
            return "Version mismatch";
        }
        return "Ok";

    }
    public void WriteWarning(bool isEarlier, string serverVersion = "")
    {
        var message = isEarlier ? "Version mismatch. You are using different versions of server and driver. Server version is earlier then 1.5.3 and Driver version: " + AltUnityDriver.VERSION
            : "Version mismatch. You are using different versions of server and driver. Server version: " + serverVersion + " and Driver version: " + AltUnityDriver.VERSION;
#if UNITY_EDITOR
        UnityEngine.Debug.LogWarning(message);
#endif
        WriteInLogFile(message);
        System.Console.WriteLine(message);
    }
}
