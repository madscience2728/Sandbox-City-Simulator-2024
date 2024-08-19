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
            Delimiter,
            Unknown,
            Ignored
        }

        public string Value { get; set; }
        public TokenType Type { get; set; }
        public int SourceLineNumber { get; set; }

        public Token(string value, TokenType type, int lineNumber)
        {
            Value = value;
            Type = type;
            SourceLineNumber = lineNumber;
        }
    }
}
