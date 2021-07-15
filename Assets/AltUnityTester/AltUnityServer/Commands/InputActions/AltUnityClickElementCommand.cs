using Altom.AltUnityDriver;
using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityClickElementCommand : AltUnityCommand
    {
        private readonly AltClientSocketHandler handler;
        readonly AltUnityObject altUnityObject;
        readonly int count;
        readonly float interval;
        private readonly bool wait;

        public AltUnityClickElementCommand(AltClientSocketHandler handler, params string[] parameters) : base(parameters, 6)
        {
            this.handler = handler;
            this.altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(parameters[2]);
            if (!int.TryParse(parameters[3], out count)) { count = 1; }
            if (!float.TryParse(parameters[4], out interval)) { interval = 0.1f; }
            this.wait = bool.Parse(parameters[5]);
        }

        public override string Execute()
        {

#if ALTUNITYTESTER
            AltUnityRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(altUnityObject.getScreenPosition().x, altUnityObject.getScreenPosition().y));
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altUnityObject);

            Input.ClickElement(gameObject, count, interval, onFinish);

            return JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject));
#else
            return AltUnityErrors.errorInputModule;
#endif
        }
        private void onFinish(UnityEngine.GameObject gameObject)
        {
            if (this.wait)
                handler.SendResponse(this.MessageId, this.CommandName, "Finished", string.Empty);
        }
    }
}
