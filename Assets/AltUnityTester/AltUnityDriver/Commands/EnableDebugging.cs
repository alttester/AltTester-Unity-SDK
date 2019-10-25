public class EnableDebugging : AltBaseCommand
{
    public EnableDebugging(SocketSettings socketSettings) : base(socketSettings)
    {
    }
    public void Execute(){
        Socket.Client.Send(toBytes(CreateCommand("enableDebug", SocketSettings.DebugFlag.ToString())));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }
}
