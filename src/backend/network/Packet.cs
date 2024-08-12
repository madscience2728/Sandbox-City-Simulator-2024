namespace Network.Core;

public class Packet : IName
{
    
    public const int DEFAULT_TLL = 256;

    public Action onDelivered = new Action(() => { });

    public string Name { get; set;  } = "Nameless Packet";
    public string Source = string.Empty;
    public string LastHop = string.Empty;
    public string NextHop = string.Empty;
    public string Destination = string.Empty;
    public int TTL = DEFAULT_TLL;
    
    public bool traceRoute = false;
    
    public Packet() {
        SetTTL();
    }

    public Packet(string source, string destination)
    {
        Source = source;
        Destination = destination;
        LastHop = source;
        NextHop = source;
        SetTTL();
    }

    private void SetTTL()
    {
        TTL = Math.Max(DEFAULT_TLL, Network.GetNodeCount());
    }

    public void Step()
    {        
        if (traceRoute)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Trace Route: Packet {Name} is at {LastHop} and is going to {NextHop}");
            Console.ResetColor();
        }
        
        if (this is not IEntity)
        {
            TTL = Math.Max(TTL - 1, 0);
        }
    }
    
    public void Deliver()
    {
        onDelivered();
    }
}