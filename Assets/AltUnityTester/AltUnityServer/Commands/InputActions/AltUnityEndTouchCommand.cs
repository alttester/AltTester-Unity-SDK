using Assets.AltUnityTester.AltUnityServer.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityEndTouchCommand : AltUnityCommand
    {
        int fingerId;
        public AltUnityEndTouchCommand(params string[] parameters) : base(parameters, 3)
        {
            if (!int.TryParse(parameters[2], out fingerId)) { fingerId = 0; }
        }
        public override string Execute()
        {
#if ALTUNITYTESTER
            Input.EndTouch(fingerId);
            return "Ok";
#else
            return AltUnityErrors.errorInputModule;
#endif
        }
    }
}