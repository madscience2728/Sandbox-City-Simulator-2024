using System.Collections.Concurrent;

namespace Network.Core;

public class Router : Node
{
    public struct RoutingEntry
    {
        public string NextHop;
        public int Distance;

        public RoutingEntry(string nextHop, int distance)
        {
            NextHop = nextHop;
            Distance = distance;
        }
    }

    ConcurrentBag<Packet> ingressPackets { get; } = new();
    ConcurrentBag<Packet> egressPackets { get; } = new();
    ConcurrentDictionary<string, RoutingEntry> routingTable = new();

    public Router(string name)
    {
        Name = name;
    }

    public void CollectNeighbourRoutes()
    {
        var neighbours = Network.GetNeighbours(Name);
        foreach (var neighbour in neighbours)
        {
            //Console.WriteLine($"{Name} is adding inital route to {neighbour}");
            routingTable[neighbour] = new RoutingEntry(neighbour, 1);
        }
    }

    public override void Receive(Packet packet)
    {
        //Console.WriteLine($"{Name} received packet from last hop {packet.LastHop}");
        if(packet.Destination == Name) // If the packet is for this router
        {
            Network.Deliver(packet);
            return;
        }
        else ingressPackets.Add(packet); // Route the packet
    }

    public override void Step()
    {
        base.Step();
        foreach (Packet packet in ingressPackets)
        {
            Route(packet);
            egressPackets.Add(packet);
        }
        ingressPackets.Clear();
    }

    public override void Transmit()
    {
        foreach (Packet packet in egressPackets)
        {
            Network.Send(packet);
        }
        egressPackets.Clear();
    }

    public void Route(Packet packet)
    {
        packet.Step();
        packet.LastHop = Name;
        bool chance = random.NextSingle() < 0.9;

        // Get a route from the routing table, but only 90% of the time. 10% of the time, choose a random route to midigate loops
        if (routingTable.TryGetValue(packet.Destination, out var entry) && chance)
        {
            //Console.WriteLine($"Route found. Routing packet to {packet.Destination} via {entry.NextHop}");
            packet.LastHop = Name;
            packet.NextHop = entry.NextHop;
        }
        else
        {
            if(chance) Console.WriteLine($"No route to {packet.Destination} from {Name} found, using random route");
            //else Console.WriteLine($"Loop evasion, using random route");
            var neighbours = Network.GetNeighbours(Name);
            if (neighbours.Count == 0) throw new ArgumentException("Router has no neighbours");
            packet.NextHop = neighbours[new Random().Next(neighbours.Count)];
        }
    }

    public bool BroadcastRoutingTable()
    {
        bool updated = false;
        //Console.WriteLine($"Broadcasting routing table from {Name}");
        var neighbours = Network.GetNeighbours(Name);
        Parallel.ForEach(neighbours, neighbour =>
        {
            var neighborNode = Network.GetNode(neighbour) as Router;
            if(neighborNode is null) return;
            
            updated |= neighborNode.UpdateRoutingTable(Name, routingTable);
        });
        return updated;
    }

    public bool UpdateRoutingTable(string neighbor, ConcurrentDictionary<string, RoutingEntry> neighborTable)
    {
        bool updated = false;
        foreach (var entry in neighborTable)
        {
            var destination = entry.Key;
            var neighborEntry = entry.Value;
            var newDistance = neighborEntry.Distance + 1;

            if (!routingTable.ContainsKey(destination) || routingTable[destination].Distance > newDistance)
            {
                //Console.WriteLine($"Updating route to {destination} via neighbour {neighbor}'s routing table");
                routingTable[destination] = new RoutingEntry(neighbor, newDistance);
                updated = true;
            }
        }
        return updated;
    }
}