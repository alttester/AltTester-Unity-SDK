namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetServerVersionCommand : AltUnityCommand
    {
        public AltUnityGetServerVersionCommand(params string[] parameters) : base(parameters, 2) { }
        public override string Execute()
        {
            LogMessage("Server version is: " + AltUnityRunner.VERSION);
            return AltUnityRunner.VERSION;
        }
    }
}
