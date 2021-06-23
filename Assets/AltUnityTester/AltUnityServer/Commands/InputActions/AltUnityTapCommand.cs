using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityTapCommand : AltUnityCommand<AltUnityTapObjectParams, AltUnityObject>
    {

        public AltUnityTapCommand(AltUnityTapObjectParams cmdParams) : base(cmdParams)
        {
        }

        public override AltUnityObject Execute()
        {
#if ALTUNITYTESTER
            AltUnityRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(CommandParams.altUnityObject.getScreenPosition().x, CommandParams.altUnityObject.getScreenPosition().y));
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObject);
            Input.TapObject(gameObject, CommandParams.count);


            var camera = AltUnityRunner._altUnityRunner.FoundCameraById(CommandParams.altUnityObject.idCamera);
            return AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera);
#else
            return null;
#endif

        }


    }
}
