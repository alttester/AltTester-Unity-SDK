namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class FindAllObjects :Command
    {
        string methodParameter;

        public FindAllObjects(string methodParameter)
        {
            this.methodParameter = methodParameter;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("all objects requested");
            var parameters = ";" + methodParameter;
            return new FindObjectsByName(parameters).Execute();
        }
    }
}
