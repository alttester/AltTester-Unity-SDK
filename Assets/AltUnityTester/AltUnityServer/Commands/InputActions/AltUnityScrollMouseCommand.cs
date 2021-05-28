using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
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
            return null;
#endif
        }
    }
}
