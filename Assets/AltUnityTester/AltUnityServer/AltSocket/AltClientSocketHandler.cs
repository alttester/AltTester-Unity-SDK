using System.Text.RegularExpressions;
using Altom.AltUnityDriver.AltSocket;
using Assets.AltUnityTester.AltUnityServer.Commands;

namespace Assets.AltUnityTester.AltUnityServer.AltSocket
{
    public interface AltIClientSocketHandlerDelegate
    {
        // callback, will be NOT be invoked on main thread, so make sure to synchronize in Unity
        void ClientSocketHandlerDidReadMessage(AltClientSocketHandler handler, string message);
    }

    public class AltClientSocketHandler
    {
        protected readonly ISocket Socket;
        protected readonly string MessageEndingString;
        protected readonly System.Text.Encoding Encoding;
        protected AltIClientSocketHandlerDelegate ClientSocketHandlerDelegate;
        public bool ToBeKilled;


        public AltClientSocketHandler(ISocket socket,
                                        AltIClientSocketHandlerDelegate clientSocketHandlerDelegate,
                                        string messageEndingString,
                                        System.Text.Encoding encoding)
        {
            Socket = socket;
            Encoding = encoding;
            MessageEndingString = messageEndingString;
            ClientSocketHandlerDelegate = clientSocketHandlerDelegate;
            ToBeKilled = false;
        }

        public void Cleanup()
        {
            if (Socket != null)
            {
                ToBeKilled = true;
                Socket.Close();
            }
        }

        public void SendResponseBackwardsCompatible(string response)
        {
            response = "altstart::" + response + "::altLog::::altend";
            Socket.Send(Encoding.GetBytes(response));
        }

        public void SendResponse(AltUnityCommand command, string response)
        {
            var logMessage = Regex.Replace(command.GetLogMessage(), @"\r\n|\n|\r$", "");//Removes the last new line

            Socket.Send(Encoding.GetBytes("altstart::" + command.MessageId + "::response::" + response + "::altLog::" + logMessage + "::altend"));
            command.EndLog(response);
        }
        public void SendScreenshotResponse(AltUnityCommand command, string response)
        {
            var logMessage = "Screenshot length " + response.Length;
            response = "altstart::" + command.MessageId + "::response::" + response + "::altLog::" + logMessage + "::altend";
            Socket.Send(Encoding.GetBytes(response));
            command.EndLog(logMessage);
        }

        public void Run()
        {
            try
            {
                System.Text.StringBuilder dataBuffer = new System.Text.StringBuilder();

                while (true)
                {

                    byte[] readBuffer = new byte[256];
                    int readLength = Socket.Receive(readBuffer);

                    // append to token
                    if (readLength > 0)
                    {
                        dataBuffer.Append(Encoding.GetString(readBuffer, 0, readLength));
                        string data = dataBuffer.ToString();
                        dataBuffer = new System.Text.StringBuilder();

                        string[] tokens = data.Split(new[] { MessageEndingString }, System.StringSplitOptions.None);

                        bool endsWithSeparator = data.EndsWith(MessageEndingString);

                        // all except the last piece
                        for (int i = 0; i < (tokens.Length - 1); i++)
                        {
                            ClientSocketHandlerDelegate.ClientSocketHandlerDidReadMessage(this, tokens[i]);
                        }

                        // for the last piece, if the data ended with separator than this is a full token
                        // otherwise, its not so append to data buffer
                        if (endsWithSeparator)
                        {
                            if (tokens[tokens.Length - 1].Length > 0)
                            {
                                ClientSocketHandlerDelegate.ClientSocketHandlerDidReadMessage(this, tokens[tokens.Length - 1]);
                            }
                        }
                        else
                        {
                            dataBuffer.Append(tokens[tokens.Length - 1]);
                        }
                    }

                    if (ToBeKilled)
                    {
                        break;
                    }
                }
            }
            catch (System.Threading.ThreadAbortException exception)
            {
                AltUnityRunner.ServerLogger.Write("Thread aborted(" + exception + ")" + System.Environment.NewLine);
                UnityEngine.Debug.Log("Thread aborted(" + exception + ")");
            }
            catch (System.Net.Sockets.SocketException exception)
            {
                AltUnityRunner.ServerLogger.Write("Socket exception(" + exception + ")" + System.Environment.NewLine);
                UnityEngine.Debug.Log("Socket exception(" + exception + ")");
            }
            catch (System.Exception exception)

            {
                AltUnityRunner.ServerLogger.Write("Exception(" + exception + ")" + System.Environment.NewLine);
                UnityEngine.Debug.Log("Exception(" + exception + ")");
            }
            finally
            {
                Socket.Close();
                ToBeKilled = true;
                AltUnityRunner.ServerLogger.Write("AltClientSocketHandler - Client closed" + System.Environment.NewLine);
                UnityEngine.Debug.Log("AltClientSocketHandler - Client closed");

            }
        }
    }
}