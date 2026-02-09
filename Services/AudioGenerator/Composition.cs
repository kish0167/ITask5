using NAudio.Wave;

namespace ITask5.Services.AudioGenerator;

public class Composition
{
    private const int SampleRate = 44100;
    private readonly Random _random;
    private readonly int _bpm;
    private readonly int _samplesPerTact;
    private readonly int _samplesPerBeat;
    private readonly int _samplesPerProgression;
    private readonly int _totalSamples;
    private readonly List<float[]> _chordsSamples;
    private readonly List<float[]> _drumsSamples;
    private readonly float[] _strikeSchema;
    private readonly float[] _kickSchema;
    private readonly float[] _hatSchema;

    public Composition(int duration, int seed)
    {
        _totalSamples = SampleRate * duration;
        _random = new Random(seed);
        _bpm = _random.Next(70, 180);
        _samplesPerTact = SampleRate * 240 / _bpm;
        _samplesPerBeat = SampleRate * 60 / _bpm;
        _chordsSamples = Synthesizer.GenerateChordsSamples(_samplesPerTact, SampleRate, _random);
        _drumsSamples = Synthesizer.GenerateDrumsSamples(_samplesPerTact, SampleRate, _random);
        _samplesPerProgression = _samplesPerTact * _chordsSamples.Count;
        _strikeSchema = MusicTheory.GetRandomStrikeSchema(_random);
        _kickSchema = MusicTheory.GetRandomDrumKickSchema(_random);
        _hatSchema = MusicTheory.GetRandomDrumHatSchema(_random);
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
        for (int i = 0; i < _totalSamples; i++)
        {
            float sample = ApplySchemas(i);
            writer.WriteSample(sample);
        }
    }

    private float ApplySchemas(int i)
    {
        float sample = 0;
        int progressionSampleIndex = i % _samplesPerProgression;
        int chordIndex = progressionSampleIndex / _samplesPerTact;

        foreach (float offset in _strikeSchema)
        {
            sample += GetSampleWithOffset(_chordsSamples[chordIndex], i, offset) * 0.03f;
        }
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