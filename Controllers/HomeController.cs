using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GP2.Controllers
{
    public class HomeController : Controller
    {
        private AppointmentContext appoinment { get; set; }

        public HomeController(AppoinementContext_app)
        {
            appoinment = _app;
        }
        public IActionResult Index()
        {
            //This removes the key-value pairs, date & time in session:
            HttpContext.Session.Remove("date");
            HttpContext.Session.Remove("time");
            return View();
        }

        //Sign Up 
        public IActionResult SignUp(string currentdate)
        {
            //This removes the key-value pairs, date & time in session:
            HttpContext.Session.Remove("date");
            HttpContext.Session.Remove("time");

            //Set the date to today if the currentdate is null:
            if (currentdate == null)
            {
                currentdate = DateTime.Now.ToString("MM/dd/yyyy");
            }
            ViewBag.CurrentDate = currentdate;

            //This is the day formatting:
            string[] dateformat = { "MM/dd/yyyy" };
            DateTime currentTime1 = DateTime.ParseExact(currentdate, dateformat, new CultureInfo("en-US"), DateTimeStyles.None);

            ViewBag.Disable = false;

            if (currentTime.AddDays(-1) < DateTime.Today)
            {
                ViewBag.Disable = true;
            }
            // This gets a list of records based on the date from DB:
            var record = appointment.Appointments.Where(i => i.Date == currentdate).ToList();
            
            return View(record);
        }

        public IActionResult Appointments()
        {
            //This removes the key-value pairs, date & time in session:
            HttpContext.Session.Remove("date");
            HttpContext.Session.Remove("time");

            //This gets a list of every record from DB:
            var record = appointment.Appointments.OrderByDescending(i => i.Date).ToList();

            return View(record);
        }

        public IActionResult PreviousDate(string currentDate)
        {
            //This removes the key-value pairs, date & time in session:
            HttpContext.Session.Remove("date");
            HttpContext.Session.Remove("time");

            //This is the day formatting:
            string[] dateformat = { "MM/dd/yyyy" };
            DateTime currentTime2 = DateTime.ParseExact(currentDate, dateformat, new CultureInfo("en-US"), DateTimeStyles.None);
            DateTime previousDate = currentTime2.AddDays(-1);
            string previous = previousDate.ToString("MM/dd/yyyy");

            ViewBag.currentDate = previous;

            //Redirecting to the SignUp page with previous Date 
            return RedirectToAction("SignUp", new { currentTime2 = previous });
        }

        public IActionResult NextDate(string )
    }
}
