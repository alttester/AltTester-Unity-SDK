namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityGetServerVersionCommand : AltUnityCommand
    {
        public AltUnityGetServerVersionCommand(params string[] parameters) : base(parameters, 2) { }
        public override string Execute()
        {
            return AltUnityRunner.VERSION;
        }
    }
}
