namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetFloatKeyPlayerPref : AltBaseCommand
    {
        readonly string keyName;
        public AltUnityGetFloatKeyPlayerPref(SocketSettings socketSettings, string keyName) : base(socketSettings)
        {
            this.keyName = keyName;
        }
        public float Execute()
        {
            SendCommand("getKeyPlayerPref", keyName, PLayerPrefKeyType.Float.ToString());
            var data = Recvall();
            return float.Parse(data);
        }
    }
}