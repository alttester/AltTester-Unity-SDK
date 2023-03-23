using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;

namespace AltTester.AltTesterUnitySdk.Commands
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