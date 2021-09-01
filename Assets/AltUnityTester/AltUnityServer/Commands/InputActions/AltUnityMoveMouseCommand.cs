using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityMoveMouseCommand : AltUnityCommand<AltUnityMoveMouseParams, string>
    {
        public AltUnityMoveMouseCommand(AltUnityMoveMouseParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {

#if ALTUNITYTESTER
            Input.MoveMouse(new Vector2(CommandParams.location.x, CommandParams.location.y), CommandParams.duration);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
