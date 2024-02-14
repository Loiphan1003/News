using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace News.Controllers;

public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(ILogger<ProfileController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
     public IActionResult Index()
    {
        return View();
    }

    public IActionResult SendPost()
    {
        return View();
    }
}