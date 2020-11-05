public class AltUnityGetServerVersion : AltBaseCommand
{
    public AltUnityGetServerVersion(SocketSettings socketSettings) : base(socketSettings)
    {
    }
    public string Execute()
    {
        string serverVersion;
        Socket.Client.Send(toBytes(CreateCommand("getServerVersion")));
        serverVersion = Recvall();
        HandleErrors(serverVersion);
        return serverVersion;
    }
}
