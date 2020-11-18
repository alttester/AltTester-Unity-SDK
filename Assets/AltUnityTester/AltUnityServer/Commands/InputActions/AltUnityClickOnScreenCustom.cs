using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityClickOnScreenCustom : AltUnityCommand
    {
        UnityEngine.Vector2 position;
        string count;
        string interval;

        public AltUnityClickOnScreenCustom(params string[] parameters) : base(parameters, 5)
        {
            this.position = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[2]);
            this.count = Parameters[3];
            this.interval = Parameters[4];
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            LogMessage("Custom click at: " + position);
            var response = AltUnityErrors.errorNotFoundMessage;

            var pCount = 1;
            var pInterval = 0f;
            int.TryParse(count, out pCount);
            float.TryParse(interval, out pInterval);

            Input.SetCustomClick(position, pCount, pInterval);
            response = "Ok";
            return response;
#else
            return null;
#endif
        }
    }
}