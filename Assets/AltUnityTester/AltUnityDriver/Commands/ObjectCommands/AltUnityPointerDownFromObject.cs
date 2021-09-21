namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPointerDownFromObject : AltUnityCommandReturningAltElement
    {
        AltUnityPointerDownFromObjectParams cmdParams;
        public AltUnityPointerDownFromObject(IDriverCommunication commHandler, AltUnityObject altUnityObject) : base(commHandler)
        {
            this.cmdParams = new AltUnityPointerDownFromObjectParams(altUnityObject);
        }
        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltUnityObject(cmdParams);
        }
    }
}