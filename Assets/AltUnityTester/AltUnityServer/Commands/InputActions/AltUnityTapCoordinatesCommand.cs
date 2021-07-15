using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Assets.AltUnityTester.AltUnityServer.Communication;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityTapCoordinatesCommand : AltUnityCommand<AltUnityTapCoordinatesParams, string>
    {
        private readonly ICommandHandler handler;
        public AltUnityTapCoordinatesCommand(ICommandHandler handler, AltUnityTapCoordinatesParams cmdParams) : base(cmdParams)
        {
            this.handler = handler;
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            Input.TapCoordinates(CommandParams.coordinates.ToUnity(), CommandParams.count, CommandParams.interval, onFinish);
            return "Ok";
#else
            return AltUnityErrors.errorInputModule;
#endif
        }

        private void onFinish()
        {
            if (CommandParams.wait)
                handler.Send(ExecuteAndSerialize(() => "Finished"));
        }
    }
}