namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class LoadSceneCommand:Command
    {
        string scene;

        public LoadSceneCommand(string scene)
        {
            this.scene = scene;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("LoadScene " + scene);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
            response = "Ok";
            return response;
        }
    }
}
