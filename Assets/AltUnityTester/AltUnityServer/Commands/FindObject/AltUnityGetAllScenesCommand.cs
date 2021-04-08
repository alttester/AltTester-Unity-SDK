using System.Collections.Generic;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityGetAllScenesCommand : AltUnityCommand
    {
        public AltUnityGetAllScenesCommand(params string[] parameters) : base(parameters, 2) { }
        public override string Execute()
        {
            var sceneNames = new List<string>();
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
            {
                var s = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
                sceneNames.Add(s);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(sceneNames);
        }
    }
}