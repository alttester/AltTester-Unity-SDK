using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltGetCurrentSceneCommand : AltCommand<AltGetCurrentSceneParams, AltObject>
    {
        public AltGetCurrentSceneCommand(AltGetCurrentSceneParams cmdParams) : base(cmdParams)
        {
        }

        public override AltObject Execute()
        {
            var scene = new AltObject(name: UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                                                             type: "UnityScene");
            return scene;
        }
    }
}