using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
{
    public class AltUnityClickCoordinatesCommand : AltUnityCommandWithWait<AltUnityClickCoordinatesParams, string>
    {
        public AltUnityClickCoordinatesCommand(ICommandHandler handler, AltUnityClickCoordinatesParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }
        public override string Execute()
        {
#if ALTUNITYTESTER
            Input.ClickCoordinates(CommandParams.coordinates.ToUnity(), CommandParams.count, CommandParams.interval, onFinish);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
