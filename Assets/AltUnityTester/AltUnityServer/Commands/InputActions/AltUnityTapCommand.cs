using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityTapCommand : AltUnityCommand
    {
        readonly AltUnityObject altUnityObject;
        readonly int count;

        public AltUnityTapCommand(params string[] parameters) : base(parameters, 4)
        {
            altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(Parameters[2]);
            count = string.IsNullOrEmpty(Parameters[3]) ? 1 : JsonConvert.DeserializeObject<int>(Parameters[3]);
            count = count < 1 ? 1 : count;
        }

        public override string Execute()
        {
#if ALTUNITYTESTER

            AltUnityRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(altUnityObject.getScreenPosition().x, altUnityObject.getScreenPosition().y));

            var response = AltUnityErrors.errorNotFoundMessage;
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altUnityObject);
            Input.TapObject(gameObject, count);

            var camera = AltUnityRunner._altUnityRunner.FoundCameraById(altUnityObject.idCamera);
            response = JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera));

            return response;
#else
            return AltUnityErrors.errorInputModule;
#endif
        }

    }
}
