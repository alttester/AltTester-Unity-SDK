using System;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPressKey : AltBaseCommand
    {
        private readonly String keyName = "";
        private readonly AltUnityKeyCode keyCode;
        private readonly float power;
        private readonly float duration;

        [Obsolete("Use AltUnityKeyPress(SocketSettings, AltUnityKeyCode, float, float) instead.")]
        public AltUnityPressKey(SocketSettings socketSettings, String keyName, float power, float duration) : base(socketSettings)
        {
            this.keyName = keyName;
            this.power = power;
            this.duration = duration;
            this.keyCode = AltUnityKeyCode.NoKey;
        }
        public AltUnityPressKey(SocketSettings socketSettings, AltUnityKeyCode keyCode, float power, float duration) : base(socketSettings)
        {
            this.keyCode = keyCode;
            this.power = power;
            this.duration = duration;
        }
        public void Execute()
        {
            if (this.keyName != "")
                SendCommand("pressKeyboardKey", keyName, power.ToString(), duration.ToString());
            else
                SendCommand("pressKeyboardKey", keyCode.ToString(), power.ToString(), duration.ToString());
            var data = Recvall();
            ValidateResponse("Ok", data);
        }
    }
}