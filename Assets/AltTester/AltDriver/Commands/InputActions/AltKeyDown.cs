using System;
using System.Threading;

namespace Altom.AltDriver.Commands
{
    public class AltKeyDown : AltBaseCommand
    {
        AltKeyDownParams cmdParams;

        public AltKeyDown(IDriverCommunication commHandler, AltKeyCode keyCode, float power) : base(commHandler)
        {
            this.cmdParams = new AltKeyDownParams(keyCode, power);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            Thread.Sleep(100);
            ValidateResponse("Ok", data);
        }
    }
}