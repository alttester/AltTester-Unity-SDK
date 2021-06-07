using System;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPressKey : AltBaseCommand
    {
        [Obsolete("Use keyCode instead.")]
        readonly String keyName;
        readonly AltUnityKeyCode keyCode;
        readonly float power;
        readonly float duration;
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
            this.keyName = "";
        }
        public void Execute()
        {
            if(this.keyName != "")
                SendCommand("pressKeyboardKey", keyName, power.ToString(), duration.ToString());
            else
                SendCommand("pressKeyboardKey", keyCode.ToString(), power.ToString(), duration.ToString());
            var data = Recvall();
            ValidateResponse("Ok", data);
        }
    }
}