using Assets.AltUnityTester.AltUnityServer.AltSocket;


namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetScreenshotPNGCommand : AltUnityCommand
    {
        AltClientSocketHandler handler;

        public AltUnityGetScreenshotPNGCommand(AltClientSocketHandler handler, params string[] parameters) : base(parameters, 2)
        {
            this.handler = handler;
        }

        public override string Execute()
        {
            LogMessage("getScreenshotPNG");
            AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.TakeScreenshot(this, handler));
            return "Ok";
        }
    }
}
