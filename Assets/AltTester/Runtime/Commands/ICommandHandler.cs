/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterUnitySDK.Commands
{
    public delegate void SendMessageHandler(string message);
    public delegate void NotificationHandler(string driverId);

    public enum HandshakeStatus { Unlicensed, AwaitingToken, Licensed, Failed }

    public interface ICommandHandler
    {
        SendMessageHandler OnSendMessage { get; set; }

        NotificationHandler OnDriverConnect { get; set; }
        NotificationHandler OnDriverDisconnect { get; set; }
        NotificationHandler OnAppConnect { get; set; }

        void Send(string data);
        void OnMessage(string data);

        HandshakeStatus CurrentHandshakeStatus { get; }
        System.Action OnHandshakeFailed { get; set; }
        void InitiateHandshake(string appName, string deviceInstanceId, string sdkVersion);
    }
}
