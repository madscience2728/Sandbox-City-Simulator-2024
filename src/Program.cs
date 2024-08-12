namespace Sandbox_City_Simulator_2024;
using Sandbox_City_Simulator_2024.PrintTools;

class Program
{
    async static Task Main(string[] args)
    {
        Print.Clear();
        Print.ConsoleResetColor();
        
        // Set the window to a standard size
        //Console.SetWindowSize(120, 45);
        
        while (true)
        {
            await new Game().Play();
            Print.Clear();
            Print.Pause("play again");
        }
    }
}
