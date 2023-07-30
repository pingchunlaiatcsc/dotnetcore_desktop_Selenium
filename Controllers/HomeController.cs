using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnetcore_desktop_app.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;
using System.Data;
using EIPLibrary.WebCrawler;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices;


namespace dotnetcore_desktop_app.Controllers
{

    public class HomeController : Controller
    {
        // ...

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        // ...
        private readonly ILogger<HomeController> _logger;
        EIP eip;
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
        public JsonResult EIP_API(string userId, string userPassword)
        {
            eip = new EIP(userId, userPassword);

            if (eip.Login() == false)
            {
                return Json("fail");
            }
            else
            {
                return Json("success");
            }
        }
        [HttpPost]
        public JsonResult TypeToGoogle(string myinput)
        {
            IWebDriver driver = new EdgeDriver();
            var url = "https://www.google.com";
            driver.Navigate().GoToUrl(url);
            //service.DriverServicePath = driverLocation;

            IWebElement element = driver.FindElement(By.Name("q"));
            element.SendKeys(myinput);
            return Json("TypeToGoogle done");
        }
        public IActionResult DragMove()
        {
            return View();
        }
        [HttpPost]
        public JsonResult PostToLogWorkshop([FromBody] LogWorkshopViewModel modelData)
        {
            eip = new EIP(modelData.userId, modelData.userPassword);
            eip.Login();
            LogWorkshop logWorkshop = new LogWorkshop(eip, modelData.model);
            logWorkshop.Write();

            ShellExecute(IntPtr.Zero, "open", logWorkshop.RedirectUrl, "", "", 1);
            return Json("PostToLogWorkshop done");
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
}