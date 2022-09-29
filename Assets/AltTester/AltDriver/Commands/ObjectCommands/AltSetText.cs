namespace Altom.AltDriver.Commands
{
    public class AltSetText : AltCommandReturningAltElement
    {
        AltSetTextParams cmdParams;

        public AltSetText(IDriverCommunication commHandler, AltObject altUnityObject, string text, bool submit) : base(commHandler)
        {
            cmdParams = new AltSetTextParams(altUnityObject, text, submit);
        }

        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltObject(cmdParams);
        }
    }
}