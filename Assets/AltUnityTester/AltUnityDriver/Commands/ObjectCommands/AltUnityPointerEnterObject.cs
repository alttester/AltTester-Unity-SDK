namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPointerEnterObject : AltUnityCommandReturningAltElement
    {
        AltUnityPointerEnterObjectParams cmdParams;
        public AltUnityPointerEnterObject(IDriverCommunication commHandler, AltUnityObject altUnityObject) : base(commHandler)
        {
            cmdParams = new AltUnityPointerEnterObjectParams(altUnityObject);
        }
        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltUnityObject(cmdParams);
        }
    }
}