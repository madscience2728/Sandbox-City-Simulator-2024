namespace Sandbox_Simulator_2024.Scripting.Parsing.Parsers
{
    public class PrintExpression : IParseStuff
    {
        public ParseResult Parse(IEnumerable<Token> tokens)
        {
            foreach (var token in tokens)
            {
                Console.BackgroundColor = ConsoleColor.Black;

                switch (token.Type)
                {
                    case Token.TokenType.Delimiter: 
                        Console.WriteLine();
                        break;
                    case Token.TokenType.Keyword: 
                        Print(token, ConsoleColor.DarkBlue); 
                        break;
                    case Token.TokenType.Identifier: 
                        Print(token, ConsoleColor.DarkGreen); 
                        break;
                    case Token.TokenType.Operator: 
                        Print(token, ConsoleColor.White); 
                        break;
                    case Token.TokenType.Literal: 
                        Print(token, ConsoleColor.Yellow); 
                        break;
                    case Token.TokenType.String: 
                        Print(token, ConsoleColor.Magenta); 
                        break;
                    case Token.TokenType.Comment: 
                        Print(token, ConsoleColor.Gray); 
                        break;
                    case Token.TokenType.Whitespace: 
                        Print(token, ConsoleColor.White); 
                        break;
                    case Token.TokenType.NewLine: 
                        Console.WriteLine(); 
                        break;
                    case Token.TokenType.Ignored: 
                        Print(token, ConsoleColor.DarkGray); 
                        break;
                    case Token.TokenType.Unknown: 
                        Print("�", ConsoleColor.Red); 
                        break;
                    default: 
                        Print($"�{token.Value}�", ConsoleColor.Red); 
                        break;
                }
            }

            Console.ResetColor();
            return new ParseResult(true, "We printed all tokens :)");
        }

        static void Print(Token token, ConsoleColor color) => Print(token.Value, color);
        static void Print(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message + " ");
        }
    }
}
