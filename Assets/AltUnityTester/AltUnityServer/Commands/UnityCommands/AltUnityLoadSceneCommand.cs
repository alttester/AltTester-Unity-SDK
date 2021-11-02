using System;
using Altom.AltUnityTester.Communication;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityDriver;

namespace Altom.AltUnityTester.Commands
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

            try
            {
                var sceneLoadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(CommandParams.sceneName, mode);
                sceneLoadingOperation.completed += sceneLoaded;
            }
            catch (System.NullReferenceException)
            {
                throw new SceneNotFoundException(String.Format("Could not found a scene with the name: {0}.", CommandParams.sceneName));
            }

            return "Ok";
        }

        private void sceneLoaded(UnityEngine.AsyncOperation obj)
        {
            handler.Send(ExecuteAndSerialize(() => "Scene Loaded"));
        }
    }
}
