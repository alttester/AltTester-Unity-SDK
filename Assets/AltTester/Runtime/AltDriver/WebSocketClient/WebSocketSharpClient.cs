using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AltWebSocketSharp;

namespace AltTester.AltTesterUnitySDK.Driver.WebSocketClient
{
    public class WebSocketSharpClient : IWebSocketClient
    {
        public event WebSocketOpenEventHandler OnOpen;
        public event WebSocketMessageEventHandler OnMessage;
        public event WebSocketErrorEventHandler OnError;
        public event WebSocketCloseEventHandler OnClose;

        private WebSocket ws = null;

        private Uri uri;
        private Dictionary<string, string> headers;
        private string proxyUrl = null;

        public WebSocketSharpClient(Uri uri, Dictionary<string, string> headers = null, string proxyUrl = null)
        {
            this.uri = uri;
            string protocol = this.uri.Scheme;
            if (!protocol.Equals("ws") && !protocol.Equals("wss"))
            {
                throw new ArgumentException($"Unsupported protocol: {protocol}");
            }

            this.headers = headers ?? new Dictionary<string, string>();

            if (proxyUrl != null)
            {
                this.proxyUrl = proxyUrl;
            }
        }

        public WebSocketClientState State
        {
            get
            {
                if (this.ws == null)
                {
                    return WebSocketClientState.Closed;
                }

                if (this.ws.IsAlive)
                {
                    return WebSocketClientState.Open;
                }

                return WebSocketClientState.Closed;
            }
        }

        public bool IsAlive
        {
            get
            {
                return this.ws != null && this.ws.IsAlive;
            }
        }

        public void SetProxy(string url)
        {
            if (url != null)
            {
                proxyUrl = url;
            }
        }

        public Task ConnectAsync()
        {
            this.ws = this.GetWebSocketSharp();
            this.ws.ConnectAsync();

            return Task.CompletedTask;
        }

        public void Connect()
        {
            this.ws = this.GetWebSocketSharp();
            this.ws.Connect();
        }

        public Task CloseAsync()
        {
            if (this.ws == null)
            {
                return Task.CompletedTask;
            }

            this.ws = this.GetWebSocketSharp();
            this.ws.CloseAsync();

            return Task.CompletedTask;
        }

        public void Close()
        {
            if (this.ws == null)
            {
                return;
            }

            this.ws.Close();
        }

        public Task SendAsync(byte[] bytes)
        {
            if (this.ws == null)
            {
                return Task.CompletedTask;
            }

            this.ws.SendAsync(bytes, null);
            return Task.CompletedTask;
        }

        public Task SendAsync(string message)
        {
            if (this.ws == null)
            {
                return Task.CompletedTask;
            }

            this.ws.SendAsync(message, null);
            return Task.CompletedTask;
        }

        public void Send(byte[] bytes)
        {
            if (this.ws == null)
            {
                return;
            }

            this.ws.Send(bytes);
        }

        public void Send(string message)
        {
            if (this.ws == null)
            {
                return;
            }

            this.ws.Send(message);
        }

        public void HandleIncomingMessages()
        {
        }

        private WebSocket GetWebSocketSharp()
        {
            ws = new WebSocket(this.uri.ToString());

            if (this.proxyUrl != null)
            {
                this.ws.SetProxy(this.proxyUrl, null, null);
            }

            this.ws.OnOpen += (sender, args) => {
                this.OnOpen?.Invoke();
            };
            this.ws.OnError += (sender, args) => {
                this.OnError?.Invoke(args.Message);
            };
            this.ws.OnClose += (sender, args) => {
                this.OnClose?.Invoke(args.Code, args.Reason);
            };
            this.ws.OnMessage += (sender, args) => {
                this.OnMessage?.Invoke(args.RawData);
            };

            return ws;
        }
    }
}
