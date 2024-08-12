namespace Sandbox_City_Simulator_2024;
using Network.Core;
using Sandbox_City_Simulator_2024.PrintTools;

public class AbstractBuilding : AbstractGameHost, IOnFire, IDestroyable
{
    public bool OnFire { get; set; }
    public bool Destroyed { get; set; }
    public Chance chanceToCatchFire { get; set; } = new(Chance.OncePer2Minutes); // How long does the risk of fire last
    public Chance chanceToBeDestroyed { get; set; } = new(Chance.OncePerHour); // How long can you last before burning down

    public AbstractBuilding(string name, string gateway) : base(name, gateway)
    {

    }

    protected override void YouveGotMail(Packet packet)
    {
        if (Destroyed) return;
        
        if (!OnFire && packet is FirePacket)
        {
            if (chanceToCatchFire.Roll())
            {
                Print.Cache($"A fire broke out at {Name}", ConsoleColor.Red);
                OnFire = true;

                var fireStation = Network.GetRandomNode<FireStation>()!;
                FireRequestPacket fireRequestPacket = new FireRequestPacket
                {
                    Name = $"Fire Request Packet from {Name}",
                    Source = Name,
                    Destination = fireStation.Name,
                    LastHop = Name,
                    NextHop = DefaultGateway,
                    //traceRoute = true
                };
                Network.Send(fireRequestPacket);
            }
        }
        else if (OnFire && packet is FireResponsePacket)
        {
            Task.Run(async () =>
            {
                Print.Cache($"The fire department has showed up to {Name}", ConsoleColor.Blue);
                await Network.WaitForTicks(30, 120);
                if (Destroyed) return;
                Print.Cache($"{Name} has been saved by the fire department", ConsoleColor.Green);
                OnFire = false;
            });
        }
    }

    public override void Step()
    {
        if (Destroyed) return;

        if (chanceToBeDestroyed.Roll())
        {
            if (OnFire)
            {
                Print.Cache($"{Name} has been destroyed by fire", ConsoleColor.DarkRed);
                OnDestruction();
            }
        }

        base.Step();
    }

    public void OnDestruction()
    {
        Destroyed = true;
    }
}