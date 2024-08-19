using Sandbox_Simulator_2024.Scripting.Parsing.Parsers;

namespace Sandbox_Simulator_2024.Scripting.Parsing;

public class Parser
{
    // Dictionary<string, List<Token>> identifiers = new();
    // Dictionary<string, string> names = new();
    // Dictionary<string, List<string>> interfaces = new();
    // List<string> PacketIdentifiers = new();
    // List<string> InterfaceIdentifiers = new();
    // List<string> RouterIdentifiers = new();
    // List<string> HostIdentifiers = new();
    // List<string> ListIdentifiers = new();
    // List<string> IdentifierIdentifiers = new();
    // Dictionary<string, List<Node>> lists = new();
    // Dictionary<string, List<ScriptableRouter>> routers = new();
    // Dictionary<string, List<ScriptableHost>> hosts = new();
    
    static readonly Token.TokenType[] tokensToSkip = new Token.TokenType[] {
        Token.TokenType.NewLine,
        Token.TokenType.Comment,
        Token.TokenType.Whitespace,
        Token.TokenType.Ignored
    };

    List<IParseStuff> chainOfResponsibility = [
        new PrintExpression(),
        new ValidateExpression(),
    ];

    public ParseResult Parse(string script)
    {
        var tokens = Tokenizer.Tokenize(script);
        Console.ResetColor();
        Console.WriteLine("Parser found " + tokens.Count + " tokens");
        ParseResult currentResult = new ParseResult(true, "Default result is true");

        IterateExpressions(tokens, (expression) =>
        {
            foreach (var parser in chainOfResponsibility)
            {
                currentResult = parser.Parse(expression);
                if (!currentResult.Success)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Parsing failed]"); 
                    Console.WriteLine(currentResult.Message);
                    Console.ResetColor();
                    return false;
                }
            }
            return true;
        });
        Console.ResetColor(); // house cleaning
        return new ParseResult(true, "Parsing successful");
    }

    private void ParseExpression(List<Token> list)
    {
        throw new NotImplementedException();
    }

    public bool IterateExpressions(IEnumerable<Token> tokens, Func<List<Token>, bool> Parse)
    {
        var lineTokens = new List<Token>();
        foreach (var token in tokens)
        {
            if (token.Type == Token.TokenType.Delimiter)
            {
                lineTokens.Add(token);
                if(!Parse(lineTokens)) return false;
                lineTokens.Clear();
            }
            else if (tokensToSkip.Contains(token.Type)) continue;
            else lineTokens.Add(token);
        }
        return true;
    }
}