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
        private AppointmentContext appoinment { get; set; }

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

            if (currentTime1.AddDays(-1) < DateTime.Today)
            {
                ViewBag.Disable = true;
            }
            // This gets a list of records based on the date from DB:
            var record = repo.Appointments.Where(i => i.Date == currentdate).ToList();
            
            return View(record);
        }

        public IActionResult Appointments()
        {
            //This removes the key-value pairs, date & time in session:
            HttpContext.Session.Remove("date");
            HttpContext.Session.Remove("time");

            //This gets a list of every record from DB:
            var record = repo.Appointments.OrderByDescending(i => i.Date).ToList();

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

        public IActionResult NextDate(string CurrentDate)
        {
            //This removes the key-value pairs, date & time in session:
            HttpContext.Session.Remove("date");
            HttpContext.Session.Remove("time");
            //This is the day formatting:
            string[] dateformat = { "MM/dd/yyyy" };
            DateTime currentTime3 = DateTime.ParseExact(CurrentDate, dateformat, new CultureInfo("en-US"), DateTimeStyles.None);
            DateTime previousDate = currentTime3.AddDays(1);
            string next = NextDate.ToString("MM/dd/yyyy");

            ViewBag.CurrentDate = next;
            return RedirectToAction("SignUp", new { currentTime3 = next });
        }

        [HttpGet]
        public IActionResult Form(string currentDate, string time)
        {
            ViewBag.currentDate = currentDate;
            ViewBag.AppTime = time;

            if (HttpContext.Session.GetString("date") == null)
            {
                HttpContext.Session.SetString("date", currentDate);
            }
            else
            {
                ViewBag.currentDate = HttpContext.Session.GetString("date");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Form (AppointmentResponse r)
        {
            if (r.Date == null)
            {
                r.Date = HttpContext.Session.GetString("date");
                
                if (r.GroupName != "" && (r.GroupSize > 0 && r.GroupSize < 16) && r.Email !="")
                {
                    ModelState.Clear();
                }
            }
            if (ModelState.IsValid)
            {
                repo.Add(r);
                repo.SaveChanges();

                //Clearing the values 
                ViewBag.currentDate = "";
                ViewBag.AppTime = "";

                //This removes the key-value pairs, date & time in session:
                HttpContext.Session.Remove("date");
                HttpContext.Session.Remove("time");

                return RedirectToAction("Index");

            }
            //When the modelstate is not valid;
            ViewBag.currentDate = HttpContext.Session.GetString("date");
            ViewBag.AppTime = r.Time;
            r.Date = HttpContext.Session.GetString("date");
            return View(r);
        }

        //Deleting the information from DB

        public IActionResult Delete(int aptid)
        {
            //Matching the DB with the id 
            var apt = repo.Appoinements.Single(i => i.AppointmentId == aptid);

            repo.Appointments.Remove(apt);
            repo.SaveChanges();

            var list = repo.Appointments.ToList();

            return RedirectToAction("Appointments", list);
        }

        //Editing the Temple Appointments

        [HttpGet]
        public IActionResult Edit (int aptid)
        {
            ViewBag.New = false;

            var apt = repo.Appointments.Single(i => i.AppointmentId == aptid);

            ViewBag.currentDate = apt.Date;
            ViewBag.AppTime = apt.Time;

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

            if (r.GroupName != "" && (r.GroupSize > 0 && r.GroupSize < 16) && r.Email != "")
            {
                ModelState.Clear();
            }

            if (ModelState.IsValid)
            {
                repo.Update(r);
                repo.SaveChanges();

                HttpContext.Session.SetString("date", apt.Date);
                HttpContext.Session.SetString("time", apt.Time);

                return RedirectToAction("Appointments");
            }

            ViewBag.New = false;

            ViewBag.currentDate = HttpContext.Session.GetString("date");

            ViewBag.AppTime = r.Time;

            r.Date = HttpContext.Session.GetString("Date");

            return View("From", r);
        }
    }
}
