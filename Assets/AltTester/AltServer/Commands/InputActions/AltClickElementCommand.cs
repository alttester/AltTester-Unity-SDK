using System;
using Altom.AltDriver;
using Altom.AltDriver.Commands;
using Altom.AltTester.Communication;

namespace Altom.AltTester.Commands
{
    class AltClickElementCommand : AltCommandWithWait<AltClickElementParams, AltObject>
    {
        public AltClickElementCommand(ICommandHandler handler, AltClickElementParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override AltObject Execute()
        {

            AltRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(CommandParams.altUnityObject.getScreenPosition().x, CommandParams.altUnityObject.getScreenPosition().y));
            UnityEngine.GameObject gameObject = AltRunner.GetGameObject(CommandParams.altUnityObject);

            InputController.ClickElement(gameObject, CommandParams.count, CommandParams.interval, onFinish);
            return AltRunner._altUnityRunner.GameObjectToAltObject(gameObject);
        }
    }
}
