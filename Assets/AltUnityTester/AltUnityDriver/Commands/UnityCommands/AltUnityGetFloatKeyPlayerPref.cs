public class AltUnityGetFloatKeyPlayerPref : AltBaseCommand
{
    string keyName;
    public AltUnityGetFloatKeyPlayerPref(SocketSettings socketSettings, string keyName) : base(socketSettings)
    {
        this.keyName = keyName;
    }
    public float Execute(){
        Socket.Client.Send(toBytes(CreateCommand("getKeyPlayerPref", keyName, PLayerPrefKeyType.Float.ToString())));
        var data = Recvall();
        if (!data.Contains("error:")) return float.Parse(data);
        HandleErrors(data);
        return 0;
    }
}