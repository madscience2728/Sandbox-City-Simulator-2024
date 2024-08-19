using Sandbox_Simulator_2024.Scripting.Parsing.Parsers;

namespace Sandbox_Simulator_2024.Scripting.Parsing;

public class Parser
{
    static readonly Token.TokenType[] tokensToSkip = new Token.TokenType[] {
        Token.TokenType.NewLine,
        Token.TokenType.Comment,
        Token.TokenType.Whitespace,
        Token.TokenType.Ignored
    };

    static readonly List<IParseStuff> chainOfResponsibility = [
        new PrintExpression(),
        new ValidateExpression(),
    ];

    ScriptInterpreter ScriptInterpreter;
    
    public Parser(ScriptInterpreter scriptInterpreter)
    {
        ScriptInterpreter = scriptInterpreter;
    }

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
                currentResult = parser.Parse(expression, ScriptInterpreter);
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