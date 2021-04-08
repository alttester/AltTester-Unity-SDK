using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetTimeScaleCommand : AltUnityCommand
    {
        public AltUnityGetTimeScaleCommand(params string[] parameters) : base(parameters, 2)
        { }
        public override string Execute()
        {
            return JsonConvert.SerializeObject(UnityEngine.Time.timeScale);
        }
    }
}
