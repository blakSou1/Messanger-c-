using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using MessengerClient;

class Commands
{
    public static volatile bool EndRcvThread = false;
    public static volatile bool RcvThreadEnded = false;
    public static bool ExitHandlingFinished = false;
    public static bool IsCommand(string command)
    {
        if (command.StartsWith("/"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void HandleCommand(TcpClient client, string command)
    {
        string[] args = command.Split(' ');
        switch (args[0].ToLower())
        {
            case "/server":

                if (args.Length >= 2)
                {
                    int startIndex = args[0].Length;
                    string commandArgs = command.Substring(startIndex + 1);
                    Client.SendMessage("[Command]" + commandArgs);
                }
                else
                {
                    Output.Message(ConsoleColor.DarkRed, "Not enough arguments");
                    return;
                }

                break;
            case "/exit":
                Client.Disconnect();
                break;
            default:
                Output.Message(ConsoleColor.DarkRed, "Unknown command.");
                return;
        }
    }

    public static void HandleResponse(string response)
    {
        // Command was sent; server did not recognise
        if (response == "[CommandInvalid]")
        {
            Output.Message(ConsoleColor.DarkRed, "The command was not recognised by the server.");
            return;
        }

        // Disconnect was sent; server acknowledges
        if (response == "[DisconnectAcknowledge]")
        {
            EndRcvThread = true;
            Output.Debug("Waiting for thread termination");
            while (!RcvThreadEnded)
            {
                Thread.Sleep(100);
            }
            Output.Debug("Thread terminated, cleaning send client");
            Client.SendMessage("");
            Client.client.Close();
            Output.Debug("Cleaned up send client");

            if (Output.DebugMode)
            {
                Console.WriteLine();
                Output.Debug("Press any key to exit");
                Console.ReadKey();
            }
            Environment.Exit(0);
        }

        ResponseCode code = ResponseCodes.GetResponse(response);
        Client.HandleResponse(code);
    }

}