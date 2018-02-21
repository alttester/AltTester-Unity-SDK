using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using System.Text;
using System.Reflection;
using System.Threading;

public class AltSocketClientThreadHolder {
    protected readonly Thread thread;
    protected readonly AltClientSocketHandler handler;

    public Thread Thread {
        get {
            return this.thread;
        }
    }

    public AltClientSocketHandler Handler {
        get {
            return this.handler;
        }
    }

    public AltSocketClientThreadHolder(Thread thread, AltClientSocketHandler handler) {
        this.thread = thread;
        this.handler = handler;
    }
}

public class AltTcpListener : TcpListener {
    public AltTcpListener(IPEndPoint localEP) : base(localEP) {
    }

    public AltTcpListener(IPAddress localaddr, int port) : base(localaddr, port) {
    }

    public new bool Active {
        get { return base.Active; }
    }
}

public class AltSocketServer {
    protected readonly AltTcpListener listener;
    protected readonly AltIClientSocketHandlerDelegate clientSocketHandlerDelegate;
    protected readonly string separatorString;
    protected readonly Encoding encoding;
    protected ArrayList clientHandlerThreads;
    protected readonly int portNumber;
    protected readonly IPEndPoint localEndPoint;
    protected readonly int maxClients;

    public int PortNumber {
        get {
            return this.portNumber;
        }
    }

    public IPEndPoint LocalEndPoint {
        get {
            return this.localEndPoint;
        }
    }

    public int MaxClients {
        get {
            return this.maxClients;
        }
    }

    public int ClientCount {
        get {
            return this.clientHandlerThreads.Count;
        }
    }

    public AltSocketServer(AltIClientSocketHandlerDelegate clientSocketHandlerDelegate,
	                         int portNumber = 13000,
                             int maxClients = 1,
                             string separatorString = "\n",
                             Encoding encoding = null) {
        this.portNumber = portNumber;
        this.clientSocketHandlerDelegate = clientSocketHandlerDelegate;
        this.separatorString = separatorString;
        this.encoding = (encoding != null) ? encoding : Encoding.UTF8;
        this.clientHandlerThreads = ArrayList.Synchronized(new ArrayList());
        this.maxClients = maxClients;


        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        this.localEndPoint = new IPEndPoint(ipAddress, this.portNumber);
        this.listener = new AltTcpListener(this.localEndPoint.Address, this.portNumber);
        Debug.Log("Created TCP listener.");
    }

    public void StartListeningForConnections() {
        Debug.Log("Began listening for TCP clients.");
        this.listener.Start();
        this.ListenForConnection();
    }

    protected void ListenForConnection() {
        this.listener.BeginAcceptTcpClient(new AsyncCallback(this.AcceptCallback), this.listener);
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
                                        this.clientSocketHandlerDelegate,
                                        this.separatorString,
                                        this.encoding);

        Thread clientThread = new Thread(new ThreadStart(clientHandler.Run));
        this.clientHandlerThreads.Add(new AltSocketClientThreadHolder(clientThread, clientHandler));
        clientThread.Start();
        Debug.Log("Client thread started");

        if (this.ClientCount < this.maxClients) {
            Debug.Log("client handler threads less than max clients. Listening again");
            this.ListenForConnection();
        } else {
            Debug.Log(String.Format("Max number of clients reached ({0}), stopping listening", this.maxClients));
            this.StopListeningForConnections();
        }
    }

    public void StopListeningForConnections() {
        this.listener.Stop();
        Debug.Log("Stopped listening for connections");
    }

    public void Cleanup() {
        StopListeningForConnections();
        foreach (AltSocketClientThreadHolder holder in this.clientHandlerThreads) {
            Debug.Log("calling stop on thread " + holder.Thread.ManagedThreadId);
            holder.Handler.Cleanup();
            Debug.Log("Calling thread abort on thread: " + holder.Thread.ManagedThreadId);
            holder.Thread.Abort();
        }
        this.clientHandlerThreads = null;
	}

    public bool IsStarted() {
        return listener != null && listener.Active;
    }
}

