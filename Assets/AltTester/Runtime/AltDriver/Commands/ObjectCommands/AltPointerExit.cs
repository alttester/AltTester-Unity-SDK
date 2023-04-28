namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class AltPointerExit : AltCommandReturningAltElement
    {
        AltPointerExitParams cmdParams;

        public AltPointerExit(IDriverCommunication commHandler, AltObject altObject) : base(commHandler)
        {
            this.cmdParams = new AltPointerExitParams(altObject);
        }
        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltObject(cmdParams);
        }
    }
}