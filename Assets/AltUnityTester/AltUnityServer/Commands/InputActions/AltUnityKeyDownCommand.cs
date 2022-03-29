using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using UnityEngine;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityKeyDownCommand : AltUnityCommand<AltUnityKeyDownParams, string>
    {
        public AltUnityKeyDownCommand(AltUnityKeyDownParams cmdParams) : base(cmdParams)
        {

        }

        public override string Execute()
        {
#if ENABLE_INPUT_SYSTEM
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
#if ALTUNITYTESTER
            var powerClamped = Mathf.Clamp01(CommandParams.power);
            Input.KeyDown((UnityEngine.KeyCode)CommandParams.keyCode, powerClamped);
            return "Ok";

#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
#endif
        }
    }
}
