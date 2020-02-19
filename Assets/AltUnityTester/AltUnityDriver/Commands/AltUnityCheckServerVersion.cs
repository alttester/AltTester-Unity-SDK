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
            if (serverVersion.Contains("error:"))
            {
                HandleErrors(serverVersion);
            }
            
            if (!AltUnityDriver.VERSION.Equals(serverVersion))
            {
            var message = "Version mismatch. You are using different versions of server and driver. Server version: " + serverVersion + " and Driver version: " + AltUnityDriver.VERSION;
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(message);
#endif
            WriteInLogFile(message);
                System.Console.WriteLine(message);
                return "Version mismatch";
            }
            else{
                return "Ok";
            }

        }
    }
