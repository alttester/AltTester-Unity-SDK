using System;
using Altom.AltUnityDriver;
using Assets.AltUnityTester.AltUnityServer.AltSocket;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityUnloadSceneCommand : AltUnityCommand
    {
        readonly string scene;
        readonly AltClientSocketHandler handler;

        public AltUnityUnloadSceneCommand(AltClientSocketHandler handler, params string[] parameters) : base(parameters, 3)
        {
            this.handler = handler;
            scene = parameters[2];
        }

        public override string Execute()
        {
            string response = AltUnityErrors.errorNotFoundMessage;
            try
            {
                var sceneLoadingOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
                if (sceneLoadingOperation == null)
                {
                    throw new CouldNotPerformOperationException("Cannot unload scene: " + scene);
                }
                sceneLoadingOperation.completed += sceneUnloaded;
            }
            catch (ArgumentException)
            {
                throw new CouldNotPerformOperationException("Cannot unload scene: " + scene);
            }

            response = "Ok";
            return response;
        }

        private void sceneUnloaded(UnityEngine.AsyncOperation obj)
        {
            handler.SendResponse(MessageId, CommandName, "Scene Unloaded", string.Empty);
        }
    }
}
