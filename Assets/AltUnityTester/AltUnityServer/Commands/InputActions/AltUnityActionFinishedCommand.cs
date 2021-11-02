using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityActionFinishedCommand : AltUnityCommand<AltUnityActionFinishedParams, string>
    {
        public AltUnityActionFinishedCommand(AltUnityActionFinishedParams cmdParams) : base(cmdParams) { }
        public override string Execute()
        {
#if ALTUNITYTESTER
            return Input.Finished ? "Yes" : "No";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
