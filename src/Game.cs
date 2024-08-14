namespace Sandbox_City_Simulator_2024;

using System.Diagnostics;
using System.Numerics;
using Network.Core;
using Sandbox_City_Simulator_2024.PrintTools;

public class Game
{
    public const int RoadBranchMin = 2;
    public const int RoadBranchMax = 8;

    public static int TimeInMinutes = 0;
    public static int NumPeople = 20000;
    public static int PersonVariability = 10;
    public static int NumNodes = 2500;
    public static int NumRoads = 100;
    public static int NumHighways = 5;
    public static int NumHouses = 4000;
    public static int NumAppartments = 100;
    public static int NumFireStations = 5;


    public int cachedReady = 0;
    string cityName = "City";

    public Random random = new Random();
    RandomEvents? randomEvents;

    public async Task Play()
    {

#if !DEBUG
        Print.Meta("Welcome to Sandbox City Simulator 2024!");
        await Task.Delay(1000);
        Print.Pause();
        

        if(Print.GiveOptionTo("customize your city"))
        {
            Console.Write("What is the name of your city? ");
            cityName = Console.ReadLine();
            Print.CollectUpdateInt("How many people live in your city?", ref NumPeople);
            Print.CollectUpdateInt("How many roads are in your city?", ref NumRoads);
            Print.CollectUpdateInt("How many highways are in your city?", ref NumHighways);
            Print.CollectUpdateInt("How many houses are in your city?", ref NumHouses);
            Print.CollectUpdateInt("How many appartments are in your city?", ref NumAppartments);
            Print.CollectUpdateInt("How many fire stations are in your city?", ref NumFireStations);
        }
#endif

        // Setup Network
        Network.AddListener(() =>
        {
            TimeInMinutes = Network.tick;
            Interlocked.Exchange(ref cachedReady, 1);
        });
        GenerateCity();
        await Network.Start(cityName);

        while (true)
        {
            if (cachedReady == 1)
            {
                Print.PrintCache();
                if (cachedReady == 1) Interlocked.Exchange(ref cachedReady, 0);
            }
            // Else if the console reads athe return/enter key
            else if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                Print.Line("Network is pausing...");
                await Network.Pause();
                Print.Line("Network is paused.");
                await Task.Delay(1000);
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.White;
                Print.Line("Welcom to the pause menu, what would you like to do?");
                Print.Line("1. View city network in map");
                Print.Line("2. View stats of the city");
                Print.Line("3. View stats of the residents");
                Print.Line("4. View stats of the network");
                Print.Line("X. Any other key to resume the simulation");
                int choice = Print.CollectInt("Enter your choice: ");
                switch (choice)
                {
                    case 1:
                        Print.Clear();
                        Print.Line("View city network in map");
                        await Task.Delay(1000);
                        Print.Clear();
                        break;
                    case 2:
                        Print.Clear();
                        Print.Line("View stats of the city");
                        await Task.Delay(1000);
                        Print.Clear();
                        break;
                    case 3:
                        Print.Clear();
                        Print.Line("View stats of the residents");
                        await Task.Delay(1000);
                        Print.Clear();
                        break;
                    case 4:
                        Print.Clear();
                        Print.Line("View stats of the network");
                        await Task.Delay(1000);
                        Print.Clear();
                        break;
                    default:
                        Print.Clear();
                        Print.Line("Resuming the simulation...");
                        await Task.Delay(1000);
                        Print.Clear();
                        Network.Resume();
                        break;
                }
            }
        }
    }

    public static string GetTime() => GetTime(TimeInMinutes);
    public static string GetTime(int _timeInMinutes)
    {
        int hours = _timeInMinutes / 60;
        int minutes = _timeInMinutes % 60;
        if (hours == 0) return $"12:{minutes:D2}am";
        if (hours < 12) return $"{hours:D2}:{minutes:D2}am";
        if (hours - 12 == 0) return $"12:{minutes:D2}pm";
        return $"{hours - 12:D2}:{minutes:D2}pm";
    }

    public static int MinutesToYears(int minutes) => minutes / (60 * 24 * 365);

    public void GenerateCity()
    {
        //>> Setup function
        Stopwatch stopwatch = new Stopwatch();
        Random random = new Random();

        //>> Start with root node main street
        AbstractGameRouter mainSt = new Road("Main " + GameText.RoadNameSuffixes[0]);
        Network.AddNode(mainSt);

        //>> Generate road (router) network
        Print.Meta("Generating roads...");
        stopwatch.Start();
        int numRoads = NumRoads;
        GenerateRoadsRecursively(mainSt, ref numRoads, random);
        stopwatch.Stop();
        Print.Line($"Generated {NumRoads} roads in {stopwatch.ElapsedMilliseconds}ms");
        Print.Line();

        //>> Generate highways
        Print.Meta("Generating highways...");
        stopwatch.Restart();
        //for (int i = 0; i < NumHighways; i++)
        Parallel.For(0, NumHighways, i =>
        {
            Road highway = new Road(GameText.GetRandomRoadName());
            Network.AddNode(highway);

            Road highwayStart;
            Road highwayEnd;
            int count = 0;
            do
            {
                highwayStart = Network.GetRandomNode<Road>()!;
                highwayEnd = Network.GetRandomNode<Road>()!;
                //count++;
                Interlocked.Increment(ref count);
                if (count > 100)
                {
                    Print.Line("Failed to connect highway");
                    break;
                }
            } while (highwayStart == highwayEnd);
            Network.AddLink(highwayStart.Name, highway.Name);
            Network.AddLink(highway.Name, highwayEnd.Name);
            //Print.Line($"{NumRoads + i} Connecting {highway.Name} to {highwayStart.Name} and {highwayEnd.Name}");
        });
        stopwatch.Stop();
        Print.Line($"Generated {NumHighways} highways in {stopwatch.ElapsedMilliseconds}ms");
        Print.Line();
        var roadsGenerated = Network.GetNodes<Road>().ToList();
        numRoads = roadsGenerated.Count;

        //>> Generate houses
        Print.Meta("Generating houses...");
        stopwatch.Restart();
        Sprinkle<House>(roadsGenerated, NumHouses, "");
        stopwatch.Stop();
        Print.Line($"Generated {NumHouses} houses in {stopwatch.ElapsedMilliseconds}ms");
        Print.Line();
        
        //>> Fill houses with people
        FillHousesWithPeople();

        //>> Generate fire stations
        Print.Meta("Generating fire stations...");
        stopwatch.Restart();
        Sprinkle<FireStation>(roadsGenerated, NumFireStations, "Fire Station");
        stopwatch.Stop();
        Print.Line($"Generated {NumFireStations} fire stations in {stopwatch.ElapsedMilliseconds}ms");
        Print.Line();

        //>> Mother Nature
        randomEvents = new RandomEvents();

        // Generate apartments        

        // Generate parks

        // Generate people

        // Generate businesses

        // Generate schools

        // Generate hospitals

        // Generate police stations

        // Generate libraries

        // Generate museums

        // Generate restaurants

        // Generate stores

        // Generate theaters

        // Generate gyms

        // Generate bars

        // Generate clubs

        // Generate hotels
    }

    public void Sprinkle<T>(List<Road> roadsGenerated, int numberOfItems, string nameType) where T : AbstractGameHost, new()
    {
        int numRoads = roadsGenerated.Count;
        Random random = new Random();

        int numItemsPerRoad = numberOfItems / numRoads;
        int numItemsPerRoadMin = 1;
        int numItemsPerRoadMax = Math.Max(1, (int)(numItemsPerRoad * 2f));
        Parallel.For(0, numRoads, i =>
        {
            //Road road = Network.GetRandomNode<Road>()!;
            Road road = roadsGenerated[i];
            int numHousesOnRoad = random.Next(numItemsPerRoadMin, numItemsPerRoadMax);
            //Console.WriteLine($"Making {numHousesOnRoad} houses on {road.Name}");

            for (int j = 0; j < numHousesOnRoad; j++)
            {
                string name = nameType;
                if (name != string.Empty) name = " " + nameType;
                T t = new()
                {
                    Name = $"{j + 1}{GetSuffix(j + 1)} {road.Name}{name}",
                    DefaultGateway = road.Name
                };

                Network.AddNode(t);
                Network.AddLink(road.Name, t.Name);
                //Print.Line($"{i} Connecting {house.Name} to {road.Name}");
            }
        });
    }

    // A function that gets 1st, 2nd, 3rd, 4th, etc... from a number
    public static string GetSuffix(int num)
    {
        int lastTwo = num % 100;
        if (lastTwo >= 11 && lastTwo <= 13)
            return "th";

        switch (num % 10)
        {
            case 1: return "st";
            case 2: return "nd";
            case 3: return "rd";
            default: return "th";
        }
    }

    void GenerateRoadsRecursively(AbstractGameRouter hub, ref int numRoads, Random random)
    {
        // Base case
        if (numRoads <= 0) return;

        // Generate a random number of branches
        int numBranches = random.Next(RoadBranchMin, RoadBranchMax);
        List<Road> newRoads = new();
        for (int i = 0; i < numBranches; i++)
        {
            Road newRoad = new Road(GameText.GetRandomRoadName());
            //Print.Line($"{numRoads} Connecting {hub.Name} to {newRoad.Name}");
            newRoads.Add(newRoad);
            numRoads--;
            if (numRoads == 0) break;
        }

        // Connect
        if (newRoads.Count > 0) Network.OneToMany(hub, newRoads);

        // Recurse
        if (numRoads > 0)
        {
            foreach (Road road in newRoads)
            {
                GenerateRoadsRecursively(road, ref numRoads, random);
            }
        }
    }

    public void FillHousesWithPeople()
    {
        // Network.GetNodes<T> returns an IEnumerbale
        House[] houses = Network.GetNodes<House>().ToArray();
        int avgNumPeopleInHouse = NumPeople / NumHouses;
        for(int i = 0; i < NumPeople; i++)
        {
            int numPeopleInHouse = random.Next(1, avgNumPeopleInHouse * 2);
            House house = houses[random.Next(houses.Length)];
            Person person = new Person(house.Name, NameGenerator.GenerateName());
            house.AddPerson(person);
        }
    }
}


/*


*/