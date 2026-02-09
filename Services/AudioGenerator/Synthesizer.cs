namespace ITask5.Services.AudioGenerator;

public static class Synthesizer
{
    private const float MaxDrive = 2f;
    private const float MinMix = 0.2f;
    public static List<float[]> GenerateChordsSamples(int length, int sampleRate, Random random)
    {
        List<float[]> samples = new();
        List<List<float>> chordsFrequencies = MusicTheory.GetRandomChordsFrequencies(random);
        for (int i = 0; i < chordsFrequencies.Count; i++)
        {
            samples.Add(GenerateChordSamples(chordsFrequencies[i], length, sampleRate));
        }

        ApplyOverdrive(samples, random);
        return samples;
    }
    
    public static List<float[]> GenerateDrumsSamples(int length, int sampleRate, Random random)
    {
        List<float[]> samples = new();
        samples.Add(new float[length]);
        samples.Add(new float[length]);
        for (int i = 0; i < length; i++)
        {
            samples[0][i] = GetKickSample((double)i / sampleRate);
            samples[1][i] = GetHiHatSample((double)i / sampleRate, random);
        }
        return samples;
    }
    
    private static float[] GenerateChordSamples(List<float> frequencies, int length, int sampleRate)
    {
        float[] chordSamples = new float[length];
        for (int i = 0; i < length; i++)
        {
            double time = (double)i / sampleRate;
            chordSamples[i] = GetChordSample(time, frequencies);
        }
        return chordSamples;
    }
    
    private static float GetChordSample(double time, List<float> frequenciesPlaying)
    {
        float sample = 0;
        float amp = (float)Math.Exp(-1.4 * time);
        float amp2 = (float)Math.Exp(-1.8 * time);
        float amp3 = (float)Math.Exp(-2.1 * time);
        float amp4 = (float)Math.Exp(-2.6 * time);
        float amp5 = (float)Math.Exp(-3.5 * time);
        foreach (float frequency in frequenciesPlaying)
        {
            sample += (float)Math.Sin(2 * Math.PI * frequency * time);
            sample += (float)Math.Sin(2 * Math.PI * frequency * 2f * time) * amp2;
            sample += (float)Math.Sin(2 * Math.PI * frequency * 3f * time) * amp3;
            sample += (float)Math.Sin(2 * Math.PI * frequency * 4f * time) * amp4;
            sample += (float)Math.Sin(2 * Math.PI * frequency * 5f * time) * amp5;
            sample += (float)Math.Sin(2 * Math.PI * frequency * 6f * time) * amp5;
            sample += (float)Math.Sin(2 * Math.PI * frequency * 7f * time) * amp5;
        }
        return sample * amp / frequenciesPlaying.Count;
    }
    
    private static float GetKickSample(double time)
    {
        float sample = 0;
        double freq = 123 / (time + 1);  
        double amp = Math.Exp(-3.0 * time);
        return (float)(Math.Sin(2.0 * Math.PI * freq * time) * 0.5f * amp);
    }
    
    private static float GetHiHatSample(double time, Random random)
    {
        float amp = (float)Math.Exp(-4.0 * time);
        return (random.NextSingle() - 0.5f) * 0.2f * amp;
    }
    
    private static void ApplyOverdrive(List<float[]> input, Random random)
    {
        float drive = random.NextSingle() * (MaxDrive - 1) + 1;
        float mix = random.NextSingle() * (1 - MinMix) + MinMix;
        foreach (float[] samples in input)
        {
            for(int i = 0; i < samples.Length; i++)
            {
                float amplified = samples[i] * drive;
                float distorted = (float)Math.Tanh(amplified);
                samples[i] = samples[i] * (1 - mix) + distorted * mix;
            }
        }
    }
}