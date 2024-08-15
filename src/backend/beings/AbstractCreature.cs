namespace Sandbox_City_Simulator_2024;

using Network.Core;

public abstract class AbstractCreature : Packet, IHasHome, IHasStats
{
    public string Home { get; set; }
    public Stats stats { get; set; }
    

    public AbstractCreature(string name, string home, Stats stats)
    {
        Name = name;
        Home = home;
        this.stats = stats;
    }
}

