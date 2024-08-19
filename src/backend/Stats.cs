namespace Sandbox_Simulator_2024;

// Applicapable to any creature entity in the game
public class Stats
{
    const float PersonVariabilitySigma = 0.1f;


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
    
    // Add a
    public class TraitOperators
    {
        // Get all float from the derived class and apple the correct operator to them, eaith +, or /, or recombine (for reproduction later down the line)
        public static TraitOperators operator +(TraitOperators a, TraitOperators b)
        {
            TraitOperators result = new TraitOperators();
            foreach (var property in a.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(float))
                {
                    property.SetValue(result, (float?)property.GetValue(a) + (float?)property.GetValue(b));
                }
            }
            return result;
        }
        
        public static TraitOperators operator /(TraitOperators a, TraitOperators b)
        {
            TraitOperators result = new TraitOperators();
            foreach (var property in a.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(float))
                {
                    property.SetValue(result, (float?)property.GetValue(a) / (float?)property.GetValue(b));
                }
            }
            return result;
        }
        
    }

    public class Traits
    {

        public class Immutable : TraitOperators
        {
            public float Luck { get; set; } = GaussianBetween01(PersonVariabilitySigma);
            public float AddictionPrepensity { get; set; } = GaussianBetween01(PersonVariabilitySigma);
            public float Empathy { get; set; } = GaussianBetween01(PersonVariabilitySigma);
            public float Happiness { get; set; } = GaussianBetween01(PersonVariabilitySigma);
            public float Criminality { get; set; } = GaussianBetween01(PersonVariabilitySigma);
            public float Agression { get; set; } = GaussianBetween01(PersonVariabilitySigma);
        }

        public class Variable : TraitOperators
        {
            public float Health { get; set; } = GaussianBetween01(PersonVariabilitySigma);
            public float Intelligence { get; set; } = GaussianBetween01(PersonVariabilitySigma);
            public float Addiction { get; set; } = GaussianBetween01(PersonVariabilitySigma);
            public float Energy { get; set; } = GaussianBetween01(PersonVariabilitySigma);
            public float Hunger { get; set; } = GaussianBetween01(PersonVariabilitySigma);
            public float SocialFulfillment { get; set; } = GaussianBetween01(PersonVariabilitySigma);
        }
        
        public Immutable immutable = new Immutable();
        public Variable variable  = new Variable();
    }
    
    public Traits traits = new Traits();

    public bool RollHealth() => random.NextSingle() < DetermineHealth();
    public float DetermineHealth() => traits.variable.Health * traits.immutable.Happiness;

    public bool RollIntelligence() => random.NextSingle() < DetermineIntelligence();
    public float DetermineIntelligence() => traits.variable.Health * Math.Min(traits.variable.Intelligence, traits.immutable.Criminality);

    public bool RollLuck() => random.NextSingle() < traits.immutable.Luck;

    public bool RollAddictionPrepensity() => random.NextSingle() < DetermineAddictionPrepensity();
    public float DetermineAddictionPrepensity() => (1f - traits.variable.Health) * Math.Min(Math.Min(traits.immutable.AddictionPrepensity, traits.immutable.Luck), traits.variable.Intelligence);

    public bool RollEmpathy() => random.NextSingle() < DetermineEmpathy();
    public float DetermineEmpathy() => (1f - traits.variable.Addiction) * traits.immutable.Empathy;

    public bool RollSurvivalOdds() => random.NextSingle() < DetermineSurvivalOdds();
    public float DetermineSurvivalOdds() =>  ((traits.variable.Health + traits.variable.Intelligence + traits.immutable.Luck) / 3f);

    public bool RollCharisma() => random.NextSingle() < DetermineCharisma();
    public float DetermineCharisma() => traits.variable.Health * Math.Max(1f, ((traits.variable.Intelligence + traits.immutable.Luck) / 2f) + traits.variable.SocialFulfillment);

    public bool RollStrength() => random.NextSingle() < DetermineStrength();
    public float DetermineStrength() => traits.variable.Health * ((traits.immutable.Agression + traits.variable.Energy) / 2f);

    public bool RollMentalHealth() => random.NextSingle() < DetermineMentalHealth();
    public float DetermineMentalHealth() => Math.Min(1f - traits.immutable.Agression, Math.Min((1f - traits.immutable.Criminality), (1f - traits.variable.Addiction))) * Math.Max(((traits.variable.Intelligence + traits.immutable.Luck) / 2f), traits.variable.SocialFulfillment);

    public bool RollEnergy() => random.NextSingle() < DetermineEnergy();
    public float DetermineEnergy() => (1f - traits.variable.Hunger) * Math.Max(0, ((traits.variable.Energy + traits.immutable.Happiness) / 2f) - traits.variable.Addiction);

    public bool RollHappiness() => random.NextSingle() < DetermineHappiness();
    public float DetermineHappiness() => traits.variable.Health * (1f - traits.variable.Addiction) * ((traits.immutable.Happiness + traits.variable.Energy + traits.variable.SocialFulfillment) / 3f);

    public bool RollCriminality() => random.NextSingle() < DetermineCriminality();
    public float DetermineCriminality() => (1f - ((traits.immutable.Empathy + traits.variable.Intelligence) /2f)) * Math.Max(traits.immutable.Criminality, traits.immutable.Luck);

    public bool RollAgression() => random.NextSingle() < DetermineAgression();
    public float DetermineAgression() => (1f - traits.immutable.Empathy) * traits.immutable.Agression;
}