public class AltSocketClientThreadHolder {
    protected readonly System.Threading.Thread thread;
    protected readonly AltClientSocketHandler handler;

    public System.Threading.Thread Thread {
        get {
            return thread;
        }
    }

    public AltClientSocketHandler Handler {
        get {
            return handler;
        }
    }

    public AltSocketClientThreadHolder(System.Threading.Thread thread, AltClientSocketHandler handler) {
        this.thread = thread;
        this.handler = handler;
    }
}

public class AltTcpListener : System.Net.Sockets.TcpListener
{
    public AltTcpListener(System.Net.IPEndPoint localEp) : base(localEp) {
    }

    public AltTcpListener(System.Net.IPAddress localaddr, int port) : base(localaddr, port) {
    }

    public new bool Active {
        get { return base.Active; }
    }
}

public class AltSocketServer {
    protected AltTcpListener Listener;
    protected readonly AltIClientSocketHandlerDelegate ClientSocketHandlerDelegate;
    protected readonly string SeparatorString;
    protected readonly System.Text.Encoding Encoding;
    protected System.Collections.ArrayList ClientHandlerThreads;
    protected readonly int portNumber;
    protected readonly System.Net.IPEndPoint localEndPoint;
    protected readonly int maxClients;
    public int PortNumber {
        get {
            return portNumber;
        }
    }

    public System.Net.IPEndPoint LocalEndPoint {
        get {
            return localEndPoint;
        }
    }

    public int MaxClients {
        get {
            return maxClients;
        }
    }

    public int ClientCount {
        get {
            return ClientHandlerThreads.Count;
        }
    }
    public bool IsServerStopped()
    {
        return ClientHandlerThreads == null || (ClientHandlerThreads.Count!=0 &&((AltSocketClientThreadHolder)ClientHandlerThreads[0]).Handler.ToBeKilled);
    }

    public AltSocketServer(AltIClientSocketHandlerDelegate clientSocketHandlerDelegate,
	                         int portNumber = 13000,
                             int maxClients = 1,
                             string separatorString = "\n",
                             System.Text.Encoding encoding = null) {
        this.portNumber = portNumber;
        ClientSocketHandlerDelegate = clientSocketHandlerDelegate;
        SeparatorString = separatorString;
        Encoding = encoding ?? System.Text.Encoding.UTF32;
        ClientHandlerThreads = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList());
        this.maxClients = maxClients;

        System.Net.IPAddress ipAddress = System.Net.IPAddress.Parse("0.0.0.0");
        localEndPoint = new System.Net.IPEndPoint(ipAddress, this.portNumber);
        Listener = new AltTcpListener(localEndPoint.Address, this.portNumber);


        UnityEngine.Debug.Log("Created TCP listener.");
    }

    public void StartListeningForConnections()
    {
        foreach (AltSocketClientThreadHolder holder in ClientHandlerThreads)
        {
            UnityEngine.Debug.Log("calling stop on thread " + holder.Thread.ManagedThreadId);
            holder.Handler.Cleanup();
            UnityEngine.Debug.Log("Calling thread abort on thread: " + holder.Thread.ManagedThreadId);
        }

        ClientHandlerThreads = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList()); 
        UnityEngine.Debug.Log("Began listening for TCP clients.");
        Listener.Start();
        ListenForConnection();
    }

    protected void ListenForConnection() {
        Listener.BeginAcceptTcpClient(AcceptCallback, Listener);
    }

    // NOT on main thread
    protected void AcceptCallback(System.IAsyncResult ar) {
        int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        UnityEngine.Debug.Log("Accept thread id: " + threadId);
        System.Net.Sockets.TcpListener listener = (System.Net.Sockets.TcpListener)ar.AsyncState;
        System.Net.Sockets.TcpClient client = listener.EndAcceptTcpClient(ar);

        UnityEngine.Debug.Log("thread id " + threadId + " accepted client " + client.Client.RemoteEndPoint);
        UnityEngine.Debug.Log("thread id " + threadId + " beginning read from client " + client.Client.RemoteEndPoint);

        AltClientSocketHandler clientHandler =
            new AltClientSocketHandler(client,
                                        ClientSocketHandlerDelegate,
                                        SeparatorString,
                                        Encoding);

        System.Threading.Thread clientThread = new System.Threading.Thread(clientHandler.Run);
        ClientHandlerThreads.Add(new AltSocketClientThreadHolder(clientThread, clientHandler));
        clientThread.Start();
        UnityEngine.Debug.Log("Client thread started");

        if (ClientCount < maxClients) {
            UnityEngine.Debug.Log("client handler threads less than max clients. Listening again");
            ListenForConnection();
        } else {
            UnityEngine.Debug.Log(System.String.Format("Max number of clients reached ({0}), stopping listening", maxClients));
            StopListeningForConnections();
        }
    }

    public void StopListeningForConnections() {
        Listener.Stop();
        UnityEngine.Debug.Log("Stopped listening for connections");
    }

    public void Cleanup() {
        StopListeningForConnections();
        foreach (AltSocketClientThreadHolder holder in ClientHandlerThreads) {
            UnityEngine.Debug.Log("calling stop on thread " + holder.Thread.ManagedThreadId);
            holder.Handler.Cleanup();
            UnityEngine.Debug.Log("Calling thread abort on thread: " + holder.Thread.ManagedThreadId);
            holder.Handler.ToBeKilled = true;
            holder.Thread.Abort();
        }
        ClientHandlerThreads = null;
	}

    public bool IsStarted() {
        return Listener != null && Listener.Active;
    }
}

