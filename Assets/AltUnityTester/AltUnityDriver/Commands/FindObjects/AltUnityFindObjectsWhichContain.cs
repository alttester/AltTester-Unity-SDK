namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityFindObjectsWhichContain : AltUnityBaseFindObjects
    {
        AltUnityFindObjectsParams cmdParams;

        public AltUnityFindObjectsWhichContain(IDriverCommunication commHandler, By by, string value, By cameraBy, string cameraPath, bool enabled) : base(commHandler)
        {
            cmdParams = new AltUnityFindObjectsParams(SetPathContains(by, value), cameraBy, SetPath(cameraBy, cameraPath), enabled);

        }
        public System.Collections.Generic.List<AltUnityObject> Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveListOfAltUnityObjects(cmdParams);
        }
    }
}
