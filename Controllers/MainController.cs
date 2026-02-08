using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ITask5.Models;
using ITask5.Services;
using ITask5.Services.DataGenerator;

namespace ITask5.Controllers;

public class MainController(ILogger<MainController> logger, IDataGenerator dataGenerator) : Controller
{
    private readonly ILogger<MainController> _logger = logger;
    private readonly IDataGenerator _dataGenerator = dataGenerator;

    public IActionResult Index(string? seed, float? likes, int? page)
    {
        string language = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        return View(_dataGenerator.GeneratePage(language, seed, likes, page, HttpContext.Session));
    }
    
    [HttpGet("music/stream/{audioId}")]
    public IActionResult Stream(string audioId)
    {
        var audioBytes = HttpContext.Session.Get(audioId);
        
        if (audioBytes == null || audioBytes.Length == 0)
            return NotFound("Audio expired or not found");
        
        return File(audioBytes, "audio/wav", $"{audioId}.wav");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}