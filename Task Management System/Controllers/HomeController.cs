using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBOperations.Models;

namespace Task_Management_System.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var userDetails = Session["UserDetails"] as Employees_Model;
            return View(userDetails);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}