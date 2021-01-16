using NUnit.Framework;

using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Altom.AltUnityDriver.AltSocket;
using Assets.AltUnityTester.AltUnityServer.Commands;
using System;

namespace unit.AltUnityServer
{
    public class TestAltIClientSocketHandlerDelegate : AltIClientSocketHandlerDelegate
    {
        public void ClientSocketHandlerDidReadMessage(AltClientSocketHandler handler, string message)
        {
        }
    }
    public class TestSocket : ISocket
    {
        Action<byte[]> sendCallback;
        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public int Receive(byte[] buffer)
        {
            throw new System.NotImplementedException();
        }

        public void Send(byte[] buffer)
        {
            if (sendCallback != null) sendCallback(buffer);
        }

        public void SetupSendCallback(Action<byte[]> callback)
        {
            sendCallback = callback;
        }
    }

    public class TestAltClientSocketHandler
    {
        private System.IO.MemoryStream memoryStream;
        private AltClientSocketHandler socketHandler;
        private TestSocket socket;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var socketHandlerDelegate = new TestAltIClientSocketHandlerDelegate();
            socket = new TestSocket();
            socketHandler = new AltClientSocketHandler(socket, socketHandlerDelegate, "&", System.Text.Encoding.UTF8);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            AltUnityRunner.ServerLogger.Close();
        }

        [SetUp]
        public void SetUp()
        {
            memoryStream = new System.IO.MemoryStream();
            AltUnityRunner.ServerLogger = new System.IO.StreamWriter(memoryStream);
        }

        [TearDown]
        public void TearDown()
        {
            AltUnityRunner.ServerLogger.Close();
        }

        private string GetLogs()
        {
            AltUnityRunner.ServerLogger.Flush();
            var buffer = memoryStream.GetBuffer();
            return System.Text.Encoding.UTF8.GetString(buffer, 0, (int)memoryStream.Length);
        }

        [Test]
        public void TestServerLogger()
        {
            AltUnityRunner.ServerLogger.Write("test log line");
            Assert.AreEqual("test log line", GetLogs());
        }

        [Test]
        public void TestSendResponse()
        {
            socket.SetupSendCallback(buffer =>
            {
                Assert.AreEqual("altstart::messageid::response::error:couldNotParseJsonString::altLog::::altend", System.Text.Encoding.UTF8.GetString(buffer));
            });


            AltUnityCouldNotParseJsonStringCommand command = new AltUnityCouldNotParseJsonStringCommand("messageid", "commandname", "param1", "param2");

            var response = command.Execute();
            socketHandler.SendResponse(command, response);

            var logs = GetLogs();

            Assert.True(logs.Contains("command received: "), logs);
            Assert.True(logs.Contains("response sent: "), logs);
        }

        [Test]
        public void TestEnableLogging()
        {
            socket.SetupSendCallback(buffer =>
            {
                string message = System.Text.Encoding.UTF8.GetString(buffer);
                Assert.IsTrue(message.StartsWith("altstart::messageid::response::Ok::altLog::"), message);
                Assert.IsTrue(message.EndsWith("Logging is set to True::altend"), message);
            });
            var command = new AltUnityEnableLoggingCommand("messageid", "enableLoggingCommand", "true");

            var response = command.Execute();
            socketHandler.SendResponse(command, response);
            var logs = GetLogs();
            Assert.True(logs.Contains("command received: messageid;enableLoggingCommand;true"), logs);
            Assert.True(logs.Contains("Logging is set to True"), logs);
            Assert.True(logs.Contains("response sent: messageid;enableLoggingCommand;Ok"), logs);
        }
    }
}