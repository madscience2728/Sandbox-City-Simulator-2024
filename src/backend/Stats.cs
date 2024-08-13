namespace Sandbox_City_Simulator_2024;

// Applicapable to any creature entity in the game
public class Stats
{
    static readonly Random random = new Random();

    public static float GaussianBetween01(float sigma)
    {
        float mean = 0.5f; // Centered at 0.5 to keep the values within [0, 1]
        float u1 = random.NextSingle();
        float u2 = random.NextSingle();
        float z0 = MathF.Sqrt(-2.0f * MathF.Log(u1)) * MathF.Cos(2.0f * MathF.PI * u2);
        float value = mean + sigma * z0;

        // Clamp the result between 0 and 1 to keep within bounds
        value = MathF.Max(0, MathF.Min(1, value));

        return value;
    }
    
    // Base internal stats that make up the rest of the stats
    public float Health { get; set; } = GaussianBetween01(Game.PersonVariability / 100f);
    public float Intelligence { get; set; } = GaussianBetween01(Game.PersonVariability / 100f);
    float Luck { get; set; } = GaussianBetween01(Game.PersonVariability / 100f);
    float AddictionPrepensity { get; set; } = GaussianBetween01(Game.PersonVariability / 100f);
    public float Addiction { get; set; } = GaussianBetween01(Game.PersonVariability / 100f);
    float Empathy { get; set; } = GaussianBetween01(Game.PersonVariability / 100f);
    public float Energy { get; set; } = GaussianBetween01(Game.PersonVariability / 100f);
    public float Hunger { get; set; } = GaussianBetween01(Game.PersonVariability / 100f);
    float Happiness { get; set; } = GaussianBetween01(Game.PersonVariability / 100f);
    float Criminality { get; set; } = GaussianBetween01(Game.PersonVariability / 100f);
    float Agression { get; set; } = GaussianBetween01(Game.PersonVariability / 100f);
    public float SocialFulfillment { get; set; } = GaussianBetween01(Game.PersonVariability / 100f);

    public bool RollHealth() => random.NextSingle() < DetermineHealth();
    public float DetermineHealth() => Health * Happiness;

    public bool RollIntelligence() => random.NextSingle() < DetermineIntelligence();
    public float DetermineIntelligence() => Health * Math.Min(Intelligence, Criminality);

    public bool RollLuck() => random.NextSingle() < Luck;

    public bool RollAddictionPrepensity() => random.NextSingle() < DetermineAddictionPrepensity();
    public float DetermineAddictionPrepensity() => (1f - Health) * Math.Min(Math.Min(AddictionPrepensity, Luck), Intelligence);

    public bool RollEmpathy() => random.NextSingle() < DetermineEmpathy();
    public float DetermineEmpathy() => (1f - Addiction) * Empathy;

    public bool RollSurvivalOdds() => random.NextSingle() < DetermineSurvivalOdds();
    public float DetermineSurvivalOdds() =>  ((Health + Intelligence + Luck) / 3f);

    public bool RollCharisma() => random.NextSingle() < DetermineCharisma();
    public float DetermineCharisma() => Health * Math.Max(1f, ((Intelligence + Luck) / 2f) + SocialFulfillment);

    public bool RollStrength() => random.NextSingle() < DetermineStrength();
    public float DetermineStrength() => Health * ((Agression + Energy) / 2f);

    public bool RollMentalHealth() => random.NextSingle() < DetermineMentalHealth();
    public float DetermineMentalHealth() => Math.Min(1f - Agression, Math.Min((1f - Criminality), (1f - Addiction))) * Math.Max(((Intelligence + Luck) / 2f), SocialFulfillment);

    public bool RollEnergy() => random.NextSingle() < DetermineEnergy();
    public float DetermineEnergy() => (1f - Hunger) * Math.Max(0, ((Energy + Happiness) / 2f) - Addiction);

    public bool RollHappiness() => random.NextSingle() < DetermineHappiness();
    public float DetermineHappiness() => Health * (1f - Addiction) * ((Happiness + Energy + SocialFulfillment) / 3f);

    public bool RollCriminality() => random.NextSingle() < DetermineCriminality();
    public float DetermineCriminality() => (1f - ((Empathy + Intelligence) /2f)) * Math.Max(Criminality, Luck);
}