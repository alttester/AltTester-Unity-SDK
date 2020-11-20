namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityInvalidParametersOnDriverCommandCommand : AltUnityCommand
    {
        public AltUnityInvalidParametersOnDriverCommandCommand(params string[] parameters) : base(parameters, parameters.Length) { }
        public override string Execute()
        {
            return AltUnityErrors.errorInvalidParametersOnDriverCommand;
        }

        public new void LogMessage(string message)
        {
            base.LogMessage(message);
        }
    }
}
