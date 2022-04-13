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
            var powerClamped = Mathf.Clamp(CommandParams.power, -1, 1);
            InputController.PressKey((UnityEngine.KeyCode)CommandParams.keyCode, CommandParams.power, CommandParams.duration, onFinish);
            return "Ok";
        }
    }
}
