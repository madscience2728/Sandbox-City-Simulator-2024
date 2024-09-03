namespace CSLox;

internal class Error
{
    public abstract class BaseException : Exception
    {
        public BaseException() : base() { }
        public BaseException(Token token, string message) : base(message) { }
    }

    public class ParseError : BaseException
    {
        public ParseError(Token token, string message) : base(token, message) { }
    }
    
    public class UnreachableCodeWasReachedError : BaseException
    {
        public UnreachableCodeWasReachedError() : base() { }
    }
    
    public class RuntimeError : BaseException
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
    
    public static void Report(BaseException error)
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