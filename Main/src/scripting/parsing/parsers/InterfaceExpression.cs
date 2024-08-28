
using Sandbox_Simulator_2024.Scripting.Scriptables;

namespace Sandbox_Simulator_2024.Scripting.Parsing.Parsers;

public class InterfaceExpression : IParseStuff
{
    public ParseResult Parse(IEnumerable<Token> tokens, ScriptInterpreter scriptInterpreter)
    {
        int count = tokens.Count();
        bool match;

        if (count == 3)
        {
            //>> One-liner condition
            match = true;
            match &= tokens.First().Type == Token.TokenType.Identifier;
            match &= tokens.Skip(1).First().Value == "is";
            match &= tokens.Skip(2).First().Value == "interface";
            if (!match) return new ParseResult(ParseResult.State.Skip, "");
            
            //>> Register the interface
            scriptInterpreter.RegisterInterface(tokens.First().Value, new ScriptableInterface(tokens.First().Value));
            return new ParseResult(ParseResult.State.Success, "Registered interface " + tokens.First().Value);
        }


        if (count < 6) return new ParseResult(ParseResult.State.Skip, "Expected at least 6 tokens (<identifier> is interface has <propertyType> <identifier>), but got: " + count, (tokens, null));

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
        match = true;
        match &= firstToken.Type == Token.TokenType.Identifier;
        match &= secondToken.Value == "is";
        match &= thirdToken.Value == "interface";
        match &= fourthToken.Value == "has";
        if (!match) return new ParseResult(ParseResult.State.Skip, "");

        //>> Create a new ScriptableInterface
        ScriptableInterface scriptableInterface = new ScriptableInterface(firstToken.Value);

        //>> Parse the properties, injest two tokens at a time, until we run out, ther first token should be a property type, and the second should be an identifier
        for (int i = 4; i < count; i += 2)
        {
            if (i + 1 >= count) return new ParseResult(ParseResult.State.Failure, "Expected a property type and an identifier, but ran out of tokens", (tokens, tokensArray[i]));
            Token propertyType = tokensArray[i];
            Token propertyName = tokensArray[i + 1];
            if (propertyType.Type != Token.TokenType.Keyword || propertyName.Type != Token.TokenType.Identifier) return new ParseResult(ParseResult.State.Failure, "Expected a property type and an identifier, but got: " + propertyType.Value + " and " + propertyName.Value, (tokens, tokensArray[i]));
            bool success = scriptableInterface.AddProperty(propertyType.Value, propertyName.Value);
            scriptInterpreter.RegisterIdentifier(ScriptInterpreter.ScriptableType.Identifier, propertyName.Value);
            if (!success) return new ParseResult(ParseResult.State.Failure, "Failed to add property " + propertyName.Value + " of type " + propertyType.Value, (tokens, tokensArray[i]));
        }

        //>> Register the interface abd return
        scriptInterpreter.RegisterInterface(firstToken.Value, scriptableInterface);
        return new ParseResult(ParseResult.State.Success, "Hmmm.");
    }
}

