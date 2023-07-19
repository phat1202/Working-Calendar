using Calendar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Calendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CalendarContext _context;

        public HomeController(ILogger<HomeController> logger, CalendarContext context)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index(DateTime start, DateTime end, string type)
        {
            //List<DateTime> myDaysOff = new List<DateTime>();
            //myDaysOff.Add(new DateTime(2023, 1, 1));
            //myDaysOff.Add(new DateTime(2023, 4, 30));
            //myDaysOff.Add(new DateTime(2023, 5, 1));
            //myDaysOff.Add(new DateTime(2023, 9, 2));
            ////;
            ///

            var LunarCalendar = new ChineseLunisolarCalendar();
            var SolarDate = new DateTime(2024, 3, 10, new ChineseLunisolarCalendar());
            //var yyyy = LunarCalendar.GetYear(SolarDate);
            //var MM = LunarCalendar.GetMonth(SolarDate); 
            //var dd = LunarCalendar.GetDayOfMonth(SolarDate);

            ////
            var dates = new List<DateTime>();
            if (end < start)
            {
                ModelState.AddModelError("", "endDate must be greater than or equal to startDate");
                return View();
            }
            var daysOff = _context.Holidays.Select(d => d.holidays).ToList();
            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                if (type == "Saturday-Non-working Company")
                {
                    if ((dt.DayOfWeek.ToString() != "Sunday") && (dt.DayOfWeek.ToString() != "Saturday"))
                    {
                        dates.Add(dt);
                        
                        foreach (DateTime dayoff in daysOff)
                        {
                            if (dayoff.DayOfWeek.ToString() == "Saturday")
                            {
                                DateTime nextday = dayoff.AddDays(1);
                                if (nextday.DayOfWeek.ToString() == "Sunday")
                                {
                                    foreach (var dayoff2 in daysOff)
                                    {
                                        if (nextday == dayoff2)
                                        {
                                            var nextdaysOff2 = nextday.AddDays(2);
                                            dates.Remove(nextdaysOff2);
                                        }
                                    }
                                    dates.Remove(nextday);
                                }
                                else
                                {
                                    var nextdaysOff = nextday.AddDays(1);
                                    dates.Remove(nextdaysOff);
                                }
                                var rostered_dayOff = dayoff.AddDays(2);
                                dates.Remove(rostered_dayOff);
                            }
                            if (dayoff.DayOfWeek.ToString() == "Sunday")
                            {
                                var rostered_dayOff = dayoff.AddDays(1);
                                dates.Remove(rostered_dayOff);
                            }
                        }
                    }
                }
                else
                {
                    if (dt.DayOfWeek.ToString() != "Sunday")
                    {
                        dates.Add(dt);
                        foreach (DateTime dayoff in daysOff)
                        {
                            if (dayoff.DayOfWeek.ToString() == "Sunday")
                            {
                                var rostered_dayOff = dayoff.AddDays(1);
                                dates.Remove(rostered_dayOff);
                            }
                        }
                    }
                }
                foreach (var date in dates.ToList())
                {
                    foreach (DateTime dayoff in daysOff)
                    {
                        if (dayoff.Day == date.Day && dayoff.Month == date.Month)
                        {
                            dates.Remove(date);

                        }
                    }
                }
            }
            var list = new List<DateTime>(dates);
            return View(list);
        }
        public IActionResult AddHoliday()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddHoliday(Holiday holiday)
        {
            _context.Add(holiday);
            _context.SaveChanges();
            return RedirectToAction("Index");
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