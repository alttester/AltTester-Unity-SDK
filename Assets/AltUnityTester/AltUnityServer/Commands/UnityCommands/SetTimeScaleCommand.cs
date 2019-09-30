namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class SetTimeScaleCommand:Command
    {
        float timeScale;

        public SetTimeScaleCommand(float timeScale)
        {
            this.timeScale = timeScale;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("SetTimeScale to: " + timeScale);
            string response = AltUnityRunner._altUnityRunner.errorCouldNotPerformOperationMessage;
            UnityEngine.Time.timeScale = timeScale;
            response = "Ok";
            return response;
        }
    }
}
