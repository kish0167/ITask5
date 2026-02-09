namespace ITask5.Services.AudioGenerator;

public static class MusicTheory
{
    private const float SemiTone = 1.0594631f;
    private static readonly float[] CMajor = {
        130.81f, 146.83f, 164.81f, 174.61f, 196f, 220f, 246.94f
    };
    private static readonly int[] StandardChordStages = {
        0, 2, 4
    };
    
    private static readonly List<int[]> PopularMinorChordsProgressions =
    [
        [1, 6, 7],
        [1, 4, 7],
        [1, 4, 5],
        [1, 6, 3, 7],
        [2, 5, 1],
        [1, 4, 5, 1],
        [6, 7, 1, 1],
        [1, 7, 6, 7],
        [1, 4, 1]
    ];
    private static readonly List<int[]> PopularMajorChordsProgressions =
    [
        [1, 4, 5],
        [1, 6, 2, 5],
        [1, 3, 4, 5],
        [1, 6, 4, 5],
        [1, 5, 6, 4],
        [1, 4, 1, 5],
        [1, 4, 2, 5],
        [1, 4, 6, 5],
        [2, 5, 1]
    ];

    private static readonly List<float[]> StrikesSchemas =
    [
        [0f, 0.5f],
        [0f, 0.25f, 0.5f],
        [0f, 0.25f, 0.5f, 0.75f],
        [0f, 0.125f, 0.5f, 0.625f],
        [0f, 0.25f, 0.5f, 0.75f],
        [0f, 0.125f, 0.75f],
        [0.25f, 0.75f],
        [0.25f, 0.5f],
        [0.25f, 0.375f]
    ];
    
    private static readonly List<float[]> DrumKickSchemas =
    [
        [0f],
        [0f, 0.5f],
        [0f, 0.25f, 0.5f],
        [0f, 0.25f, 0.5f],
        [0.25f, 0.75f],
        [0f, 0.125f, 0.5f, 0.625f]
    ];
    
    private static readonly List<float[]> DrumHatSchemas =
    [
        [0f, 0.5f],
        [0f, 0.25f, 0.5f, 0.75f],
        [0.25f, 0.75f],
    ];
    
    public static List<List<float>> GetRandomChordsFrequencies(Random random)
    {
        float[] notes = CreateRandomKey(random, out bool isMajor);
        int[] progression = GetRandomProgression(random, isMajor);
        List<List<float>> chords = new();
        for (int i = 0; i < progression.Length; i++)
        {
            chords.Add(new List<float>());
            for (int j = 0; j < StandardChordStages.Length; j++)
            {
                chords[i].Add(notes[progression[i] + StandardChordStages[j] - 1]);
            }
        }
        return chords;
    }

    public static float[] GetRandomStrikeSchema(Random random)
    {
        return StrikesSchemas[random.Next(StrikesSchemas.Count)];
    }
    
    public static float[] GetRandomDrumKickSchema(Random random)
    {
        return DrumKickSchemas[random.Next(DrumKickSchemas.Count)];
    }
    
    public static float[] GetRandomDrumHatSchema(Random random)
    {
        return DrumHatSchemas[random.Next(DrumHatSchemas.Count)];
    }

    private static float[] CreateRandomKey(Random random, out bool isMajor)
    {
        float[] notes = new float[CMajor.Length];
        Array.Copy(CMajor, notes, CMajor.Length);
        KeyShift(notes, random.Next(-6, 2));
        isMajor = random.Next() % 1 == 0;
        if (!isMajor) MinorFromMajor(notes);
        return ExtendKey(notes);
    }

    private static int[] GetRandomProgression(Random random, bool isMajor)
    {
        List<int[]> progressions = isMajor ? PopularMajorChordsProgressions : PopularMinorChordsProgressions;
        return progressions[random.Next(progressions.Count)];
    }
    
    private static void KeyShift(float[] notes, int shift)
    {
        float totalFactor = (float)Math.Pow(SemiTone, shift);
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i] *= totalFactor;
        }
    }

    private static void MinorFromMajor(float[] notes)
    {
        notes[2] /= SemiTone;
        notes[5] /= SemiTone;
        notes[6] /= SemiTone;
    }

    private static float[] ExtendKey(float[] notes)
    {
        float[] extendedNotes = new float[notes.Length * 3];
        Array.Copy(notes, extendedNotes, notes.Length);
        for (int i = 0; i < notes.Length * 2; i++)
        {
            extendedNotes[i + notes.Length] = extendedNotes[i] * 2;
        }
        return extendedNotes;
    }
}