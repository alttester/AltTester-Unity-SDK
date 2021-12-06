using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityScrollCommand : AltUnityCommandWithWait<AltUnityScrollParams, string>
    {


        public AltUnityScrollCommand(ICommandHandler handler, AltUnityScrollParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {

#if ALTUNITYTESTER
            Input.Scroll(CommandParams.speed, CommandParams.duration, onFinish);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);

#endif
        }
    }
}
