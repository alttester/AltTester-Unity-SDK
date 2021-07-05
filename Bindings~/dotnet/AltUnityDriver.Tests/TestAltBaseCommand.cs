using NUnit.Framework;
using Moq;
using System.Linq;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.AltSocket;
using Altom.AltUnityDriver.Commands;

using Altom.AltUnityDriver.Logging;


namespace unit.AltUnityDriverTests
{
    public static class Extension
    {
        public static T[] Concatenate<T>(this T[] first, T[] second)
        {
            if (first == null)
            {
                return second;
            }
            if (second == null)
            {
                return first;
            }

            return first.Concat(second).ToArray();
        }
    }
    public class TestSocket : ISocket
    {
        private readonly string returnMessage;
        private readonly string log;
        string command = null;
        string message = null;
        string messageId = null;
        int position = 0;
        private byte[] returnMessageBytes = null;

        public TestSocket(string returnMessage, string log)
        {
            this.returnMessage = returnMessage;
            this.log = log;
        }
        public void Close()
        {
        }

        public int Receive(byte[] buffer)
        {
            if (position >= returnMessageBytes.Length)
                return 0;
            int i;
            for (i = 0; i + position < returnMessageBytes.Length && i < buffer.Length; i++)
            {
                buffer[i] = returnMessageBytes[i + position];
            }
            position += i;
            return i;
        }

        public void Send(byte[] buffer)
        {
            this.message = System.Text.Encoding.UTF8.GetString(buffer);
            var parts = message.Split(new[] { ";", "&" }, System.StringSplitOptions.None);
            this.messageId = parts[0];
            this.command = parts[1];

            returnMessageBytes = System.Text.Encoding.UTF8.GetBytes("altstart::" + messageId + "::response::" + returnMessage + "::altLog::" + log + "::altend");
            if (this.command == "doublemessage")
                returnMessageBytes = returnMessageBytes.Concatenate(returnMessageBytes);
            position = 0;
        }

        public string MessageSent { get { return this.message; } }
    }
    public class AltBaseCommandImpl : AltBaseCommand
    {
        public AltBaseCommandImpl(SocketSettings socketSettings) : base(socketSettings)
        {

        }
        public string Execute()
        {
            SendCommand("altBaseCommand");
            return Recvall();
        }
    }

    public class AltDoubleBaseCommandImpl : AltBaseCommand
    {
        public AltDoubleBaseCommandImpl(SocketSettings socketSettings) : base(socketSettings)
        {

        }
        public string Execute()
        {
            SendCommand("doublemessage");
            return Recvall() + "___" + Recvall();
        }
    }

    [Timeout(1000)]

    public class TestAltBaseCommand
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DriverLogManager.SetMinLogLevel(AltUnityLogger.Console, AltUnityLogLevel.Debug);

        }
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void TestSendCommandAndRecvall()
        {
            TestSocket socket = new TestSocket("error:couldNotParseJsonString", "");
            try
            {
                AltBaseCommandImpl command = new AltBaseCommandImpl(new SocketSettings(socket, ";", "&"));
                var response = command.Execute();
                Assert.Fail();
            }
            catch (CouldNotParseJsonStringException)
            {
                int expectedLength = System.DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString().Length + ";altBaseCommand&".Length;
                Assert.IsTrue(socket.MessageSent.EndsWith(";altBaseCommand&"), socket.MessageSent);
                Assert.AreEqual(socket.MessageSent.Length, expectedLength, socket.MessageSent);
            }
        }
        [Test]
        public void TestAltUnitySyncCommand()
        {
            TestSocket socket = new TestSocket("0.0.1", "");
            AltUnitySyncCommand command = new AltUnitySyncCommand(new SocketSettings(socket, ";", "&"));

            command.Execute();

            int expectedLength = System.DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString().Length + ";getServerVersion&".Length;
            Assert.IsTrue(socket.MessageSent.EndsWith(";getServerVersion&"), socket.MessageSent);
            Assert.AreEqual(socket.MessageSent.Length, expectedLength, socket.MessageSent);
        }

        [Test]
        public void SendMultipleResponsesInSameChunk()
        {
            TestSocket socket = new TestSocket("message", "");
            var cmd = new AltDoubleBaseCommandImpl(new SocketSettings(socket, ";", "&"));
            var response = cmd.Execute();

            Assert.AreEqual("message___message", response);

        }
    }
}