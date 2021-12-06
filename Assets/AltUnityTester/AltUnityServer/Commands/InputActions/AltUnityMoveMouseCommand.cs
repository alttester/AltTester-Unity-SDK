using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;
using UnityEngine;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityMoveMouseCommand : AltUnityCommandWithWait<AltUnityMoveMouseParams, string>
    {
        public AltUnityMoveMouseCommand(ICommandHandler handler, AltUnityMoveMouseParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {

#if ALTUNITYTESTER
            Input.MoveMouse(new Vector2(CommandParams.coordinates.x, CommandParams.coordinates.y), CommandParams.duration, onFinish);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
