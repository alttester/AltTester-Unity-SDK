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
#if ALTUNITYTESTER
            var powerClamped = Mathf.Clamp01(CommandParams.power);
            Input.KeyPress((UnityEngine.KeyCode)CommandParams.keyCode, CommandParams.power, CommandParams.duration, onFinish);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
