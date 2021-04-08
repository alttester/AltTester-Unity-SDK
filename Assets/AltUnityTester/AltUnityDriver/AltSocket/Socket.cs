namespace Altom.AltUnityDriver.AltSocket
{
    public class Socket : ISocket
    {
        private readonly System.Net.Sockets.Socket socket;
        public Socket(System.Net.Sockets.Socket socket)
        {
            this.socket = socket;
        }

        public void Send(byte[] buffer)
        {
            this.socket.Send(buffer);
        }
        public int Receive(byte[] buffer)
        {
            return this.socket.Receive(buffer);
        }

        public void Close()
        {
            this.socket.Close();
        }

    }
}
