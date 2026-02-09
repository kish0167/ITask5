using ITask5.Services.AudioGenerator.ReverbEffect;
using NAudio.Wave;

namespace ITask5.Services.AudioGenerator;

public class Composition
{
    private const int SampleRate = 44100;
    private readonly int _samplesPerTact;
    private readonly int _samplesPerProgression;
    private readonly int _totalSamples;
    private readonly List<float[]> _chordsSamples;
    private readonly List<float[]> _drumsSamples;
    private readonly float[] _strikeSchema1;
    private readonly float[] _strikeSchema2;
    private readonly float[] _kickSchema;
    private readonly float[] _hatSchema;

    public Composition(int duration, int seed)
    {
        _totalSamples = SampleRate * duration;
        Random random = new Random(seed);
        int bpm = random.Next(70, 180);
        _samplesPerTact = SampleRate * 240 / bpm;
        _chordsSamples = Synthesizer.GenerateChordsSamples(_samplesPerTact, SampleRate, random);
        _drumsSamples = Synthesizer.GenerateDrumsSamples(_samplesPerTact, SampleRate, random);
        _samplesPerProgression = _samplesPerTact * _chordsSamples.Count;
        _strikeSchema1 = MusicTheory.GetRandomStrikeSchema(random);
        _strikeSchema2 = MusicTheory.GetRandomStrikeSchema(random);
        _kickSchema = MusicTheory.GetRandomDrumKickSchema(random);
        _hatSchema = MusicTheory.GetRandomDrumHatSchema(random);
    }
    
    public byte[] CreateWavTrack()
    {
        using var ms = new MemoryStream();
        var waveFormat = new WaveFormat(SampleRate, 16, 1);
        using (var writer = new WaveFileWriter(ms, waveFormat))
        {
            WriteToWav(writer);
        }
        return ms.ToArray();
    }

    private void WriteToWav(WaveFileWriter writer)
    {
        Reverb reverb = new Reverb(SampleRate);
        for (int i = 0; i < _totalSamples; i++)
        {
            float sample = ApplySchemas(i);
            sample = reverb.Process(sample);
            writer.WriteSample(sample);
        }
    }

    private float ApplySchemas(int i)
    {
        float sample = 0;
        sample = ApplyStrikeSchemas(i, sample);
        sample = ApplyDrumsSchemas(i, sample);
        return sample;
    }

    private float ApplyDrumsSchemas(int i, float sample)
    {
        foreach (float offset in _kickSchema)
        {
            sample += GetSampleWithOffset(_drumsSamples[0], i, offset) * 0.3f;
        }

        foreach (float offset in _hatSchema)
        {
            sample += GetSampleWithOffset(_drumsSamples[1], i, offset) * 0.3f;
        }

        return sample;
    }

    private float ApplyStrikeSchemas(int i, float sample)
    {
        int progressionSampleIndex = i % _samplesPerProgression;
        int chordIndex = progressionSampleIndex / _samplesPerTact;
        float[] strikeSchema = chordIndex % 2 == 1 ? _strikeSchema2 : _strikeSchema1;
        foreach (float offset in strikeSchema)
        {
            sample += GetSampleWithOffset(_chordsSamples[chordIndex], i, offset) * 0.03f;
        }
        return sample;
    }

    private float GetSampleWithOffset(float[] samples, int i, float offset)
    {
        if (offset < 0 || offset > 1)
        {
            offset = 0;
        }
        int tactSampleIndex = i % _samplesPerTact;
        int sampleOffset =tactSampleIndex - (int)(_samplesPerTact * offset);
        sampleOffset = sampleOffset < 0 ? sampleOffset + _samplesPerTact : sampleOffset;
        return samples[sampleOffset];
    }
}