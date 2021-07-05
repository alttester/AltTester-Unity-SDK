using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityBeginTouchCommand : AltUnityCommand
    {
        UnityEngine.Vector2 position;
        public AltUnityBeginTouchCommand(params string[] parameters) : base(parameters, 3)
        {
            this.position = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[2]);
        }
        public override string Execute()
        {
#if ALTUNITYTESTER
            return Input.BeginTouch(position).ToString();
#else
            return AltUnityErrors.errorInputModule;
#endif
        }
    }
}
