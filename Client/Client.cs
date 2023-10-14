using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MessengerClient;

class Client
{
    private static ASCIIEncoding encoder = new ASCIIEncoding();
    private static int clientId = 0;
    public static int GetClientId()
    {
        return clientId;
    }
    public static TcpClient client = new TcpClient();
    private static IPEndPoint serverEndPoint;

    public static void Start(string ip, int port)
    {
        serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        try
        {
            client.Connect(serverEndPoint);
        }
        catch (Exception e)
        {
            throw new Exception("No connection was made: " + e.Message);
        }

        while (true)
        {
            Output.Write(ConsoleColor.DarkBlue, "Me: ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            string message = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Black;

            if (Commands.IsCommand(message))
            {
                Commands.HandleCommand(client, message);
                continue;
            }

            SendMessage(message);
        }
    }

    public static void SendMessage(string message)
    {
        NetworkStream clientStream = client.GetStream();
        byte[] buffer;
        if (message.StartsWith("[Disconnect]") || message.StartsWith("[Command]"))
        {
            buffer = encoder.GetBytes(message);
        }
        else
        {
            buffer = encoder.GetBytes("[Send]" + message);
        }

        clientStream.Write(buffer, 0, buffer.Length);
        clientStream.Flush();
    }

    public static void HandleResponse(ResponseCode code)
    {
        switch (code)
        {
            case ResponseCode.Success:
                return;
            case ResponseCode.ServerError:
                Output.Message(ConsoleColor.DarkRed, "The server could not process your message. (100)");
                break;
            case ResponseCode.NoDateFound:
                Output.Message(ConsoleColor.DarkRed, "Could not retrieve messages from the server. (200)");
                break;
            case ResponseCode.BadDateFormat:
                Output.Message(ConsoleColor.DarkRed, "Could not retrieve messages from the server. (201)");
                break;
            case ResponseCode.NoMessageFound:
                Output.Message(ConsoleColor.DarkRed, "The server could not process your message. (300)");
                break;
            case ResponseCode.NoHandlingProtocol:
                Output.Message(ConsoleColor.DarkRed, "The server could not process your message. (400)");
                break;
            case ResponseCode.NoCode:
                Output.Message(ConsoleColor.DarkRed, "Could not process the server's response. (NoCode)");
                break;
            default:
                return;
        }
    }

    public static void ParseClientId(string id)
    {
        clientId = Int32.Parse(id);
    }

    public static void Disconnect()
    {
        SendMessage("[Disconnect]");
        Commands.EndRcvThread = true;
        Output.Debug("Requested receive thread termination.");
        Output.Message(ConsoleColor.DarkGreen, "Shutting down...");
    }

}