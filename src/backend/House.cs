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

    public override void Step()
    {
        base.Step();

        if (welcomeMessage) return;
        welcomeMessage = true;

        //>> Generate a packet
        Packet packet = new Packet
        {
            Name = "Sexy Test Packet",
            Source = Name,
            Destination = Network.GetRandomNode()!.Name,
            LastHop = Name,
            NextHop = DefaultGateway,
            //traceRoute = true
        };
        Console.ForegroundColor = ConsoleColor.Cyan;
        //Print.Line($"House {Name} sending packet to {packet.Destination}");
        Console.ResetColor();
        egressPackets.Add(packet);
    }
}