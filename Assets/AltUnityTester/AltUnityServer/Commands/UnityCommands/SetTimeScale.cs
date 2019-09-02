namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class SetTimeScale:Command
    {
        float timeScale;

        public SetTimeScale(float timeScale)
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
