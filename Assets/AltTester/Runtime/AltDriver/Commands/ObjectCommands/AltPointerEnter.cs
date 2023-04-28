namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class AltPointerEnter : AltCommandReturningAltElement
    {
        AltPointerEnterParams cmdParams;
        public AltPointerEnter(IDriverCommunication commHandler, AltObject altObject) : base(commHandler)
        {
            cmdParams = new AltPointerEnterParams(altObject);
        }
        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltObject(cmdParams);
        }
    }
}