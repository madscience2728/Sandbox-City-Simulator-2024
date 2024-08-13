namespace Sandbox_City_Simulator_2024;

using Network.Core;
using Sandbox_City_Simulator_2024.PrintTools;

public class RandomEvents
{
    Chance fireChance = new(Chance.OncePer6Hours);
    Random random = new Random();

    public RandomEvents()
    {
        Network.AddListener(Step);
    }

    public void Step()
    {
        if (fireChance.NotRoll()) return;

        int causeIndex = random.Next(0, potentialFireCauses.Length);
        Print.Cache(potentialFireCauses[causeIndex][0], ConsoleColor.Yellow);
        
        
        // Send a fire packet to a random node from a random node
        var source = Network.GetRandomHost()!;
        var destination = Network.GetRandomNode()!;
        Network.Send(new FirePacket
        {
            Name = "Fire Packet",
            Source = source.Name,
            Destination = destination.Name,
            LastHop = source.Name,
            NextHop = source.DefaultGateway,
            causeIndex = causeIndex,
            //traceRoute = true
        });
    }

    public static readonly string[][] potentialFireCauses = [
        ["Someone fell alsleep smoking...", "...the cigrette went out. "],
        ["There is a gas leak...", "...the gas leak was discovered and fixed. "],
        ["A rat chewed through some wires...", "...the chewed through wires were discovered and fixed. "],
        ["A mouse chewed through some wires...", "...the chewed through wires were discovered and fixed. "],
        ["A squirrel chewed through some wires...", "...the chewed through wires were discovered and fixed. "],
        ["A chipmunk chewed through some wires...", "...the chewed through wires were discovered and fixed. "],
        ["A racoon chewed through some wires...", "...the chewed through wires were discovered and fixed. "],
        ["There was lightning strike...", "... the lightning didn't strike anything important. "],
        ["There was a power surge...", "... the power surge didn't cause any damage. "],
        ["A candle was left unattended...", "... the candle was put out. "],
        ["A stove was left on...", "... the stove was turned off. "],
        ["An arsonists is about...", "... the arsonist realized it was a bad idea. "],
        ["A firework went off in the house...", "...the firework didn't cause any damage. "],
        ["A child is playing with a lighter...", "...the child had the lighter confiscated. "],
        ["A child is playing with matches...", "...the child had the matches confiscated. "],
        ["A child is playing with fireworks...", "...the child had the fireworks confiscated. "],
        ["A child is playing with a magnifying glass...", "...the child stopped playing with the magnifying glass. "],
        ["Some college students are playing with a high powered laser pointer...", "...the college students stopped playing with the laser pointer. "],
        ["Some guy is playing with a flamethrower...", "...the guy stopped playing with the flamethrower. "],
        ["A child is playing with a blowtorch...", "...the child stopped playing with the blowtorch. "],
        ["There is a free ember from a fire in the fireplace...", "...the ember was put out. "],
        ["The risk of fire is afoot...", "...the risk of fire was mitigated. "],
        ["A drug lab just blew up...", "...the drug lab stayed covert. "],
        ["Some DIY wiring is sparking...", "...the sparking wires were discovered and fixed. "],
        ["Some bad wiring is sparking...", "...the sparking wires were discovered and fixed. "],
        ["Some old wiring is sparking...", "...the sparking wires were discovered and fixed. "],
    ];
}