using dotnetcore_desktop_app.Library;
using dotnetcore_desktop_app.Models;
using dotnetcore_desktop_app.ViewModels;
using EIPLibrary.WebCrawler;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using prjC349WebMVC.Library.WebCrawler;
using System.Collections;
using System.Net;
using static prjC349WebMVC.Library.WebCrawler.pbjg2F.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using System;
using System.IO;

namespace dotnetcore_desktop_app.Controllers
{
    public class ScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Schedule_Type2(string month_shift)
        {
            month_shift = month_shift == null ? "0" : month_shift;
            string filePath = @"History_attendance_data\" + DateTime.Now.AddMonths(Int32.Parse(month_shift)).ToString("yyyy-MM") + "_attendance.json";
            ScheduleViewModel deserializedViewModel = null;
            if (System.IO.File.Exists(filePath))
            {
                // Read the JSON string from the file
                string json = System.IO.File.ReadAllText(filePath);

                // Deserialize the JSON string to a ScheduleViewModel object
                deserializedViewModel = JsonConvert.DeserializeObject<ScheduleViewModel>(json);

            }

            return View(deserializedViewModel);
        }
        [HttpPost]
        public IActionResult Schedule_Type2(string userId, string userPassword, string month_shift)
        {
            EIP eip = new EIP(userId, userPassword);
            if (eip.isLogin == false)
            {
                return View();
            }
            pbjg2F tmp_pbjg2F = new pbjg2F(eip);
            List<string> EMPNOList = new List<string>();
            foreach (pbjg2F.Model.Employee employee in tmp_pbjg2F.memberList)
            {
                EMPNOList.Add(employee.EMPNO_PBA0);
            }
            pdwn tmp_pdwn = new pdwn(eip, EMPNOList, month_shift);

            ScheduleViewModel scheduleViewModel = new ScheduleViewModel();
            List<ScheduleModel> tmp_scheduleModelList = new List<ScheduleModel>();
            for (int i = 0; i < tmp_pbjg2F.memberList.Count; i++)
            {
                ScheduleModel schedule_model = new ScheduleModel()
                {
                    employee = tmp_pbjg2F.memberList[i],
                    WorkShiftList = tmp_pdwn.List[i].WorkShift
                };
                tmp_scheduleModelList.Add(schedule_model);
            }
            scheduleViewModel.schedules = tmp_scheduleModelList;

            DateTime qdate = DateTime.Today.Date.AddMonths(int.Parse(month_shift));
            var daysInMonth = DateTime.DaysInMonth(qdate.Year, qdate.Month);
            List<DateTime> tmp_datesList = new List<DateTime>();
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(qdate.Year, qdate.Month, day);
                tmp_datesList.Add(date);
            }
            scheduleViewModel.dates = tmp_datesList;
            WriteJson(scheduleViewModel);
            //WriteExcel(scheduleViewModel);
            return View(scheduleViewModel);
        }

        [HttpGet]
        public IActionResult Schedule(string month_shift)
        {
            month_shift = month_shift == null ? "0" : month_shift;
            string filePath = @"History_attendance_data\" + DateTime.Now.AddMonths(Int32.Parse(month_shift)).ToString("yyyy-MM") + "_attendance.json";
            ScheduleViewModel deserializedViewModel = null;
            if (System.IO.File.Exists(filePath))
            {
                // Read the JSON string from the file
                string json = System.IO.File.ReadAllText(filePath);

                // Deserialize the JSON string to a ScheduleViewModel object
                deserializedViewModel = JsonConvert.DeserializeObject<ScheduleViewModel>(json);

            }

            return View(deserializedViewModel);
        }
        [HttpPost]
        public IActionResult Schedule(string userId, string userPassword, string month_shift)
        {
            EIP eip = new EIP(userId, userPassword);
            if (eip.isLogin == false)
            {
                return View();
            }
            pbjg2F tmp_pbjg2F = new pbjg2F(eip);
            List<string> EMPNOList = new List<string>();
            foreach (pbjg2F.Model.Employee employee in tmp_pbjg2F.memberList)
            {
                EMPNOList.Add(employee.EMPNO_PBA0);
            }
            pdwn tmp_pdwn = new pdwn(eip, EMPNOList, month_shift);

            ScheduleViewModel scheduleViewModel = new ScheduleViewModel();
            List<ScheduleModel> tmp_scheduleModelList = new List<ScheduleModel>();
            for (int i = 0; i < tmp_pbjg2F.memberList.Count; i++)
            {
                ScheduleModel schedule_model = new ScheduleModel()
                {
                    employee = tmp_pbjg2F.memberList[i],
                    WorkShiftList = tmp_pdwn.List[i].WorkShift
                };
                tmp_scheduleModelList.Add(schedule_model);
            }
            scheduleViewModel.schedules = tmp_scheduleModelList;

            DateTime qdate = DateTime.Today.Date.AddMonths(int.Parse(month_shift));
            var daysInMonth = DateTime.DaysInMonth(qdate.Year, qdate.Month);
            List<DateTime> tmp_datesList = new List<DateTime>();
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(qdate.Year, qdate.Month, day);
                tmp_datesList.Add(date);
            }
            scheduleViewModel.dates = tmp_datesList;

            WriteExcel(scheduleViewModel);
            return View(scheduleViewModel);
        }

        static void WriteJson (ScheduleViewModel scheduleViewModel)
        {
            // Serializing the ScheduleViewModel instance to JSON
            string json = JsonConvert.SerializeObject(scheduleViewModel, Formatting.Indented);

            // Specify the file path where you want to save the JSON file
            string filePath = @"History_attendance_data\" + scheduleViewModel.dates.ElementAt(0).ToString("yyyy-MM") + "_attendance.json";

            // Write the JSON string to the file
            System.IO.File.WriteAllText(filePath, json);
        }
        static void WriteExcel(ScheduleViewModel scheduleViewModel)
        {
            // 產生一個新的Excel工作簿
            IWorkbook workbook = new XSSFWorkbook();

            // 建立一個工作表
            ISheet sheet = workbook.CreateSheet("班表");

            // 資料
            string[] headers = {"日期","姓名", "職工編號",  "班別" };
            //string[] data1 = { "John Doe", "30", "New York" };
            //string[] data2 = { "Jane Doe", "25", "Los Angeles" };

            // 建立表頭行
            IRow headerRow = sheet.CreateRow(0);
            for (int i = 0; i < headers.Length; i++)
            {
                headerRow.CreateCell(i).SetCellValue(headers[i]);
            }
            int row = 1;
            for (int employee = 0; employee < scheduleViewModel.schedules.Count; employee++)
            {
                for (int day = 0; day < scheduleViewModel.dates.Count; day++)
                {
                    // 建立資料行
                    IRow dataRow1 = sheet.CreateRow(row);
                    dataRow1.CreateCell(0).SetCellValue(scheduleViewModel.dates[day]);
                    dataRow1.CreateCell(1).SetCellValue(scheduleViewModel.schedules[employee].employee.CNAME_PBA0);
                    dataRow1.CreateCell(2).SetCellValue(scheduleViewModel.schedules[employee].employee.EMPNO_PBA0);
                    dataRow1.CreateCell(3).SetCellValue(scheduleViewModel.schedules[employee].WorkShiftList[day]);
                    row ++;
                }
            }

            // 儲存Excel檔案
            using (FileStream fileStream = new FileStream("attendance.xlsx", FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }

            Console.WriteLine("Excel檔案已成功建立。");
        }


        public static LogWorkshop.Model ReadLogWorkshopXLSM(string filePath, int sheetIndex)
        {
            IWorkbook wk = null;
            string extension = Path.GetExtension(filePath);

            LogWorkshop.Model uploadData = new LogWorkshop.Model();
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                if (extension.Equals(".xls"))
                {
                    //把xls檔案中的資料寫入wk中
                    wk = new HSSFWorkbook(fs);
                }
                else
                {
                    //把xlsx檔案中的資料寫入wk中
                    wk = new XSSFWorkbook(fs);
                }

                fs.Close();
                //讀取當前表資料

                //公式計算器，有了這一行後續就可以針對各個cell做計算
                XSSFFormulaEvaluator formula = new XSSFFormulaEvaluator(wk);

                var zz = wk.GetSheetName(sheetIndex);
                ISheet sheet = wk.GetSheetAt(sheetIndex);

                for (int i = 1; i <= sheet.LastRowNum; i++)  //LastRowNum 是當前表的總行數-1（注意）
                { }

                    IRow row = sheet.GetRow(1);
                ICell cell = row.GetCell(0);
                int row_index = 1;
                int column_index = 0;
                //民國年
                uploadData.TextBox14 = sheet.GetRow(row_index).GetCell(column_index).ToString();
                column_index += 2;
                //月
                uploadData.TextBox1 = sheet.GetRow(row_index).GetCell(column_index).ToString();
                column_index += 2;
                //日
                uploadData.DropDownList11 = sheet.GetRow(row_index).GetCell(column_index).ToString();

                row_index++;
                //單位
                column_index = 0;
                uploadData.DropDownList2 = sheet.GetRow(row_index).GetCell(column_index).ToString();
                column_index += 2;
                //班別
                uploadData.DropDownList4 = sheet.GetRow(row_index).GetCell(column_index).ToString();
                column_index += 2;

                row_index++;
                //值班負責人
                column_index = 1;
                uploadData.DropDownList5 = sheet.GetRow(row_index).GetCell(column_index).ToString();

                row_index++;
                //值班開始時間
                column_index = 2;
                uploadData.TextBox3 = sheet.GetRow(row_index).GetCell(column_index).ToString();
                column_index += 2;

                //值班結束時間
                uploadData.TextBox4 = sheet.GetRow(row_index).GetCell(column_index).ToString();

                row_index++;
                //代班負責人
                column_index = 1;
                uploadData.DropDownList6 = sheet.GetRow(row_index).GetCell(column_index).ToString();

                row_index++;
                //代班開始時間
                column_index = 2;
                uploadData.TextBox5 = sheet.GetRow(row_index).GetCell(column_index).ToString();
                column_index += 2;

                //代班結束時間
                uploadData.TextBox6 = sheet.GetRow(row_index).GetCell(column_index).ToString();

                row_index++;
                //天氣
                column_index = 1;
                uploadData.DropDownList3 = sheet.GetRow(row_index).GetCell(column_index).ToString();

                row_index++;
                //應到
                uploadData.TextBox7 = formula.EvaluateInCell(sheet.GetRow(row_index).GetCell(column_index)).ToString();
                column_index += 2;
                //實到
                uploadData.TextBox8 = formula.EvaluateInCell(sheet.GetRow(row_index).GetCell(column_index)).ToString();
                column_index += 2;
                //休假
                uploadData.TextBox9 = sheet.GetRow(row_index).GetCell(column_index).ToString();
                column_index += 2;

                row_index++;
                //病假
                column_index = 1;
                uploadData.TextBox10 = sheet.GetRow(row_index).GetCell(column_index).ToString();
                column_index += 2;
                //其他
                uploadData.TextBox11 = sheet.GetRow(row_index).GetCell(column_index).ToString();

                //記事時間
                row_index = 15;
                column_index = 0;
                var yy = formula.EvaluateInCell(sheet.GetRow(row_index).GetCell(column_index)).DateCellValue.ToString("HH:mm");
                Func<int, int, string> get_AA_Odd = (row_index, column_index) => formula.EvaluateInCell(sheet.GetRow(row_index).GetCell(column_index)).DateCellValue.ToString("HH:mm") != "00:00" ?
                    formula.EvaluateInCell(sheet.GetRow(row_index).GetCell(column_index)).DateCellValue.ToString("HH:mm") : "";


                uploadData.AA1 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA3 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA5 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA7 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA9 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA11 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA13 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA15 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA17 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA19 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA21 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA23 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA25 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA27 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA29 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA31 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA33 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA35 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA37 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA39 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA41 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA43 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA45 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA47 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA49 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA51 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA53 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA55 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA57 = get_AA_Odd(row_index, column_index);
                row_index++;
                uploadData.AA59 = get_AA_Odd(row_index, column_index);

                //記事內容
                row_index = 15;
                column_index = 1;
                Func<int, int, string> get_AA_Even = (row_index, column_index) => sheet.GetRow(row_index).GetCell(column_index).ToString() + formula.EvaluateInCell(sheet.GetRow(row_index).GetCell(column_index + 1)).ToString();

                //uploadData.AA2 = sheet.GetRow(row_index).GetCell(column_index).ToString() + formula.EvaluateInCell(sheet.GetRow(row_index).GetCell(column_index + 1)).ToString();
                uploadData.AA2 = get_AA_Even(row_index, column_index);
                row_index++;



                uploadData.AA4 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA6 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA8 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA10 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA12 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA14 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA16 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA18 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA20 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA22 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA24 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA26 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA28 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA30 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA32 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA34 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA36 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA38 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA40 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA42 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA44 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA46 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA48 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA50 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA52 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA54 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA56 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA58 = get_AA_Even(row_index, column_index);
                row_index++;
                uploadData.AA60 = get_AA_Even(row_index, column_index);


                //特別記載
                column_index = 2;
                row_index = 45;
                Func<int, int, string> get_AB_and_AC = (row_index, column_index) => sheet.GetRow(row_index).GetCell(column_index).ToString();

                uploadData.AB1 = get_AB_and_AC(row_index, column_index);
                row_index++;
                uploadData.AB2 = get_AB_and_AC(row_index, column_index);
                row_index++;
                uploadData.AB3 = get_AB_and_AC(row_index, column_index);
                row_index++;
                uploadData.AB4 = get_AB_and_AC(row_index, column_index);
                row_index++;
                uploadData.AB5 = get_AB_and_AC(row_index, column_index);
                row_index++;
                uploadData.AB6 = get_AB_and_AC(row_index, column_index);

                //特別記載
                row_index = 51;
                uploadData.AC1 = get_AB_and_AC(row_index, column_index);
                row_index++;
                uploadData.AC2 = get_AB_and_AC(row_index, column_index);
                row_index++;
                uploadData.AC3 = get_AB_and_AC(row_index, column_index);
                row_index++;
                uploadData.AC4 = get_AB_and_AC(row_index, column_index);
                row_index++;
                uploadData.AC5 = get_AB_and_AC(row_index, column_index);
                row_index++;
                uploadData.AC6 = get_AB_and_AC(row_index, column_index);

                return uploadData;
            }
            catch (Exception e)
            {
                //只在Debug模式下才輸出
                Console.WriteLine(e.Message);
                return uploadData;

            }
        }
    }
}
