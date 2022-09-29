using Altom.AltDriver;
using Altom.AltDriver.Commands;

namespace Altom.AltTester.Commands
{
    class AltPointerDownFromObjectCommand : AltCommand<AltPointerDownFromObjectParams, AltObject>
    {

        public AltPointerDownFromObjectCommand(AltPointerDownFromObjectParams cmdParams) : base(cmdParams)
        {
        }

        public override AltObject Execute()
        {
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltRunner.GetGameObject(CommandParams.altUnityObject);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
            var camera = AltRunner._altUnityRunner.FoundCameraById(CommandParams.altUnityObject.idCamera);
            if (camera != null)
            {
                return AltRunner._altUnityRunner.GameObjectToAltObject(gameObject, camera);
            }
            else
            {
                return AltRunner._altUnityRunner.GameObjectToAltObject(gameObject);
            }
        }
    }
}
