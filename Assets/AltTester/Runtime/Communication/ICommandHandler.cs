namespace AltTester.AltTesterUnitySDK.Communication
{
    public delegate void SendMessageHandler(string message);
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