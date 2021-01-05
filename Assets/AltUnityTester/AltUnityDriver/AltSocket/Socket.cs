namespace Altom.AltUnityDriver.AltSocket
{
    public class Socket : ISocket
    {
        private readonly System.Net.Sockets.Socket _socket;
        public Socket(System.Net.Sockets.Socket socket)
        {
            this._socket = socket;
        }

        public void Send(byte[] buffer)
        {
            this._socket.Send(buffer);
        }
        public int Receive(byte[] buffer)
        {
            return this._socket.Receive(buffer);
        }

        public void Close()
        {
            this._socket.Close();
        }

    }
}
