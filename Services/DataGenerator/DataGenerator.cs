using ITask5.Models;
using Microsoft.Extensions.Options;

namespace ITask5.Services.DataGenerator;

public class DataGenerator(IOptions<DataGeneratorOptions> options) : IDataGenerator
{
    private readonly DataGeneratorOptions _options = options.Value;

    public PageViewModel Generate(string? language, string? seed, float? likes, int? page)
    {
        GenerationParameters parameters = CreateGenerationParameters(language, seed, likes, page);

        List<SongViewModel> songs = new List<SongViewModel>(){new SongViewModel()
        {
            Album = "Intensity",
            Genre = "Rap",
            Artist = parameters.Language + " gotrin",
            CoverImageUrl = "https://picsum.photos/id/870/200/300?grayscale&blur=2",
            DurationSeconds = (int)(parameters.Likes * 10),
            Id = 1,
            Label = parameters.Seed,
            PreviewAudioUrl = "https://commondatastorage.googleapis.com/codeskulptor-demos/pyman_assets/ateapill.ogg",
            ReviewText = "this is page #" + parameters.Page,
            Title = "Never gonna give u up!",
            Year = 2025
        }};
        
        return new PageViewModel()
        {
            Songs = songs,
            CurrentPage = Math.Max(1, parameters.Page)
        };
    }

    private class GenerationParameters(string language, string seed, float likes, int page)
    {
        public string Language = language;
        public string Seed = seed;
        public float Likes = likes;
        public int Page = page;
    }
    private GenerationParameters CreateGenerationParameters(string? language, string? seed, float? likes, int? page)
    {
        language = !string.IsNullOrEmpty(language) ? language : _options.DefaultLanguage;
        seed = !string.IsNullOrEmpty(seed) ? seed : _options.DefaultSeed.ToString("x8");
        return new GenerationParameters(language, seed, likes ?? _options.DefaultLikes, page ?? _options.DefaultPage);
    }
}