/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;

namespace AltTester.AltTesterUnitySDK.Commands
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
            AltInput.Instance.StartCoroutine(SceneLoadedCoroutine());
        }

        private System.Collections.IEnumerator SceneLoadedCoroutine()
        {
            yield return AltInput.DummyClickAt(new UnityEngine.Vector2(-100, -100));
            handler.Send(ExecuteAndSerialize(() => "Scene Loaded"));
        }
    }
}
