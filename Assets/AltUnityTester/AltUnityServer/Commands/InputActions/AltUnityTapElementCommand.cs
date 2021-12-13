using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityTapElementCommand : AltUnityCommandWithWait<AltUnityTapElementParams, AltUnityObject>
    {
        public AltUnityTapElementCommand(ICommandHandler handler, AltUnityTapElementParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override AltUnityObject Execute()
        {

#if ALTUNITYTESTER
            AltUnityRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(CommandParams.altUnityObject.getScreenPosition().x, CommandParams.altUnityObject.getScreenPosition().y));
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObject);

            Input.TapElement(gameObject, CommandParams.count, CommandParams.interval, onFinish);

            return AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject);
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
