namespace Altom.AltDriver.Commands
{
    public class AltDragObject : AltCommandReturningAltElement
    {
        AltDragObjectParams cmdParams;
        public AltDragObject(IDriverCommunication commHandler, AltVector2 position, AltObject altUnityObject) : base(commHandler)
        {
            cmdParams = new AltDragObjectParams(altUnityObject, position);
        }
        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltObject(cmdParams);
        }
    }
}