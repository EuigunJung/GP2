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

        //Connecting to AppointmentContext DB 
        public HomeController(AppointmentContext temp)
        {
            repo = temp;
        }

        //Index Page 
        public IActionResult Index()
        {
  
            return View();
        }

        //Sign Up Page  
        public IActionResult SignUp(string currentDate)
        {
       
            //Set the date to today if the currentdate is null:
            if (currentDate == null)
            {
                //This converts the DateTime to sting with today's date 
                currentDate = DateTime.Now.ToString("MM/dd/yyyy");
            }

            //Create a viewbag and store currentDate value to the viewbag.CurrnetDate
            ViewBag.CurrentDate = currentDate;

            //This is the day formatting, makes it easier to put information to the DB as string:
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

        //Appointments page
        public IActionResult Appointments()
        {

            //This gets a list of every record from DB. Order by Ascending order so the users can see the date close to today:
            var responses = repo.Response.OrderBy(i => i.Date).ToList();

            return View(responses);
        }

        //When the button is pressed, set the date to previous date, by subtracting 1. Then, convert the datatypee to string. 
        public IActionResult PreviousDate(string CurrentDate)
        {
  

            //This is the day formatting:
            string[] dateformat = { "MM/dd/yyyy" };
            DateTime currentTime = DateTime.ParseExact(CurrentDate, dateformat, System.Globalization.CultureInfo.InvariantCulture);
            DateTime previousDate = currentTime.AddDays(-1);
            string previous = previousDate.ToString("MM/dd/yyyy");

            ViewBag.CurrentDate = previous;

            //Redirecting to the SignUp page with previous Date 
            return RedirectToAction("SignUp", new { CurrentDate = previous });
        }

        //Very similar to PreviousDate function. (Extra Credit) However, the users can go more than 3 months worth of dalendar date data.
        public IActionResult NextDate(string CurrentDate)
        {
     
            //This is the day formatting:
            string[] dateformat = { "MM/dd/yyyy" };
            DateTime currentTime = DateTime.ParseExact(CurrentDate, dateformat, System.Globalization.CultureInfo.InvariantCulture);
            DateTime nextDate = currentTime.AddDays(1);
            string next = nextDate.ToString("MM/dd/yyyy");

            ViewBag.CurrentDate = next;
            return RedirectToAction("SignUp", new { CurrentDate = next });
        }

        //The actual signup form is redirected when the user selects a respective timeslot.  
        [HttpGet]
        public IActionResult Form(string currentDate, string time)
        {
            ViewBag.CurrentDate = currentDate;
            ViewBag.scheduledTime = time;

            //This prevents from getting a null value and getting an error. 
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
                //Multiple conditions that validates AppointmentResponse.cs
                if (r.Name != "" && r.Email != ""  && (r.Size > 0 && r.Size <= 15))
                {
                    ModelState.Clear();
                }
            }
            //Add validated form to the DB and redirect to Index page: 
            if (ModelState.IsValid)
            {
                repo.Add(r);
                repo.SaveChanges();

                //Clearing the current values 
                ViewBag.CurrentDate = "";
                ViewBag.scheduledTime = "";


                return RedirectToAction("Index");

            }
            //When the modelstate is not valid;
            ViewBag.CurrentDate = HttpContext.Session.GetString("date");
            ViewBag.scheduledTime = r.Time;
            r.Date = HttpContext.Session.GetString("date");
            return View(r);
        }

        //Deleting the information from DB -- Users can directly see the changes:
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

            //Fetching existing information from AppointmentContext DB:
            var apt = repo.Response.Single(i => i.AppointmentId == aptid);

            
            ViewBag.CurrentDate = apt.Date;
            ViewBag.scheduledTime = apt.Time;

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

            //Validation conditions
            if (r.Name != "" && r.Email != "" && (r.Size > 0 && r.Size <= 15))
            {
                ModelState.Clear();
            }

            //Assigning viewbag values to the repo values (updating): 
            if (ModelState.IsValid)
            {
                repo.Update(r);
                repo.SaveChanges();

                ViewBag.CurrentDate = "";
                ViewBag.scheduledTime = "";
                HttpContext.Session.Remove("date");
                HttpContext.Session.Remove("time");

                return RedirectToAction("Appointments");
            }

            ViewBag.New = false;

            ViewBag.CurrentDate = HttpContext.Session.GetString("date");

            ViewBag.scheduledTime = r.Time;

            r.Date = HttpContext.Session.GetString("date");
            
            return View("Form", r);
        }
    }
}
