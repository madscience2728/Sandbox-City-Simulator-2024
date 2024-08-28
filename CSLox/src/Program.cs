namespace CSLox;

class Program
{
    static void Main(string[] args)
    {
        Run(Source.source);
    }

    static void Run(String source)
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
