using GP2.Models;
using Microsoft.AspNetCore.Http;
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
        private AppointmentContext repo { get; set; }

        public HomeController(AppointmentContext temp)
        {
            repo = temp;
        }
        public IActionResult Index()
        {
            //This removes the key-value pairs, date & time in session:
            HttpContext.Session.Remove("date");
            HttpContext.Session.Remove("time");
            return View();
        }

        //Sign Up 
        public IActionResult SignUp(string currentDate)
        {
            //This removes the key-value pairs, date & time in session:
            HttpContext.Session.Remove("date");
            HttpContext.Session.Remove("time");

            //Set the date to today if the currentdate is null:
            if (currentDate == null)
            {
                currentDate = DateTime.Now.ToString("MM/dd/yyyy");
            }
            ViewBag.CurrentDate = currentDate;

            //This is the day formatting:
            string[] dateformat = { "MM/dd/yyyy" };
            DateTime currentTime = DateTime.ParseExact(currentDate, dateformat, System.Globalization.CultureInfo.InvariantCulture);

            ViewBag.Disable = false;

            if (currentTime.AddDays(-1) < DateTime.Today)
            {
                ViewBag.Disable = true;
            }
            // This gets a list of records based on the date from DB:
            var responses = repo.Response.Where(i => i.Date == currentDate).ToList();
            
            return View(responses);
        }

        public IActionResult Appointments()
        {
            //This removes the key-value pairs, date & time in session:
            HttpContext.Session.Remove("date");
            HttpContext.Session.Remove("time");

            //This gets a list of every record from DB:
            var responses = repo.Response.OrderByDescending(i => i.Date).ToList();

            return View(responses);
        }

        public IActionResult PreviousDate(string CurrentDate)
        {
            //This removes the key-value pairs, date & time in session:
            HttpContext.Session.Remove("date");
            HttpContext.Session.Remove("time");

            //This is the day formatting:
            string[] dateformat = { "MM/dd/yyyy" };
            DateTime currentTime = DateTime.ParseExact(CurrentDate, dateformat, System.Globalization.CultureInfo.InvariantCulture);
            DateTime previousDate = currentTime.AddDays(-1);
            string previous = previousDate.ToString("MM/dd/yyyy");

            ViewBag.CurrentDate = previous;

            //Redirecting to the SignUp page with previous Date 
            return RedirectToAction("SignUp", new { CurrentDate = previous });
        }

        public IActionResult NextDate(string CurrentDate)
        {
            //This removes the key-value pairs, date & time in session:
            HttpContext.Session.Remove("date");
            HttpContext.Session.Remove("time");

            //This is the day formatting:
            string[] dateformat = { "MM/dd/yyyy" };
            DateTime currentTime = DateTime.ParseExact(CurrentDate, dateformat, System.Globalization.CultureInfo.InvariantCulture);
            DateTime nextDate = currentTime.AddDays(1);
            string next = nextDate.ToString("MM/dd/yyyy");

            ViewBag.CurrentDate = next;
            return RedirectToAction("SignUp", new { CurrentDate = next });
        }

        [HttpGet]
        public IActionResult Form(string currentDate, string time)
        {
            ViewBag.CurrentDate = currentDate;
            ViewBag.scheduledTime = time;

            if (HttpContext.Session.GetString("date") == null)
            {
                HttpContext.Session.SetString("date", currentDate);
            }
            else
            {
                ViewBag.CurrentDate = HttpContext.Session.GetString("date");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Form (AppointmentResponse r)
        {
            if (r.Date == null)
            {
                r.Date = HttpContext.Session.GetString("date");
                
                if (r.Name != "" && (r.Size > 0 && r.Size <= 15) && r.Email !="")
                {
                    ModelState.Clear();
                }
            }
            if (ModelState.IsValid)
            {
                repo.Add(r);
                repo.SaveChanges();

                //Clearing the values 
                ViewBag.CurrentDate = "";
                ViewBag.scheduleTime = "";

                //This removes the key-value pairs, date & time in session:
                HttpContext.Session.Remove("date");
                HttpContext.Session.Remove("time");

                return RedirectToAction("Appointments");

            }
            //When the modelstate is not valid;
            ViewBag.CurrentDate = HttpContext.Session.GetString("date");
            ViewBag.scheduleTime = r.Time;
            r.Date = HttpContext.Session.GetString("date");
            return View(r);
        }

        //Deleting the information from DB

        public IActionResult Delete(int aptid)
        {
            //Matching the DB with the id 
            var apt = repo.Response.Single(i => i.AppointmentId == aptid);

            repo.Response.Remove(apt);
            repo.SaveChanges();

            var list = repo.Response.ToList();

            return RedirectToAction("Appointments", list);
        }

        //Editing the Temple Appointments

        [HttpGet]
        public IActionResult Edit (int aptid)
        {
            ViewBag.New = false;

            var apt = repo.Response.Single(i => i.AppointmentId == aptid);

            ViewBag.CurrentDate = apt.Date;
            ViewBag.scheduleTime = apt.Time;

            HttpContext.Session.SetString("date", apt.Date);
            HttpContext.Session.SetString("time", apt.Time);
            return View("Form", apt);
        }

        [HttpPost]
        public IActionResult Edit(AppointmentResponse r)
        {
            if (r.Date == null || r.Time == null)
            {
                r.Date = HttpContext.Session.GetString("date");
                r.Time = HttpContext.Session.GetString("time");
            }

            if (r.Name != "" && (r.Size > 0 && r.Size <= 15) && r.Email != "")
            {
                ModelState.Clear();
            }

            if (ModelState.IsValid)
            {
                repo.Update(r);
                repo.SaveChanges();

                ViewBag.CurrentDate = "";
                ViewBag.scheduleTime = "";

                HttpContext.Session.Remove("date");
                HttpContext.Session.Remove("time");

                return RedirectToAction("Appointments");
            }

            ViewBag.New = false;

            ViewBag.CurrentDate = HttpContext.Session.GetString("date");

            ViewBag.scheduleTime = r.Time;

            r.Date = HttpContext.Session.GetString("date");

            return View("From", r);
        }
    }
}
