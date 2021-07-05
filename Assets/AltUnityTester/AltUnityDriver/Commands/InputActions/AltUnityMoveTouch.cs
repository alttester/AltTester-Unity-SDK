namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityMoveTouch : AltBaseCommand
    {
        AltUnityVector2 coordinates;
        int fingerId;

        public AltUnityMoveTouch(SocketSettings socketSettings, int fingerId, AltUnityVector2 coordinates) : base(socketSettings)
        {
            this.coordinates = coordinates;
            this.fingerId = fingerId;
        }
        public void Execute()
        {
            var posJson = PositionToJson(coordinates.x, coordinates.y);
            SendCommand("moveTouch", fingerId.ToString(), posJson);
            string data = Recvall();
            ValidateResponse("Ok", data);
        }
    }
}