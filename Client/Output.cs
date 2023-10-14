using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

enum LogType
{
    Info,
    Warn,
    Error
}

class Output
{
    public static bool DebugMode = false;

    public static void Log(string message, LogType type)
    {
        DateTime now = DateTime.Now;
        string date = now.ToShortDateString();
        string time = now.ToShortTimeString();
        switch (type)
        {
            case LogType.Info:
                Console.WriteLine("[" + time + " INFO] " + message);
                break;
            case LogType.Warn:
                Console.WriteLine("[" + time + " WARN] " + message);
                break;
            case LogType.Error:
                Console.WriteLine("[" + time + " ERROR] " + message);
                break;
        }
    }

    public static void Message(ConsoleColor color, string message)
    {
        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = originalColor;
    }

    public static void Message(string message)
    {
        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(message);
        Console.ForegroundColor = originalColor;
    }

    public static void Message(string message, bool lineBreak)
    {
        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        if (lineBreak)
        {
            Console.WriteLine(message);
        }
        else
        {
            Console.Write(message);
        }
        Console.ForegroundColor = originalColor;
    }

    public static void Write(ConsoleColor color, string message)
    {
        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ForegroundColor = originalColor;
    }

    public static void ClearLine()
    {
        int currentLineCursor = Console.CursorTop - 1;
        Console.SetCursorPosition(0, currentLineCursor);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }

    public static void Debug(string message)
    {
        if (DebugMode)
        {
            Console.WriteLine("<DEBUG> " + message);
        }
    }

}
