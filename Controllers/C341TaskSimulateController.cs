using Microsoft.AspNetCore.Mvc;
using EIPLibrary.WebCrawler;
using prjC349WebMVC.Library.WebCrawler;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using dotnetcore_desktop_app.Models;

namespace dotnetcore_desktop_app.Controllers
{
    public class C341TaskSimulateController : Controller
    {
        public IActionResult Index()
        {
            string userId = "";
            string userPassword = "";
            EIP eip = new EIP(userId, userPassword);
            if (eip.isLogin == false)
            {
                return View();
            }
            IA7F tmp_IA7F = new IA7F(eip);
            IA77 tmp_IA77 = new IA77(eip);

            foreach (IA7F.Model model in tmp_IA7F.IA7FDataList)
            {
                foreach (string area in model.areaList)
                {
                    tmp_IA77.GetLocData($"{model.warehouse}{area}");
                }

            }

            ViewData["dataforview"] = tmp_IA77.IA77DataList;
            return View();
        }

    }
}
