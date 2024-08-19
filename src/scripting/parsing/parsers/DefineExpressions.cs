namespace Sandbox_Simulator_2024.Scripting.Parsing.Parsers;
using System.Collections.Generic;

public class DefineExpression : IParseStuff
{
    public ParseResult Parse(IEnumerable<Token> tokens, ScriptInterpreter scriptInterpreter)
    {
        //>> Check counts
        if (tokens.Count() != 3) return new ParseResult(ParseResult.State.Skip, "");
        
        //>> Pull tokens 
        Token firstToken = tokens.First();
        Token secondToken = tokens.Skip(1).First();
        Token thirdToken = tokens.Skip(2).First();

        //>> Check types
        if (firstToken.Type != Token.TokenType.Identifier
            &&
            secondToken.Type != Token.TokenType.Keyword
            &&
            (thirdToken.Type != Token.TokenType.Keyword || thirdToken.Type != Token.TokenType.Identifier)
            ) return new ParseResult(ParseResult.State.Skip, "");
            
        //>> Check second token
        if (secondToken.Value != "is") return new ParseResult(ParseResult.State.Skip, "Expected 'is' keyword", (tokens, secondToken));

        //>> Evaluate
        // Not interfaces ahave their own IParseStuff
        switch (thirdToken.Value)
        {
            case "router": return scriptInterpreter.RegisterIdentifier(ScriptInterpreter.ScriptableType.Router, firstToken.Value);
            case "host": return scriptInterpreter.RegisterIdentifier(ScriptInterpreter.ScriptableType.Host, firstToken.Value);
            case "list": return scriptInterpreter.RegisterIdentifier(ScriptInterpreter.ScriptableType.List, firstToken.Value);
            case "packet": return scriptInterpreter.RegisterIdentifier(ScriptInterpreter.ScriptableType.Packet, firstToken.Value);
        }
        
        //>> Special case for derived identifiers
        if (thirdToken.Type == Token.TokenType.Identifier) return scriptInterpreter.RegisterIdentifier(ScriptInterpreter.ScriptableType.Identifier, firstToken.Value);
        


        return new ParseResult(ParseResult.State.Success, "Successfully defined " + firstToken.Value + " as a " + thirdToken.Value);
    }
}
