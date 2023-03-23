using System;
using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;
using AltTester.AltTesterUnitySdk.Communication;

namespace AltTester.AltTesterUnitySdk.Commands
{
    class AltUnloadSceneCommand : AltCommand<AltUnloadSceneParams, string>
    {
        readonly ICommandHandler handler;

        public AltUnloadSceneCommand(ICommandHandler handler, AltUnloadSceneParams cmdParams) : base(cmdParams)
        {
            this.handler = handler;
        }

        public override string Execute()
        {
            try
            {
                var sceneLoadingOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(CommandParams.sceneName);
                if (sceneLoadingOperation == null)
                {
                    throw new CouldNotPerformOperationException("Cannot unload scene: " + CommandParams.sceneName);
                }
                sceneLoadingOperation.completed += sceneUnloaded;
            }
            catch (ArgumentException)
            {
                throw new CouldNotPerformOperationException("Cannot unload scene: " + CommandParams.sceneName);
            }

            return "Ok";
        }

        private void sceneUnloaded(UnityEngine.AsyncOperation obj)
        {
            handler.Send(ExecuteAndSerialize(() => "Scene Unloaded"));
        }
    }
}
