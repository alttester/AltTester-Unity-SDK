namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class GetServerVersionCommand : Command
    {
        public override string Execute()
        {
            return AltUnityRunner.VERSION;
        }
    }
}
