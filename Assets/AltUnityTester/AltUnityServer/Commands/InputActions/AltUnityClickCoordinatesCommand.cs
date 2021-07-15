using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Assets.AltUnityTester.AltUnityServer.Communication;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityClickCoordinatesCommand : AltUnityCommand<AltUnityClickCoordinatesParams, string>
    {
        private readonly ICommandHandler handler;

        public AltUnityClickCoordinatesCommand(ICommandHandler handler, AltUnityClickCoordinatesParams cmdParams) : base(cmdParams)
        {
            this.handler = handler;
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
        private void onFinish()
        {
            if (CommandParams.wait)
                handler.Send(ExecuteAndSerialize(() => "Finished"));
        }
    }
}
