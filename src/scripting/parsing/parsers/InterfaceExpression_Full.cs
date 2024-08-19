
using Sandbox_Simulator_2024.Scripting.Scriptables;

namespace Sandbox_Simulator_2024.Scripting.Parsing.Parsers;

public class InterfaceExpression_Full : IParseStuff
{
    public ParseResult Parse(IEnumerable<Token> tokens, ScriptInterpreter scriptInterpreter)
    {
        int count = tokens.Count();
        if(count < 6) return new ParseResult(ParseResult.State.Skip, "Expected at least 6 tokens (<identifier> is interface has <propertyType> <identifier>), but got: " + count, (tokens, null));
        
        var tokensArray = tokens.ToArray();
        Token firstToken = tokensArray[0];
        Token secondToken = tokensArray[1];
        Token thirdToken = tokensArray[2];
        Token fourthToken = tokensArray[3];
        

        //>> Make sure the first token is an identifier, 
        //>> the second token is the 'is' keyword, 
        //>> the third token is the 'interface' keyword, 
        //>> the fourth token is a colon, 
        //>> and the fifth token is an identifier [PASSED]
        bool match = true;
        match &= firstToken.Type == Token.TokenType.Identifier;
        match &= secondToken.Value == "is";
        match &= thirdToken.Value == "interface";
        match &= fourthToken.Value == "has";
        if (!match) return new ParseResult(ParseResult.State.Skip, "");
        
        scriptInterpreter.RegisterIdentifier(ScriptInterpreter.ScriptableType.Interface, firstToken.Value);
        ScriptableInterface scriptableInterface = new ScriptableInterface(firstToken.Value);
        
        //>> Parse the properties, injest two tokens at a time, until we hit a delimeter , or run out
        for(int i = 4; i < count; i += 2)
        {
            if(i + 1 >= count) return new ParseResult(ParseResult.State.Failure, "Expected a property type and an identifier, but ran out of tokens", (tokens, tokensArray[i]));
            Token propertyType = tokensArray[i];
            Token propertyName = tokensArray[i + 1];
            if(propertyType.Type != Token.TokenType.Keyword || propertyName.Type != Token.TokenType.Identifier) return new ParseResult(ParseResult.State.Failure, "Expected a property type and an identifier, but got: " + propertyType.Value + " and " + propertyName.Value, (tokens, tokensArray[i]));
            bool success = scriptableInterface.AddProperty(propertyType.Value, propertyName.Value);
            if(!success) return new ParseResult(ParseResult.State.Failure, "Failed to add property " + propertyName.Value + " of type " + propertyType.Value, (tokens, tokensArray[i]));
        }
        
        return new ParseResult(ParseResult.State.Success, "Hmmm.");
    }
}

