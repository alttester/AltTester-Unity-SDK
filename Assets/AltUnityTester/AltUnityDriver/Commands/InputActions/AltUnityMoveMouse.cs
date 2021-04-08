namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityMoveMouse : AltBaseCommand
    {
        AltUnityVector2 location;
        float duration;
        public AltUnityMoveMouse(SocketSettings socketSettings, AltUnityVector2 location, float duration) : base(socketSettings)
        {
            this.location = location;
            this.duration = duration;
        }
        public void Execute()
        {
            var locationJson = PositionToJson(location);
            SendCommand("moveMouse", locationJson, duration.ToString());
            var data = Recvall();
            ValidateResponse("Ok", data);
        }
    }
}