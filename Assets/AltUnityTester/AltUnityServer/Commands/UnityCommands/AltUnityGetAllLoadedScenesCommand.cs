namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllLoadedScenesCommand: AltUnityCommand
    {
        private System.Collections.Generic.List<string> sceneNames = new System.Collections.Generic.List<string>();

        public AltUnityGetAllLoadedScenesCommand()
        {
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("getAllLoadedScenes");
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                var sceneName = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name;
                sceneNames.Add(sceneName);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(sceneNames);
        }

    }
}
