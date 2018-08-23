using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class AltSocketClientThreadHolder {
    protected readonly Thread thread;
    protected readonly AltClientSocketHandler handler;

    public Thread Thread {
        get {
            return thread;
        }
    }

    public AltClientSocketHandler Handler {
        get {
            return handler;
        }
    }

    public AltSocketClientThreadHolder(Thread thread, AltClientSocketHandler handler) {
        this.thread = thread;
        this.handler = handler;
    }
}

public class AltTcpListener : TcpListener {
    public AltTcpListener(IPEndPoint localEp) : base(localEp) {
    }

    public AltTcpListener(IPAddress localaddr, int port) : base(localaddr, port) {
    }

    public new bool Active {
        get { return base.Active; }
    }
}

public class AltSocketServer {
    protected AltTcpListener Listener;
    protected readonly AltIClientSocketHandlerDelegate ClientSocketHandlerDelegate;
    protected readonly string SeparatorString;
    protected readonly Encoding Encoding;
    protected ArrayList ClientHandlerThreads;
    protected readonly int portNumber;
    protected readonly IPEndPoint localEndPoint;
    protected readonly int maxClients;

    public int PortNumber {
        get {
            return portNumber;
        }
    }

    public IPEndPoint LocalEndPoint {
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

    public AltSocketServer(AltIClientSocketHandlerDelegate clientSocketHandlerDelegate,
	                         int portNumber = 13000,
                             int maxClients = 1,
                             string separatorString = "\n",
                             Encoding encoding = null) {
        this.portNumber = portNumber;
        ClientSocketHandlerDelegate = clientSocketHandlerDelegate;
        SeparatorString = separatorString;
        Encoding = encoding ?? Encoding.UTF8;
        ClientHandlerThreads = ArrayList.Synchronized(new ArrayList());
        this.maxClients = maxClients;

        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        localEndPoint = new IPEndPoint(ipAddress, this.portNumber);
        Listener = new AltTcpListener(localEndPoint.Address, this.portNumber);


        Debug.Log("Created TCP listener.");
    }

    public void StartListeningForConnections()
    {
        foreach (AltSocketClientThreadHolder holder in ClientHandlerThreads)
        {
            Debug.Log("calling stop on thread " + holder.Thread.ManagedThreadId);
            holder.Handler.Cleanup();
            Debug.Log("Calling thread abort on thread: " + holder.Thread.ManagedThreadId);
        }

        ClientHandlerThreads = ArrayList.Synchronized(new ArrayList()); 
        Debug.Log("Began listening for TCP clients.");
        Listener.Start();
        ListenForConnection();
    }

    protected void ListenForConnection() {
        Listener.BeginAcceptTcpClient(AcceptCallback, Listener);
    }

    // NOT on main thread
    protected void AcceptCallback(IAsyncResult ar) {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Debug.Log("Accept thread id: " + threadId);
        TcpListener listener = (TcpListener)ar.AsyncState;
        TcpClient client = listener.EndAcceptTcpClient(ar);

        Debug.Log("thread id " + threadId + " accepted client " + client.Client.RemoteEndPoint);
        Debug.Log("thread id " + threadId + " beginning read from client " + client.Client.RemoteEndPoint);

        AltClientSocketHandler clientHandler =
            new AltClientSocketHandler(client,
                                        ClientSocketHandlerDelegate,
                                        SeparatorString,
                                        Encoding);

        Thread clientThread = new Thread(clientHandler.Run);
        ClientHandlerThreads.Add(new AltSocketClientThreadHolder(clientThread, clientHandler));
        clientThread.Start();
        Debug.Log("Client thread started");

        if (ClientCount < maxClients) {
            Debug.Log("client handler threads less than max clients. Listening again");
            ListenForConnection();
        } else {
            Debug.Log(String.Format("Max number of clients reached ({0}), stopping listening", maxClients));
            StopListeningForConnections();
        }
    }

    public void StopListeningForConnections() {
        Listener.Stop();
        Debug.Log("Stopped listening for connections");
    }

    public void Cleanup() {
        StopListeningForConnections();
        foreach (AltSocketClientThreadHolder holder in ClientHandlerThreads) {
            Debug.Log("calling stop on thread " + holder.Thread.ManagedThreadId);
            holder.Handler.Cleanup();
            Debug.Log("Calling thread abort on thread: " + holder.Thread.ManagedThreadId);
            holder.Handler.ToBeKilled = true;
            holder.Thread.Abort();
        }
        ClientHandlerThreads = null;
	}

    public bool IsStarted() {
        return Listener != null && Listener.Active;
    }
}

