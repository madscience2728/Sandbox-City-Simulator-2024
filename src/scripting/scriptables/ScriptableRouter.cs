namespace Sandbox_Simulator_2024.Scripting.Scriptables;
using Network.Core;
using Identifier = string;

public class ScriptableRouter : Router, IScriptable
{
    public Identifier identifier { get; private set; }
    
    public ScriptableRouter(Identifier name) : base(name)
    {
        identifier = name;
    }
}