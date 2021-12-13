using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityTiltCommand : AltUnityCommandWithWait<AltUnityTiltParams, string>
    {
        public AltUnityTiltCommand(ICommandHandler handler, AltUnityTiltParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            Input.Acceleration(CommandParams.acceleration.ToUnity(), CommandParams.duration, onFinish);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }

    }
}
