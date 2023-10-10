using dotnetcore_desktop_app.Models;
using dotnetcore_desktop_app.ViewModels;
using EIPLibrary.WebCrawler;
using Microsoft.AspNetCore.Mvc;
using prjC349WebMVC.Library.WebCrawler;
using System.Collections;
using System.Net;
using static prjC349WebMVC.Library.WebCrawler.pbjg2F.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace dotnetcore_desktop_app.Controllers
{
    public class ScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Schedule()
        {
            return View();
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
            return View(scheduleViewModel);
        }

    }
}
