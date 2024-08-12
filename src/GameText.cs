namespace Sandbox_City_Simulator_2024;

using Sandbox_City_Simulator_2024.PrintTools;

public static class GameText
{
    static Random random = new();
    
    public static string[] RoadNameSuffixes = new string[]
    {
        "St",
        "Ave",
        "Blvd",
        "Ct",
        "Dr",
        "Ln",
        "Pkwy",
        "Rd",
        "Way"
    };
    
    public static string GetRandomRoadName()
    {
        return $"{NameGenerator.GenerateName()} {RoadNameSuffixes[random.Next(0, RoadNameSuffixes.Length)]}";
    }
}