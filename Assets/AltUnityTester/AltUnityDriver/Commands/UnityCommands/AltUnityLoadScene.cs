public class AltUnityLoadScene : AltBaseCommand
{
    string sceneName;
    bool loadSingle;
    public AltUnityLoadScene(SocketSettings socketSettings,string sceneName,bool loadSingle) : base(socketSettings)
    {
        this.sceneName=sceneName;
        this.loadSingle = loadSingle;
    }
    public void Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("loadScene", sceneName,loadSingle.ToString())));
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