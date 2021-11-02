using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
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
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }

        private void onFinish(Exception err)
        {
            if (CommandParams.wait)
                if (err != null)
                {
                    handler.Send(ExecuteAndSerialize<string>(() => throw new AltUnityInnerException(err)));
                }
                else
                {
                    handler.Send(ExecuteAndSerialize(() => "Finished"));
                }
        }
    }
}