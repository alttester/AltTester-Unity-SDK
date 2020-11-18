namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityCouldNotParseJsonStringCommand : AltUnityCommand
    {
        public AltUnityCouldNotParseJsonStringCommand(params string[] parameters) : base(parameters, parameters.Length) { }
        public override string Execute()
        {
            return AltUnityErrors.errorCouldNotParseJsonString;
        }

        public new void LogMessage(string message)
        {
            base.LogMessage(message);
        }
    }
}
