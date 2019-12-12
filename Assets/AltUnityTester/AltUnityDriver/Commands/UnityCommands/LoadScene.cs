public class LoadScene : AltBaseCommand
{
    string sceneName;
    public LoadScene(SocketSettings socketSettings,string sceneName) : base(socketSettings)
    {
        this.sceneName=sceneName;
    }
    public void Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("loadScene", sceneName)));
        var data = Recvall();
        if (data.Equals("Ok"))
        {
            data = Recvall();
            if(data.Equals("Scene Loaded"))
                return;
        }
        HandleErrors(data);
    }
}