using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityFindObject : AltUnityBaseFindObjects
    {
        AltUnityFindObjectParams cmdParams;

        public AltUnityFindObject(IDriverCommunication commHandler, By by, string value, By cameraBy, string cameraPath, bool enabled) : base(commHandler)
        {
            cameraPath = SetPath(cameraBy, cameraPath);
            string path = SetPath(by, value);
            cmdParams = new AltUnityFindObjectParams(path, cameraBy, cameraPath, enabled);
        }

        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);
            var altUnityObject = CommHandler.Recvall<AltUnityObject>(cmdParams).data;
            altUnityObject.CommHandler = CommHandler;
            return altUnityObject;
        }
    }
}