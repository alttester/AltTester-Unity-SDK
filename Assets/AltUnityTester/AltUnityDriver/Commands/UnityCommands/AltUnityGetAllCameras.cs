public class AltUnityGetAllCameras : AltUnityBaseFindObjects
{
    public AltUnityGetAllCameras(SocketSettings socketSettings) : base(socketSettings)
    {
    }
    public System.Collections.Generic.List<AltUnityObject> Execute(){
        Socket.Client.Send(toBytes(CreateCommand("getAllCameras")));
        return ReceiveListOfAltUnityObjects();
       
    }
}