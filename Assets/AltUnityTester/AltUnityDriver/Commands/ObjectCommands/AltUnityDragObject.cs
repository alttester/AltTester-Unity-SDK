namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityDragObject : AltUnityCommandReturningAltElement
    {
        AltUnityDragObjectParams cmdParams;
        public AltUnityDragObject(IDriverCommunication commHandler, AltUnityVector2 position, AltUnityObject altUnityObject) : base(commHandler)
        {
            cmdParams = new AltUnityDragObjectParams(altUnityObject, position);
        }
        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltUnityObject(cmdParams);
        }
    }
}