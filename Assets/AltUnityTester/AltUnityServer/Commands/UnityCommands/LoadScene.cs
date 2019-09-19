namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class LoadScene:Command
    {
        string scene;

        public LoadScene(string scene)
        {
            this.scene = scene;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("LoadScene " + scene);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
            response = "Ok";
            return response;
        }
    }
}
