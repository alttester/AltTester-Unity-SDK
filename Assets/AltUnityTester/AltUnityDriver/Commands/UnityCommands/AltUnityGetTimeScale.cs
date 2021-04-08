using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetTimeScale : AltBaseCommand
    {
        public AltUnityGetTimeScale(SocketSettings socketSettings) : base(socketSettings)
        {
        }
        public float Execute()
        {
            SendCommand("getTimeScale");
            var data = Recvall();
            return JsonConvert.DeserializeObject<float>(data);
        }
    }
}