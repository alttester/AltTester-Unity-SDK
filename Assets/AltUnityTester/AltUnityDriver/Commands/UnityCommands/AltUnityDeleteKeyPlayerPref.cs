namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityDeleteKeyPlayerPref : AltBaseCommand
    {
        string keyName;
        public AltUnityDeleteKeyPlayerPref(SocketSettings socketSettings, string keyname) : base(socketSettings)
        {
            this.keyName = keyname;
        }
        public void Execute()
        {
            SendCommand("deleteKeyPlayerPref", keyName);
            var data = Recvall();
            if (data.Equals("Ok"))
                return;
            HandleErrors(data);
        }
    }
}