namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllCameras : AltUnityBaseFindObjects
    {
        private readonly AltUnityGetAllCamerasParams cmdParams;
        public AltUnityGetAllCameras(IDriverCommunication commHandler) : base(commHandler)
        {
            cmdParams = new AltUnityGetAllCamerasParams();
        }
        public System.Collections.Generic.List<AltUnityObject> Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveListOfAltUnityObjects(cmdParams);
        }
    }
}