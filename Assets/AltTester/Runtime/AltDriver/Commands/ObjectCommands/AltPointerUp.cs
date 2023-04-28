namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class AltPointerUp : AltCommandReturningAltElement
    {
        AltPointerUpParams cmdParams;

        public AltPointerUp(IDriverCommunication commHandler, AltObject altObject) : base(commHandler)
        {
            this.cmdParams = new AltPointerUpParams(altObject);
        }
        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltObject(cmdParams);
        }
    }
}