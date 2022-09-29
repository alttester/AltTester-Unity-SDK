namespace Altom.AltDriver.Commands
{
    public class AltPointerExitObject : AltCommandReturningAltElement
    {
        AltPointerExitObjectParams cmdParams;

        public AltPointerExitObject(IDriverCommunication commHandler, AltObject altUnityObject) : base(commHandler)
        {
            this.cmdParams = new AltPointerExitObjectParams(altUnityObject);
        }
        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltObject(cmdParams);
        }
    }
}