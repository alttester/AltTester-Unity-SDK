using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityClickEventCommand : AltUnityCommand
    {
        readonly AltUnityObject altUnityObject;

        public AltUnityClickEventCommand(params string[] parameters) : base(parameters, 3)
        {
            this.altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(parameters[2]);
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            AltUnityRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(altUnityObject.getScreenPosition().x, altUnityObject.getScreenPosition().y));

            string response = AltUnityErrors.errorNotFoundMessage;
            UnityEngine.GameObject foundGameObject = AltUnityRunner.GetGameObject(altUnityObject);
            Input.ClickObject(foundGameObject);
            response = JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundGameObject));
            return response;
#else
            return AltUnityErrors.errorInputModule;
#endif
        }
    }
}
