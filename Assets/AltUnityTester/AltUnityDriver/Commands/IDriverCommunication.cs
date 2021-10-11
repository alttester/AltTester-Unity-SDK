namespace Altom.AltUnityDriver.Commands
{
    public interface IDriverCommunication
    {
        void Send(CommandParams param);
        CommandResponse<T> Recvall<T>(CommandParams param);
        void Connect();
        void Close();
    }
}