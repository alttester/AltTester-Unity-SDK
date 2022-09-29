using System;
using Altom.AltDriver;
using Altom.AltDriver.Commands;
using Altom.AltTester.Communication;
using UnityEngine;

namespace Altom.AltTester.Commands
{
    class AltPressKeyboardKeyCommand : AltCommandWithWait<AltPressKeyboardKeyParams, string>
    {
        public AltPressKeyboardKeyCommand(ICommandHandler handler, AltPressKeyboardKeyParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
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
