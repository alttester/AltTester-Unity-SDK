namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPressKey : AltBaseCommand
    {
        AltUnityKeyCode keyCode;
        float power;
        float duration;
        public AltUnityPressKey(SocketSettings socketSettings, AltUnityKeyCode keyCode, float power, float duration) : base(socketSettings)
        {
            this.keyCode = keyCode;
            this.power = power;
            this.duration = duration;
        }
        public void Execute()
        {
            SendCommand("pressKeyboardKey", keyCode.ToString(), power.ToString(), duration.ToString());
            var data = Recvall();
            if (data.Equals("Ok"))
                return;
            HandleErrors(data);
        }
    }
}