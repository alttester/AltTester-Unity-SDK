public class AltUnityGetStringKeyPlayerPref : AltBaseCommand
{
    string keyName;
    public AltUnityGetStringKeyPlayerPref(SocketSettings socketSettings, string keyName) : base(socketSettings)
    {
        this.keyName = keyName;
    }
    public string Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("getKeyPlayerPref", keyName, PLayerPrefKeyType.String.ToString())));
        var data = Recvall();
        if (!data.Contains("error:")) return data;
        HandleErrors(data);
        return null;
    }
}