using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    public class AltUnityGetCurrentSceneCommand : AltUnityCommand<AltUnityGetCurrentSceneParams, AltUnityObject>
    {
        public AltUnityGetCurrentSceneCommand(AltUnityGetCurrentSceneParams cmdParams) : base(cmdParams)
        {
        }

        public override AltUnityObject Execute()
        {
            var scene = new AltUnityObject(name: UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                                                             type: "UnityScene");
            return scene;
        }
    }
}