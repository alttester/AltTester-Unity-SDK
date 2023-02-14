using System;
using AltTester.AltDriver;
using AltTester.AltDriver.Commands;
using AltTester.Communication;

namespace AltTester.Commands
{
    class AltClickElementCommand : AltCommandWithWait<AltClickElementParams, AltObject>
    {
        public AltClickElementCommand(ICommandHandler handler, AltClickElementParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override AltObject Execute()
        {
            AltRunner._altRunner.ShowClick(new UnityEngine.Vector2(CommandParams.altObject.GetScreenPosition().x, CommandParams.altObject.GetScreenPosition().y));
            UnityEngine.GameObject gameObject = AltRunner.GetGameObject(CommandParams.altObject.id);
            
            InputController.ClickElement(gameObject, CommandParams.count, CommandParams.interval, onFinish);
            return AltRunner._altRunner.GameObjectToAltObject(gameObject);
        }
    }
}
