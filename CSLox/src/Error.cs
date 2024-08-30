namespace CSLox;

internal class Error
{
    public class ParseError : Exception { }
    public class UnreachableCodeWasReachedError : Exception { }
    public class RuntimeError : Exception { }

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