namespace Sandbox_Simulator_2024.Scripting;
using Sandbox_Simulator_2024.Scripting.Parsing;

public class ScriptInterpreter
{
    Parser parser = new Parser();
    
    public ScriptInterpreter(string script)
    {
        //DebugPrintTokenized(script);
        parser.Parse(script);
    }    
}