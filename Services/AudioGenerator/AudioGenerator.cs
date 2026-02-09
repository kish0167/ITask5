using NAudio.Mixer;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace ITask5.Services.AudioGenerator;

public class AudioGenerator : IAudioGenerator
{
    public byte[] Generate(int seed, int duration)
    {
        Composition composition = new Composition(duration, seed);
        return composition.CreateWavTrack();
    }
}