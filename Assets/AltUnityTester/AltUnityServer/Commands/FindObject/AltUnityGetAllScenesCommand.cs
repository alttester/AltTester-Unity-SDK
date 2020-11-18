namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityGetAllScenesCommand : AltUnityCommand
    {
        public AltUnityGetAllScenesCommand(params string[] parameters) : base(parameters, 2) { }
        public override string Execute()
        {
            LogMessage("getAllScenes");
            System.Collections.Generic.List<string> SceneNames = new System.Collections.Generic.List<string>();
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
            {
                var s = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
                SceneNames.Add(s);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(SceneNames);
        }
    }
}