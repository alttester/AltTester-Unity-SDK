namespace Altom.AltDriver.Commands
{
    public class AltPointerEnterObject : AltCommandReturningAltElement
    {
        AltPointerEnterObjectParams cmdParams;
        public AltPointerEnterObject(IDriverCommunication commHandler, AltObject altUnityObject) : base(commHandler)
        {
            cmdParams = new AltPointerEnterObjectParams(altUnityObject);
        }
        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltObject(cmdParams);
        }
    }
}