using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASPNETITSTEP.Models;
using ASPNETITSTEP.Services.Hash;
using ASPNETITSTEP.Services.Time;

namespace ASPNETITSTEP.Controllers;
// primary constructor - прямо при оголощенні класу
public class HomeController(IHashService hashService, ITimeService timeService) : Controller
{
    // Інжекція через конструктор у формі Primary
    private readonly IHashService _hashService = hashService;
    private readonly ITimeService _timeService = timeService;
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Intro()
    {
        return View();
    }

    public IActionResult Razor()
    {
        return View();
    }

    public IActionResult IoC()
    {
        String digest = _hashService.Digest("123");
        // передача даних до представлення
        // варіанти спільних ресурсів
        ViewBag.Hash = _hashService.GetHashCode();
        ViewBag.Timestamp = _timeService.GetTimestamp();
        ViewData["digest"] = digest;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
