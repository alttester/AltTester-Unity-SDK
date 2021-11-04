using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityScrollMouseCommand : AltUnityCommand<AltUnityScrollMouseParams, string>
    {


        public AltUnityScrollMouseCommand(AltUnityScrollMouseParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {

#if ALTUNITYTESTER
            Input.Scroll(CommandParams.speed, CommandParams.duration);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);

#endif
        }
    }
}
