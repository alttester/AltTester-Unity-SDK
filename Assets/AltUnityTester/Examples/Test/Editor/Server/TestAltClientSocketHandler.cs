using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.AltSocket;
using Assets.AltUnityTester.AltUnityServer;
using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Assets.AltUnityTester.AltUnityServer.Commands;
using NUnit.Framework;

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
            throw new NotImplementedException();
        }

        public int Receive(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] buffer)
        {
            if (sendCallback != null)
                sendCallback.Invoke(buffer);
        }

        public void SetupSendCallback(Action<byte[]> callback)
        {
            sendCallback = callback;
        }
    }

    public class TestAltClientSocketHandler
    {
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
        }

        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TestAltUnityErrorCommand()
        {
            socket.SetupSendCallback(buffer =>
            {
                var result = System.Text.Encoding.UTF8.GetString(buffer);
                Assert.IsTrue(result.StartsWith("altstart::messageid::response::error:couldNotParseJsonString::altLog::errormessage"), result);
            });
            var command = new AltUnityErrorCommand(AltUnityErrors.errorCouldNotParseJsonString, new Exception("errormessage"), "messageid", "commandname", "param1", "param2");

            var cmdResult = command.ExecuteHandleErrors(command.Execute);

            socketHandler.SendResponse(command.MessageId, command.CommandName, cmdResult.Item1, command.GetLogs());
        }

        [Test]
        public void TestSuccessCommand()
        {
            socket.SetupSendCallback(buffer =>
            {
                string message = System.Text.Encoding.UTF8.GetString(buffer);
                Assert.IsTrue(message.Equals("altstart::messageid::response::Ok::altLog::::altend"), message);
            });
            var command = new AltUnityEnableLoggingCommand("messageid", "enableLoggingCommand", "true");

            var response = command.Execute();
            socketHandler.SendResponse(command.MessageId, command.CommandName, response, string.Empty);
        }


        [Test]
        public void TestInvalidParametersException()
        {
            try
            {
                var command = new AltUnityEnableLoggingCommand("messageid", "enableLoggingCommand", "true", "some", "extra");
                Assert.Fail();
            }
            catch (InvalidParametersOnDriverCommandException)
            {

            }
        }
    }
}