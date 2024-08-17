namespace Sandbox_Simulator_2024;

using Network.Core;
using Sandbox_Simulator_2024.Scripting;

public class Parser
{
    public void Parse(string script)
    {
        var tokens = Tokenizer.Tokenize(script);
        Console.WriteLine("Found " + tokens.Count + " tokens");
        
        // Collect next line by injesting tokens until a delimiter token is found
        var lineTokens = new List<Token>();
        foreach (var token in tokens)
        {
            if (token.Type == Token.TokenType.Delimiter)
            {
                lineTokens.Add(token);
                ParseLine(lineTokens);
                lineTokens.Clear();
            }
            else
            {
                lineTokens.Add(token);
            }
        }
    }

    public void ParseLine(IEnumerable<Token> tokens)
    {
        foreach (var token in tokens)
        {
            bool breakFlag = false;
            bool errorFlag = false;
            Console.BackgroundColor = ConsoleColor.Black;
            
            switch (token.Type)
            {
                case Token.TokenType.Delimiter:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{token.Value}");
                    breakFlag = true; // should be last token anyway, so this is redudndant
                    break;
                case Token.TokenType.Keyword:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    if(ProcessKeyword(token, tokens)) breakFlag = true;
                    break;
                case Token.TokenType.Identifier:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    if(ProcessIdentifier(token, tokens)) breakFlag = true;
                    break;
                case Token.TokenType.Operator:
                    Console.ForegroundColor = ConsoleColor.White;
                    if(ProcessOperator(token, tokens)) breakFlag = true;
                    break;
                case Token.TokenType.Literal:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if(ProcessLiteral(token, tokens)) breakFlag = true;
                    break;
                case Token.TokenType.String:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    if(ProcessString(token, tokens)) breakFlag = true;
                    break;
                case Token.TokenType.Comment:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"{token.Value}");
                    break;
                case Token.TokenType.Whitespace:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{token.Value}");
                    break;
                case Token.TokenType.NewLine:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    break;
                case Token.TokenType.Unknown:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    breakFlag = true;
                    errorFlag = true;
                    break;
                case Token.TokenType.Ignored:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"{token.Value}");
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (breakFlag) break;
            if (errorFlag) throw new Exception("Error parsing unknown token: " + token.Value);
        }
    }
    
    private bool ProcessKeyword(Token token, IEnumerable<Token> tokens)
    {
        Console.Write($"{token.Value}");
        return false;
    }
    
    private bool ProcessIdentifier(Token token, IEnumerable<Token> tokens)
    {
        Console.Write($"{token.Value}");
        return false;
    }
    
    private bool ProcessOperator(Token token, IEnumerable<Token> tokens)
    {
        Console.Write($"{token.Value}");
        return false;
    }
    
    private bool ProcessLiteral(Token token, IEnumerable<Token> tokens)
    {
        Console.Write($"{token.Value}");
        return false;
    }

    private bool ProcessString(Token token, IEnumerable<Token> tokens)
    {
        Console.Write($"{token.Value}");
        return false;
    }
}