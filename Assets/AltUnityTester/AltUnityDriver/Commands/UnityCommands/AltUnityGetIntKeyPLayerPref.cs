namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetIntKeyPLayerPref : AltBaseCommand
    {
        readonly string keyName;
        public AltUnityGetIntKeyPLayerPref(SocketSettings socketSettings, string keyName) : base(socketSettings)
        {
            this.keyName = keyName;
        }
        public int Execute()
        {
            SendCommand("getKeyPlayerPref", keyName, PLayerPrefKeyType.Int.ToString());
            var data = Recvall();
            return int.Parse(data);
        }
    }
}