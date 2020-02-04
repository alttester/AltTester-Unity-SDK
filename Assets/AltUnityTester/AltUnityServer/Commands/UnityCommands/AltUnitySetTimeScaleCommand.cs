namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetTimeScaleCommand :AltUnityCommand
    {
        float timeScale;

        public AltUnitySetTimeScaleCommand (float timeScale)
        {
            this.timeScale = timeScale;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("SetTimeScale to: " + timeScale);
            string response = AltUnityRunner._altUnityRunner.errorCouldNotPerformOperationMessage;
            UnityEngine.Time.timeScale = timeScale;
            response = "Ok";
            return response;
        }
    }
}
