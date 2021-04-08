using System.Threading;
using Altom.AltUnityDriver.AltSocket;
using Altom.Server.Logging;
using NLog;

namespace Assets.AltUnityTester.AltUnityServer.AltSocket
{
    public class AltSocketClientThreadHolder
    {

        public Thread Thread
        {
            get;
            protected set;
        }

        public AltClientSocketHandler Handler
        {
            get;
            protected set;
        }

        public AltSocketClientThreadHolder(Thread thread, AltClientSocketHandler handler)
        {
            this.Thread = thread;
            this.Handler = handler;
        }
    }

    public class AltTcpListener : System.Net.Sockets.TcpListener
    {
        public AltTcpListener(System.Net.IPEndPoint localEp) : base(localEp)
        {
        }

        public AltTcpListener(System.Net.IPAddress localaddr, int port) : base(localaddr, port)
        {
        }

        public new bool Active
        {
            get { return base.Active; }
        }
    }

    public class AltSocketServer
    {
        private static readonly Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        protected AltTcpListener Listener;
        protected readonly AltIClientSocketHandlerDelegate ClientSocketHandlerDelegate;
        protected readonly string MessageEndingString;
        protected readonly System.Text.Encoding Encoding;
        protected System.Collections.ArrayList ClientHandlerThreads;
        public int PortNumber
        {
            get;
            protected set;

        }

        public System.Net.IPEndPoint LocalEndPoint
        {
            get;
            protected set;

        }

        public int MaxClients
        {
            get;
            protected set;
        }

        public int ClientCount
        {
            get
            {
                return ClientHandlerThreads.Count;
            }
        }
        public bool IsServerStopped()
        {
            return ClientHandlerThreads == null || (ClientHandlerThreads.Count != 0 && ((AltSocketClientThreadHolder)ClientHandlerThreads[0]).Handler.ToBeKilled);
        }

        public AltSocketServer(AltIClientSocketHandlerDelegate clientSocketHandlerDelegate,
                                 int portNumber = 13000,
                                 int maxClients = 1,
                                 string messageEndingString = "&",
                                 System.Text.Encoding encoding = null)
        {
            this.PortNumber = portNumber;
            ClientSocketHandlerDelegate = clientSocketHandlerDelegate;
            MessageEndingString = messageEndingString;
            Encoding = encoding != null ? encoding : System.Text.Encoding.UTF8;
            ClientHandlerThreads = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList());
            this.MaxClients = maxClients;

            System.Net.IPAddress ipAddress = System.Net.IPAddress.Parse("0.0.0.0");
            LocalEndPoint = new System.Net.IPEndPoint(ipAddress, this.PortNumber);
            Listener = new AltTcpListener(LocalEndPoint.Address, this.PortNumber);


            logger.Debug("Created TCP listener.");
        }

        public void StartListeningForConnections()
        {
            foreach (AltSocketClientThreadHolder holder in ClientHandlerThreads)
            {
                logger.Debug("calling stop on thread " + holder.Thread.ManagedThreadId);
                holder.Handler.Cleanup();
            }

            ClientHandlerThreads = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList());
            logger.Debug("Began listening for TCP clients.");
            Listener.Start();
            ListenForConnection();
        }

        protected void ListenForConnection()
        {
            Listener.BeginAcceptTcpClient(AcceptCallback, Listener);
        }

        // NOT on main thread
        protected void AcceptCallback(System.IAsyncResult ar)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            logger.Debug("Accept thread id: " + threadId);
            System.Net.Sockets.TcpListener listener = (System.Net.Sockets.TcpListener)ar.AsyncState;
            System.Net.Sockets.TcpClient client = listener.EndAcceptTcpClient(ar);


            logger.Debug("thread id " + threadId + " accepted client " + client.Client.RemoteEndPoint);

            ISocket altClient = new Socket(client.Client);

            var clientHandler = new AltClientSocketHandler(altClient,
                                            ClientSocketHandlerDelegate,
                                            MessageEndingString,
                                            Encoding);

            var clientThread = new Thread(clientHandler.Run);
            ClientHandlerThreads.Add(new AltSocketClientThreadHolder(clientThread, clientHandler));
            clientThread.Start();
            logger.Debug("Client thread started");

            if (ClientCount < MaxClients)
            {
                logger.Debug("client handler threads less than max clients. Listening again");
                ListenForConnection();
            }
            else
            {
                logger.Debug(string.Format("Max number of clients reached ({0}), stopping listening", MaxClients));
                StopListeningForConnections();
            }
        }

        public void StopListeningForConnections()
        {
            Listener.Stop();
            logger.Debug("Stopped listening for connections");
        }

        public void Cleanup()
        {
            StopListeningForConnections();
            foreach (AltSocketClientThreadHolder holder in ClientHandlerThreads)
            {
                logger.Debug("calling stop on thread " + holder.Thread.ManagedThreadId);
                holder.Handler.Cleanup();
                logger.Debug("Calling thread abort on thread: " + holder.Thread.ManagedThreadId);
                holder.Handler.ToBeKilled = true;
                holder.Thread.Abort();
            }
            ClientHandlerThreads = null;
        }

        public bool IsStarted()
        {
            return Listener != null && Listener.Active;
        }
    }
}