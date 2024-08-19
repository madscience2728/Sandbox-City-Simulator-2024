namespace Sandbox_Simulator_2024.Scripting.Scriptables;
using Network.Core;
using Identifier = string;

public class ScriptableHost : Host, IScriptable
{
    public Identifier identifier { get; private set; }
    
    public ScriptableHost(Identifier name, Identifier defaultGateway) : base(name, defaultGateway)
    {
        identifier = name;
    }
}