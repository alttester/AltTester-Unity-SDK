namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityEnableLoggingCommand : AltUnityCommand
    {
        bool activateDebug;

        public AltUnityEnableLoggingCommand(bool activateDebug)
        {
            this.activateDebug = activateDebug;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.logEnabled = activateDebug;
            AltUnityRunner._altUnityRunner.LogMessage("Logging is set to "+activateDebug);
            return "Ok";
        }
    }
}
