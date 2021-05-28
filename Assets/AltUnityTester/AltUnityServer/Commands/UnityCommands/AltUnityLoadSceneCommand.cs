using Assets.AltUnityTester.AltUnityServer.Communication;
using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityLoadSceneCommand : AltUnityCommand<AltUnityLoadSceneParams, string>
    {
        readonly ICommandHandler handler;

        public AltUnityLoadSceneCommand(ICommandHandler handler, AltUnityLoadSceneParams cmdParams) : base(cmdParams)
        {
            this.handler = handler;
        }
        public override string Execute()
        {
            var mode = CommandParams.loadSingle ? UnityEngine.SceneManagement.LoadSceneMode.Single : UnityEngine.SceneManagement.LoadSceneMode.Additive;
            var sceneLoadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(CommandParams.sceneName, mode);
            sceneLoadingOperation.completed += sceneLoaded;

            return "Ok";
        }

        private void sceneLoaded(UnityEngine.AsyncOperation obj)
        {
            handler.Send(ExecuteAndSerialize(() => "Scene Loaded"));
        }
    }
}
