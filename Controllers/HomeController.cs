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
using Newtonsoft.Json;
using System.IO;
using OpenQA.Selenium.Chrome;

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
            //IWebDriver driver = new EdgeDriver();
            //var url = "https://www.google.com";
            //driver.Navigate().GoToUrl(url);
            //service.DriverServicePath = driverLocation;

            var service = EdgeDriverService.CreateDefaultService(@".", "msedgedriver.exe");
            using (IWebDriver driver = new OpenQA.Selenium.Edge.EdgeDriver(service))
            {
                driver.Navigate().GoToUrl("https://www.google.com");
                var source = driver.PageSource;
                //IWebElement element = driver.FindElement(By.Name("q"));
                //element.SendKeys(myinput);
            }


            //IWebElement element = driver.FindElement(By.Name("q"));
            //element.SendKeys(myinput);
            return Json("TypeToGoogle done");
        }
        public IActionResult DragMove()
        {
            return View();
        }
        public JsonResult ReadFromLocal()
        {            
            string filePath = "SaveFile/save_1.json";
            string jsonContent = System.IO.File.ReadAllText(filePath);

            // 将 JSON 字符串转换为 C# 对象（根据之前的数据模型定义 YourDataModel 进行定义）
            LogWorkshopViewModel viewModel = JsonConvert.DeserializeObject<LogWorkshopViewModel>(jsonContent);
            return Json(viewModel);
        }
        [HttpPost]
        public JsonResult ReadFromLogWorkshop([FromBody] LogWorkshopViewModel modelData)
        {
            eip = new EIP(modelData.userId, modelData.userPassword);
            eip.Login();
            LogWorkshop logWorkshop = new LogWorkshop(eip, modelData.model);
            logWorkshop.Read();
            // 將資料填入ViewModel
            LogWorkshopViewModel viewModel = new LogWorkshopViewModel
            {
                userId = modelData.userId,
                userPassword = modelData.userPassword,
                model = logWorkshop.PostData,
                DutyOfficers = logWorkshop.dutyOfficer
            };
            // 将ViewModel转换为JSON字符串
            string json = JsonConvert.SerializeObject(viewModel, Formatting.Indented);

            // 将JSON字符串保存到文件
            string filePath = "SaveFile/save_1.json";
            System.IO.File.WriteAllText(filePath, json);

            return Json(viewModel);
        }

        [HttpPost]
        public JsonResult ReadFromKW96([FromBody] KW96ViewModel modelData)
        {
            eip = new EIP(modelData.userId, modelData.userPassword);
            eip.Login();
            KW96 logWorkshop = new KW96(eip, modelData.model);
            logWorkshop.Read();
            // 將資料填入ViewModel
            KW96ViewModel viewModel = new KW96ViewModel
            {
                userId = modelData.userId,
                userPassword = modelData.userPassword,
            };
            // 将ViewModel转换为JSON字符串
            string json = JsonConvert.SerializeObject(viewModel, Formatting.Indented);

            // 将JSON字符串保存到文件
            string filePath = "SaveFile/save_kw96.json";
            System.IO.File.WriteAllText(filePath, json);

            return Json(viewModel);
        }

        [HttpPost]
        public JsonResult WriteToLogWorkshop([FromBody] LogWorkshopViewModel modelData)
        {
            eip = new EIP(modelData.userId, modelData.userPassword);
            eip.Login();
            LogWorkshop logWorkshop = new LogWorkshop(eip, modelData.model);
            logWorkshop.Write();

            LogWorkshopViewModel viewModel = new LogWorkshopViewModel
            {
                userId = modelData.userId,
                userPassword = modelData.userPassword,
                model = logWorkshop.PostData,
                DutyOfficers = logWorkshop.dutyOfficer
            };
            // 将ViewModel转换为JSON字符串
            string json = JsonConvert.SerializeObject(viewModel, Formatting.Indented);

            // 将JSON字符串保存到文件
            string filePath = "SaveFile/save_1.json";
            System.IO.File.WriteAllText(filePath, json);
            ShellExecute(IntPtr.Zero, "open", logWorkshop.RedirectUrl, "", "", 1);
            return Json("WriteToLogWorkshop done");
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