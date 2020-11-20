using Assets.AltUnityTester.AltUnityServer.AltSocket;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityLoadSceneCommand : AltUnityCommand
    {
        string scene;
        UnityEngine.SceneManagement.LoadSceneMode mode;
        AltClientSocketHandler handler;

        public AltUnityLoadSceneCommand(AltClientSocketHandler handler, params string[] parameters) : base(parameters, 4)
        {
            this.handler = handler;
            this.scene = parameters[2];
            var loadSingle = bool.Parse(parameters[3]);
            mode = loadSingle ? UnityEngine.SceneManagement.LoadSceneMode.Single : UnityEngine.SceneManagement.LoadSceneMode.Additive;
        }

        public override string Execute()
        {
            LogMessage("LoadScene " + scene);
            string response = AltUnityErrors.errorNotFoundMessage;

            var sceneLoadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene, mode);
            sceneLoadingOperation.completed += SceneLoaded;

            response = "Ok";
            return response;
        }

        private void SceneLoaded(UnityEngine.AsyncOperation obj)
        {
            LogMessage("Scene Loaded");
            handler.SendResponse(this, "Scene Loaded");
        }
    }
}
