namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetFloatKeyPlayerPref : AltBaseCommand
    {
        readonly AltUnityGetKeyPlayerPrefParams cmdParams;
        public AltUnityGetFloatKeyPlayerPref(IDriverCommunication commHandler, string keyName) : base(commHandler)
        {
            cmdParams = new AltUnityGetKeyPlayerPrefParams(keyName, PlayerPrefKeyType.Float);
        }
        public float Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<float>(cmdParams);
        }
    }
}