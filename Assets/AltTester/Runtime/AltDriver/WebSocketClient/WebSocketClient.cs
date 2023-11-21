#if !UNITY_WEBGL
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;

namespace AltTester.AltTesterUnitySDK.Driver.WebSocketClient
{
    public class WebSocketClient : IWebSocketClient
    {
        public event WebSocketOpenEventHandler OnOpen;
        public event WebSocketMessageEventHandler OnMessage;
        public event WebSocketErrorEventHandler OnError;
        public event WebSocketCloseEventHandler OnClose;

        private Uri uri;
        private Dictionary<string, string> headers;
        private IWebProxy proxy;

        private ClientWebSocket ws = new ClientWebSocket();

        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public WebSocketClient(Uri url, Dictionary<string, string> headers = null, string proxyUrl = null)
        {
            this.uri = url;
            string protocol = this.uri.Scheme;
            if (!protocol.Equals("ws") && !protocol.Equals("wss"))
            {
                throw new ArgumentException($"Unsupported protocol: {protocol}");
            }

            this.headers = headers ?? new Dictionary<string, string>();

            if (proxyUrl != null)
            {
                this.SetProxy(proxyUrl);
            }
        }

        public WebSocketClientState State
        {
            get
            {
                if (ws == null) {
                    return WebSocketClientState.Closed;
                }

                switch (ws.State)
                {
                    case System.Net.WebSockets.WebSocketState.Connecting:
                        return WebSocketClientState.Connecting;

                    case System.Net.WebSockets.WebSocketState.Open:
                        return WebSocketClientState.Open;

                    case System.Net.WebSockets.WebSocketState.CloseSent:
                    case System.Net.WebSockets.WebSocketState.CloseReceived:
                        return WebSocketClientState.Closing;

                    case System.Net.WebSockets.WebSocketState.Closed:
                        return WebSocketClientState.Closed;

                    default:
                        return WebSocketClientState.Closed;
                }
            }
        }

        public bool IsAlive
        {
            get
            {
                return State == WebSocketClientState.Open || State == WebSocketClientState.Connecting;
            }
        }

        public void SetProxy(string url)
        {
            if (url != null)
            {
                proxy = new WebProxy(new Uri(url));
            }
        }

        public async Task ConnectAsync()
        {
            try
            {
                cancellationTokenSource = new CancellationTokenSource();
                cancellationToken = cancellationTokenSource.Token;

                if (proxy != null)
                {
                    WebRequest.DefaultWebProxy = proxy;
                }

                ws = new ClientWebSocket();

                if (proxy != null)
                {
                    ws.Options.Proxy = proxy;
                }

                foreach (var header in headers)
                {
                    ws.Options.SetRequestHeader(header.Key, header.Value);
                }

                await ws.ConnectAsync(uri, cancellationToken);
                OnOpen?.Invoke();

                await StartReceiving();
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
                OnClose?.Invoke((int)WebSocketCloseCode.Abnormal, ex.ToString());

                throw;
            }
            finally
            {
                if (ws != null)
                {
                    cancellationTokenSource.Cancel();
                    ws.Dispose();
                }
            }
        }

        public void Connect()
        {
            Task asyncTask = Task.Run(() => ConnectAsync());
            asyncTask.Wait();
        }

        public void CancelConnection()
        {
            cancellationTokenSource?.Cancel();
        }

        public async Task StartReceiving()
        {
            int closeCode = (int)WebSocketCloseCode.Abnormal;
            string closeReason = null;

            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);
            try
            {
                while (ws.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    WebSocketReceiveResult result = null;

                    using (var ms = new MemoryStream())
                    {
                        do
                        {
                            result = await ws.ReceiveAsync(buffer, cancellationToken);
                            ms.Write(buffer.Array, buffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);

                        ms.Seek(0, SeekOrigin.Begin);

                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            OnMessage?.Invoke(ms.ToArray());
                        }
                        else if (result.MessageType == WebSocketMessageType.Binary)
                        {
                            OnMessage?.Invoke(ms.ToArray());
                        }
                        else if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await CloseAsync();
                            closeCode = (int)result.CloseStatus;
                            closeReason = result.CloseStatusDescription;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                cancellationTokenSource.Cancel();
            }
            finally
            {
                OnClose?.Invoke(closeCode, closeReason);
            }
        }

        public Task SendAsync(byte[] bytes)
        {
            return ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        public void Send(byte[] bytes)
        {
            Task asyncTask = Task.Run(() => SendAsync(bytes));
            asyncTask.Wait();
        }

        public Task SendAsync(string message)
        {
            var encoded = Encoding.UTF8.GetBytes(message);
            return ws.SendAsync(new ArraySegment<byte>(encoded, 0, encoded.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public void Send(string message)
        {
            Task asyncTask = Task.Run(() => SendAsync(message));
            asyncTask.Wait();
        }

        public async Task CloseAsync()
        {
            if (State == WebSocketClientState.Open)
            {
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
            }
        }

        public void Close()
        {
            Task asyncTask = Task.Run(() => CloseAsync());
            asyncTask.Wait();
        }
    }
}
#endif
