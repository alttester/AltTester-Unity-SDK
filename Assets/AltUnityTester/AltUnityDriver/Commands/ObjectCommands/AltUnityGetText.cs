namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetText : AltBaseCommand
    {
        readonly AltUnityGetTextParams cmdParams;

        public AltUnityGetText(IDriverCommunication commHandler, AltUnityObject altUnityObject) : base(commHandler)
        {
            cmdParams = new AltUnityGetTextParams(altUnityObject);
        }

        public string Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<string>(cmdParams);
        }
    }
}