namespace Sandbox_Simulator_2024.Scripting;

using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Tokenizer
{
    private static readonly Dictionary<string, Token.TokenType> keywords = new Dictionary<string, Token.TokenType>
        {
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
            { "interface", Token.TokenType.Keyword },
            // Add other keywords here
        };

    private static readonly Regex identifierRegex = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*");
    private static readonly Regex numberRegex = new Regex(@"^\d+(\.\d+)?");
    private static readonly Regex stringRegex = new Regex("^\"[^\"]*\"");
    private static readonly Regex commentRegex = new Regex(@"^#.*");
    private static readonly Regex operatorRegex = new Regex(@"^[=+\-*/]");
    private static readonly Regex specialCharRegex = new Regex(@"^[\(\)\{\},]");

    public List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        int index = 0;

        while (index < input.Length)
        {
            if (char.IsWhiteSpace(input[index]))
            {
                if (input[index] == '\n')
                {
                    tokens.Add(new Token("\n", Token.TokenType.NewLine));
                }
                else
                {
                    tokens.Add(new Token(input[index].ToString(), Token.TokenType.Whitespace));
                }
                index++;
                continue;
            }

            string remainingInput = input.Substring(index);

            if (commentRegex.IsMatch(remainingInput))
            {
                string comment = commentRegex.Match(remainingInput).Value;
                tokens.Add(new Token(comment, Token.TokenType.Comment));
                index += comment.Length;
                continue;
            }

            if (keywords.TryGetValue(remainingInput.Split(' ')[0], out var keywordType))
            {
                string keyword = remainingInput.Split(' ')[0];
                tokens.Add(new Token(keyword, keywordType));
                index += keyword.Length;
                continue;
            }

            if (identifierRegex.IsMatch(remainingInput))
            {
                string identifier = identifierRegex.Match(remainingInput).Value;
                tokens.Add(new Token(identifier, Token.TokenType.Identifier));
                index += identifier.Length;
                continue;
            }

            if (numberRegex.IsMatch(remainingInput))
            {
                string number = numberRegex.Match(remainingInput).Value;
                tokens.Add(new Token(number, Token.TokenType.Literal));
                index += number.Length;
                continue;
            }

            if (stringRegex.IsMatch(remainingInput))
            {
                string str = stringRegex.Match(remainingInput).Value;
                tokens.Add(new Token(str, Token.TokenType.String));
                index += str.Length;
                continue;
            }

            if (operatorRegex.IsMatch(remainingInput))
            {
                string op = operatorRegex.Match(remainingInput).Value;
                tokens.Add(new Token(op, Token.TokenType.Operator));
                index += op.Length;
                continue;
            }

            if (specialCharRegex.IsMatch(remainingInput))
            {
                string specialChar = specialCharRegex.Match(remainingInput).Value;
                tokens.Add(new Token(specialChar, Token.TokenType.SpecialCharacter));
                index += specialChar.Length;
                continue;
            }

            tokens.Add(new Token(input[index].ToString(), Token.TokenType.Unknown));
            index++;
        }

        return tokens;
    }
}