namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class UnknowString :  Command
    {
        public override string Execute()
        {
            return AltUnityRunner._altUnityRunner.errorUnknownError;
        }
    }
}
