using System;
using System.Diagnostics;
using Altom.AltTester.Communication;
using AltWebSocketSharp;
using AltWebSocketSharp.Server;
using NUnit.Framework;

namespace Altom.AltInstrumentation.Tests
{
    public class MockCommandHandler : ICommandHandler
    {
        public SendMessageHandler OnSendMessage { get; set; }

        public void OnMessage(string data)
        {
            throw new System.NotImplementedException();
        }

        public void Send(string data)
        {
            if (this.OnSendMessage != null)
            {
                this.OnSendMessage.Invoke(data);
            }
        }
    }



    public class MockServerHandler : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            base.OnOpen();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
        }

    }
    public class WebSocketClientCommunicationTests
    {

        [Test]
        public void TestConnectWithNoServer()
        {
            var cmdHandler = new MockCommandHandler();

            var client = new WebSocketClientCommunication(cmdHandler, "localhost", 13420);
            bool onConnect = false;
            client.OnConnect += () =>
            {
                onConnect = true;
            };

            bool onDisconnect = false;
            client.OnDisconnect += () =>
            {
                onDisconnect = true;
            };

            client.OnError += (string message, Exception error) =>
            {
                Assert.Fail("No errors expected");
            };

            client.Start();
            client.Stop();
            Assert.IsFalse(onConnect, "Should not be able to connect");
            Assert.IsTrue(onDisconnect, "Should call disconnect");
        }

        [Test]
        public void TestConnectWithServer()
        {
            var cmdHandler = new MockCommandHandler();

            var wsServer = new WebSocketServer("ws://0.0.0.0:13420");

            wsServer.AddWebSocketService<MockServerHandler>("/altws/game", (context, handler) =>
            {

            });
            wsServer.Start();
            var stopwatch = Stopwatch.StartNew();
            while (!wsServer.IsListening && stopwatch.ElapsedMilliseconds < 2000) ;
            Assert.IsTrue(wsServer.IsListening, "Expected to be listening");

            try
            {
                bool connected = false;
                bool disconnected = false;

                var client = new WebSocketClientCommunication(cmdHandler, "localhost", 13420);

                client.OnConnect += () =>
                {
                    connected = true;

                };

                client.OnDisconnect += () =>
                {
                    disconnected = true;

                };

                client.OnError += (string message, Exception error) =>
                {
                    Assert.Fail("No errors expected");
                };

                client.Start();

                stopwatch = Stopwatch.StartNew();
                while (!connected && stopwatch.ElapsedMilliseconds < 10000) ;
                Assert.IsTrue(connected, "Expected to be connected");

                client.Stop();
                stopwatch = Stopwatch.StartNew();
                while (!disconnected && stopwatch.ElapsedMilliseconds < 2000) ;

                Assert.IsTrue(disconnected, "Expected to be disconnected");
            }
            finally
            {
                wsServer.Stop();
            }
        }
    }
}