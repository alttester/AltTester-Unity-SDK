namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityTapCoordinates : AltBaseCommand
    {
        AltUnityVector2 coordinates;
        readonly int count;
        readonly float interval;
        readonly bool wait;
        public AltUnityTapCoordinates(SocketSettings socketSettings, AltUnityVector2 coordinates, int count, float interval, bool wait) : base(socketSettings)
        {
            this.coordinates = coordinates;
            this.count = count;
            this.interval = interval;
            this.wait = wait;
        }
        public void Execute()
        {
            var posJson = PositionToJson(coordinates.x, coordinates.y);
            SendCommand("tapCoordinates", posJson, count.ToString(), interval.ToString(), wait.ToString());
            string data = Recvall();
            ValidateResponse("Ok", data);
            if (wait)
            {
                data = Recvall();
                ValidateResponse("Finished", data);
            }
        }
    }
}