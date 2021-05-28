namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPointerExitObject : AltUnityCommandReturningAltElement
    {
        AltUnityPointerExitObjectParams cmdParams;

        public AltUnityPointerExitObject(IDriverCommunication commHandler, AltUnityObject altUnityObject) : base(commHandler)
        {
            this.cmdParams = new AltUnityPointerExitObjectParams(altUnityObject);
        }
        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltUnityObject(cmdParams);
        }
    }
}