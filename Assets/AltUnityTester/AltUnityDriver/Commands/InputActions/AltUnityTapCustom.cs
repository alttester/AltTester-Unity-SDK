namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityTapCustom : AltBaseCommand
    {
        float x;
        float y;
        int count;
        float interval;
        public AltUnityTapCustom(SocketSettings socketSettings, float x, float y, int count, float interval) : base(socketSettings)
        {
            this.x = x;
            this.y = y;
            this.count = count;
            this.interval = interval;
        }
        public void Execute()
        {
            var posJson = PositionToJson(x, y);
            SendCommand("tapCustom", posJson, count.ToString(), interval.ToString());
            string data = Recvall();
            ValidateResponse("Ok", data);
        }
    }
}