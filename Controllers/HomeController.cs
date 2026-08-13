using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASPNETITSTEP.Models;

namespace ASPNETITSTEP.Controllers;

public class HomeController : Controller
{
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
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
