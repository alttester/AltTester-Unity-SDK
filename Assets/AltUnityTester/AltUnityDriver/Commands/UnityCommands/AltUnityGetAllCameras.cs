namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllCameras : AltUnityBaseFindObjects
    {
        public AltUnityGetAllCameras(SocketSettings socketSettings) : base(socketSettings) {}
        public System.Collections.Generic.List<AltUnityObject> Execute()
        {
            SendCommand("getAllCameras");
            return ReceiveListOfAltUnityObjects();
        }
    }
}