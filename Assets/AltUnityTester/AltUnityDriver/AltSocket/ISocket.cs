namespace Altom.AltUnityDriver.AltSocket
{
    public interface ISocket
    {
        void Send(byte[] buffer);
        int Receive(byte[] buffer);

        void Close();
    }
}
