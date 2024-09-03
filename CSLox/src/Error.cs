namespace CSLox;

internal class Error
{
    public abstract class StatementError : Exception
    {
        public StatementError() : base() { }
        public StatementError(Token token, string message) : base(message) { }
    }

    public class ParseError : StatementError
    {
        public ParseError(Token token, string message) : base(token, message) { }
    }
    
    public class UnreachableCodeWasReachedError : StatementError
    {
        public UnreachableCodeWasReachedError() : base() { }
    }
    
    public class RuntimeError : StatementError
    {
        public Token token;

        public RuntimeError(Token token, string message) : base(token, message)
        {
            this.token = token;
        }
    }

    public static bool hadError = false;

    public static void Reset()
    {
        hadError = false;
    }
    
    public static void Report(StatementError error)
    {
        hadError = true;
        if (error is RuntimeError runtimeError)
        {
            Token token = runtimeError.token;
            Console.Error.WriteLine($"[line {token.line}][token {token.lexeme}]: {runtimeError.Message}");
        }
        else Console.Error.WriteLine(error.Message);
    }

    public static void Report(int line, string message)
    {
        hadError = true;
        Console.Error.WriteLine($"[line {line}]: {message}");
    }
}