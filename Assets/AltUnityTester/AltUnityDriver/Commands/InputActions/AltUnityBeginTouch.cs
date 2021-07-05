using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityBeginTouch : AltBaseCommand
    {
        AltUnityVector2 coordinates;

        public AltUnityBeginTouch(SocketSettings socketSettings, AltUnityVector2 coordinates) : base(socketSettings)
        {
            this.coordinates = coordinates;
        }
        public int Execute()
        {
            var posJson = PositionToJson(coordinates.x, coordinates.y);
            SendCommand("beginTouch", posJson);
            string data = Recvall();
            int fingerId = int.Parse(data);
            return fingerId;
        }
    }
}