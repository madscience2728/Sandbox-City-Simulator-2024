namespace Network.Core;

public class Host : Node
{
    public string DefaultGateway = "";

    public Host(string name)
    {
        Name = name;
    }
    
    public Host(string name, string defaultRouter)
    {
        Name = name;
        DefaultGateway = defaultRouter;
    }

    public override void Step()
    {
        base.Step();
    }

    public override void Receive(Packet packet)
    {
        Console.WriteLine($"Host {Name} received packet from {packet.LastHop} with source {packet.Source} and destination {packet.Destination}");
        
        if(packet.Destination == Name)
        {
            //>> Success
            Network.Deliver(packet);
        }
        else 
        {
            //>> Bounce back to the router
            //Console.WriteLine($"Host {Name} received wrong packet. Bouncing back to router {DefaultRouter}");
            packet.LastHop = Name;
            packet.NextHop = DefaultGateway;
            packetCache.Add(packet);
        }       
    }
    
    public override void Transmit()
    {
        foreach (Packet packet in packetCache)
        {
            packet.LastHop = Name;
            packet.NextHop = DefaultGateway;
            Network.PushPacket(packet, 1, () => { });
        }
        packetCache.Clear();
    }
    
    public Router GetRouter()
    {
        return (Router) Network.GetNode(DefaultGateway);
    }
}