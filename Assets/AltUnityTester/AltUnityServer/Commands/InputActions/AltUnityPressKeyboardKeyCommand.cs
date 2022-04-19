using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;
using UnityEngine;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityPressKeyboardKeyCommand : AltUnityCommandWithWait<AltUnityPressKeyboardKeyParams, string>
    {
        public AltUnityPressKeyboardKeyCommand(ICommandHandler handler, AltUnityPressKeyboardKeyParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
#if ENABLE_INPUT_SYSTEM
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
#if ALTUNITYTESTER
            var powerClamped = Mathf.Clamp01(CommandParams.power);
            Input.KeyPress((UnityEngine.KeyCode)CommandParams.keyCode, CommandParams.power, CommandParams.duration, onFinish);

#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
#endif
            return "Ok";

        }
    }
}
