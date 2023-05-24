namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class AltPointerDown : AltCommandReturningAltElement
    {
        AltPointerDownParams cmdParams;
        public AltPointerDown(IDriverCommunication commHandler, AltObject altObject) : base(commHandler)
        {
            this.cmdParams = new AltPointerDownParams(altObject);
        }
        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltObject(cmdParams);
        }
    }
}