namespace Sandbox_City_Simulator_2024;

using Network.Core;
using Sandbox_City_Simulator_2024.PrintTools;

public class House : AbstractBuilding
{    
    public House() : base("No Name", "No Gateway")
    {
        
    }
    
    public House(string name, string gateway) : base(name, gateway)
    {
        
    }
    
    bool welcomeMessage = false;

    protected override void YouveGotMail(Packet packet)
    {
        base.YouveGotMail(packet);
        if(packet == null) return;
        
        if(packet is Person person)
        {
            people.Add(person);
        }
    }

    public override void Step()
    {
        base.Step();

        if (welcomeMessage) return;
        welcomeMessage = true;
    }
}