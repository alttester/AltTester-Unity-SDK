namespace Altom.AltDriver.Commands
{
    public class AltPointerUpFromObject : AltCommandReturningAltElement
    {
        AltPointerUpFromObjectParams cmdParams;

        public AltPointerUpFromObject(IDriverCommunication commHandler, AltObject altUnityObject) : base(commHandler)
        {
            this.cmdParams = new AltPointerUpFromObjectParams(altUnityObject);
        }
        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltObject(cmdParams);
        }
    }
}