using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityScrollMouseCommand : AltUnityCommand
    {
        readonly float scrollValue;
        readonly float duration;

        public AltUnityScrollMouseCommand(params string[] parameters) : base(parameters, 4)
        {
            this.scrollValue = JsonConvert.DeserializeObject<float>(parameters[2]);
            this.duration = JsonConvert.DeserializeObject<float>(parameters[3]);
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            Input.Scroll(scrollValue, duration);
            return "Ok";
#else
            return AltUnityErrors.errorInputModule;
#endif
        }
    }
}
