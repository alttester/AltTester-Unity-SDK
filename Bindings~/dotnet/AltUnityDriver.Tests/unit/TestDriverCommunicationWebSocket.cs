using Altom.AltUnityDriver.Commands;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Altom.AltUnityDriver.Tests
{
    public class TestDriverCommunicationWebSocket
    {
        [Test]
        public void TestRecvall()
        {
            Mock<IWebSocketClient> wsClient = new Mock<IWebSocketClient>();
            DriverCommunicationWebSocket socket = new DriverCommunicationWebSocket(wsClient.Object);

            CommandResponse<string> response = new CommandResponse<string>();
            response.commandName = "commandName";
            response.data = "data";
            response.messageId = "messageId";

            wsClient.Raise(foo => foo.OnMessage += null, this, JsonConvert.SerializeObject(response));

            var cmdParams = new CommandParams("commandName", "messageId");

            var result = socket.Recvall<string>(cmdParams);

            Assert.AreEqual(response.data, result.data);
        }
    }
}