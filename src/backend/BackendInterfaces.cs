namespace Sandbox_City_Simulator_2024;

public interface IAmOnFire
{
    bool OnFire { get; set; }
    int timeCaughtOnFire { get; set; }
    Chance chanceToCatchFire { get; set; }
}

public interface IAmDestroyable
{
    bool Destroyed { get; set; }
    Chance chanceToBeDestroyed { get; set; }
    void OnDestruction();
}

public interface IHasHome
{
    string Home { get; set; }
}

public interface IHasStats
{
    Stats stats { get; set; }
}