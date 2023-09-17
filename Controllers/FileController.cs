using dotnetcore_desktop_app.Models;
using EIPLibrary.WebCrawler;
using Microsoft.AspNetCore.Mvc;

namespace dotnetcore_desktop_app.Controllers
{
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult OpenFile(string path)
        {            
            if (System.IO.File.Exists(path))
            {
                System.Diagnostics.Process.Start("explorer", path);
                return Json("OpenFolder done");
            }
            else if (Directory.Exists(path))
            {
                // 路徑是資料夾，直接開啟資料夾
                System.Diagnostics.Process.Start("explorer", path);
                return Json("OpenFolder done");
            }
            else
            {
                Console.WriteLine("路徑不存在");
                return Json("path not exist");
            }

        }

        [HttpPost]
        public JsonResult TraverseDirectory([FromBody] TraverseDirectoryViewModel modelData)
        {
            List<string> resultPaths = new List<string>();
            resultPaths = TraverseDirectory(modelData.TargetPath);
            // 將資料填入ViewModel
            TraverseDirectoryViewModel viewModel = new TraverseDirectoryViewModel
            {
                TargetPath = modelData.TargetPath,
                ResultPaths = resultPaths,
                Count = resultPaths.Count
            };
            return Json(viewModel);
        }
        public List<string> TraverseDirectory(string directoryPath)
        {
            List<string> outputList = new List<string>();

            try
            {
                // 取得目錄中的檔案清單
                string[] files = Directory.GetFiles(directoryPath);
                foreach (string file in files)
                {
                    // 處理檔案 (在這裡你可以進行你需要的處理)
                    outputList.Add(file);
                }

                // 遞迴處理子目錄
                string[] subDirectories = Directory.GetDirectories(directoryPath);
                foreach (string subDirectory in subDirectories)
                {
                    outputList.Add(subDirectory);
                    List<string> subOutputList = TraverseDirectory(subDirectory); // 遞迴呼叫 TraverseDirectory
                    outputList.AddRange(subOutputList);
                }

                return outputList;
            }
            catch (Exception ex)
            {
                // 處理例外狀況
                Console.WriteLine("發生例外狀況：" + ex.Message);
                return outputList;
            }
        }
    }
}
