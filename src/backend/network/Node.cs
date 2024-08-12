
using System.Collections.Concurrent;
using Sandbox_City_Simulator_2024.PrintTools;

namespace Network.Core;

public abstract class Node : IName
{
    public string Name { get; set; } = "No Name";
    protected Random random = new();

    protected ConcurrentBag<Packet> ingressPackets { get; } = new();
    protected ConcurrentBag<Packet> egressPackets { get; } = new();
    //protected ConcurrentBag<Packet> packetCache = new();

    public abstract void Receive(Packet packet);
    public abstract void Transmit();

    public virtual void Step()
    {
        foreach (Packet packet in ingressPackets)
        {
            egressPackets.Add(packet);
        }
        ingressPackets.Clear();
    }    
}