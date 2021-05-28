using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityClickEventCommand : AltUnityCommand<AltUnityClickEventParams, AltUnityObject>
    {

        public AltUnityClickEventCommand(AltUnityClickEventParams cmdParams) : base(cmdParams)
        {

        }

        public override AltUnityObject Execute()
        {
            AltUnityRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(CommandParams.altUnityObject.getScreenPosition().x, CommandParams.altUnityObject.getScreenPosition().y));

            UnityEngine.GameObject foundGameObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObject);
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.EventSystems.ExecuteEvents.Execute(foundGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
            return AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundGameObject);
        }
    }
}
