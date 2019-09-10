public class LoadSceneDriver : AltBaseCommand
{
    string sceneName;
    public LoadSceneDriver(SocketSettings socketSettings,string sceneName) : base(socketSettings)
    {
        this.sceneName=sceneName;
    }
    public void Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("loadScene", sceneName)));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }
}