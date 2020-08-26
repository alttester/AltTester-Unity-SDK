namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityLoadSceneCommand : AltUnityCommand
    {
        string scene;
        UnityEngine.SceneManagement.LoadSceneMode mode;
        AltClientSocketHandler handler;

        public AltUnityLoadSceneCommand(string scene, bool loadSingle, AltClientSocketHandler handler)
        {
            mode = loadSingle ? UnityEngine.SceneManagement.LoadSceneMode.Single : UnityEngine.SceneManagement.LoadSceneMode.Additive;
            this.scene = scene;
            this.handler = handler;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("LoadScene " + scene);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            
            var sceneLoadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene,mode);
            sceneLoadingOperation.completed += SceneLoaded; 

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
