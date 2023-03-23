namespace AltTester.AltTesterUnitySdk.Communication
{
    public delegate void NotificationHandler(string driverId);

    public interface ICommandHandler
    {
        SendMessageHandler OnSendMessage { get; set; }

        NotificationHandler OnDriverConnect { get; set; }
        NotificationHandler OnDriverDisconnect { get; set; }

        void Send(string data);
        void OnMessage(string data);
    }
}
