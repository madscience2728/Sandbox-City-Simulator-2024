namespace Sandbox_Simulator_2024.Scripting.Parsing;

public class ParseResult
{
    public bool Success { get; }
    public string Message { get; }
    
    public ParseResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public ParseResult(bool success, string message, (IEnumerable<Token>, Token?) effected)
    {
        //>> Setup the method
        Success = success;
        Message = message;
        var expression = effected.Item1;
        var effectedToken = effected.Item2;

        //>> Print the error message line
        int count = expression.Count();
        Console.ForegroundColor = ConsoleColor.Red;
        if (count >= 1)
        {
            Token firstToken = expression.First();
            Token? lastToken = null;
            if (count >= 2) lastToken = expression.Last();

            string errorMessage = count == 1 || firstToken.SourceLineNumber == lastToken!.SourceLineNumber
                ? $"Error on line: {firstToken.SourceLineNumber}"
                : $"Error on lines: {firstToken.SourceLineNumber} to {lastToken.SourceLineNumber}";

            Console.WriteLine(errorMessage);
        }

        //>> Print the error message
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(Message);


        //>> Print the effected expression and/or token
        foreach (var token in expression)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (effectedToken != null && token == effectedToken)
                Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(token.Value + " ");
        }
        Console.WriteLine();
        
        //>> House cleaning
        Console.ResetColor();
    }
}
