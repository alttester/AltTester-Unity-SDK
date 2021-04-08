namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySetKeyPLayerPref : AltBaseCommand
    {
        readonly string keyName;
        readonly int intValue;
        readonly float floatValue;
        readonly string stringValue;
        readonly int option = 0;
        public AltUnitySetKeyPLayerPref(SocketSettings socketSettings, string keyName, int intValue) : base(socketSettings)
        {
            this.keyName = keyName;
            this.intValue = intValue;
            option = 1;
        }
        public AltUnitySetKeyPLayerPref(SocketSettings socketSettings, string keyName, float floatValue) : base(socketSettings)
        {
            this.keyName = keyName;
            this.floatValue = floatValue;
            option = 2;
        }
        public AltUnitySetKeyPLayerPref(SocketSettings socketSettings, string keyName, string stringValue) : base(socketSettings)
        {
            this.keyName = keyName;
            this.stringValue = stringValue;
            option = 3;
        }
        public void Execute()
        {
            switch (option)
            {
                case 1:
                    setIntKey();
                    break;
                case 2:
                    setFloatKey();
                    break;
                case 3:
                    setStringKey();
                    break;
            }
        }
        private void setStringKey()
        {
            SendCommand("setKeyPlayerPref", keyName, stringValue.ToString(), PLayerPrefKeyType.String.ToString());
            var data = Recvall();
            ValidateResponse("Ok", data);
        }
        private void setIntKey()
        {
            SendCommand("setKeyPlayerPref", keyName, intValue.ToString(), PLayerPrefKeyType.Int.ToString());
            var data = Recvall();
            ValidateResponse("Ok", data);
        }
        private void setFloatKey()
        {
            SendCommand("setKeyPlayerPref", keyName, floatValue.ToString(), PLayerPrefKeyType.Float.ToString());
            var data = Recvall();
            ValidateResponse("Ok", data);
        }
    }
}