namespace Network.Core;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Hello, World!");
        
        //Network.debugText = true;
        await new Test3().GenerateNetwork();
        await Network.Start();
        Test3.Test();
        
        while (true)
        {
            await Task.Delay(1000);
        }
    }
}
