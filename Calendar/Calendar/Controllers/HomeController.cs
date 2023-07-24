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
            var dates = new List<DateTime>();


            var LunarCalendar = new ChineseLunisolarCalendar();
            var SolarDate = new DateTime(2024, 3, 10, new ChineseLunisolarCalendar());
            //var yyyy = LunarCalendar.GetYear(SolarDate);
            //var MM = LunarCalendar.GetMonth(SolarDate); 
            //var dd = LunarCalendar.GetDayOfMonth(SolarDate);

            ////

            if (end < start)
            {
                ModelState.AddModelError("", "endDate must be greater than or equal to startDate");
                return View();
            }
            var daysOff = _context.Holidays.Select(d => d.holidays).ToList();
            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                var dataExist = _context.workingDays.Any(d => d.Date == dt);
                if (dataExist == false)
                {
                    var wDay = new WorkingDay()
                    {
                        Date = dt,
                    };
                    _context.Add(wDay);
                    _context.SaveChanges();
                }
                if (type == "Saturday-Non-working Company")
                {
                    // tính ngày nghỉ bù
                    // công ty nghỉ làm thứ 7
                    if ((dt.DayOfWeek.ToString() != "Sunday") && (dt.DayOfWeek.ToString() != "Saturday"))
                    {

                        foreach (DateTime dayoff in daysOff)
                        {
                            //GioTOHungVUong
                            if (dayoff.Month == 3 && dayoff.Day == 10)
                            {
                                var GioToHungVuongDuongLich = new DateTime(2022, dayoff.Month, dayoff.Day, new ChineseLunisolarCalendar());
                                if (GioToHungVuongDuongLich.DayOfWeek.ToString() == "Saturday")
                                {
                                    DateTime nextday = GioToHungVuongDuongLich.AddDays(2);
                                    //dates.Remove(nextday);

                                    var selected_day = _context.workingDays.First(d => d.Date == nextday);
                                    selected_day.Selection = true;
                                    _context.SaveChanges();
                                }
                                else if (GioToHungVuongDuongLich.DayOfWeek.ToString() == "Sunday")
                                {
                                    DateTime nextday = GioToHungVuongDuongLich.AddDays(1);
                                    var selected_day = _context.workingDays.FirstOrDefault(d => d.Date == nextday);
                                    if (selected_day != null)
                                    {
                                        selected_day.Selection = true;
                                        _context.SaveChanges();
                                    }

                                    _context.SaveChanges();
                                    //dates.Remove(nextday);

                                }
                            }
                            ////// nghỉ bù
                            else if (dayoff.DayOfWeek.ToString() == "Saturday")
                            {
                                DateTime nextday = dayoff.AddDays(1);
                                //var selectDay = _context.workingDays.First(d => d.Date == nextday);
                                //selectDay.Selection = true;
                                if (nextday.DayOfWeek.ToString() == "Sunday")
                                {
                                    foreach (var dayoff2 in daysOff)
                                    {
                                        if (nextday == dayoff2)
                                        {
                                            var nextdaysOff2 = nextday.AddDays(2);
                                            var selected_day = _context.workingDays.First(d => d.Date == nextdaysOff2);
                                            selected_day.Selection = true;
                                            _context.SaveChanges();
                                            //dates.Remove(nextdaysOff2);
                                        }
                                    }
                                    //dates.Remove(nextday);
                                }
                                else
                                {
                                    var nextdaysOff = nextday.AddDays(1);
                                    //dates.Remove(nextdaysOff);
                                    var selected_day = _context.workingDays.First(d => d.Date == nextdaysOff);
                                    selected_day.Selection = true;
                                    _context.SaveChanges();


                                }
                                var rostered_dayOff = dayoff.AddDays(2);
                                //dates.Remove(rostered_dayOff);
                                var selected_day2 = _context.workingDays.FirstOrDefault(d => d.Date == rostered_dayOff);
                                if (selected_day2 != null)
                                {
                                    selected_day2.Selection = true;
                                    _context.SaveChanges();
                                }

                                _context.SaveChanges();


                            }
                            else if (dayoff.DayOfWeek.ToString() == "Sunday")
                            {
                                var rostered_dayOff = dayoff.AddDays(1);
                                //dates.Remove(rostered_dayOff);
                                var selected_day2 = _context.workingDays.FirstOrDefault(d => d.Date == rostered_dayOff);
                                if (selected_day2 != null)
                                {
                                    selected_day2.Selection = true;
                                    _context.SaveChanges();
                                }

                                _context.SaveChanges();

                            }
                        }

                    }

                }
                // tính ngày nghỉ bù
                // công ty đi làm thứ 7
                else
                {
                    if (dt.DayOfWeek.ToString() != "Sunday")
                    {
                        //dates.Add(dt);
                        foreach (DateTime dayoff in daysOff)
                        {
                            if (dayoff.Month == 3 && dayoff.Day == 10)
                            {
                                var GioToHungVuongDuongLich = new DateTime(2022, 3, 10, new ChineseLunisolarCalendar());
                                if (GioToHungVuongDuongLich.DayOfWeek.ToString() == "Sunday")
                                {
                                    DateTime nextday = GioToHungVuongDuongLich.AddDays(1);
                                    //dates.Remove(nextday);
                                    var selected_day = _context.workingDays.FirstOrDefault(d => d.Date == nextday);
                                    if (selected_day != null)
                                    {
                                        selected_day.Selection = true;
                                        _context.SaveChanges();
                                    }

                                    _context.SaveChanges();
                                }
                            }
                            else if (dayoff.DayOfWeek.ToString() == "Sunday")
                            {
                                var rostered_dayOff = dayoff.AddDays(1);

                                //dates.Remove(rostered_dayOff);
                                var selected_day = _context.workingDays.FirstOrDefault(d => d.Date == rostered_dayOff);
                                if (selected_day != null)
                                {
                                    selected_day.Selection = true;
                                    _context.SaveChanges();
                                }
                                _context.SaveChanges();
                            }
                        }
                    }
                }
                // nghỉ làm!
                foreach (var date in _context.workingDays.ToList())
                {
                    foreach (DateTime dayoff in daysOff)
                    {
                        if (dayoff.Month == 3 && dayoff.Day == 10)
                        {
                            var GioToHungVuongDuongLich = new DateTime(2022, dayoff.Month, dayoff.Day, new ChineseLunisolarCalendar());
                            //dates.Remove(GioToHungVuongDuongLich);
                            var selected_day = _context.workingDays.FirstOrDefault(d => d.Date == GioToHungVuongDuongLich);
                            if (selected_day != null)
                            {
                                selected_day.IsHoliday = true;
                                _context.SaveChanges();
                            }

                        }
                        else if (dayoff.Day == date.Date.Value.Day && dayoff.Month == date.Date.Value.Month)
                        {
                            date.IsHoliday = true;
                            _context.SaveChanges();
                            //dates.Remove(date);
                        }
                    }
                    if (type == "Saturday-Non-working Company")
                    {
                        if (date.Date.Value.DayOfWeek.ToString() == "Saturday" || date.Date.Value.DayOfWeek.ToString() == "Sunday")
                        {
                            date.Weekend = true;
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        if(date.Date.Value.DayOfWeek.ToString() == "Saturday")
                        {
                            date.Weekend = false;

                        }
                        if (date.Date.Value.DayOfWeek.ToString() == "Sunday")
                        {
                            date.Weekend = true;

                        }
                        _context.SaveChanges();
                    }

                }
            }
            List<WorkingDay> list = _context.workingDays.Where(d => d.Date >= start && d.Date <= end).ToList();
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