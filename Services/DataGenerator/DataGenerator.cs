using ITask5.Models;

namespace ITask5.Services;

public class DataGenerator : IDataGenerator
{
    public PageViewModel Generate(string language, string seed, float likes, int page)
    {
        List<SongViewModel> songs = new List<SongViewModel>(){new SongViewModel()
        {
            Album = "Intensity",
            Genre = "Rap",
            Artist = language + " gotrin",
            CoverImageUrl = "https://picsum.photos/id/870/200/300?grayscale&blur=2",
            DurationSeconds = (int)(likes * 10),
            Id = 1,
            Label = seed,
            PreviewAudioUrl = "https://commondatastorage.googleapis.com/codeskulptor-demos/pyman_assets/ateapill.ogg",
            ReviewText = "this is page #" + page,
            Title = "Never gonna give u up!",
            Year = 2025
        }};
        
        return new PageViewModel()
        {
            Songs = songs,
            CurrentPage = Math.Max(1, page)
        };
    }
}