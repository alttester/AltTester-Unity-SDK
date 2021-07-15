using Assets.AltUnityTester.AltUnityServer.Commands;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityMoveTouchCommand : AltUnityCommand
    {
        UnityEngine.Vector2 position;
        int fingerId;
        public AltUnityMoveTouchCommand(params string[] parameters) : base(parameters, 4)
        {
            if (!int.TryParse(parameters[2], out fingerId)) { fingerId = 0; }
            this.position = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[3]);
        }
        public override string Execute()
        {
#if ALTUNITYTESTER
            Input.MoveTouch(fingerId, position);
            return "Ok";
#else
            return AltUnityErrors.errorInputModule;
#endif
        }
    }
}