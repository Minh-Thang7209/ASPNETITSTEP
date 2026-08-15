using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASPNETITSTEP.Models;
using ASPNETITSTEP.Services.Hash;
using ASPNETITSTEP.Services.Time;
using ASPNETITSTEP.Models.Home.Models;

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
    [HttpPost] // Обмежуємо використання сторінки лише методом POST
    public IActionResult ModelsForm(HomeModelsFormModel formModel)
    {
        return View(formModel);
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

    public IActionResult Models(String? id) // id - опціональний параметр маршруту
    {
        // Одна з центральних задач контролерів - підготовка і трансформація моделей
        HomeModelsViewModels viewModel = new()
        {
            PageTitle = "Моделі в ASP",
            Intro = "Модель (у MVC) - архітектура частина проєкту, які відповідає за взаємодію з даними. Модель (в ASP) - клас (обʼєкт), призначений для передачі даних (DTO - Data Transfer Object, Entity).",
            ClassificationHeader = "Розрізняють декілька типів моделей за призначенням:",
            ExampleHeader = "Наприклад, для моделі \"користувач\":",
            ClassificationList = [
              "Модель представлення (ViewModel або PageModel) - дані, з яких будується сторінка (або її частина - представлення)",
              "Модель форми (FormModel) - дані що, заповнюються користувачем на сторінці і передаються на обробку.",
              "Модель даних (DTO - Data Transfer Object, Entity) - дані, що зберігаються на постійній основі, частіше за все у БД",
            ],
            ExampleList = [
                "Модель форми (реєстрація) - логін, пароль, повтор пароля, ...",
                "Модель даних (у БД) - логін, DH(хеш пароля), сіль, ..., дата створення",
                "Модель представлення (профіль або кабінет) - логін, ..., дата створення (паролів немає)",
            ],
        };
        return id == "json" ? Json(viewModel) : View(viewModel); // передаємо модель (обʼєкт) до представлення
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
