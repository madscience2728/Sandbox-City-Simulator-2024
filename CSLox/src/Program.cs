namespace CSLox;

using CSLox.Parsing;
using CSLox.Scanning;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(DateTime.Now);
        
        TestParser(Source.source);
        // TestScanner(Source.source);
        // TestAstPrinter();
    }

    static void TestParser(string source)
    {
        Error.Reset();
        //
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();
        foreach(Token token in tokens) Console.WriteLine(token);
        //
        Parser parser = new Parser(tokens);
        Expression? expression = parser.Parse();

        if (Error.hadError)
        {
            Console.WriteLine("Error parsing.");
        }

        if (expression != null)
        {
            Console.WriteLine(new AstPrinter().Print(expression));
        }
    }

    static void TestAstPrinter()
    {
        Expression expression = new Expression.Binary(
            new Expression.Unary(new Token(TokenType.MINUS, "-", null, 1), new Expression.Literal(123)),
            new Token(TokenType.STAR, "*", null, 1),
            new Expression.Grouping(new Expression.Literal(45.67))
        );

        Console.WriteLine(new AstPrinter().Print(expression));
    }

    static void TestScanner(string source)
    {
        Scanner scanner = new Scanner(source);
        Error.Reset();
        
        List<Token> tokens = scanner.ScanTokens();

        // For now, just print the tokens.
        foreach(Token token in tokens)
        {
            Console.WriteLine(token);
        }
        
        if(Error.hadError)
        {
            Console.WriteLine("Error parsing.");
        }
    }
}
