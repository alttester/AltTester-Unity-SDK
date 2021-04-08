namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityScrollMouse : AltBaseCommand
    {
        readonly float speed;
        readonly float duration;
        public AltUnityScrollMouse(SocketSettings socketSettings, float speed, float duration) : base(socketSettings)
        {
            this.speed = speed;
            this.duration = duration;
        }
        public void Execute()
        {
            SendCommand("scrollMouse", speed.ToString(), duration.ToString());
            var data = Recvall();
            ValidateResponse("Ok", data);
        }
    }
}