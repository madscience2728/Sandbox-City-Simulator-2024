
namespace Network.Core;

public class Host : Node
{
    public string DefaultGateway = "";
    
    public Host(string name, string defaultRouter)
    {
        Name = name;
        DefaultGateway = defaultRouter;
    }

    public override void Receive(Packet packet)
    {
        //Console.WriteLine($"Host {Name} received packet from {packet.LastHop} with source {packet.Source} and destination {packet.Destination}");
        
        if(packet.Destination == Name)
        {
            //>> Success
            Network.Delivered(packet);
            YouveGotMail(packet);
        }
        else 
        {
            //>> Bounce back to the router
            //Console.WriteLine($"Host {Name} received wrong packet. Bouncing back to router {DefaultRouter}");
            packet.LastHop = Name;
            packet.NextHop = DefaultGateway;
            ingressPackets.Add(packet);
        }       
    }
    
    protected virtual void YouveGotMail(Packet packet)
    {
    
    }
    
    public override void Step()
    {
        base.Step();
    }

    public override void Transmit()
    {
        foreach (Packet packet in egressPackets)
        {
            packet.LastHop = Name;
            packet.NextHop = DefaultGateway;
            Network.Send(packet);
        }
        egressPackets.Clear();
    }
    
    public Router GetRouter()
    {
        return (Router) Network.GetNode(DefaultGateway);
    }

    public override IEnumerable<T> ReportPackets<T>()
    {
        return Array.Empty<T>();
    }
}