// using System;
// using System.Collections.Generic;
// using System.Threading;
// using WebSocketSharp;

// namespace Altom.AltUnityDriver.AltSocket
// {
//     public class Socket : ISocket
//     {
//         private readonly System.Net.Sockets.Socket socket;
//         public Socket(System.Net.Sockets.Socket socket)
//         {
//             this.socket = socket;
//         }

//         public void Send(string message)
//         {
//             byte[] buffer = toBytes(message);
//             this.socket.Send(buffer);
//         }
//         public string Receive()
//         {
//             throw new NotImplementedException("Method not implemented");
//         }

//         public void Close()
//         {
//             this.socket.Close();
//         }

//         private byte[] toBytes(string text)
//         {
//             return System.Text.Encoding.UTF8.GetBytes(text);
//         }
//     }

//     public class AuWebSocket : ISocket
//     {
//         private readonly WebSocket clientWebSocket;
//         private Queue<string> messages;

//         public AuWebSocket(WebSocket clientWebSocket)
//         {
//             this.clientWebSocket = clientWebSocket;
//             messages = new Queue<string>();

//             clientWebSocket.OnMessage += onMessage;
//         }

//         public void Close()
//         {
//             clientWebSocket.Close();
//         }

//         public string Receive()
//         {
//             while (messages.Count == 0)
//             {
//                 Thread.Sleep(10);
//             }
//             return messages.Dequeue();
//         }

//         public void Send(string message)
//         {
//             clientWebSocket.Send(message);
//         }


//         private void onMessage(object sender, MessageEventArgs e)
//         {
//             messages.Enqueue(e.Data);
//         }
//     }
// }
