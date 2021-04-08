using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityLoadScene : AltBaseCommand
    {
        readonly string sceneName;
        readonly bool loadSingle;
        public AltUnityLoadScene(SocketSettings socketSettings, string sceneName, bool loadSingle) : base(socketSettings)
        {
            this.sceneName = sceneName;
            this.loadSingle = loadSingle;
        }
        public void Execute()
        {
            SendCommand("loadScene", sceneName, JsonConvert.SerializeObject(loadSingle));

            var data = Recvall();
            ValidateResponse("Ok", data);

            data = Recvall();
            ValidateResponse("Scene Loaded", data);
        }
    }
}