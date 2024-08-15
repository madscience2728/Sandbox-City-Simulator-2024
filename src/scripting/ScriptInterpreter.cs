namespace Sandbox_Simulator_2024.Scripting;

public class ScriptInterpreter
{
    Parser parser = new Parser();
    
    public ScriptInterpreter(string script)
    {
        PrintTokenized(script);
        parser.Parse(script);
    }

    public static void PrintTokenized(string script)
    {
        foreach (Token token in Tokenizer.Tokenize(script))
        {
            switch (token.Type)
            {
                case Token.TokenType.Keyword:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case Token.TokenType.Identifier:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case Token.TokenType.Operator:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case Token.TokenType.Literal:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case Token.TokenType.String:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case Token.TokenType.Comment:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case Token.TokenType.Whitespace:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case Token.TokenType.NewLine:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case Token.TokenType.SpecialCharacter:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case Token.TokenType.Unknown:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                    
                case Token.TokenType.Ignored:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }
            Console.Write(token.Value);
        }
    }
    
}