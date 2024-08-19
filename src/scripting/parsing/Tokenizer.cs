namespace Sandbox_Simulator_2024.Scripting;

using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Tokenizer
{
    private static readonly Dictionary<string, Token.TokenType> keywords = new Dictionary<string, Token.TokenType>
        {
            // Delimiters
            { ".", Token.TokenType.Delimiter },
            //
            //
            // Keywords
            { "define", Token.TokenType.Keyword },
            { "is", Token.TokenType.Keyword },
            { "has", Token.TokenType.Keyword },
            { "new", Token.TokenType.Keyword },
            { "if", Token.TokenType.Keyword },
            { "then", Token.TokenType.Keyword },
            { "and", Token.TokenType.Keyword },
            { "or", Token.TokenType.Keyword },
            { "not", Token.TokenType.Keyword },
            { "random", Token.TokenType.Keyword },
            { "action", Token.TokenType.Keyword },
            { "includes", Token.TokenType.Keyword },
            { "interface", Token.TokenType.Keyword },
            { "interfaces", Token.TokenType.Keyword },
            { "list", Token.TokenType.Keyword },
            { "router", Token.TokenType.Keyword },
            { "host", Token.TokenType.Keyword },
            { "print", Token.TokenType.Keyword },
            { "red", Token.TokenType.Keyword },
            { "green", Token.TokenType.Keyword },
            { "blue", Token.TokenType.Keyword },
            { "cyan", Token.TokenType.Keyword },
            { "yellow", Token.TokenType.Keyword },
            { "magenta", Token.TokenType.Keyword },
            { "white", Token.TokenType.Keyword },
            { "black", Token.TokenType.Keyword },
            { "onStep", Token.TokenType.Keyword },
            { "onReceivePacket", Token.TokenType.Keyword },
            { "packet", Token.TokenType.Keyword},
            { "create", Token.TokenType.Keyword },
            { "many", Token.TokenType.Keyword },
            { "true", Token.TokenType.Keyword },
            { "false", Token.TokenType.Keyword },
            { "from", Token.TokenType.Keyword },
            { ">", Token.TokenType.Keyword },
            { "<", Token.TokenType.Keyword },
            { "send", Token.TokenType.Keyword },
            { "hubAndSpoke", Token.TokenType.Keyword },
            { "tree", Token.TokenType.Keyword },
            { "connections", Token.TokenType.Keyword },
            { "for", Token.TokenType.Keyword },
            { "bool", Token.TokenType.Keyword },
            { "int", Token.TokenType.Keyword },
            { "string", Token.TokenType.Keyword },
            { "name", Token.TokenType.Keyword },
            { "roll", Token.TokenType.Keyword },
            { "set", Token.TokenType.Keyword },
            { ",", Token.TokenType.Keyword },
            { "implements", Token.TokenType.Keyword },
            { ":", Token.TokenType.Keyword },
            { "call", Token.TokenType.Keyword },
            { "take", Token.TokenType.Keyword },
            //
            //
            // Ignored
            { "to", Token.TokenType.Ignored },
            { "of", Token.TokenType.Ignored },
            { "type", Token.TokenType.Ignored },
            { "%", Token.TokenType.Ignored },
            { "on", Token.TokenType.Ignored },            
            { "chance", Token.TokenType.Ignored },
            { "children", Token.TokenType.Ignored },
            { "that", Token.TokenType.Ignored },
            { "an", Token.TokenType.Ignored },
            { "a", Token.TokenType.Ignored },
            { "with", Token.TokenType.Ignored },
            
            
            // Add other keywords here
        };

    private static readonly Regex identifierRegex = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*");
    private static readonly Regex numberRegex = new Regex(@"^\d+(\.\d+)?");
    private static readonly Regex stringRegex = new Regex("^\"[^\"]*\"");
    private static readonly Regex commentRegex = new Regex(@"^#.*");
    private static readonly Regex operatorRegex = new Regex(@"^[=+\-*/]");

    public static List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        int index = 0;
        int sourceLineNumber = 0;
        
        while (index < input.Length)
        {
            if (char.IsWhiteSpace(input[index]))
            {
                if (input[index] == '\n' || input[index] == '\r')
                {
                    tokens.Add(new Token("\n", Token.TokenType.NewLine, sourceLineNumber));
                    sourceLineNumber++;
                }
                else if (input[index] == '\t')
                {
                    tokens.Add(new Token(" ", Token.TokenType.Whitespace, sourceLineNumber));
                    tokens.Add(new Token(" ", Token.TokenType.Whitespace, sourceLineNumber));
                    tokens.Add(new Token(" ", Token.TokenType.Whitespace, sourceLineNumber));
                    tokens.Add(new Token(" ", Token.TokenType.Whitespace, sourceLineNumber));
                }
                else
                {
                    tokens.Add(new Token(input[index].ToString(), Token.TokenType.Whitespace, sourceLineNumber));
                }
                index++;
                continue;
            }

            string remainingInput = input.Substring(index);

            // Match keywords properly without splitting prematurely
            bool keywordMatched = false;
            foreach (var keyword in keywords.Keys)
            {
                if (remainingInput.StartsWith(keyword) &&
                    (remainingInput.Length == keyword.Length || !char.IsLetterOrDigit(remainingInput[keyword.Length])))
                {
                    tokens.Add(new Token(keyword, keywords[keyword], sourceLineNumber));
                    index += keyword.Length;
                    keywordMatched = true;
                    break;
                }
            }

            if (keywordMatched) continue;

            // Match comments
            if (commentRegex.IsMatch(remainingInput))
            {
                string comment = commentRegex.Match(remainingInput).Value;
                tokens.Add(new Token(comment, Token.TokenType.Comment, sourceLineNumber));
                index += comment.Length;
                continue;
            }

            // Match identifiers
            if (identifierRegex.IsMatch(remainingInput))
            {
                string identifier = identifierRegex.Match(remainingInput).Value;
                tokens.Add(new Token(identifier, Token.TokenType.Identifier, sourceLineNumber));
                index += identifier.Length;
                continue;
            }

            // Match numbers
            if (numberRegex.IsMatch(remainingInput))
            {
                string number = numberRegex.Match(remainingInput).Value;
                tokens.Add(new Token(number, Token.TokenType.Literal, sourceLineNumber));
                index += number.Length;
                continue;
            }

            // Match strings
            if (stringRegex.IsMatch(remainingInput))
            {
                string str = stringRegex.Match(remainingInput).Value;
                tokens.Add(new Token(str, Token.TokenType.String, sourceLineNumber));
                index += str.Length;
                continue;
            }

            // Match operators
            if (operatorRegex.IsMatch(remainingInput))
            {
                string op = operatorRegex.Match(remainingInput).Value;
                tokens.Add(new Token(op, Token.TokenType.Operator, sourceLineNumber));
                index += op.Length;
                continue;
            }

            tokens.Add(new Token(input[index].ToString(), Token.TokenType.Unknown, sourceLineNumber));
            index++;
        }

        return tokens;
    }

}