using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityTiltCommand : AltUnityCommand
    {
        private UnityEngine.Vector3 acceleration;
        private readonly float duration;

        public AltUnityTiltCommand(params string[] parameters) : base(parameters, 4)
        {
            this.acceleration = JsonConvert.DeserializeObject<UnityEngine.Vector3>(parameters[2]);
            this.duration = JsonConvert.DeserializeObject<float>(parameters[3]);
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            Input.Acceleration(acceleration, duration);
            return "OK";
#else
            return AltUnityErrors.errorInputModule;
#endif
        }
    }
}
