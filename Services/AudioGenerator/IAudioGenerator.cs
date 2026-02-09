using ITask5.Helpers;
using ITask5.Models;
using ITask5.Services.DataGenerator;

namespace ITask5.Services.AudioGenerator;

public interface IAudioGenerator
{
    public byte[] Generate(int seed, int duration);
}