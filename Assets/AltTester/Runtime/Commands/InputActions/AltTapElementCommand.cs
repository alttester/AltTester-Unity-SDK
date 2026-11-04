/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltTapElementCommand : AltCommandWithWait<AltTapElementParams, AltObject>
    {
        public AltTapElementCommand(ICommandHandler handler, AltTapElementParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override AltObject Execute()
        {

                UnityEngine.GameObject gameObject = AltRunner.GetGameObject(CommandParams.altObject.id);

                InputController.TapElement(gameObject, CommandParams.count, CommandParams.interval, onFinish);

                return AltRunner._altRunner.GameObjectToAltObject(gameObject);
        }
    }
}
