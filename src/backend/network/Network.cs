using System.Collections.Concurrent;
using System.Diagnostics;
using Sandbox_City_Simulator_2024.PrintTools;

namespace Network.Core;

public class Network
{
    

    private static ConcurrentDictionary<string, Node> nodes = new();
    private static ConcurrentDictionary<string, List<string>> links = new();

    public static bool isRunning { get; private set; } = false;
    public static bool debugText = false;

    public static int numPacketsSent { get; private set; } = 0;
    public static int numPacketsDelivered { get; private set; } = 0;

    public static int tick { get; private set; } = 0;
    public static int millisecondsPerTick { get; private set; } = 0;

    private static Action PostStep = () => { };

    //| START / STEP

    public static async Task Start(string networkName, int _millisecondsPerTick = 100)
    {
        millisecondsPerTick = _millisecondsPerTick;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Starting Network...");
        Console.ResetColor();

        // Collect all the routes from the routers neighbours
        foreach (var node in nodes.Values)
        {
            if (node is Router router) router.CollectNeighbourRoutes();
        }

        UpdateRoutingTables();

        isRunning = true;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Network Started");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Stay tuned...\n");
        Console.ResetColor();
        await Task.CompletedTask;

        

        new Thread(async () =>
        {
            while (isRunning)
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();

                //>>
                await Step();
                //<<

                
                const int hour = 60;
                const int day = 24 * hour;
                const int timeless = 1_000 * 365 * 24 * 60;
                const int printTimeRate = hour;
                
                int currentHour = tick / hour;
                int currentDay = tick / day;
                int hourOfTheDay = tick % day;
                string age = string.Empty;
                
                string AddS(int value, string s) => value == 1 ? $"{value} {s}" : $"{value} {s}s";
                
                age = tick switch
                {
                    0 => string.Empty,
                    
                    hour => "1 hour old",
                    
                    < day => $"{AddS(currentHour, "hour")} old",
                    
                    day => "1 day old",
                    
                    < timeless when hourOfTheDay is 0 => $"{AddS(currentDay, "day")} old",
                    
                    < timeless => $"{AddS(currentDay, "day")} and {AddS(hourOfTheDay, "hour")}",
                    
                    _ => "Timeless"
                };
                if (tick % printTimeRate == 0f && age != string.Empty) Print.Cache($"{networkName} is {age}");

                //>>
                PostStep();
                //<<

                stopwatch.Stop();

                int sleepTime = Math.Clamp(
                    millisecondsPerTick - (int)stopwatch.ElapsedMilliseconds,
                    0,
                    millisecondsPerTick
                );
                Thread.Sleep(sleepTime);
                tick++;
            }
        }).Start();
    }

    public static void AddListener(Action action)
    {
        PostStep += action;
    }

    public static void UpdateRoutingTables()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        // While there are still updates to be made, broadcast the routing tables
        int updated = 0;
        int counter = 0;
        do
        {
            updated = 0;
            counter++;
            Console.WriteLine($"Broadcasting routing tables x{counter}");
            Parallel.ForEach(nodes.Values, node =>
            {
                if (node is Router router)
                {
                    // Thread safe
                    if (router.BroadcastRoutingTable()) Interlocked.Exchange(ref updated, 1);
                }
            });
            if (counter >= nodes.Count) break; // Max possible updates, assumes a chain of routers
        } while (updated == 1);
        Console.WriteLine($"Routing tables updated in {counter} broadcasts and took {stopwatch.ElapsedMilliseconds / 1000f:F3} seconds\n");
    }

    public static void StepSerial()
    {
        CheckRunning();

        foreach (var node in nodes.Values) node.Step();
        foreach (var node in nodes.Values) node.Transmit();
    }

    public static async Task Step()
    {
        CheckRunning();

        await Task.WhenAll(nodes.Values.Select(node => Task.Run(() => node.Step())));
        await Task.WhenAll(nodes.Values.Select(node => Task.Run(() => node.Transmit())));
    }

    //| GET


    public static IEnumerable<T> GetNodes<T>() where T : Node => nodes.Values.OfType<T>();
    public static IEnumerable<Node> GetNodes() => nodes.Values;
    public static Node GetNode(string nodeName) => nodes[nodeName];
    public static List<string> GetNeighbours(string nodeName) => links[nodeName];
    public static int GetNodeCount() => nodes.Count;
    public static IEnumerable<T> GetNodes<T>(Func<T, bool> predicate) where T : Node => nodes.Values.OfType<T>().Where(predicate);
    public static IEnumerable<Node> GetNodes(Func<Node, bool> predicate) => nodes.Values.Where(predicate);

    public static Node? GetRandomNode()
    {
        // Not thread safe, count could be different by the time it is accessed
        // return nodes.Values.ElementAt(new Random().Next(0, nodes.Count));

        // Thread safe
        return nodes.Values.ToArray().OrderBy(x => Guid.NewGuid()).FirstOrDefault();
    }

    public static Host? GetRandomHost()
    {
        Node? n;
        int count = 0;
        do
        {
            count++;
            if (count > nodes.Count * 2) return null;
            n = GetRandomNode();
        } while (n is null || n is not Host);
        return (Host)n;
    }

    public static Router? GetRandomRouter()
    {
        Node? n;
        int count = 0;
        do
        {
            count++;
            if (count > nodes.Count * 2) return null;
            n = GetRandomNode();
        } while (n is null || n is not Router);
        return (Router)n;
    }

    public static T? GetRandomNode<T>() where T : Node
    {
        Node? n;
        int count = 0;
        do
        {
            count++;
            if (count > nodes.Count * 2) return null;
            n = GetRandomNode();
        } while (n is null || n is not T);
        return (T)n;
    }

    //|| ADD

    public static Node AddNode(Node node)
    {
        nodes.TryAdd(node.Name, node);
        //if (node is Host host) AddLink(host.Name, host.DefaultGateway);
        return node;
    }

    public static void AddNodes(IEnumerable<Node> newNodes)
    {
        foreach (Node node in newNodes)
        {
            if (!nodes.ContainsKey(node.Name)) AddNode(node);
        }
    }

    public static void AddLink(string source, string destination)
    {
        // Verify that the source and destination nodes exist and that the link is valid
        /*bool allowed = true;
        allowed &= nodes.ContainsKey(source);
        allowed &= nodes.ContainsKey(destination);
        if(!allowed) throw new ArgumentException("Invalid link");
        allowed &= (
            nodes[source] is Router && nodes[destination] is Router
            ||
            nodes[source] is Router && nodes[destination] is Host
            ||
            nodes[source] is Host && nodes[destination] is Router
        );
        if (!allowed) throw new ArgumentException("Invalid link");*/

        bool allowed = true;
        allowed &= nodes.ContainsKey(source);
        allowed &= nodes.ContainsKey(destination);
        if (!allowed) throw new ArgumentException($"Invalid link: one or both nodes do not exist {source} {destination}");
        var sourceNode = nodes[source];
        var destNode = nodes[destination];
        allowed &=
        (
            (sourceNode is Router || sourceNode.GetType().IsSubclassOf(typeof(Router))) &&
            (destNode is Router || destNode.GetType().IsSubclassOf(typeof(Router)))
            ||
            (sourceNode is Router || sourceNode.GetType().IsSubclassOf(typeof(Router))) &&
            (destNode is Host || destNode.GetType().IsSubclassOf(typeof(Host)))
            ||
            (sourceNode is Host || sourceNode.GetType().IsSubclassOf(typeof(Host))) &&
            (destNode is Router || destNode.GetType().IsSubclassOf(typeof(Router)))
        );

        // Make sure the nodes already have entries
        if (!links.ContainsKey(source)) links.TryAdd(source, new List<string>());
        if (!links.ContainsKey(destination)) links.TryAdd(destination, new List<string>());

        if (links[source].Contains(destination)) { } //Console.WriteLine("Link already exists");
        else links[source].Add(destination);
        if (links[destination].Contains(source)) { } //Console.WriteLine("Link already exists");
        else links[destination].Add(source);
    }

    //| Functions do many things at once

    public static void AddChain(params string[] routerNames) => AddChain(out _, routerNames);
    public static void AddChain(out List<Router> routers, params string[] routerNames)
    {
        routers = new List<Router>();
        for (int i = 0; i < routerNames.Length; i++)
        {
            string routerName = routerNames[i];
            Console.WriteLine($"Creating chain link router {routerName}");
            routers.Add((Router)AddNode(new Router(routerName)));
            if (i > 0) AddLink(routerNames[i - 1], routerName);
        }
    }

    public static void AddHubAndSpoke<T>(string routerName, params string[] hostNames) => AddHubAndSpoke<T>(out _, out _, routerName, hostNames);
    public static void AddHubAndSpoke<T>(out Router router, out List<Host> hosts, string routerName, params string[] hostNames)
    {
        // Create the hub
        Console.WriteLine($"Creating hub router {routerName}");
        router = (Router)AddNode(new Router(routerName));

        // Create the spokes
        hosts = new List<Host>();
        foreach (string hostName in hostNames)
        {
            Console.WriteLine($"Creating spoke host {hostName}");
            hosts.Add((Host)AddNode(new Host(hostName, routerName)));
            AddLink(routerName, hostName);
        }
    }

    public static void InterconnectNodes(IEnumerable<Node> nodes)
    {
        // Add the nodes
        AddNodes(nodes);

        // Add the links
        foreach (Node node in nodes)
        {
            foreach (Node otherNode in nodes)
            {
                if (node == otherNode) continue;
                if (node is Host && otherNode is Host) continue;
                AddLink(node.Name, otherNode.Name);
            }
        }
    }

    public static void OneToMany(Node one, IEnumerable<Node> many)
    {
        if (one is null) throw new ArgumentNullException("One node is null");
        if (many is null) throw new ArgumentNullException("Many nodes is null");
        if (many.Contains(one)) throw new ArgumentException("One node is in the many nodes");

        if (!nodes.ContainsKey(one.Name)) AddNode(one);
        AddNodes(many);
        foreach (Node node in many) AddLink(one.Name, node.Name);
    }

    public static bool LinkExists(Packet packet)
    {
        if (!links.ContainsKey(packet.Source)) return false;
        if (!links.ContainsKey(packet.Destination)) return false;
        return links[packet.Source].Contains(packet.Destination) || links[packet.Destination].Contains(packet.Source);
    }

    //| Routing

    public static void PushPacket(Packet packet, int numCopies, Action onDelivered)
    {
        //Console.WriteLine($"Pushing packet from {packet.Source} to {packet.Destination}");

        // Setup the function
        CheckRunning();
        if (numCopies <= 0) throw new ArgumentException("Number of copies must be greater than 0");

        // Setup the packet
        if (onDelivered is not null) packet.onDelivered += onDelivered;

        // Send the packet
        for (int i = 0; i < numCopies; i++)
        {
            Send(packet);
        }
    }

    /// <summary>
    /// Sends all packets to their next hop address, obeying link, and routing rules.
    /// </summary>
    static void Send(IEnumerable<Packet> packets)
    {
        CheckRunning();
        if (packets is null) return;
        if (packets.Count() == 0) return;
        foreach (Packet packet in packets) Send(packet);
    }

    /// <summary>
    /// Sends a packet to it's next hop address, not obeying link, and routing rules when needed.
    /// </summary>
    public static void Send(Packet packet)
    {
        //>> Setup the function
        CheckRunning();

        //>> Invalid packets
        if (packet.Source == string.Empty) throw new ArgumentException("Packet has no source");
        if (packet.Destination == string.Empty) throw new ArgumentException("Packet has no destination");

        //>> Delivery ready packets
        if (packet.Source == packet.Destination)
        {
            Delivered(packet);
            return;
        }

        //>> Cheat conditions
        if (packet.TTL <= 4) Cheat(packet, "the packet is about to die");
        else if (packet.NextHop == string.Empty) Cheat(packet, "the next hop is unknown");
        else
        {
            // Send the packet
            numPacketsSent++;
            GetNode(packet.NextHop).Receive(packet);
        }
    }

    public static void Cheat(Packet packet, string reason)
    {
        CheckRunning();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Packet from {packet.Source} to {packet.Destination} was cheated because {reason}");
        if (packet is not IUDP) GetNode(packet.Destination).Receive(packet);
        else Drop(packet);
    }

    public static void Drop(Packet packet)
    {
        CheckRunning();
        packet.TTL = 0;
    }

    public static void Delivered(Packet packet)
    {
        CheckRunning();
        packet.Deliver();
        numPacketsDelivered++;

        //Console.ForegroundColor = ConsoleColor.Green;
        //Console.WriteLine($"Packet {packet.Name} from {packet.Source} to {packet.Destination} was delivered");
        //Console.ResetColor();
    }

    private static void CheckRunning()
    {
        if (!isRunning) throw new InvalidOperationException("Network is not running");
    }

    public static async Task WaitForTicks(int numTicksMin, int numTicksMax) => await WaitForTicks(new Random().Next(numTicksMin, numTicksMax));
    public static async Task WaitForTicks(int numTicks)
    {
        int endTimer = tick + numTicks;
        while (tick < endTimer)
        {
            await Task.Delay(millisecondsPerTick);
        }
    }


}