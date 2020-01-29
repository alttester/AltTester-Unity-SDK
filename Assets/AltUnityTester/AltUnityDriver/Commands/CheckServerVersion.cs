    public class CheckServerVersion : AltBaseCommand
    {
        public CheckServerVersion(SocketSettings socketSettings) : base(socketSettings)
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
                System.ComponentModel.WarningException myEx =new System.ComponentModel.WarningException("Mismatch version. You are using different version of server and driver. Server version: " + serverVersion + " and Driver version: " + AltUnityDriver.VERSION);	
                WriteInLogFile(myEx.Message);
                System.Console.WriteLine(myEx.Message);
                return "Version mismatch";
            }
            else{
                return "Ok";
            }

        }
    }
