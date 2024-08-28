namespace Sandbox_Simulator_2024.Scripting.Scriptables;
using Network.Core;
using Identifier = string;

public class ScriptablePacket : Packet, IScriptable
{
    public Identifier identifier { get; private set; }

    public ScriptablePacket(Identifier name) : base(name)
    {
        identifier = name;
    }
}
