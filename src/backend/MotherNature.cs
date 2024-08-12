namespace Sandbox_City_Simulator_2024;

using Network.Core;
using Sandbox_City_Simulator_2024.PrintTools;

public class MotherNature
{
    Chance fireChance = new(Chance.OncePer6Hours);

    public MotherNature()
    {
        //Network.AddNode(router);
        Network.AddListener(Step);
    }

    public void Step()
    {
        if (fireChance.NotRoll()) return;

        Print.Cache("The risk of fire is upon us...", ConsoleColor.Yellow);
        
        // Send a fire packet to a random node from a random node
        var source = Network.GetRandomHost()!;
        var destination = Network.GetRandomNode()!;
        Network.Send(new FirePacket
        {
            Name = "Fire Packet",
            Source = source.Name,
            Destination = destination.Name,
            LastHop = source.Name,
            NextHop = source.DefaultGateway
            //traceRoute = true
        });
    }
}