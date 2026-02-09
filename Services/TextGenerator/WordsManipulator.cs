using Bogus;

namespace ITask5.Services.TextGenerator;

public static class WordsManipulator
{
    private static readonly string[] EnglishAdjectives =
    [
        "Broken", "Silent", "Wild", "Golden", "Lost", "Fading", "Electric", 
        "Crystal", "Midnight", "Endless", "Fragile", "Burning", "Frozen",
        "Velvet", "Neon", "Shadow", "Secret", "Forbidden", "Eternal", "Raw",
        "Bittersweet", "Distant", "Radiant", "Tangled", "Hollow", "Vivid",
        "Wasted", "Precious", "Haunted", "Blind", "Naked", "Savage", "Tender"
    ];

    private static readonly string[] EnglishNouns =
    [
        "Heart", "Dream", "Night", "Sky", "Ocean", "Memory", "Ghost", "Fire",
        "Rain", "Storm", "Mirror", "Road", "Voice", "Silence", "Moment",
        "Distance", "Edge", "Light", "Darkness", "Melody", "Rhythm", "Soul",
        "Desire", "Secrets", "Promises", "Shadows", "Echoes", "Scars", "Time",
        "Love", "Pain", "Freedom", "Chains", "Roses", "Thunder", "Ashes"
    ];

    private static readonly string[] EnglishVerbs =
    [
        "Running", "Falling", "Rising", "Breaking", "Chasing", "Fading",
        "Dancing", "Fighting", "Searching", "Waiting", "Burning", "Crying",
        "Flying", "Drifting", "Hiding", "Finding", "Losing", "Holding"
    ];
    
    private static readonly string[] PolishAdjectives =
    [
        "Złamane", "Ciche", "Dzikie", "Złote", "Zagubione", "Blaknące", "Elektryczne",
        "Kryształowe", "Północne", "Wieczne", "Kruche", "Płonące", "Mrożone",
        "Aksamitne", "Neonowe", "Cieniste", "Tajne", "Zakazane", "Surowe",
        "Gorzkie", "Odległe", "Promieniste", "Splątane", "Puste", "Żywe",
        "Zmarnowane", "Cenne", "Nawiedzone", "Ślepe", "Nagie", "Dzikie", "Czułe"
    ];

    private static readonly string[] PolishNouns =
    [
        "Serce", "Sen", "Noc", "Niebo", "Ocean", "Wspomnienie", "Duch", "Ogień",
        "Deszcz", "Burza", "Lustro", "Droga", "Głos", "Cisza", "Chwila",
        "Odległość", "Krawędź", "Światło", "Ciemność", "Melodia", "Rytm", "Dusza",
        "Pragnienie", "Sekrety", "Obietnice", "Cienie", "Echa", "Błizny", "Czas",
        "Miłość", "Ból", "Wolność", "Łańcuchy", "Róże", "Grzmot", "Popiół"
    ];

    private static readonly string[] PolishVerbs =
    [
        "Biegnące", "Spadające", "Wznoszące", "Łamiące", "Goniące", "Blaknące",
        "Tańczące", "Walczące", "Szukające", "Czekające", "Płonące", "Płaczące",
        "Latające", "Dryfujące", "Chowające", "Znajdujące", "Tracące", "Trzymające"
    ];

    // Common prepositions and connectors
    private static readonly string[] EnglishConnectors = new[] { "of", "in", "the", "and", "to", "from", "with", "without" };
    private static readonly string[] PolishConnectors = new[] { "i", "w", "z", "bez", "na", "pod", "nad", "przez" };

    public static string GenerateWords(Faker faker, string locale, bool isTitle)
    {
        var isPolish = IsPolish(locale);

        if (!isTitle && faker.Random.Int(0,9) > 7)
        {
            return SingleString(locale);
        }
        
        if (isPolish)
        {
            return GeneratePolishTitle(faker, isTitle);
        }
        return GenerateEnglishTitle(faker, isTitle);
    }
    
    private static string SingleString(string locale)
    {
        return IsPolish(locale) ? "Singiel" : "Single";
    }

    private static string GenerateEnglishTitle(Faker faker, bool isTitle)
    {
        int pattern = faker.Random.Int(0, 5);
        return pattern switch
        {
            0 => $"The {faker.PickRandom(EnglishAdjectives)} {faker.PickRandom(EnglishNouns)}",
            1 => $"{faker.PickRandom(EnglishAdjectives)} {faker.PickRandom(EnglishNouns)}",
            2 => $"{faker.PickRandom(EnglishVerbs)} {faker.PickRandom(EnglishNouns)}",
            3 => $"{faker.PickRandom(EnglishNouns)} of {faker.PickRandom(EnglishNouns)}",
            4 => $"The {faker.PickRandom(EnglishNouns)} {faker.PickRandom(EnglishConnectors)} {faker.PickRandom(EnglishNouns)}",
            5 => !isTitle
                ? $"{faker.PickRandom(EnglishAdjectives)} {faker.PickRandom(EnglishNouns)}" 
                : faker.PickRandom(EnglishNouns),
            _ => $"{faker.PickRandom(EnglishAdjectives)} {faker.PickRandom(EnglishNouns)}"
        };
    }

    private static string GeneratePolishTitle(Faker faker, bool isTitle)
    {
        int pattern = faker.Random.Int(0, 5);
        return pattern switch
        {
            0 => $"{faker.PickRandom(PolishAdjectives)} {faker.PickRandom(PolishNouns)}",
            1 => $"{faker.PickRandom(PolishNouns)} {faker.PickRandom(PolishAdjectives)}",
            2 => $"{faker.PickRandom(PolishVerbs)} {faker.PickRandom(PolishNouns)}",
            3 => $"{faker.PickRandom(PolishNouns)} {faker.PickRandom(PolishConnectors)} {faker.PickRandom(PolishNouns)}",
            4 => $"{faker.PickRandom(PolishAdjectives)} {faker.PickRandom(PolishNouns)} {faker.PickRandom(PolishConnectors)} {faker.PickRandom(PolishNouns)}",
            5 => !isTitle
                ? $"{faker.PickRandom(PolishAdjectives)} {faker.PickRandom(PolishNouns)}"
                : faker.PickRandom(PolishNouns),
            _ => $"{faker.PickRandom(PolishAdjectives)} {faker.PickRandom(PolishNouns)}"
        };
    }
    
    private static bool IsPolish(string locale)
    {
        bool isPolish = locale == "pl";
        return isPolish;
    }
}