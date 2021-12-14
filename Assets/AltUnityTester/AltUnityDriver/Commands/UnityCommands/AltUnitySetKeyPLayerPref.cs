namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySetKeyPLayerPref : AltBaseCommand
    {
        AltUnitySetKeyPlayerPrefParams cmdParams;
        public AltUnitySetKeyPLayerPref(IDriverCommunication commHandler, string keyName, int intValue) : base(commHandler)
        {
            cmdParams = new AltUnitySetKeyPlayerPrefParams(keyName, intValue);
        }
        public AltUnitySetKeyPLayerPref(IDriverCommunication commHandler, string keyName, float floatValue) : base(commHandler)
        {
            cmdParams = new AltUnitySetKeyPlayerPrefParams(keyName, floatValue);
        }
        public AltUnitySetKeyPLayerPref(IDriverCommunication commHandler, string keyName, string stringValue) : base(commHandler)
        {
            cmdParams = new AltUnitySetKeyPlayerPrefParams(keyName, stringValue);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}