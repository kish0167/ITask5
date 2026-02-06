namespace ITask5.Models;

public class PageViewModel
{
    public PageViewModel()
    {
        Songs = new List<SongViewModel>();
        CurrentPage = 1;
    }
    
    public int CurrentPage { get; set; }
    public List<SongViewModel> Songs { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < Int32.MaxValue;
    public static int Capacity => 30;
}