using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityLoadSceneCommand : AltUnityCommand
    {
        readonly string scene;
        readonly UnityEngine.SceneManagement.LoadSceneMode mode;
        readonly AltClientSocketHandler handler;

        public AltUnityLoadSceneCommand(AltClientSocketHandler handler, params string[] parameters) : base(parameters, 4)
        {
            this.handler = handler;
            this.scene = parameters[2];
            var loadSingle = JsonConvert.DeserializeObject<bool>(parameters[3].ToLower());
            mode = loadSingle ? UnityEngine.SceneManagement.LoadSceneMode.Single : UnityEngine.SceneManagement.LoadSceneMode.Additive;
        }

        public override string Execute()
        {
            var sceneLoadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene, mode);
            sceneLoadingOperation.completed += sceneLoaded;

            return "Ok";
        }

        private void sceneLoaded(UnityEngine.AsyncOperation obj)
        {
            handler.SendResponse(MessageId, CommandName, "Scene Loaded", string.Empty);
        }
    }
}
