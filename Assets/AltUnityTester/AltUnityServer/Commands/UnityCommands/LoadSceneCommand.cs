namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class LoadSceneCommand:Command
    {
        string scene;
        AltClientSocketHandler handler;

        public LoadSceneCommand(string scene, AltClientSocketHandler handler)
        {
            this.scene = scene;
            this.handler = handler;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("LoadScene " + scene);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            var sceneLoadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
            sceneLoadingOperation.completed += SceneLoaded;

            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
            response = "Ok";
            return response;
        }

        private void SceneLoaded(UnityEngine.AsyncOperation obj)
        {
            AltUnityRunner.logMessage = "Scene Loaded";
            handler.SendResponse("Scene Loaded");
        }
    }
}
