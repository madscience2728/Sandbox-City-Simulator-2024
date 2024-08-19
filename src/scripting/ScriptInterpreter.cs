namespace Sandbox_Simulator_2024.Scripting;
using Sandbox_Simulator_2024.Scripting.Parsing;
using Sandbox_Simulator_2024.Scripting.Scriptables;
using Identifier = string;

public class ScriptInterpreter
{
    public enum ScriptableType
    {
        List,
        Host,
        Packet,
        Router,
        Interface,
        Identifier
    }

    List<Identifier> allIdentifiers = new();

    public ScriptInterpreter(string script)
    {
        Parser parser = new Parser(this);
        ParseResult parseResult = parser.Parse(script);
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine(parseResult.Message);
    }
    
    public ParseResult RegisterIdentifier(ScriptableType scriptableType, Identifier identifier)
    {
        // Check if the identifier exists
        if(allIdentifiers.Contains(identifier))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("(duplicate) ");
            return new ParseResult(ParseResult.State.Failure, "Identifier already exists: " + identifier);
        }
        
        allIdentifiers.Add(identifier);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("(registered) ");
        return new ParseResult(ParseResult.State.Success, "Registered " + scriptableType + " with identifier: " + identifier);
    }
}