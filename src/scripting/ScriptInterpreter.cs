namespace Sandbox_Simulator_2024.Scripting;
using Sandbox_Simulator_2024.Scripting.Parsing;

public class ScriptInterpreter
{   
    public ScriptInterpreter(string script)
    {   
        Parser parser = new Parser(this);
        parser.Parse(script);
    }    
}