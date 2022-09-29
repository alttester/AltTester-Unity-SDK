namespace Altom.AltDriver.Commands
{
    public class AltPointerDownFromObject : AltCommandReturningAltElement
    {
        AltPointerDownFromObjectParams cmdParams;
        public AltPointerDownFromObject(IDriverCommunication commHandler, AltObject altUnityObject) : base(commHandler)
        {
            this.cmdParams = new AltPointerDownFromObjectParams(altUnityObject);
        }
        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltObject(cmdParams);
        }
    }
}