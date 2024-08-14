namespace Sandbox_City_Simulator_2024;
using Network.Core;

public class Person : AbstractCreature
{
    public Person(string home) : base(NameGenerator.GenerateName(), home, new Stats())
    {
        
    }
    
    public Person(string home, string name) : base(name, home, new Stats())
    {
        
    }
    
    public Person(string home, Stats stats) : base(NameGenerator.GenerateName(), home, stats)
    {
        
    }
    
    public Person(string home, string name, Stats stats) : base(name, home, stats)
    {
        
    }
}