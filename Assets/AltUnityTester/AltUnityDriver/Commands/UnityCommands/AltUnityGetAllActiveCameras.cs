namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllActiveCameras : AltUnityBaseFindObjects
    {
        private readonly AltUnityGetAllActiveCamerasParams cmdParams;
        public AltUnityGetAllActiveCameras(IDriverCommunication commHandler) : base(commHandler)
        {

            cmdParams = new AltUnityGetAllActiveCamerasParams();
        }
        public System.Collections.Generic.List<AltUnityObject> Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveListOfAltUnityObjects(cmdParams);
        }
    }
}