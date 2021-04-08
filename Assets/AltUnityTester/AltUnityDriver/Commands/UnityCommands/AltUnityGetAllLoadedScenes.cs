using System.Collections.Generic;

namespace Altom.AltUnityDriver.Commands
{
    internal class AltUnityGetAllLoadedScenes : AltBaseCommand
    {
        public AltUnityGetAllLoadedScenes(SocketSettings socketSettings) : base(socketSettings)
        {
        }
        public List<string> Execute()
        {
            SendCommand("getAllLoadedScenes");
            var response = Recvall();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(response);

        }
    }
}