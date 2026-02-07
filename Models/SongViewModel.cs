namespace ITask5.Models;

public class SongViewModel
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public string? Album { get; set; }
    public string? Genre { get; set; }
    
    public string? CoverImageUrl { get; set; }
    public string? PreviewAudioUrl { get; set; }
    public int? DurationSeconds { get; set; }
    public int? Year { get; set; }
    public int? Likes { get; set; }
    public string? Label { get; set; }
}