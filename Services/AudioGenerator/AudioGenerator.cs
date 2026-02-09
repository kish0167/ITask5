using ITask5.Models;
using ITask5.Services.DataGenerator;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace ITask5.Services.AudioGenerator;

public class AudioGenerator : IAudioGenerator
{
    public byte[] Generate(int seed, int duration)
    {
        return GenerateRandomMusic(seed, duration);
    }

    private static readonly float[] CMajor = {
        261.63f, 293.66f, 329.63f, 349.23f, 392.00f, 440.00f, 493.88f, 523.25f
    };
    private const float SemiTone = 1.0594631f;

    public byte[] GenerateRandomMusic(int seed, int durationSeconds)
    {
        Random random = new Random(seed);
        int sampleRate = 44100;
        int channels = 2;
        int totalSamples = sampleRate * durationSeconds;
        
        float[] notes = new float[CMajor.Length];
        Array.Copy(CMajor, notes, CMajor.Length);
        KeyShift(notes, random.Next(-5, 5));
        MajorMinorKey(notes, random.Next() % 2);
        
        int samplesPerNote = sampleRate / 2; 
        using var ms = new MemoryStream();
        var waveFormat = new WaveFormat(sampleRate, 16, channels);
    
        using (var writer = new WaveFileWriter(ms, waveFormat))
        {
            for (int i = 0; i < totalSamples; i++)
            {
                int noteIndex = (i / samplesPerNote) % notes.Length;
                float frequency = notes[noteIndex];
            
                double time = (double)i / sampleRate;
                float sample = (float)Math.Sin(2 * Math.PI * frequency * time);
                
                int positionInNote = i % samplesPerNote;
                float envelope = 1.0f;
                int fadeSamples = 2000;
            
                if (positionInNote < fadeSamples) 
                    envelope = positionInNote / (float)fadeSamples;
                else if (positionInNote > samplesPerNote - fadeSamples) 
                    envelope = (samplesPerNote - positionInNote) / (float)fadeSamples;
            
                sample *= envelope * 0.3f;
            
                // Stereo
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

    private float[] MajorMinorKey(float[] notes, int value)
    {
        if (value > 0)
        {
            notes[2] /= SemiTone;
            notes[5] /= SemiTone;
            notes[6] /= SemiTone;
        }
        return notes;
    }
}