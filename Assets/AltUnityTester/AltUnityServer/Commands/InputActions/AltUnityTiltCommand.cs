using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityTiltCommand : AltUnityCommand
    {
        UnityEngine.Vector3 acceleration;
        float duration;

        public AltUnityTiltCommand(params string[] parameters) : base(parameters, 4)
        {
            this.acceleration = JsonConvert.DeserializeObject<UnityEngine.Vector3>(parameters[2]);
            this.duration = float.Parse(parameters[3]);
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            LogMessage("Tilt device with: " + acceleration);
            Input.Acceleration(acceleration, duration);
            return "OK";
#else
            return null;
#endif
        }
    }
}
