/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltSetText : AltCommandReturningAltElement
    {
        AltSetTextParams cmdParams;

        public AltSetText(IDriverCommunication commHandler, AltObject altObject, string text, bool submit) : base(commHandler)
        {
            cmdParams = new AltSetTextParams(altObject, text, submit);
        }

        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltObject(cmdParams);
        }
    }
}
