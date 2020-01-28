    public class CheckServerVersion : AltBaseCommand
    {
        public CheckServerVersion(SocketSettings socketSettings) : base(socketSettings)
        {
        }
        public void Execute()
        {
            string serverVersion;
            Socket.Client.Send(toBytes(CreateCommand("getServerVersion")));
            serverVersion = Recvall();
            if (serverVersion.Contains("error:"))
            {
                HandleErrors(serverVersion);
            }
            var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Assets\\AltUnityTester\\AltUnityDriver\\CSharpDriverVersion.txt");
            var driverVersion= System.IO.File.ReadAllText(path);
            if (!driverVersion.Equals(serverVersion))
                throw new System.Exception("Mismatch version. You are using different version of server and driver. Server version: " + serverVersion + " and Driver version: " + driverVersion);

        }
    }
