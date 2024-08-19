namespace Sandbox_Simulator_2024.Scripting.Scriptables;
using Network.Core;

public class ScriptableHost : Host, IScriptable
{
    public ScriptableHost(string name, string defaultGateway) : base(name, defaultGateway)
    {
        
    }
}