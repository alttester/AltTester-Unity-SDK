using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySetTimeScale : AltBaseCommand
    {
        readonly float timeScale;

        public AltUnitySetTimeScale(SocketSettings socketSettings, float timescale) : base(socketSettings)
        {
            this.timeScale = timescale;
        }
        public void Execute()
        {
            SendCommand("setTimeScale", JsonConvert.SerializeObject(timeScale));
            var data = Recvall();
            ValidateResponse("Ok", data);
        }
    }
}