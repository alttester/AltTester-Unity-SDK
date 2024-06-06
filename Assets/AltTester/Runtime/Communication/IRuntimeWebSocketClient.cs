/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using AltWebSocketSharp;

namespace AltTester.AltTesterUnitySDK.Communication
{
    public delegate void CommunicationHandler();
    public delegate void CommunicationDisconnectHandler(int code, string reason);
    public delegate void CommunicationErrorHandler(string message, Exception error);
    public delegate void CommunicationMessageHandler(string message);

    public interface IRuntimeWebSocketClient
    {
        CommunicationHandler OnConnect { get; set; }
        CommunicationDisconnectHandler OnDisconnect { get; set; }
        CommunicationErrorHandler OnError { get; set; }
        CommunicationMessageHandler OnMessage { get; set; }
        bool IsConnected { get; }
        WebSocketState ReadyState { get; }

        void Connect();
        void Close();
        void Send(string message);
        void Send(byte[] message);
    }

    public class RuntimeWebSocketClientException : Exception
    {
        public RuntimeWebSocketClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
