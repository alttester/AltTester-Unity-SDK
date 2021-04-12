using Assets.AltUnityTester.AltUnityServer.AltSocket;


namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetScreenshotPNGCommand : AltUnityBaseScreenshotCommand
    {
        public AltUnityGetScreenshotPNGCommand(AltClientSocketHandler handler, params string[] parameters) : base(handler, parameters, 2)
        {
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.StartCoroutine(SendPNGScreenshotCoroutine());
            return "Ok";
        }
    }
}
