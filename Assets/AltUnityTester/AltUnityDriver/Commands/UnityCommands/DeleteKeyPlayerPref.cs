public class DeleteKeyPlayerPrefDriver : AltBaseCommand
{
    string keyName;
    public DeleteKeyPlayerPrefDriver(SocketSettings socketSettings, string keyname) : base(socketSettings)
    {
        this.keyName = keyname;
    }
    public void Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("deleteKeyPlayerPref", keyName)));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }

}