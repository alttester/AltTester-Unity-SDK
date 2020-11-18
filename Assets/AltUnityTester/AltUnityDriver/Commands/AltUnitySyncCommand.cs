using Assets.AltUnityTester.AltUnityDriver;

public class AltUnitySyncCommand : AltBaseCommand
{
    public AltUnitySyncCommand(SocketSettings socketSettings) : base(socketSettings)
    {
    }
    public void Execute()
    {
        SendCommand("getServerVersion");
        int retry = 0;
        while (true)
        {
            try
            {
                Recvall();
                break;
            }
            catch (AltUnityRecvallMessageIdException)
            {
                if (retry == 5)
                {
                    throw;
                }
                retry++;
            }
        }
    }
}
