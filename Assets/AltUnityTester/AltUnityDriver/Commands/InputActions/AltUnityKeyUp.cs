using System;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityKeyUp : AltBaseCommand
    {
        readonly AltUnityKeyCode keyCode;
        public AltUnityKeyUp(SocketSettings socketSettings, AltUnityKeyCode keyCode) : base(socketSettings)
        {
            this.keyCode = keyCode;
        }
        public void Execute()
        {
            SendCommand("keyUp", keyCode.ToString());
            var data = Recvall();
            ValidateResponse("Ok", data);
        }
    }
}