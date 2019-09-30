namespace altunitytester.Assets.AltUnityTester.AltUnityServer
{
    public class GetCurrentSceneCommand : Command
    {
        public override string Execute()
        {
            UnityEngine.Debug.Log("get current scene");
            AltUnityObject scene = new AltUnityObject(name: UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                                                             type: "UnityScene");
            return UnityEngine.JsonUtility.ToJson(scene);

        }
    }
}