namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityUnknowStringCommand :  AltUnityCommand
    {
        public override string Execute()
        {
            return AltUnityRunner._altUnityRunner.errorUnknownError;
        }
    }
}
