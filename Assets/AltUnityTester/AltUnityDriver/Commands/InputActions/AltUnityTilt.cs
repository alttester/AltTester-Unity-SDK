using System;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityTilt : AltBaseCommand
    {
        AltUnityVector3 acceleration;
        readonly float duration;
        public AltUnityTilt(SocketSettings socketSettings, AltUnityVector3 acceleration, float duration) : base(socketSettings)
        {
            this.acceleration = acceleration;
            this.duration = duration;
        }
        public void Execute()
        {
            string accelerationString = JsonConvert.SerializeObject(acceleration, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

            });
            SendCommand("tilt", accelerationString, duration.ToString());
            string data = Recvall();
            ValidateResponse("Ok", data, StringComparison.OrdinalIgnoreCase);
        }
    }
}