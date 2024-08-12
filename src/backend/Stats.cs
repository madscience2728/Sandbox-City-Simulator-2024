namespace Sandbox_City_Simulator_2024;

// Applicapable to any creature entity in the game
public class Stats
{
    static readonly Random random = new Random();

    // Base internal stats that make up the rest of the stats
    public float Health { get; set; } = random.NextSingle();
    public float Intelligence { get; set; } = random.NextSingle();
    float Luck { get; set; } = random.NextSingle();
    float AddictionPrepensity { get; set; } = random.NextSingle();
    public float Addiction { get; set; } = random.NextSingle();
    float Empathy { get; set; } = random.NextSingle();
    public float Energy { get; set; } = random.NextSingle();
    public float Hunger { get; set; } = random.NextSingle();
    float Happiness { get; set; } = random.NextSingle();
    float Criminality { get; set; } = random.NextSingle();
    float Agression { get; set; } = random.NextSingle();
    public float SocialFulfillment { get; set; } = random.NextSingle();

    public bool RollHealth() => random.NextSingle() < DetermineHealth();
    public float DetermineHealth() => Health * Happiness;

    public bool RollIntelligence() => random.NextSingle() < DetermineIntelligence();
    public float DetermineIntelligence() => Health * Math.Min(Intelligence, Criminality);

    public bool RollLuck() => random.NextSingle() < Luck;

    public bool RollAddictionPrepensity() => random.NextSingle() < DetermineAddictionPrepensity();
    public float DetermineAddictionPrepensity() => (1f - Health) * AddictionPrepensity;

    public bool RollEmpathy() => random.NextSingle() < DetermineEmpathy();
    public float DetermineEmpathy() => (1f - Addiction) * Empathy;

    public bool RollSurvivalOdds() => random.NextSingle() < DetermineSurvivalOdds();
    public float DetermineSurvivalOdds() => Health * ((Intelligence + Luck) / 2f);

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
}