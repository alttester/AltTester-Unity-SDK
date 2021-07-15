using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityClickCoordinatesCommand : AltUnityCommand
    {
        private readonly AltClientSocketHandler handler;
        UnityEngine.Vector2 position;
        readonly int count;
        readonly float interval;
        private readonly bool wait;

        public AltUnityClickCoordinatesCommand(AltClientSocketHandler handler, params string[] parameters) : base(parameters, 6)
        {
            this.handler = handler;
            this.position = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[2]);
            if (!int.TryParse(parameters[3], out count)) { count = 1; }
            if (!float.TryParse(parameters[4], out interval)) { interval = 0.1f; }
            this.wait = bool.Parse(parameters[5]);
        }
        public override string Execute()
        {
#if ALTUNITYTESTER
            Input.ClickCoordinates(position, count, interval, onFinish);
            return "Ok";
#else
            return AltUnityErrors.errorInputModule;
#endif
        }
        private void onFinish()
        {
            if (this.wait)
                handler.SendResponse(this.MessageId, this.CommandName, "Finished", string.Empty);
        }
    }
}
