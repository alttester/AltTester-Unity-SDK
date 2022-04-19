using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;
using UnityEngine;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityPressKeyboardKeysCommand : AltUnityCommandWithWait<AltUnityPressKeyboardKeysParams, string>
    {
        public AltUnityPressKeyboardKeysCommand(ICommandHandler handler, AltUnityPressKeyboardKeysParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            var powerClamped = Mathf.Clamp01(CommandParams.power);
            foreach (var keyCode in CommandParams.keyCodes)
                InputController.PressKey((UnityEngine.KeyCode)keyCode, CommandParams.power, CommandParams.duration, onFinish);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
