using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityTapAtCoordinatesCommand : AltUnityCommand
    {
        readonly float x;
        readonly float y;

        public AltUnityTapAtCoordinatesCommand(params string[] parameters) : base(parameters, 4)
        {
            this.x = JsonConvert.DeserializeObject<float>(parameters[2]);
            this.y = JsonConvert.DeserializeObject<float>(parameters[3]);
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            var clickPosition = new UnityEngine.Vector2(x, y);
            UnityEngine.GameObject gameObject;
            UnityEngine.Camera camera;
            Input.TapAtCoordinates(clickPosition, out gameObject, out camera);
            if (gameObject == null)
                return AltUnityErrors.errorNotFoundMessage;
            return JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera));
#else
            return AltUnityErrors.errorInputModule;
#endif
        }
    }
}
