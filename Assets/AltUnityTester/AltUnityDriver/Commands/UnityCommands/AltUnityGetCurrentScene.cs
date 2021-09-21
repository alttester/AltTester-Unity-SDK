using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetCurrentScene : AltUnityBaseFindObjects
    {
        private readonly AltUnityGetCurrentSceneParams cmdParams;
        public AltUnityGetCurrentScene(IDriverCommunication commHandler) : base(commHandler)
        {
            cmdParams = new AltUnityGetCurrentSceneParams();
        }
        public string Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltUnityObject(cmdParams).name;
        }
    }
}