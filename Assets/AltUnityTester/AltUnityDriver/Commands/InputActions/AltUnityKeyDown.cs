using System;
using System.Threading;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityKeyDown : AltBaseCommand
    {
        AltUnityKeyDownParams cmdParams;

        public AltUnityKeyDown(IDriverCommunication commHandler, AltUnityKeyCode keyCode, float power) : base(commHandler)
        {
            this.cmdParams = new AltUnityKeyDownParams(keyCode, power);
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