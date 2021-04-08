namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetStringKeyPlayerPref : AltBaseCommand
    {
        readonly string keyName;
        public AltUnityGetStringKeyPlayerPref(SocketSettings socketSettings, string keyName) : base(socketSettings)
        {
            this.keyName = keyName;
        }
        public string Execute()
        {
            SendCommand("getKeyPlayerPref", keyName, PLayerPrefKeyType.String.ToString());
            var data = Recvall();
            return data;
        }
    }
}