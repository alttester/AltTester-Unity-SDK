using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetTimeScaleCommand : AltUnityCommand
    {
        readonly float timeScale;

        public AltUnitySetTimeScaleCommand(params string[] parameters) : base(parameters, 3)
        {
            this.timeScale = JsonConvert.DeserializeObject<float>(parameters[2]);
        }

        public override string Execute()
        {
            UnityEngine.Time.timeScale = timeScale;
            return "Ok";
        }
    }
}
