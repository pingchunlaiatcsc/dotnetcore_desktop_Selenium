using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnetcore_desktop_app.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;

namespace dotnetcore_desktop_app.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var methods = typeof(HomeController)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => typeof(IActionResult).IsAssignableFrom(m.ReturnType) && m.Name != "Index" && m.Name != "Privacy" && m.Name != "Error" && m.Name != "TypeToGoogle" && m.Name != "OpenFolder")
            .Select(m => new ActionMethodInfo
            {
                Name = m.Name,
                Url = Url.Action(m.Name)
            })
            .ToList();

        return View(methods);
    }
    [HttpPost]
    public JsonResult TypeToGoogle(string myinput)
    {
        IWebDriver driver = new EdgeDriver();
        var url = "https://www.google.com";
        driver.Navigate().GoToUrl(url);

        IWebElement element = driver.FindElement(By.Name("q"));
        element.SendKeys(myinput);
        return Json("TypeToGoogle done");
    }
    public IActionResult DragMove()
    {
        return View();
    }
    [HttpPost]
    public JsonResult OpenFolder(string myinput)
    {
        System.Diagnostics.Process.Start("explorer", myinput);
        return Json("OpenFolder done");
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
