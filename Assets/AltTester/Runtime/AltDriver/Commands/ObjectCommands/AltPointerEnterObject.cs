/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltPointerEnterObject : AltCommandReturningAltElement
    {
        AltPointerEnterObjectParams cmdParams;
        public AltPointerEnterObject(IDriverCommunication commHandler, AltObject altObject) : base(commHandler)
        {
            cmdParams = new AltPointerEnterObjectParams(altObject);
        }
        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltObject(cmdParams);
        }
    }
}
