public class AltUnitySetKeyPLayerPref : AltBaseCommand
{
    string keyName;
    int intValue;
    float floatValue;
    string stringValue;
    int option = 0;
    public AltUnitySetKeyPLayerPref(SocketSettings socketSettings, string keyName, int intValue) : base(socketSettings)
    {
        this.keyName = keyName;
        this.intValue = intValue;
        option = 1;
    }
    public AltUnitySetKeyPLayerPref(SocketSettings socketSettings, string keyName, float floatValue) : base(socketSettings)
    {
        this.keyName = keyName;
        this.floatValue = floatValue;
        option = 2;
    }
    public AltUnitySetKeyPLayerPref(SocketSettings socketSettings, string keyName, string stringValue) : base(socketSettings)
    {
        this.keyName = keyName;
        this.stringValue = stringValue;
        option = 3;
    }
    public void Execute()
    {
        switch (option)
        {
            case 1:
                SetIntKey();
                break;
            case 2:
                SetFloatKey();
                break;
            case 3:
                SetStringKey();
                break;
        }
    }
    private void SetStringKey()
    {
        Socket.Client.Send(toBytes(CreateCommand("setKeyPlayerPref", keyName, stringValue.ToString(), PLayerPrefKeyType.String.ToString())));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }
    private void SetIntKey()
    {
        Socket.Client.Send(toBytes(CreateCommand("setKeyPlayerPref", keyName, intValue.ToString(), PLayerPrefKeyType.Int.ToString())));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;

        HandleErrors(data);
    }
    private void SetFloatKey()
    {
        Socket.Client.Send(toBytes(CreateCommand("setKeyPlayerPref", keyName, floatValue.ToString(), PLayerPrefKeyType.Float.ToString())));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }

}