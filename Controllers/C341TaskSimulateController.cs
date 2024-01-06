using Microsoft.AspNetCore.Mvc;
using EIPLibrary.WebCrawler;
using prjC349WebMVC.Library.WebCrawler;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

            List<IA7F.Model> tmp_list = tmp_IA7F.IA7FDataList;

            return View();
        }
        
    }
}
