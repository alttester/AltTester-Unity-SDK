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
            if (!data.Contains("error"))
                return Newtonsoft.Json.JsonConvert.DeserializeObject<float>(data);
            HandleErrors(data);
            return -1f;
        }
    }
}