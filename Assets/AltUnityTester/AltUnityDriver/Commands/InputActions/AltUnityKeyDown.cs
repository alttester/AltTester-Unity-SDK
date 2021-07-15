using System;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityKeyDown : AltBaseCommand
    {
        readonly AltUnityKeyCode keyCode;
        readonly float power;
        public AltUnityKeyDown(SocketSettings socketSettings, AltUnityKeyCode keyCode, float power) : base(socketSettings)
        {
            this.keyCode = keyCode;
            this.power = power;
        }
        public void Execute()
        {
            SendCommand("keyDown", keyCode.ToString(), power.ToString());
            var data = Recvall();
            ValidateResponse("Ok", data);
        }
    }
}