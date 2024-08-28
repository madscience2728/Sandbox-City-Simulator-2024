namespace Sandbox_Simulator_2024;

public static class NameGenerator
{
    const string consonants = "bcdfghjklmnprstvwxz";
    const string vowels = "aeiouaeiouaeioueeeeeaay";

    static readonly string[] NameEnders = {
        "a", "ia", "en", "on", 
        "ar", "or", "ur", "ir",
        "el", "il", "ol", "ul",
        "es", "is", "os", "us",
        "an", "in", "on", "un",
    };
    static readonly string[] Awks = [
        "pf", "bv", "lp", "rp",
        "tk", "dk", "gk",
        "mr", "lr", "nr",
        "zm", "zn", "zl",
        "vl", "vr", "vn",
        "js", "jz", "jm",
        "kp", "kd", "kt",
        "xb", "xd", "xg",
        "uu", "ii", "oo",
        "aou", "eiu", "oie",
        "uie", "eao", "aei",
        "bt", "ct", "ft", "pt",
        "md", "mt", "ng",
        "wl", "wn", "wt",
        "rb", "rg", "rk",
        "sv", "sr", "sl",
        "tl", "tn", "gn",
        "ngl", "mbl", "ntl",
        "rts", "lts", "nts",
        "str", "spr", "scr",
        "hg", "hk", "hb",
        "vh", "jh", "lh",
        "qk", "qg", "qd",
        "cx", "cv", "cb",
        "fv", "fw", "fz",
        "shs", "shz", "zhz",
        "sts", "szs", "zsh",
        "edo", "otu", "aki",
    ];
    
    static Random random = new();
    
    static HashSet<string> usedNames = new();
    
    public static string GenerateName()
    {
        return GenerateName(random);
    }
    public static string GenerateName(Random r)
    {
        try
        {
            string name = "";

            // Start with a partial name
            name += PartialName(r);

            // Add a random number of partial names
            int numParts = r.Next(0, 2);
            for (int i = 0; i < numParts; i++)
            {
                name += PartialName(r);
            }

            // Chop off the end bits maybe
            if (name.Length > 9)
            {
                int startIndex = random.Next(0, 3);
                int endIndex = random.Next(0, 3);

                // Chop off the end bits
                name = name.Substring(0, name.Length - endIndex);

                // Chop off the start bits
                name = name.Substring(startIndex);
            }

            if (random.NextSingle() > 0.5f)
            {
                // If the last letter is a vowel, add a consonant
                if (random.NextSingle() > 0.5f && vowels.Contains(name.Last())) name += GenerateConsonant(r);
                name += NameEnders[r.Next(0, NameEnders.Length)];
            }

            // Remove any awkward letter combinations
            foreach (var awk in Awks)
            {
                string candidateName = name.Replace(awk, "");
                if (candidateName != name)
                {
                    // Only remove 1 awkward letter combination
                    name = candidateName;
                    break;
                }
            }

            // Remove triplicate letters
            for (int i = 0; i < name.Length - 2; i++)
            {
                if (name[i] == name[i + 1] && name[i] == name[i + 2])
                {
                    name = name.Remove(i + 2, 1);
                }
            }

            // Remove triplicate vowels
            for (int i = 0; i < name.Length - 2; i++)
            {
                if (vowels.Contains(name[i]) && vowels.Contains(name[i + 1]) && vowels.Contains(name[i + 2]))
                {
                    name = name.Remove(i + 2, 1);
                }
            }

            // Remove triplicate consonants
            for (int i = 0; i < name.Length - 2; i++)
            {
                if (consonants.Contains(name[i]) && consonants.Contains(name[i + 1]) && consonants.Contains(name[i + 2]))
                {
                    name = name.Remove(i + 2, 1);
                }
            }

            // Remove double-consonant endings
            if (consonants.Contains(name[name.Length - 1]) && consonants.Contains(name[name.Length - 2]))
            {
                name = name.Remove(name.Length - 1);
            }

            // Remove double-consonants beginnings
            if (consonants.Contains(name[0]) && consonants.Contains(name[1]))
            {
                name = name.Remove(1);
            }

            // Remove double-vowels that aren't 'oo' or 'ee'
            for (int i = 0; i < name.Length - 1; i++)
            {
                if (vowels.Contains(name[i]) && vowels.Contains(name[i + 1]) && name[i] != 'o' && name[i] != 'e')
                {
                    name = name.Remove(i + 1);
                }
            }

            // If the name is too short, try again
            if (name.Length <= 1) return GenerateName(r);

            // Capitalize the first letter and return
            string finalCandidateName = name.First().ToString().ToUpper() + name.Substring(1);
            if (usedNames.Contains(finalCandidateName)) return GenerateName(r);
            usedNames.Add(finalCandidateName);
            return finalCandidateName;
        }
        catch (Exception)
        {
            return GenerateName(r);
        }
    }
    
    static string PartialName(Random r)
    {
        if (r.NextSingle() > 0.5f)
        {
            return GenerateConsonant(r) + OneOrTwoVowels(r) + GenerateConsonant(r);
        }
        else
        {
            return GenerateVowel(r) + OneOrTwoConsonants(r) + GenerateVowel(r);
        }
    }
    
    static string OneOrTwoConsonants(Random r)
    {
        if(r.NextSingle() > 0.5f) return GenerateConsonant(r);
        else return GenerateConsonant(r).ToString() + GenerateConsonant(r);
    }
    
    static string OneOrTwoVowels(Random r)
    {
        if(r.NextSingle() > 0.5f) return GenerateVowel(r);
        else return GenerateVowel(r).ToString() + GenerateVowel(r);
    }
    
    static string GenerateConsonant(Random r)
    {
        return consonants[r.Next(0, consonants.Length)].ToString();
    }
    
    static string GenerateVowel(Random r)
    {
        return vowels[r.Next(0, vowels.Length)].ToString();
    }
}