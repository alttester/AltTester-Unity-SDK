namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class EnableLoggingCommand : Command
    {
        bool activateDebug;

        public EnableLoggingCommand(bool activateDebug)
        {
            this.activateDebug = activateDebug;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.loggingEnabled = activateDebug;
            AltUnityRunner._altUnityRunner.LogMessage("Logging is set to "+activateDebug);
            return "Ok";
        }
    }
}
