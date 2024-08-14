namespace Sandbox_City_Simulator_2024;

using Network.Core;

public abstract class AbstractCreature : Packet
{
    Stats stats;  

    public AbstractCreature(string name, Stats stats)
    {
        Name = name;
        this.stats = stats;
    }
}

