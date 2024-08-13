namespace Sandbox_City_Simulator_2024;

public interface IOnFire
{
    bool OnFire { get; set; }
    int timeCaughtOnFire { get; set; }
    Chance chanceToCatchFire { get; set; }
}

public interface IDestroyable
{
    bool Destroyed { get; set; }
    Chance chanceToBeDestroyed { get; set; }
    void OnDestruction();
}