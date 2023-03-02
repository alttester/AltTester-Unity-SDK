using System;
using AltTester.AltDriver;
using AltTester.AltDriver.Commands;
using AltTester.Communication;

namespace AltTester.Commands
{
    public class AltLoadSceneCommand : AltCommand<AltLoadSceneParams, string>
    {
        readonly ICommandHandler handler;

        public AltLoadSceneCommand(ICommandHandler handler, AltLoadSceneParams cmdParams) : base(cmdParams)
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
