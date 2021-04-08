namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySwipe : AltBaseCommand
    {
        AltUnityVector2 start;
        AltUnityVector2 end;
        readonly float duration;
        public AltUnitySwipe(SocketSettings socketSettings, AltUnityVector2 start, AltUnityVector2 end, float duration) : base(socketSettings)
        {
            this.start = start;
            this.end = end;
            this.duration = duration;
        }
        public void Execute()
        {
            var vectorStartJson = PositionToJson(start);
            var vectorEndJson = PositionToJson(end);

            SendCommand("multipointSwipe", vectorStartJson, vectorEndJson, duration.ToString());
            var data = Recvall();
            ValidateResponse("Ok", data);
        }
    }
}