using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityFindObjectAtCoordinatesCommand : AltUnityCommand<AltUnityFindObjectAtCoordinatesParams, AltUnityObject>
    {
        public AltUnityFindObjectAtCoordinatesCommand(AltUnityFindObjectAtCoordinatesParams cmdParam) : base(cmdParam) { }

        public override AltUnityObject Execute()
        {
#if ALTUNITYTESTER
            UnityEngine.GameObject gameObject = Input.FindObjectAtCoordinates(CommandParams.coordinates.ToUnity());

            if (gameObject == null) return null;

            return AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject);
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
