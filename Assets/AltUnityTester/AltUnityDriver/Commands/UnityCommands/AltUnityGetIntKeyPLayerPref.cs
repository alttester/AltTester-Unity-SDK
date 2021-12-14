namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetIntKeyPlayerPref : AltBaseCommand
    {
        readonly AltUnityGetKeyPlayerPrefParams cmdParams;
        public AltUnityGetIntKeyPlayerPref(IDriverCommunication commHandler, string keyName) : base(commHandler)
        {
            cmdParams = new AltUnityGetKeyPlayerPrefParams(keyName, PlayerPrefKeyType.Int);
        }
        public int Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<int>(cmdParams);
        }
    }
}