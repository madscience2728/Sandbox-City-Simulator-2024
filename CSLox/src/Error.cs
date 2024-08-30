namespace CSLox;

internal class Error
{
    public class ParseError : Exception { }
    
    public static bool hadError = false;

    public static void Reset()
    {
        hadError = false;
    }

    public static void Report(int line, string message)
    {
        hadError = true;
        Console.Error.WriteLine($"[line {line}]: {message}");
    }
}