using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllScenes : AltBaseCommand
    {
        public AltUnityGetAllScenes(SocketSettings socketSettings) : base(socketSettings)
        {
        }
        public List<string> Execute()
        {
            SendCommand("getAllScenes");
            string data = Recvall();
            return JsonConvert.DeserializeObject<List<string>>(data);

        }
    }
}