using System;
using System.Threading;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPressKeyAndWait : AltBaseCommand
    {
        private readonly String keyName = "";
        private readonly AltUnityKeyCode keyCode;
        private readonly float power;
        private readonly float duration;

        [Obsolete("Use AltUnityPressKeyAndWait(SocketSettings, AltUnityKeyCode, float, float) instead.")]
        public AltUnityPressKeyAndWait(SocketSettings socketSettings, String keyName, float power, float duration) : base(socketSettings)
        {
            this.keyName = keyName;
            this.power = power;
            this.duration = duration;
            this.keyCode = AltUnityKeyCode.NoKey;
        }
        public AltUnityPressKeyAndWait(SocketSettings socketSettings, AltUnityKeyCode keyCode, float power, float duration) : base(socketSettings)
        {
            this.keyCode = keyCode;
            this.power = power;
            this.duration = duration;
            this.keyName = "";
        }
        public void Execute()
        {
            if (keyName != "")
                new AltUnityPressKey(SocketSettings, keyName, power, duration).Execute();
            else
                new AltUnityPressKey(SocketSettings, keyCode, power, duration).Execute();
            Thread.Sleep((int)(duration * 1000));
            string data;
            do
            {
                SendCommand("actionFinished");
                data = Recvall();
            } while (data == "No");
            ValidateResponse("Yes", data);
        }
    }
}