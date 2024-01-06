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
            EIP eip = new EIP("214585", "791005");
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
