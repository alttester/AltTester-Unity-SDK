namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityClickEvent : AltUnityCommandReturningAltElement
    {
        AltUnityClickEventParams cmdParams;
        public AltUnityClickEvent(IDriverCommunication commHandler, AltUnityObject altUnityObject) : base(commHandler)
        {
            cmdParams = new AltUnityClickEventParams(altUnityObject);
        }
        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltUnityObject(cmdParams);
        }
    }
}