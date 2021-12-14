using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityLoadScene : AltBaseCommand
    {
        AltUnityLoadSceneParams cmdParams;
        public AltUnityLoadScene(IDriverCommunication commHandler, string sceneName, bool loadSingle) : base(commHandler)
        {
            cmdParams = new AltUnityLoadSceneParams(sceneName, loadSingle);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);

            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);

            data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Scene Loaded", data);
        }
    }
}