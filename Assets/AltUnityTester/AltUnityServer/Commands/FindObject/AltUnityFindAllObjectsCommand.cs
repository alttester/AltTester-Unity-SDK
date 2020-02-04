namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindAllObjectsCommand :AltUnityCommand
    {
        string methodParameter;

        public AltUnityFindAllObjectsCommand(string methodParameter)
        {
            this.methodParameter = methodParameter;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("all objects requested");
            var parameters = ";" + methodParameter;
            return new AltUnityFindObjectsByNameCommand(parameters).Execute();
        }
    }
}
