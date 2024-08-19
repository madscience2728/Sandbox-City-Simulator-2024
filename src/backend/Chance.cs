namespace Sandbox_Simulator_2024;

public class Chance
{
    // Note: Assumes called rolls are in the time frame once per minutes.
    public const float OncePer2Minutes = 1f / 2f;
    public const float OncePer3Minutes = 1f / 3f;
    public const float OncePer4Minutes = 1f / 4f;
    public const float OncePer5Minutes = 1f / 5f;
    public const float OncePer10Minutes = 1f / 10f;
    public const float OncePer15Minutes = 1f / 15f;
    public const float OncePer30Minutes = 1f / 30f;
    public const float OncePer45Minutes = 1f / 45f;
    public const float OncePerHour = 1f / 60f;
    public const float OncePer2Hours = 1f / (60f * 2f);
    public const float OncePer3Hours = 1f / (60f * 3f);
    public const float OncePer4Hours = 1f / (60f * 4f);
    public const float OncePer6Hours = 1f / (60f * 6f);
    public const float OncePer8Hours = 1f / (60f * 8f);
    public const float OncePer12Hours = 1f / (60f * 12f);
    public const float OncePer16Hours = 1f / (60f * 16f);
    public const float OncePerDay = 1f / (60f * 24f);
    public const float OncePer2Days = 1f / (60f * 48f);
    public const float OncePer3Days = 1f / (60f * 72f);
    public const float OncePer4Days = 1f / (60f * 96f);
    public const float OncePerWeek = 1f / (60f * 24f * 7f);
    public const float OncePer2Weeks = 1f / (60f * 24f * 14f);
    public const float OncePerMonth = 1f / (60f * 24f * 30f);
    public const float OncePerYear = 1f / (60f * 24f * 365f);

    Random random = new();
    Func<float> determineMin = () => 0f;
    Func<float> determineMax = () => 0.5f;
    
    /// <summary>
    /// Create a new chance object with a 50% chance of rolling true.
    /// </summary>
    public Chance() { }
    
    /// <summary>
    /// Create a new chance object with a specific chance of rolling true. The roll will be between 0f and 1f, and to roll true you must roll a value less that the given chance. Equivalent to (min: 0f, max: value).   
    /// </summary>
    public Chance(float value)
    {
        determineMin = () => 0f;
        determineMax = () => value;
    }
    
    /// <summary>
    /// Create a new chance object with a minimum and maximum value. The roll will be between 0f and 1f, and to roll true you must roll a value between Min and Max.
    /// </summary>
    public Chance(float min, float max)
    {
        determineMin = () => min;
        determineMax = () => max;
    }
    
    /// <summary>
    /// Create a new chance object with a dynamic lambda value. The roll will be between 0f and 1f, and to roll true you must roll a value between Min and Max.
    /// </summary>
    public Chance(Func<float> min, Func<float> max)
    {
        determineMin = min;
        determineMax = max;
    }
    
    /// <summary>
    ///  The roll will be between 0f and 1f, and to roll true you must roll a value between Min and Max.
    /// </summary>
    public bool Roll()
    {
        float value = (float) random.NextDouble();
        return value > determineMin() && value < determineMax();
    }

    public bool NotRoll()
    {
        return !Roll();
    }
}