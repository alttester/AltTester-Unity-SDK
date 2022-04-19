using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using UnityEngine;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityKeysDownCommand : AltUnityCommand<AltUnityKeysDownParams, string>
    {
        public AltUnityKeysDownCommand(AltUnityKeysDownParams cmdParams) : base(cmdParams)
        {

        }

        public override string Execute()
        {

#if ALTUNITYTESTER
            var powerClamped = Mathf.Clamp01(CommandParams.power);
            foreach (var keyCode in CommandParams.keyCodes)
                InputController.KeyDown((UnityEngine.KeyCode)keyCode, powerClamped);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
