using Altom.AltUnityDriver.Commands;
using System.Collections.Generic;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllLoadedScenesCommand : AltUnityCommand<AltUnityGetAllLoadedScenesParams, List<string>>
    {
        private readonly List<string> sceneNames = new List<string>();

        public AltUnityGetAllLoadedScenesCommand(AltUnityGetAllLoadedScenesParams cmdParams) : base(cmdParams) { }

        public override List<string> Execute()
        {
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                var sceneName = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name;
                sceneNames.Add(sceneName);
            }
            return sceneNames;
        }

    }
}
