namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySetText : AltUnityCommandReturningAltElement
    {
        AltUnitySetTextParams cmdParams;

        public AltUnitySetText(IDriverCommunication commHandler, AltUnityObject altUnityObject, string text) : base(commHandler)
        {
            cmdParams = new AltUnitySetTextParams(altUnityObject, text);
        }

        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltUnityObject(cmdParams);
        }
    }
}