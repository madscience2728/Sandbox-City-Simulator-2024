namespace CSLox;

using CSLox.Parsing;
using CSLox.Scanning;

/*


source → SCANNER → tokens → PARSER → expressions → INTERPRETER → output

*/

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(DateTime.Now);

        TestInterpreter(Source.source);
        //TestParser(Source.source);
        // TestScanner(Source.source);
        // TestAstPrinter();
    }
    
    //| TESTER
    static void TestInterpreter(string source)
    {
        Console.WriteLine("[                                      ]");
        Console.WriteLine("[    <<<    INTERPRETER TEST    >>>    ]");
        Console.WriteLine("[                                      ]");
        Console.WriteLine();

        Error.Reset();
        //
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();
        //
        Parser parser = new Parser(tokens);
        Expression? expression = parser.ParseSingle();

        if (Error.hadError)
        {
            Console.WriteLine("Error parsing.");
        }

        if (expression != null)
        {
            Console.WriteLine("[SOURCE]");
            Console.WriteLine(source);
            Console.WriteLine();
            
            Console.WriteLine("[TOKENS]");
            foreach (Token token in tokens) Console.WriteLine(token);
            Console.WriteLine();

            Console.WriteLine("[AST]");
            Console.WriteLine(new AstPrinter().Print(expression));
            Console.WriteLine();
            
            Console.WriteLine("[INTERPRETER RESULT]");
            new Interpreter().Interpret(expression);
            Console.WriteLine();
        }
    }

    //| TESTER
    static void TestParser(string source)
    {
        Error.Reset();
        //
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();
        foreach (Token token in tokens) Console.WriteLine(token);
        //
        Parser parser = new Parser(tokens);
        Expression? expression = parser.ParseSingle();

        if (Error.hadError)
        {
            Console.WriteLine("Error parsing.");
        }

        if (expression != null)
        {
            Console.WriteLine(new AstPrinter().Print(expression));
        }
    }

    //| TESTER
    static void TestAstPrinter()
    {
        Expression expression = new Expression.Binary(
            new Expression.Unary(new Token(TokenType.MINUS, "-", null, 1), new Expression.Literal(123)),
            new Token(TokenType.STAR, "*", null, 1),
            new Expression.Grouping(new Expression.Literal(45.67))
        );

        Console.WriteLine(new AstPrinter().Print(expression));
    }

    //| TESTER
    static void TestScanner(string source)
    {
        Scanner scanner = new Scanner(source);
        Error.Reset();

        List<Token> tokens = scanner.ScanTokens();

        // For now, just print the tokens.
        foreach (Token token in tokens)
        {
            Console.WriteLine(token);
        }

        if (Error.hadError)
        {
            Console.WriteLine("Error parsing.");
        }
    }

    //| HELPER
    public static string Substring(string source, int start, int end)
    {
        return source.Substring(start, end - start);
    }
}
