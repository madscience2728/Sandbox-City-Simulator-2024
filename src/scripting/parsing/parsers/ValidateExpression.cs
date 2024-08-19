
namespace Sandbox_Simulator_2024.Scripting.Parsing.Parsers;

public class ValidateExpression : IParseStuff
{
    public ParseResult Parse(IEnumerable<Token> tokens)
    {
        // Check for empty lines
        int Count = tokens.Count();
        if (Count == 0) return new ParseResult(false, "Empty line found. You may have a stray delimiter token (a period '.')");
        if (Count == 1) return new ParseResult(false, "Expected more than one token, only got: " + tokens.First().Value);
        if (Count == 2) return new ParseResult(false, "You seem to be missing a token or more. Expected more than two tokens, only got: " + tokens.First().Value + " and " + tokens.Last().Value);

        foreach (var token in tokens)
        {
            if (token.Type == Token.TokenType.Unknown)
                return new ParseResult(false, "Could not parse the tokens. Reason unknown.");
        }
        return new ParseResult(true, "All tokens are valid.");
    }
}

