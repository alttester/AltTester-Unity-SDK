namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityTap : AltUnityCommandReturningAltElement
    {
        AltUnityTapObjectParams cmdParams;
        public AltUnityTap(IDriverCommunication commHandler, AltUnityObject altUnityObject, int count) : base(commHandler)
        {
            this.cmdParams = new AltUnityTapObjectParams(altUnityObject, count);
        }

        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltUnityObject(cmdParams);
        }
    }
}