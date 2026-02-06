using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ITask5.Models;

namespace ITask5.Controllers;

public class MainController(ILogger<MainController> logger) : Controller
{
    private readonly ILogger<MainController> _logger = logger;

    public IActionResult Index(string language, string seed, float likes, int page = 1)
    {
        List<SongViewModel> songs = new List<SongViewModel>(){new SongViewModel()
        {
            Album = "Intensity",
            Genre = "Rap",
            Artist = "gotrin",
            CoverImageUrl = "https://picsum.photos/id/870/200/300?grayscale&blur=2",
            DurationSeconds = 195,
            Id = 1,
            Label = "Roger that!",
            PreviewAudioUrl = "https://commondatastorage.googleapis.com/codeskulptor-demos/pyman_assets/ateapill.ogg",
            ReviewText = "suuuuper cool!",
            Title = "Never gonna give u up!",
            Year = 2025
        }};
        
        return View(new PageViewModel()
        {
            Songs = songs,
            CurrentPage = page
        });
    }
    
    [HttpGet]
    public IActionResult GetDetails(int id)
    {
        //var song = GetSongById(id);
        //return PartialView("_SongDetails", song);
        return Redirect("/Main");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}