namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityFindObject : AltUnityBaseFindObjects
    {
        AltUnityFindObjectParams cmdParams;

        public AltUnityFindObject(IDriverCommunication commHandler, By by, string value, By cameraBy, string cameraValue, bool enabled) : base(commHandler)
        {
            cameraValue = SetPath(cameraBy, cameraValue);
            string path = SetPath(by, value);
            cmdParams = new AltUnityFindObjectParams(path, cameraBy, cameraValue, enabled);
        }

        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);
            var altUnityObject = CommHandler.Recvall<AltUnityObject>(cmdParams);
            altUnityObject.CommHandler = CommHandler;
            return altUnityObject;
        }
    }
}