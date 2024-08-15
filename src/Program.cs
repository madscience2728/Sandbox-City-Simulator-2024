namespace Sandbox_Simulator_2024;
using Sandbox_Simulator_2024.PrintTools;
using Sandbox_Simulator_2024.Scripting;

class Program
{
    async static Task Main(string[] args)
    {
        Print.Clear();
        Print.ConsoleResetColor();

        /*for (int i = 0; i < 100; i++)
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
        }*/
        
        Print.Line("Welcome to Sandbox Simulator 2024");
        
        ScriptInterpreter interpreter = new ScriptInterpreter(FireStationExampleScript.draft);
        await Task.Delay(-1);

        while (true)
        {
            Print.Clear();
            Print.Pause("play again");
        }
    }
}
