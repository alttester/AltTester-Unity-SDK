using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.Linq;

public interface AltIClientSocketHandlerDelegate
{
    // callback, will be NOT be invoked on main thread, so make sure to synchronize in Unity
    void ClientSocketHandlerDidReadMessage(AltClientSocketHandler handler, string message);
}

public class AltClientSocketHandler
{

    protected readonly TcpClient Client;
    protected readonly string SeparatorSequence;
    protected readonly char[] SeparatorSequenceChars;
    protected readonly Encoding Encoding;
    protected AltIClientSocketHandlerDelegate ClientSocketHandlerDelegate;
    public bool ToBeKilled;


    public AltClientSocketHandler(TcpClient client,
                                    AltIClientSocketHandlerDelegate clientSocketHandlerDelegate,
                                    string separatorString,
                                    Encoding encoding)
    {
        Client = client;
        Encoding = encoding;
        SeparatorSequence = separatorString;
        SeparatorSequenceChars = separatorString.ToCharArray();
        ClientSocketHandlerDelegate = clientSocketHandlerDelegate;
        ToBeKilled = false;
    }

    public void Cleanup()
    {
        if (Client != null)
        {
            Client.Close();
        }

    }

    public void SendResponse(string response)
    {
        response = "altstart::" + response + "::altend";
        Debug.Log("sending response: " + response);
        Client.Client.Send(Encoding.GetBytes(response));
    }
    public void SendResponse(byte[] response)
    {
        response = Encoding.ASCII.GetBytes("altstart::").Concat(response).Concat( Encoding.ASCII.GetBytes("::altend")).ToArray();
        Debug.Log("sending response: " + Encoding.ASCII.GetString(response));
        Client.Client.Send(response);
    }

    public void Run()
    {
        try
        {
            StringBuilder dataBuffer = new StringBuilder();

            while (true)
            {
                byte[] readBuffer = new byte[256];
                int readLength = Client.Client.Receive(readBuffer);

                // append to token
                if (readLength > 0)
                {
                    dataBuffer.Append(Encoding.GetString(readBuffer, 0, readLength));
                    string data = dataBuffer.ToString();
                    dataBuffer = new StringBuilder();

                    string[] tokens = data.Split(new[] { SeparatorSequence }, StringSplitOptions.None);

                    bool endsWithSeparator = data.EndsWith(SeparatorSequence);

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
        catch (ThreadAbortException exception)
        {
            Debug.Log("Thread aborted(" + exception + ")");
        }
        catch (SocketException exception)
        {
            Debug.Log("Socket exception(" + exception + ")");
        }
        catch (Exception exception)

        {
            Debug.Log("Exception(" + exception + ")");
        }
        finally
        {
            Client.Close();
            Debug.Log("AltClientSocketHandler - Client closed");

        }
    }

}
