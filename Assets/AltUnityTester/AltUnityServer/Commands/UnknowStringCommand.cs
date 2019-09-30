namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class UnknowStringCommand :  Command
    {
        public override string Execute()
        {
            return AltUnityRunner._altUnityRunner.errorUnknownError;
        }
    }
}
