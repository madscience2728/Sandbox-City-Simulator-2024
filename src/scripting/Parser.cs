namespace Sandbox_Simulator_2024;

using Network.Core;
using Sandbox_Simulator_2024.Scripting;

public class Parser
{
    public class LineOfCode
    {
        public int lineNumber = 0;
        public List<Token> tokens = new();
    }

    public void Parse(string script)
    {   
        var tokens = Tokenizer.Tokenize(script);
        var lines = new List<LineOfCode>();
        var currentLine = new LineOfCode(); 
        foreach (Token token in tokens)
        {
            switch (token.Type)
            {
                case Token.TokenType.NewLine:
                    currentLine.lineNumber = lines.Count;
                    lines.Add(currentLine);
                    currentLine = new LineOfCode();
                    break;
                default:
                    currentLine.tokens.Append(token);
                    break;
            }
        }

        // Now process each line
        foreach (var line in lines)
        {
            ParseLine(line);
        }
    }
    
    public void ParseLine(LineOfCode line)
    {
        foreach (Token token in line.tokens)
        {
            
        }
    }
}