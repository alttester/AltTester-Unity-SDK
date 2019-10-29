namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class EnableDebuggingCommand : Command
    {
        bool activateDebug;

        public EnableDebuggingCommand(bool activateDebug)
        {
            this.activateDebug = activateDebug;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.debugOn = activateDebug;
            AltUnityRunner._altUnityRunner.LogMessage("Debugging is set to "+activateDebug);
            return "Ok";
        }
    }
}
