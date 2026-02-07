namespace ITask5.Services.DataGenerator;

public class DataGeneratorOptions
{
    public string DefaultLanguage { get; set; } = Languages.Default;
    public long DefaultSeed { get; set; } = 0;
    public float DefaultLikes { get; set; } = 5f;
    public int DefaultPage { get; set; } = 1;
    public int SongsPerPage { get; set; } = 30;
}