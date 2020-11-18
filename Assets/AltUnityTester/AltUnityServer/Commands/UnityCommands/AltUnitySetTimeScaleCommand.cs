using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetTimeScaleCommand : AltUnityCommand
    {
        float timeScale;

        public AltUnitySetTimeScaleCommand(params string[] parameters) : base(parameters, 3)
        {
            this.timeScale = JsonConvert.DeserializeObject<float>(parameters[2]);
        }

        public override string Execute()
        {
            LogMessage("SetTimeScale to: " + timeScale);
            string response = AltUnityErrors.errorCouldNotPerformOperationMessage;
            UnityEngine.Time.timeScale = timeScale;
            response = "Ok";
            return response;
        }
    }
}
