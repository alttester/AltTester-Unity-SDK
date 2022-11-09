using System;
using Altom.AltDriver;
using Altom.AltDriver.Commands;
using Altom.AltTester.Communication;

namespace Altom.AltTester.Commands
{
    class AltTapElementCommand : AltCommandWithWait<AltTapElementParams, AltObject>
    {
        public AltTapElementCommand(ICommandHandler handler, AltTapElementParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override AltObject Execute()
        {
            AltRunner._altRunner.ShowClick(new UnityEngine.Vector2(CommandParams.altObject.GetScreenPosition().x, CommandParams.altObject.GetScreenPosition().y));
            UnityEngine.GameObject gameObject = AltRunner.GetGameObject(CommandParams.altObject.id);

            InputController.TapElement(gameObject, CommandParams.count, CommandParams.interval, onFinish);

            return AltRunner._altRunner.GameObjectToAltObject(gameObject);
        }
    }
}
