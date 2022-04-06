using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityClickElementCommand : AltUnityCommandWithWait<AltUnityClickElementParams, AltUnityObject>
    {
        public AltUnityClickElementCommand(ICommandHandler handler, AltUnityClickElementParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override AltUnityObject Execute()
        {

            AltUnityRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(CommandParams.altUnityObject.getScreenPosition().x, CommandParams.altUnityObject.getScreenPosition().y));
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObject);

            InputController.ClickElement(gameObject, CommandParams.count, CommandParams.interval, onFinish);
            return AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject);
        }
    }
}
