using System.Globalization;
using ITask5.Models;
using ITask5.Services.AudioGenerator;
using Microsoft.Extensions.Options;
using ITask5.Services.TextGenerator;

namespace ITask5.Services.DataGenerator;

public class DataGenerator(IOptions<DataGeneratorOptions> options, ITextGenerator textGenerator, IAudioGenerator audioGenerator) : IDataGenerator
{
    private readonly DataGeneratorOptions _options = options.Value;
    private readonly ITextGenerator _textGenerator = textGenerator;
    private readonly IAudioGenerator _audioGenerator = audioGenerator;

    public PageViewModel GeneratePage(string? language, string? seed, float? likes, int? page, ISession session)
    {
        GenerationParameters parameters = CreateGenerationParameters(language, seed, likes, page);
        List<SongViewModel> songs = _textGenerator.GenerateSongsWithText(parameters);
        songs = _audioGenerator.AddAudio(songs, session, parameters);
        return new PageViewModel()
        {
            Songs = songs,
            CurrentPage = parameters.Page
        };
    }
    
    private GenerationParameters CreateGenerationParameters(string? language, string? seed, float? likes, int? page)
    {
        language = !string.IsNullOrEmpty(language) ? language : _options.DefaultLanguage;
        if (!long.TryParse(seed, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out long seedLong))
        {
            seedLong = _options.DefaultSeed;
        }
        return new GenerationParameters(language, seedLong, likes ?? _options.DefaultLikes,
            page ?? _options.DefaultPage, _options.SongsPerPage);
    }
}