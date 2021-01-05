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
            if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data).name;
            HandleErrors(data);
            return null;
        }
    }
}