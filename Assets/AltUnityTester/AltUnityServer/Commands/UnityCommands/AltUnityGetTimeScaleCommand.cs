namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetTimeScaleCommand : AltUnityCommand
    {
        public AltUnityGetTimeScaleCommand(params string[] parameters) : base(parameters, 2)
        { }
        public override string Execute()
        {
            LogMessage("GetTimeScale");
            string response = AltUnityErrors.errorCouldNotPerformOperationMessage;
            response = Newtonsoft.Json.JsonConvert.SerializeObject(UnityEngine.Time.timeScale);
            return response;
        }
    }
}
