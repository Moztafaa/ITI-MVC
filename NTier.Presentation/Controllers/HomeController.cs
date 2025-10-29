using System.Diagnostics;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NTier.Presentation.Models;

namespace NTier.Presentation.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public void PrintInConsole()
    {
        Console.WriteLine("Hangfire Job Executed at: " + DateTime.Now);
    }
    public IActionResult Hang()
    {
        System.Console.WriteLine("Hangfire Job Scheduled at: " + DateTime.Now);
        // BackgroundJob.Enqueue(() => PrintInConsole());
        // BackgroundJob.Schedule(() => PrintInConsole(), TimeSpan.FromSeconds(10));
        RecurringJob.AddOrUpdate("PrintInConsoleJob", () => PrintInConsole(), Cron.Minutely);
        System.Console.WriteLine("Hangfire Job Enqueued at: " + DateTime.Now);

        return View();
    }

    [HttpGet]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        return LocalRedirect(returnUrl);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
