using AltTester.AltDriver;
using AltTester.AltDriver.Commands;

namespace AltTester.Commands
{
    class AltPointerDownFromObjectCommand : AltCommand<AltPointerDownFromObjectParams, AltObject>
    {

        public AltPointerDownFromObjectCommand(AltPointerDownFromObjectParams cmdParams) : base(cmdParams)
        {
        }

        public override AltObject Execute()
        {
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltRunner.GetGameObject(CommandParams.altObject.id);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
            var camera = AltRunner._altRunner.FoundCameraById(CommandParams.altObject.idCamera);
            if (camera != null)
            {
                return AltRunner._altRunner.GameObjectToAltObject(gameObject, camera);
            }
            else
            {
                return AltRunner._altRunner.GameObjectToAltObject(gameObject);
            }
        }
    }
}
