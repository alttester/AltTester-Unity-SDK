namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllActiveCameras : AltUnityBaseFindObjects
    {
        public AltUnityGetAllActiveCameras(SocketSettings socketSettings) : base(socketSettings) {}
        public System.Collections.Generic.List<AltUnityObject> Execute()
        {
            SendCommand("getAllActiveCameras");
            return ReceiveListOfAltUnityObjects();
        }
    }
}