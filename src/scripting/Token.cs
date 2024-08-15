namespace Sandbox_Simulator_2024.Scripting
{
    public class Token
    {
        public enum TokenType
        {
            Keyword,
            Identifier,
            Operator,
            Literal,
            String,
            Comment,
            Whitespace,
            NewLine,
            SpecialCharacter,
            Unknown
        }

        public string Value { get; set; }
        public TokenType Type { get; set; }

        public Token(string value, TokenType type)
        {
            Value = value;
            Type = type;
        }
    }
}
