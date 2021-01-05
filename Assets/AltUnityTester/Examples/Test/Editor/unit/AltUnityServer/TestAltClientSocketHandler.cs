using NUnit.Framework;
using Moq;

using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Altom.AltUnityDriver.AltSocket;
using Assets.AltUnityTester.AltUnityServer.Commands;

namespace unit.AltUnityServer
{
    public class TestAltClientSocketHandler
    {
        private System.IO.MemoryStream memoryStream;
        private AltClientSocketHandler socketHandler;
        private Mock<ISocket> socketMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            socketMock = new Mock<ISocket>();

            var socketHandlerDelegate = new Mock<AltIClientSocketHandlerDelegate>();
            socketHandler = new AltClientSocketHandler(socketMock.Object, socketHandlerDelegate.Object, "&", System.Text.Encoding.UTF8);
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
            socketMock.Reset();
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
            socketMock.Setup(c => c.Send(It.IsAny<byte[]>())).Callback<byte[]>(buffer =>
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
            socketMock.Setup(c => c.Send(It.IsAny<byte[]>())).Callback<byte[]>(buffer =>
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