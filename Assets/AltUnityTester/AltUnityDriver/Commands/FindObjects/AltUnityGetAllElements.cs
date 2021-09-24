namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllElements : AltUnityBaseFindObjects
    {
        AltUnityFindObjectsParams cmdParams;

        public AltUnityGetAllElements(IDriverCommunication commHandler, By cameraBy, string cameraValue, bool enabled) : base(commHandler)
        {
            cmdParams = new AltUnityFindObjectsParams("//*", cameraBy, SetPath(cameraBy, cameraValue), enabled);
        }
        public System.Collections.Generic.List<AltUnityObject> Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveListOfAltUnityObjects(cmdParams);
        }
    }
}