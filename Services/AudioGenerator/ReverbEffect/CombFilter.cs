namespace ITask5.Services.AudioGenerator.ReverbEffect;

public class CombFilter
{
    private readonly float[] _buffer;
    private int _index;
    public float Feedback { get; set; } = 0.84f;

    public CombFilter(int delay)
    {
        _buffer = new float[delay];
    }

    public float Process(float input)
    {
        float output = _buffer[_index];
        _buffer[_index] = input + output * Feedback;
        if (++_index >= _buffer.Length) _index = 0;
        return output;
    }
}