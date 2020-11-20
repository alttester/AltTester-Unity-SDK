namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityUnknowStringCommand : AltUnityCommand
    {
        public AltUnityUnknowStringCommand(params string[] parameters) : base(parameters, 2) { }
        public override string Execute()
        {
            return AltUnityErrors.errorUnknownError;
        }
    }
}
