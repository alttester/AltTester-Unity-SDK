public class AltUnityGetIntKeyPLayerPref : AltBaseCommand
{
    string keyName;
    public AltUnityGetIntKeyPLayerPref(SocketSettings socketSettings, string keyName) : base(socketSettings)
    {
        this.keyName = keyName;
    }
    public int Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("getKeyPlayerPref", keyName, PLayerPrefKeyType.Int.ToString())));
        var data = Recvall();
        if (!data.Contains("error:")) return System.Int32.Parse(data);
        HandleErrors(data);
        return 0;
    }
}