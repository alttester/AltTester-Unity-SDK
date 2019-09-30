namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class GetTimeScaleCommand :  Command
    {
        public override string Execute()
        {
            UnityEngine.Debug.Log("GetTimeScale");
            string response = AltUnityRunner._altUnityRunner.errorCouldNotPerformOperationMessage;
            response = Newtonsoft.Json.JsonConvert.SerializeObject(UnityEngine.Time.timeScale);
            return response;
        }
    }
}
