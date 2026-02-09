namespace ITask5.Services.AudioGenerator.ReverbEffect;

public class AllPassFilter
{
    private readonly float[] _buffer;
    private int _index;
    private const float G = 0.5f;

    public AllPassFilter(int delay)
    {
        _buffer = new float[delay];
    }

    public float Process(float input)
    {
        float bufOut = _buffer[_index];
        float output = -G * input + bufOut;
        _buffer[_index] = input + G * bufOut;
        if (++_index >= _buffer.Length) _index = 0;
        return output;
    }
}