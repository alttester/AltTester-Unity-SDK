using System;
using Altom.AltUnityDriver.AltSocket;
using Altom.Server.Logging;
using NLog;

namespace Assets.AltUnityTester.AltUnityServer.AltSocket
{
    public interface AltIClientSocketHandlerDelegate
    {
        // callback, will be NOT be invoked on main thread, so make sure to synchronize in Unity
        void ClientSocketHandlerDidReadMessage(AltClientSocketHandler handler, string message);
    }

    public class AltClientSocketHandler
    {
        private static readonly Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        protected readonly ISocket Socket;
        protected readonly string MessageEndingString;
        protected readonly System.Text.Encoding Encoding;
        protected AltIClientSocketHandlerDelegate ClientSocketHandlerDelegate;
        public bool ToBeKilled;
        public static int MaxLogLength = 100;

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
            logger.Debug("sent: " + response);
        }

        public void SendResponse(string messageId, string commandName, string response, string logs)
        {
            var logMessage = logs;

            Socket.Send(Encoding.GetBytes("altstart::" + messageId + "::response::" + response + "::altLog::" + logMessage + "::altend"));
            if (response != null && MaxLogLength != 0 && response.Length > MaxLogLength)
                response = "Response longer then max length '" + AltUnityRunner._altUnityRunner.MaxLogLength + "'. Response length = '" + response.Length.ToString() + "'";

            logger.Debug("sent: " + string.Join(";", messageId, commandName, response, logs));
        }

        public void SendScreenshotResponse(string messageId, string commandName, string response)
        {
            var logMessage = "Screenshot length " + response.Length;
            response = "altstart::" + messageId + "::response::" + response + "::altLog::" + logMessage + "::altend";
            Socket.Send(Encoding.GetBytes(response));
            logger.Debug("sent: " + messageId + ";" + commandName + ";" + logMessage);
        }

        public void Run()
        {
            try
            {
                var dataBuffer = new System.Text.StringBuilder();

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
                logger.Error(exception);
            }
            catch (System.Net.Sockets.SocketException exception)
            {
                logger.Error(exception);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }
            finally
            {
                Socket.Close();
                ToBeKilled = true;
                logger.Debug("AltClientSocketHandler - Client closed");
            }
        }
    }
}