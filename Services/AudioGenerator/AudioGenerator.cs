using ITask5.Models;
using ITask5.Services.DataGenerator;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace ITask5.Services.AudioGenerator;

public class AudioGenerator : IAudioGenerator
{
    public List<SongViewModel> AddAudio(List<SongViewModel> songs, ISession session, GenerationParameters parameters)
    {
        byte[] audioBytes = GenerateRandomMusic(15);
        var audioId = Guid.NewGuid().ToString("N");
        session.Set(audioId, audioBytes);
        songs[0].SessionAudioDataId = audioId;
        return songs;
    }

    public byte[] Generate(int seed)
    {
        return GenerateRandomMusic(10);
    }

    public byte[] GenerateRandomMusic(int durationSeconds = 10)
    {
        var sampleRate = 44100;
        var channels = 2;
        var totalSamples = sampleRate * durationSeconds;
    
        // C major scale frequencies (C4 to B4)
        var notes = new[] { 261.63f, 293.66f, 329.63f, 349.23f, 392.00f, 440.00f, 493.88f, 523.25f };
        var samplesPerNote = sampleRate / 2; // Half second per note
    
        using var ms = new MemoryStream();
        var waveFormat = new WaveFormat(sampleRate, 16, channels);
    
        using (var writer = new WaveFileWriter(ms, waveFormat))
        {
            for (int i = 0; i < totalSamples; i++)
            {
                // Sequential note selection
                var noteIndex = (i / samplesPerNote) % notes.Length;
                var frequency = notes[noteIndex];
            
                var time = (double)i / sampleRate;
                var sample = (float)Math.Sin(2 * Math.PI * frequency * time);
            
                // Simple envelope (attack/release)
                var positionInNote = i % samplesPerNote;
                var envelope = 1.0f;
                var fadeSamples = 2000;
            
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
}