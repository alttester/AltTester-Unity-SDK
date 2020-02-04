namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityLoadSceneCommand :AltUnityCommand
    {
        string scene;
        AltClientSocketHandler handler;

        public AltUnityLoadSceneCommand (string scene, AltClientSocketHandler handler)
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
