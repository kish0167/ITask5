using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ITask5.Models;
using ITask5.Services;
using ITask5.Services.AudioGenerator;
using ITask5.Services.DataGenerator;

namespace ITask5.Controllers;

public class MainController(ILogger<MainController> logger, IDataGenerator dataGenerator, IAudioGenerator audioGenerator) : Controller
{
    private readonly ILogger<MainController> _logger = logger;
    private readonly IDataGenerator _dataGenerator = dataGenerator;
    private readonly IAudioGenerator _audioGenerator = audioGenerator;

    public IActionResult Index(string? seed, float? likes, int? page)
    {
        string language = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        return View(_dataGenerator.GeneratePage(language, seed, likes, page));
    }
    
    [HttpGet("music/stream/{songSeed}/{duration}")]
    public IActionResult Stream(int songSeed, int duration)
    {
        byte[] audioBytes = _audioGenerator.Generate(songSeed, duration);
        return File(audioBytes, "audio/wav", $"s{songSeed}.wav");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}