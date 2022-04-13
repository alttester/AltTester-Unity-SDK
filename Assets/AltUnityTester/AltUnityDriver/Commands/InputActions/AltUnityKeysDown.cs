using System;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityKeysDown : AltBaseCommand
    {
        AltUnityKeysDownParams cmdParams;

        public AltUnityKeysDown(IDriverCommunication commHandler, AltUnityKeyCode[] keyCodes, float power) : base(commHandler)
        {
            this.cmdParams = new AltUnityKeysDownParams(keyCodes, power);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}