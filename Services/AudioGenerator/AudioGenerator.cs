using NAudio.Mixer;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace ITask5.Services.AudioGenerator;

public class AudioGenerator : IAudioGenerator
{
    public byte[] Generate(int seed, int duration)
    {
        return GenerateRandomMusic(seed, duration);
    }

    private const float SemiTone = 1.0594631f;
    private static readonly float[] CMajor = {
        261.63f, 293.66f, 329.63f, 349.23f, 392.00f, 440.00f, 493.88f
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
    
    private byte[] GenerateRandomMusic(int seed, int durationSeconds)
    {
        Random random = new Random(seed);
        int sampleRate = 44100;
        int channels = 2;
        int totalSamples = sampleRate * durationSeconds;
        int bpm = random.Next(70, 180);

        float[] notes = CreateKey(random, out bool isMajor);
        int[] progression = GetRandomProgression(random, isMajor);
        List<List<float>> chordsFrequencies = GetChordFrequencies(notes, progression);
        
        int samplesPerTact = sampleRate * 240 / bpm;
        int samplesPerDrum1 = samplesPerTact / 2;
        int samplesPerDrum2 = samplesPerTact / 4;
        
        using var ms = new MemoryStream();
        var waveFormat = new WaveFormat(sampleRate, 16, channels);
    
        using (var writer = new WaveFileWriter(ms, waveFormat))
        {
            for (int i = 0; i < totalSamples; i++)
            {
                int currentNote = (i / samplesPerTact) % progression.Length;
                double timeSinceTact = (double)(i % samplesPerTact) / sampleRate;
                double timeSinceDrum1 = (double)(i % samplesPerDrum1) / sampleRate;
                double timeSinceDrum2 = (double)(i % samplesPerDrum2) / sampleRate;
                
                float sample = GetSample(timeSinceTact, chordsFrequencies[currentNote]) * 0.05f;
                sample = ApplyOverdrive(sample, 10f, 1f);
                
                int positionInNote = i % samplesPerTact;
                float envelope = 1.0f;
                int fadeSamples = 1000;
            
                if (positionInNote < fadeSamples) 
                    envelope = positionInNote / (float)fadeSamples;
                else if (positionInNote > samplesPerTact - fadeSamples) 
                    envelope = (samplesPerTact - positionInNote) / (float)fadeSamples;
            
                sample *= envelope * 0.3f;
                
                sample += GetKickSample(timeSinceDrum1);
                sample += GetHiHatSample(timeSinceDrum2, random);
                
                writer.WriteSample(sample);
                writer.WriteSample(sample);
            }
        }
    
        return ms.ToArray();
    }

    private float[] KeyShift(float[] notes, int shift)
    {
        float totalFactor = (float)Math.Pow(SemiTone, shift);
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i] *= totalFactor;
        }
        return notes;
    }

    private float[] MinorFromMajor(float[] notes)
    {
        notes[2] /= SemiTone;
        notes[5] /= SemiTone;
        notes[6] /= SemiTone;
        return notes;
    }

    private float[] ExtendKey(float[] notes)
    {
        float[] extendedNotes = new float[notes.Length * 2];
        Array.Copy(notes, extendedNotes, notes.Length);
        for (int i = 0; i < notes.Length; i++)
        {
            extendedNotes[i + notes.Length] = notes[i] * 2;
        }
        return extendedNotes;
    }

    private float GetSample(double time, List<float> frequenciesPlaying)
    {
        float sample = 0;
        foreach (float frequency in frequenciesPlaying)
        {
            sample += (float)Math.Sin(2 * Math.PI * frequency * time);
        }
        return sample;
    }

    private List<List<float>> GetChordFrequencies(float[] notes, int[] progression)
    {
        List<List<float>> chords = new();
        for (int i = 0; i < progression.Length; i++)
        {
            chords.Add(new List<float>());
            for (int j = 0; j < StandardChordStages.Length; j++)
            {
                chords[i].Add(notes[progression[i] + StandardChordStages[j]]);
            }
        }
        return chords;
    }

    private float[] CreateKey(Random random, out bool isMajor)
    {
        float[] notes = new float[CMajor.Length];
        Array.Copy(CMajor, notes, CMajor.Length);
        KeyShift(notes, random.Next(-5, 5));
        isMajor = random.Next() % 1 == 0;
        if (!isMajor) MinorFromMajor(notes);
        return ExtendKey(notes);
    }

    private int[] GetRandomProgression(Random random, bool isMajor)
    {
        List<int[]> progressions = isMajor ? PopularMajorChordsProgressions : PopularMinorChordsProgressions;
        return progressions[random.Next(progressions.Count)];
    }
    
    float GetKickSample(double time)
    {
        double freq = 123 / (time + 1);  
        double amp = Math.Exp(-3.0 * time);
        return (float)(Math.Sin(2.0 * Math.PI * freq * time) * 0.5f * amp);
    }
    
    float GetHiHatSample(double time, Random random)
    {
        float amp = (float)Math.Exp(-4.0 * time);
        return (random.NextSingle() - 0.5f) * 0.2f * amp;
    }
    
    public static float ApplyOverdrive(float input, float drive, float mix)
    {
        float amplified = input * drive;
        float distorted = (float)Math.Tanh(amplified);
        return input * (1 - mix) + distorted * mix;
    }
}