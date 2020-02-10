namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetTimeScaleCommand :  AltUnityCommand
    {
        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("GetTimeScale");
            string response = AltUnityRunner._altUnityRunner.errorCouldNotPerformOperationMessage;
            response = Newtonsoft.Json.JsonConvert.SerializeObject(UnityEngine.Time.timeScale);
            return response;
        }
    }
}
