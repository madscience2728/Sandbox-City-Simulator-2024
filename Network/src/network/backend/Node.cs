
using System.Collections.Concurrent;

namespace Network.Core;

public abstract class Node : IName
{
    public string Name { get; set; } = "No Name";
    protected Random random = new();
    protected ConcurrentBag<Packet> packetCache = new();

    public abstract void Receive(Packet packet);
    public abstract void Transmit();

    public virtual void Step()
    {
        foreach(Packet packet in packetCache)
        {
            packet.Step();
        }
    }

    
}