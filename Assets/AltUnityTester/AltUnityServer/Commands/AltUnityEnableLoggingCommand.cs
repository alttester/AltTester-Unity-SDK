namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityEnableLoggingCommand : AltUnityCommand
    {
        bool activateDebug;

        public AltUnityEnableLoggingCommand(params string[] parameters) : base(parameters, 3)
        {
            this.activateDebug = bool.Parse(parameters[2]);
        }

        public override string Execute()
        {
            LogEnabled = activateDebug;
            LogMessage("Logging is set to " + activateDebug);
            return "Ok";
        }
    }
}
