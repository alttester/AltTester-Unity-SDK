namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityFindObjects : AltUnityBaseFindObjects
    {
        AltUnityFindObjectsParams cmdParams;

        public AltUnityFindObjects(IDriverCommunication commHandler, By by, string value, By cameraBy, string cameraPath, bool enabled) : base(commHandler)
        {
            cmdParams = new AltUnityFindObjectsParams(SetPath(by, value), cameraBy, SetPath(cameraBy, cameraPath), enabled);
        }
        public System.Collections.Generic.List<AltUnityObject> Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveListOfAltUnityObjects(cmdParams);
        }
    }
}