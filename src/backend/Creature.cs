namespace Sandbox_City_Simulator_2024;

using Network.Core;

public class Creature : AbstractGameHost
{
    Stats stats;  

    public Creature(string name, string defaultGateway, Stats stats) : base(name, defaultGateway)
    {
        Name = name;
        this.stats = stats;
    }
}

