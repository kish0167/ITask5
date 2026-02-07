namespace ITask5.Services.DataGenerator;

public class GenerationParameters(string language, long seed, float likes, int page, int songsPerPage)
{
    public readonly string Language = language;
    public readonly long Seed = seed;
    public readonly float Likes = likes;
    public readonly int Page = page;
    public readonly int SongsPerPage = songsPerPage;
}