using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityGetCurrentSceneCommand : AltUnityCommand
    {
        public AltUnityGetCurrentSceneCommand(params string[] parameters) : base(parameters, 2)
        {
        }

        public override string Execute()
        {
            var scene = new AltUnityObject(name: UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                                                             type: "UnityScene");
            return UnityEngine.JsonUtility.ToJson(scene);

        }
    }
}