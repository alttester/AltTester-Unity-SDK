namespace Altom.AltDriver.Commands
{
    public class AltGetText : AltBaseCommand
    {
        readonly AltGetTextParams cmdParams;

        public AltGetText(IDriverCommunication commHandler, AltObject altUnityObject) : base(commHandler)
        {
            cmdParams = new AltGetTextParams(altUnityObject);
        }

        public string Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<string>(cmdParams);
        }
    }
}