
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;


namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityClickOnScreenCustom : AltUnityCommand<AltUnityTapCustomParams, string>
    {
        public AltUnityClickOnScreenCustom(AltUnityTapCustomParams cmdParams) : base(cmdParams)
        {

        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            var position = new UnityEngine.Vector2(CommandParams.x, CommandParams.y);

            Input.TapAtCoordinates(position, CommandParams.count, CommandParams.interval);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);

#endif
        }
    }
}