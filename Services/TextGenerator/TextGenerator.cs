using ITask5.Models;
using ITask5.Services.DataGenerator;
using Bogus;
using Bogus.DataSets;

namespace ITask5.Services.TextGenerator;

public class TextGenerator : ITextGenerator
{
    public List<SongViewModel> GenerateSongsWithText(GenerationParameters parameters)
    {
        Faker<SongViewModel> songFaker = CreateFaker(parameters);
        songFaker.UseSeed(GetSeedForFaker(parameters));
        List<SongViewModel> songs = songFaker.Generate(parameters.SongsPerPage);
        GenerateIndividualSeeds(songs, parameters);
        return songs;
    }

    private Faker<SongViewModel> CreateFaker(GenerationParameters parameters)
    {
        return new Faker<SongViewModel>(parameters.Language)
            .RuleFor(s => s.Id, f => f.IndexFaker + 1 + parameters.SongsPerPage * (parameters.Page - 1))
            .RuleFor(s => s.Title, f => GenerateSongTitle(f, parameters.Language))
            .RuleFor(s => s.Artist, f => f.Name.FullName())
            .RuleFor(s => s.Album, f => GenerateAlbumTitle(f, parameters.Language))
            .RuleFor(s => s.Genre, f => f.Music.Genre())
            .RuleFor(s => s.CoverImageUrl, f => f.Image.PicsumUrl(300, 300))
            .RuleFor(s => s.DurationSeconds, f => f.Random.Int(110, 420))
            .RuleFor(s => s.Year, f => f.Random.Int(1950, 2026))
            .RuleFor(s => s.Likes, f => f.Random.Int(0, 10))
            .RuleFor(s => s.Label, f => f.Company.CompanyName());
    }

    private string GenerateAlbumTitle(Faker faker, string locale)
    {
        return "abc";
    }

    private string GenerateSongTitle(Faker f, string locale)
    {
        return "Song!";
    }
    
    private int GetSeedForFaker(GenerationParameters parameters)
    {
        return HashCode.Combine(parameters.Seed, parameters.Page);
    }
    
    private void GenerateIndividualSeeds(List<SongViewModel> songs, GenerationParameters parameters)
    {
        foreach (SongViewModel song in songs)
        {
            song.Seed = HashCode.Combine(GetSeedForFaker(parameters), song.Id);;
        }
    }
}