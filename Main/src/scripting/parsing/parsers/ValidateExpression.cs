
namespace Sandbox_Simulator_2024.Scripting.Parsing.Parsers;

public class ValidateExpression : IParseStuff
{
    public ParseResult Parse(IEnumerable<Token> tokens, ScriptInterpreter scriptInterpreter)
    {
        //>> Check for empty or short lines [PASSED]
        int Count = tokens.Count();
        if (Count == 0) return new ParseResult(ParseResult.State.Failure, "Empty line found. You may have a stray delimiter token (a period '.')", (tokens, null));
        if (Count == 1) return new ParseResult(ParseResult.State.Failure, "Expected more than one token, only got: " + tokens.First().Value, (tokens, null));
        if (Count == 2) return new ParseResult(ParseResult.State.Failure, "You seem to be missing a token or more. Expected more than two tokens, only got: " + tokens.First().Value + " and " + tokens.Skip(1).First().Value, (tokens, null));

        //>> Check for unknown tokens [PASSED]
        foreach (var token in tokens)
        {
            if (token.Type == Token.TokenType.Unknown)
                return new ParseResult(ParseResult.State.Failure, $"Could not parse the tokens. Token of unknown type found. Token is {token.Value}.", (tokens, token));
        }
        
        //>> Check for non-identifiers in the first token [PASSED]
        if (tokens.First().Type != Token.TokenType.Identifier)
            return new ParseResult(ParseResult.State.Failure, "Expected an identifier as the first token, but got: " + tokens.First().Value, (tokens, tokens.First()));
            
        //>> Check for non-keywords in the second token [PASSED]
        if (tokens.Skip(1).First().Type != Token.TokenType.Keyword)
            return new ParseResult(ParseResult.State.Failure, "Expected a keyword as the second token, but got: " + tokens.Skip(1).First().Value, (tokens, tokens.Skip(1).First()));
        
        return new ParseResult(ParseResult.State.Success, "All tokens are valid.");
    }
}

