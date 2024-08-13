namespace Sandbox_City_Simulator_2024;
using Sandbox_City_Simulator_2024.PrintTools;

class Program
{
    async static Task Main(string[] args)
    {
        Print.Clear();
        Print.ConsoleResetColor();

        for (int i = 0; i < 100; i++)
        {
            
            
            Stats stats = new Stats();
            Print.Line($"Health: {stats.RollHealth()}");
            Print.Line($"Intelligence: {stats.RollIntelligence()}");
            Print.Line($"Luck: {stats.RollLuck()}");
            Print.Line($"Addiction Prepensity: {stats.RollAddictionPrepensity()}");
            Print.Line($"Empathy: {stats.RollEmpathy()}");
            Print.Line($"Survival Odds: {stats.RollSurvivalOdds()}");
            Print.Line($"Charisma: {stats.RollCharisma()}");
            Print.Line($"Strength: {stats.RollStrength()}");
            Print.Line($"Mental Health: {stats.RollMentalHealth()}");
            Print.Line($"Energy: {stats.RollEnergy()}");
            Print.Line($"Happiness: {stats.RollHappiness()}");
            Print.Line($"Criminality: {stats.RollCriminality()}");
            Print.Line();
            Print.Line();
            Print.Line();
        }




        while (true)
        {
            await new Game().Play();
            Print.Clear();
            Print.Pause("play again");
        }
    }
}
