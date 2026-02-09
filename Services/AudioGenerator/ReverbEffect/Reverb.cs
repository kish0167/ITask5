namespace ITask5.Services.AudioGenerator.ReverbEffect;

public class Reverb
{
    private readonly CombFilter[] _combs;
    private readonly AllPassFilter[] _allpasses;
    private readonly float _wet = 0.3f;
    private readonly float _dry = 0.7f;
    public Reverb(int sampleRate)
    {
        int[] combDelays = { 
            (int)(0.0297 * sampleRate),
            (int)(0.0371 * sampleRate), 
            (int)(0.0411 * sampleRate),
            (int)(0.0437 * sampleRate) 
        };
        
        int[] allPassDelays = { 
            (int)(0.0050 * sampleRate),
            (int)(0.0017 * sampleRate) 
        };

        _combs = new CombFilter[4];
        for (int i = 0; i < 4; i++)
        {
            _combs[i] = new CombFilter(combDelays[i]);
        }
        _allpasses = new AllPassFilter[2];
        for (int i = 0; i < 2; i++)
        {
            _allpasses[i] = new AllPassFilter(allPassDelays[i]);
        }
            
    }

    public float Process(float input)
    {
        float combSum = 0;
        foreach (var c in _combs)
        {
            combSum += c.Process(input);
        }
        float signal = combSum * 0.25f;
        foreach (var ap in _allpasses)
        {
            signal = ap.Process(signal);
        }
        return input * _dry + signal * _wet;
    }
}