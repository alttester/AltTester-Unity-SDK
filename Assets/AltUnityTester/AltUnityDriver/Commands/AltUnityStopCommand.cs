public class AltUnityStopCommand : AltBaseCommand
{
    public AltUnityStopCommand(SocketSettings socketSettings) : base(socketSettings)
    {
    }
    public void Execute()
    {
        try
        {
            Socket.Client.Send(toBytes(CreateCommand("closeConnection")));
            System.Threading.Thread.Sleep(1000);
            Socket.Close();
        }
        catch (System.Exception exception)
        {
            System.Console.WriteLine(exception.Message);
        }
    }
}