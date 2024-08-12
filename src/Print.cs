namespace Sandbox_City_Simulator_2024.PrintTools;

using System.Collections.Concurrent;
using Network.Core;


public static class Print
{
    struct DayPrint
    {
        public int time;
        public string message;
        public ConsoleColor color;
    }

#if !DEBUG
#pragma warning disable CS0414
    static bool fastPrint = false;
#pragma warning restore CS0414
#endif

    public static bool printFlag { get; private set; } = false;    
    static ConcurrentBag<DayPrint> dayMessages = new();

    public static void Pause(string action = "continue")
    {
        Line($"Press any key to {action}...");
        Console.ReadKey();
        Console.Clear();
    }

    public static void Line()
    {
        Console.WriteLine();
    }

    public static void Clear()
    {
        Console.Clear();
        ResetSkip();
    }

    public static void Line(object o)
    {
        ByWord(o);
    }

    public static void Meta(object o, bool appendTime = false)
    {
        if (appendTime) ByWord($"[{Game.GetTime()}]");
        ConsoleResetColor();

        Console.ForegroundColor = ConsoleColor.Green;
        ByWord(o);
        ConsoleResetColor();
        printFlag = true;
    }

    public static void PrintWithDelay() => PrintWithDelay("", 50);
    public static void PrintWithDelay(int ms) => PrintWithDelay("", ms);
    public static void PrintWithDelay(object o) => PrintWithDelay(o, 50);
    public static void PrintWithDelay(object o, int ms)
    {
        Console.WriteLine(o.ToString());
        Thread.Sleep(ms);
    }

    public static void ClearPrintFlag() => printFlag = false;
    public static void ResetSkip()
    {
        #if !DEBUG
        fastPrint = false;
        #endif
    }
    
    public static void CollectUpdateInt(string message, ref int currentValue)
    {
        int result = CollectInt(message, currentValue);
    }

    public static int CollectInt(string message, int defaultValue = 0)
    {
        Console.Write(message + $" (leave blank for {defaultValue}): ");
        string input = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(input))
        {
            return defaultValue;
        }
        else if (int.TryParse(input, out int result))
        {
            return result;
        }
        else
        {
            // Erase the current line and replace with invalid message
            Console.WriteLine("Invalid input.");
            return CollectInt(message);
        }
    }

    public static bool GiveOptionTo(string option)
    {
        ByWord($"Would you like to {option}? (y/n)");
        Console.Write(">> ");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Y)
            {
                Console.WriteLine();
                Pause();
                Clear();
                return true;
            }
            else
            if (key.Key == ConsoleKey.N)
            {
                Console.WriteLine();
                Pause();
                Clear();
                return false;
            }
            else
            {
                Console.Write("\b \b");
            }
        }
    }

    static void ByWord(object o, bool writeLine = true)
    {
#if DEBUG
        if (writeLine) Console.WriteLine(o);
        else Console.Write(o);
#else
        if (o == null) return;
        string? raw = o.ToString();
        if (raw == null) return;
        string[] words = raw.Split(' ');

        foreach (var word in words!)
        {
            int charTime = fastPrint ? 0 : 10 / word.Length;
            charTime += fastPrint ? 0 : word.Length;

            foreach (char c in word)
            {
                if (Console.KeyAvailable)
                {
                    fastPrint = true;
                    charTime = 0;
                    Console.ReadKey(true);
                }

                Console.Write(c);
                if (!fastPrint) Thread.Sleep(charTime);
            }
            Console.Write(' ');
            if (!fastPrint) Thread.Sleep(100);
        }
        if(writeLine) Console.WriteLine();
        Thread.Sleep(200);
#endif
    }


    public static void Cache(object? o, ConsoleColor color = ConsoleColor.White)
    {
        printFlag = true;
        dayMessages.Add(new DayPrint { message = o!.ToString()!, color = color, time = Network.tick % (24*60)});
    }
    
    public static bool PrintCache()
    {
        if(dayMessages.Count == 0) return false;
        
        // Sort by time
        dayMessages = new(dayMessages.OrderBy(x => x.time));
        
        foreach (var message in dayMessages)
        {
            
            DayInternal(message);
        }
        dayMessages.Clear();
        return true;
    }


    static void DayInternal(DayPrint message)
    {
        if (message.message == null) return;

        ConsoleResetColor();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"[{Game.GetTime(message.time)}] "); 
        Console.ForegroundColor = message.color;
        ByWord(message.message);
        ConsoleResetColor();
        printFlag = true;
    }

    public static void Immediate()
    {
        Console.WriteLine();
    }

    public static void Immediate(object o)
    {
        Console.WriteLine(o);
    }

    public static void Delay(int ms = 1000)
    {
        Thread.Sleep(ms);
    }

    public static void ConsoleResetColor()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void ClearLine()
    {
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 1);
    }
}