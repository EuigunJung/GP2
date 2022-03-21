using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        }

    }
}
