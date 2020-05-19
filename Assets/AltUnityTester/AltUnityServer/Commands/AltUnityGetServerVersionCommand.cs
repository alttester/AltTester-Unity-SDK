namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetServerVersionCommand : AltUnityCommand
    {
        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("Server version is: " + AltUnityRunner.VERSION);
            return AltUnityRunner.VERSION;
        }
    }
}
