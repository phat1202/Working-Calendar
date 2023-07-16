using Calendar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Specialized;
using System.Diagnostics;

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
        public IActionResult Index(DateTime start, DateTime end)
        {
            var dates = new List<DateTime>();
            if (end < start)
            {
                ModelState.AddModelError("", "endDate must be greater than or equal to startDate");
                return View();
            }
            var daysOff = _context.Holidays.First(d => d.Number != 0).holidays;
            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {

                //if (dt != daysOff)
                //{
                //    dates.Add(dt);
                //}
               dates.Add(dt);
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