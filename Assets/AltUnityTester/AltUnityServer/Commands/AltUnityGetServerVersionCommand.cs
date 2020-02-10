namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetServerVersionCommand : AltUnityCommand
    {
        public override string Execute()
        {
            return AltUnityRunner.VERSION;
        }
    }
}
