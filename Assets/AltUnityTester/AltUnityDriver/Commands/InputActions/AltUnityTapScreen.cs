using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityTapScreen : AltBaseCommand
    {
        readonly float x;
        readonly float y;
        public AltUnityTapScreen(SocketSettings socketSettings, float x, float y) : base(socketSettings)
        {
            this.x = x;
            this.y = y;
        }
        public AltUnityObject Execute()
        {
            SendCommand("tapScreen", x.ToString(), y.ToString());
            try
            {
                string data = Recvall();
                return JsonConvert.DeserializeObject<AltUnityObject>(data);
            }
            catch (NotFoundException)
            {
                return null;
            }
        }
    }
}