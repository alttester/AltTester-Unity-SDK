using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityFindObjectAtCoordinatesCommand : AltUnityCommand<AltUnityFindObjectAtCoordinatesParams, AltUnityObject>
    {
        public AltUnityFindObjectAtCoordinatesCommand(AltUnityFindObjectAtCoordinatesParams cmdParam) : base(cmdParam) { }

        public override AltUnityObject Execute()
        {
            UnityEngine.GameObject gameObject = FindObjectViaRayCast.FindObjectAtCoordinates(CommandParams.coordinates.ToUnity());

            if (gameObject == null) return null;

            return AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject);
        }


    }
}
