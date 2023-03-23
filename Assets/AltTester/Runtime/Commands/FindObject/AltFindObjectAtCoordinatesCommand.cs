using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;

namespace AltTester.AltTesterUnitySdk.Commands
{
    class AltFindObjectAtCoordinatesCommand : AltCommand<AltFindObjectAtCoordinatesParams, AltObject>
    {
        public AltFindObjectAtCoordinatesCommand(AltFindObjectAtCoordinatesParams cmdParam) : base(cmdParam) { }

        public override AltObject Execute()
        {
            UnityEngine.GameObject gameObject = FindObjectViaRayCast.FindObjectAtCoordinates(CommandParams.coordinates.ToUnity());

            if (gameObject == null) return null;

            return AltRunner._altRunner.GameObjectToAltObject(gameObject);
        }


    }
}
