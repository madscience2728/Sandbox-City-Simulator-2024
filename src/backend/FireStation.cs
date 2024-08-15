namespace Sandbox_City_Simulator_2024;

using Network.Core;
using Sandbox_City_Simulator_2024.PrintTools;

// TODO: Implement the FireStation class

public class FireStation : AbstractGameHost
{
    public FireStation() : base("No Name", "No Gateway")
    {
        
    }
    
    public FireStation(string name, string gateway) : base(name, gateway)
    {
        
    }
    
    protected override void YouveGotMail(Packet packet)
    {
        if(packet is FireRequestPacket)
        {
            Print.Cache($"{Name} received a request for help with a fire at {packet.Source}", ConsoleColor.Magenta);
            FireResponsePacket fireResponsePacket = new FireResponsePacket
            {
                Name = $"Fire Response Packet from {Name}",
                Source = Name,
                Destination = packet.Source,
                LastHop = Name,
                NextHop = packet.LastHop,
                //traceRoute = true
            };
            Network.Send(fireResponsePacket);
        }
    }
    
    public override void Step()
    {
        base.Step();
    }
}