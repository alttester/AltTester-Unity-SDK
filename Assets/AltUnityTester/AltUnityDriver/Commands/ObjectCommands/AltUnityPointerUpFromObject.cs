namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPointerUpFromObject : AltUnityCommandReturningAltElement
    {
        AltUnityPointerUpFromObjectParams cmdParams;

        public AltUnityPointerUpFromObject(IDriverCommunication commHandler, AltUnityObject altUnityObject) : base(commHandler)
        {
            this.cmdParams = new AltUnityPointerUpFromObjectParams(altUnityObject);
        }
        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltUnityObject(cmdParams);
        }
    }
}