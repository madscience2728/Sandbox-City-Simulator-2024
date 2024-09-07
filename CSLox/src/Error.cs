namespace CSLox;

internal class Error
{
    public abstract class BaseError : Exception
    {
        public Token? token = null;
        public BaseError() : base() { }
        public BaseError(Token token, string message) : base(message) { 
            this.token = token;
        }
    }

    public class ParseError : BaseError
    {
        public ParseError(Token token, string message) : base(token, message) { }
    }
    
    public class UnreachableCodeWasReachedError : BaseError
    {
        public UnreachableCodeWasReachedError() : base() { }
    }
    
    public class RuntimeError : BaseError
    {
        public RuntimeError(Token token, string message) : base(token, message)
        {
            this.token = token;
        }
    }
    
    public class CompileError : BaseError
    {
        public CompileError(Token token, string message) : base(token, message) { }
    }

    public static bool hadError = false;

    public static void Reset()
    {
        hadError = false;
    }
    
    public static void Report(BaseError error)
    {
        hadError = true;
        if (error.token != null)
        {
            Token token = error.token;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"[line {token.line} at token '{token.lexeme}']: {error.Message}");
            Console.ResetColor();
        }
        else Console.Error.WriteLine(error.Message);
    }

    public static void Report(int line, string message)
    {
        hadError = true;
        Console.Error.WriteLine($"[line {line}]: {message}");
    }
}