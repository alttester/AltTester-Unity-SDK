using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;

namespace AltTester.AltTesterUnitySDK.Communication
{
#if UNITY_WEBGL
    public class WebSocketWebGLCommunication : ICommunication
    {
        WebGLWebSocket webglWebSocket;

        public WebSocketWebGLCommunication(ICommandHandler cmdHandler, string host, int port)
        {
            Uri uri = Utils.CreateURI(host, port, "/altws/app", appName);
            webglWebSocket = new WebGLWebSocket(uri.ToString());

            var webGLWebSocketHandler = new AltWebGLWebSocketHandler(cmdHandler, webglWebSocket);

            webglWebSocket.OnOpen += () =>
            {
                if (this.OnConnect != null) this.OnConnect.Invoke();
                webglWebSocket.OnError += (string errorMsg) =>
                {
                    if (this.OnError != null) this.OnError.Invoke(errorMsg, null);
                };
            };

            webglWebSocket.OnClose += (WebSocketCloseCode closeCode) =>
            {
                if (this.OnDisconnect != null) this.OnDisconnect.Invoke();
            };
        }
        public bool IsConnected => webglWebSocket.State == WebSocketState.Open;
        public bool IsListening => false;

        public CommunicationHandler OnConnect { get; set; }
        public CommunicationHandler OnDisconnect { get; set; }
        public CommunicationErrorHandler OnError { get; set; }

        public void Start()
        {
            try
            {
                webglWebSocket.Connect().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new UnhandledStartCommError("An error occurred while starting client CommunicationProtocol", ex);
            }
        }

        public void Stop()
        {
            webglWebSocket.Close().GetAwaiter().GetResult();
        }
    }
#endif
}
