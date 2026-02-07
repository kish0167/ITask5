using ITask5.Models;
using ITask5.Services.DataGenerator;

namespace ITask5.Services.TextGenerator;

public interface ITextGenerator
{
    public List<SongViewModel> GenerateSongsWithText(GenerationParameters parameters);
}