namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetStringKeyPlayerPref : AltBaseCommand
    {
        readonly AltUnityGetKeyPlayerPrefParams cmdParams;
        public AltUnityGetStringKeyPlayerPref(IDriverCommunication commHandler, string keyName) : base(commHandler)
        {
            cmdParams = new AltUnityGetKeyPlayerPrefParams(keyName, PlayerPrefKeyType.String);
        }
        public string Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<string>(cmdParams);
        }
    }
}