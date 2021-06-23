using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityTapAtCoordinatesCommand : AltUnityCommand<AltUnityTapScreenParams, AltUnityObject>
    {


        public AltUnityTapAtCoordinatesCommand(AltUnityTapScreenParams cmdParams) : base(cmdParams)
        {

        }

        public override AltUnityObject Execute()
        {
#if ALTUNITYTESTER
            var clickPosition = new UnityEngine.Vector2(CommandParams.x, CommandParams.y);
            UnityEngine.GameObject gameObject;
            UnityEngine.Camera camera;
            Input.TapAtCoordinates(clickPosition, out gameObject, out camera);
            if (gameObject == null)
                throw new NotFoundException(AltUnityErrors.errorNotFoundMessage);
            return AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera);
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);

#endif
        }
    }
}
