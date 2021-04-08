using System.Threading;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPressKeyAndWait : AltBaseCommand
    {
        readonly AltUnityKeyCode keyCode;
        readonly float power;
        readonly float duration;
        public AltUnityPressKeyAndWait(SocketSettings socketSettings, AltUnityKeyCode keyCode, float power, float duration) : base(socketSettings)
        {
            this.keyCode = keyCode;
            this.power = power;
            this.duration = duration;
        }
        public void Execute()
        {
            new AltUnityPressKey(SocketSettings, keyCode, power, duration).Execute();
            Thread.Sleep((int)duration * 1000);
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