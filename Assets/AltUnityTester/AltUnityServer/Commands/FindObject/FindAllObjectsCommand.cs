namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class FindAllObjectsCommand :Command
    {
        string methodParameter;

        public FindAllObjectsCommand(string methodParameter)
        {
            this.methodParameter = methodParameter;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("all objects requested");
            var parameters = ";" + methodParameter;
            return new FindObjectsByNameCommand(parameters).Execute();
        }
    }
}
