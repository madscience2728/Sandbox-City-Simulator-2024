namespace Sandbox_Simulator_2024.Scripting.Scriptables;
using Identifier = string;

public class ScriptableIdentifier : IScriptable
{
    public Identifier identifier { get; private set; }

    public ScriptableIdentifier(Identifier name)
    {
        identifier = name;
    }
}