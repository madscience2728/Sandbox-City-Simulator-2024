namespace Sandbox_City_Simulator_2024;
using Network.Core;

public class Person : AbstractCreature
{
    public Person() : base(NameGenerator.GenerateName(), new Stats())
    {
        
    }
    
    public Person(string name) : base(name, new Stats())
    {
        
    }
    
    public Person(Stats stats) : base(NameGenerator.GenerateName(), stats)
    {
        
    }
    
    public Person(string name, Stats stats) : base(name, stats)
    {
        
    }
    
    
}