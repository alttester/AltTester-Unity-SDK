using System.Collections.Generic;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    public class AltUnityGetAllScenesCommand : AltUnityCommand<AltUnityGetAllScenesParams, List<string>>
    {
        public AltUnityGetAllScenesCommand(AltUnityGetAllScenesParams cmdParam) : base(cmdParam) { }
        public override List<string> Execute()
        {
            var sceneNames = new List<string>();
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
            {
                var s = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
                sceneNames.Add(s);
            }
            return sceneNames;
        }
    }
}