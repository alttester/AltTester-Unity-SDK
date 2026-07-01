/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltBeginTouch : AltBaseCommand
    {
        AltBeginTouchParams cmdParams;

        public AltBeginTouch(IDriverCommunication commHandler, AltVector2 coordinates) : base(commHandler)
        {
            this.cmdParams = new AltBeginTouchParams(coordinates);
        }
        public int Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<int>(cmdParams);  //finger id
            //TODO: add handling for "Finished"
        }
    }
}
