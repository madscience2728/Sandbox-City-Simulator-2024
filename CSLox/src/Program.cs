namespace CSLox;

/*


source → SCANNER → tokens → PARSER → expressions → INTERPRETER → output

*/

class Program
{
    static void Main(string[] args)
    {
        Run(Source.source);
    }

    static void Run(string source)
    {
        Console.WriteLine("[                                       ]");
        Console.WriteLine("[    <<<    CSLOX INTERPRETER    >>>    ]");
        string dt = DateTime.Now.ToString();
        int dtl = dt.Length;
        string reff = "[                                       ]";
        int spacer = reff.Length - dtl - 2;
        int left_side = spacer / 2;
        int right_side = spacer - left_side ;
        string leftSpacingString = new string(' ', left_side - 1);
        string rightSpacingString = new string(' ', right_side - 1);
        Console.WriteLine("[ " + leftSpacingString + dt + rightSpacingString + " ]");
        Console.WriteLine("[                                       ]");
        Console.WriteLine();

        Error.Reset();
        //
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();
        //
        Parser parser = new Parser(tokens);
        List<Statement> statements = parser.Parse();

        if (Error.hadError)
        {
            Console.WriteLine("Error parsing.");
        }
        else
        {
            Interpreter interpreter = new Interpreter();
            interpreter.Interpret(statements);
        }
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

    //| HELPER
    public static string Stringify(object o)
    {
        if (o == null) return "nil";

        if (o is double)
        {
            string text = o.ToString()!;
            if (text.EndsWith(".0"))
            {
                text = Substring(text, 0, text.Length - 2);
            }
            return text;
        }

        return o.ToString()!;
    }
}
