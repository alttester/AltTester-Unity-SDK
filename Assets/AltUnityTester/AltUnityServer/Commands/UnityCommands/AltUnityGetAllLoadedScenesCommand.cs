using System.Collections.Generic;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllLoadedScenesCommand : AltUnityCommand
    {
        private readonly List<string> sceneNames = new List<string>();

        public AltUnityGetAllLoadedScenesCommand(params string[] parameters) : base(parameters, 2) { }

        public override string Execute()
        {
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                var sceneName = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name;
                sceneNames.Add(sceneName);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(sceneNames);
        }

    }
}
