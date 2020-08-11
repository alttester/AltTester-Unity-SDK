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
                var sceneName = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
                sceneNames.Add(sceneName);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(sceneNames);
        }

    }
}
