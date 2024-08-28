using System.Collections.Concurrent;

namespace Network.Core;

public class PacketTypeT : Packet
{
    public PacketTypeT()
    {
        
    }
    
    public PacketTypeT(string source, string destination) : base(source, destination)
    {
    }
}

public class PacketTypeK : Packet
{
    public PacketTypeK()
    {

    }

    public PacketTypeK(string source, string destination) : base(source, destination)
    {
    }
}

public class PacketTypeI : Packet, IUDP
{
    public PacketTypeI()
    {

    }

    public PacketTypeI(string source, string destination) : base(source, destination)
    {
    }
}

public class Test3 : IGenerateNetworks
{
    static int numDeliveries = 0;
    
    static HashSet<string> packetsLeft = new();
    
    public async Task GenerateNetwork()
    {        
        //>> CREATE
        
        Router ISP = (Router) Network.AddNode(new Router("ISP"));
        
        Node[] edgeRouters = new Node[10];
        
        for (int i = 0; i < edgeRouters.Length; i++)
        {

            Network.AddHubAndSpoke(
                out Router edgeRouter,
                out _,
                $"Main St {i}",
                $"Main St {i} Library", $"1st Main St {i}"
            );
            edgeRouters[i] = edgeRouter;

            Network.AddChain(
                out List<Router> longRoad,
                $"Long Rd Route Marker 0 {i}",
                $"Long Rd Route Marker 1 {i}",
                $"Long Rd Route Marker 2 {i}",
                $"Long Rd Route Marker 3 {i}"
            );

            Router head = longRoad[0];
            Router tail = longRoad[^1];

            Network.AddNode(new Host($"Crack House {i}", tail.Name));
            Network.AddLink(head.Name, edgeRouter.Name);
        }
        
        Network.InterconnectNodes(edgeRouters.Append(ISP));
        
        await Task.CompletedTask;
    }
    
    public static void Test()
    {
        Node[] samples = new Node[10];
        Random random = new Random();
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = Network.GetRandomNode();
        }

        int count = 0;
        var packetsToSend = new List<Packet>();
        packetsLeft.Clear();
        for (int i = 0; i < samples.Length; i += 2)
        {
            int indexA = i;
            int indexB = i + 1;
            string source = samples[indexA].Name;
            string destination = samples[indexB].Name;
            if (source == destination) continue;

            count++;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Sending packet from {source} to {destination}");
            Console.ResetColor();

            PacketTypeT packet = new PacketTypeT(source, destination);
            
            packet.Name = $"Test3 Packet {count}";
            packetsLeft.Add(packet.Name);      
            packetsToSend.Add(packet);      
        }

        foreach (Packet packet in packetsToSend)
        {
            Network.PushPacket(packet, 1, () =>
            {
                if (packetsLeft.Contains(packet.Name))
                {
                    Interlocked.Increment(ref numDeliveries);
                    packetsLeft.Remove(packet.Name);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Woot woot! {numDeliveries} packets delivered.");
                }
            });
        }

        Console.WriteLine($"Sent {count} packets");
    }
}

/*


After a long cycle of thinking I have fixed routing in my city network, I decided to create a separate project focused primarily on the routing aspect of the game. 
I based it on the RIP protocol, and ar now using host-router nodes instead of every stop being a router.

After 8-10 hours I have a reliable network.

Keep in mind all host and routers are nodes. 
Host receive and send packet to routers. 
Routers receive and send packets to other routers, as well as host.
Host only connect to routers, and routers connect to other routers and hosts.
Routers have a routing table, and they broadcast it to their neighbours when the network starts.
If you change the network, you have to call UpdateRoutingTables() to update the routing tables.
This may take time for really big networks.
It has however, been optimized to be as fast as possible using parallel programming.

When testing I simulated broken conditions such as a packet never making it, or an invalid hop.
In thoses cases I was successfully witnessed the Cheat() method being called.
Cheat() ensures a packet is delivered no matter what, keeping the network stable and protecting it from itself. 
It protects from large networks where packets can get lost in the shuffle.
It also protects from the network getting stuck in a loop, which is a common problem with routing.
And finally it protects from poor or inefficient network design, crucial for a proceural game.

I am very happy with the results thus far.
*/