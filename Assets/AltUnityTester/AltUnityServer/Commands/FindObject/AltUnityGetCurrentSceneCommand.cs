namespace altunitytester.Assets.AltUnityTester.AltUnityServer
{
    public class AltUnityGetCurrentSceneCommand : AltUnityCommand
    {
        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("get current scene");
            AltUnityObject scene = new AltUnityObject(name: UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                                                             type: "UnityScene");
            return UnityEngine.JsonUtility.ToJson(scene);

        }
    }
}