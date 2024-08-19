namespace Sandbox_Simulator_2024.Scripting.Parsing;

public interface IParseStuff
{
    ParseResult Parse(IEnumerable<Token> tokens);
}