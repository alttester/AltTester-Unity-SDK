using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetCurrentScene : AltBaseCommand
    {
        public AltUnityGetCurrentScene(SocketSettings socketSettings) : base(socketSettings)
        {
        }
        public string Execute()
        {
            SendCommand("getCurrentScene");
            string data = Recvall();
            return JsonConvert.DeserializeObject<AltUnityObject>(data).name;
        }
    }
}