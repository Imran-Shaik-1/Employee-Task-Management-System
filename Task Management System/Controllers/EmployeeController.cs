using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBOperations.DBOps;
using DBOperations.Models;

namespace Task_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeRepository repository = new EmployeeRepository();

        public ActionResult GetEmployees()   
        {
            var result = repository.GetEmployees();
            return View(result);
        }

        public ActionResult GetMyEmployees(int id)
        {
            if (repository.IsManager(id))
            {
                var result = repository.GetMyTeam(id);
                ViewBag.Flag = true;
                return View(result);
            }
            ViewBag.Flag = false;
            return View();
        }

        public ActionResult GetMyTeam(int id)
        {
            var result = repository.GetMyTeam(id);
            var Manager = repository.GetEmployee((int)result[0].ManagerID);
            ViewBag.Name = Manager.FirstName;
            ViewBag.Email = Manager.EmailAddress;
            return View(result);
        }

        public ActionResult GetMyTeam_NoManager()
        {
            return View();
        }

        public ActionResult AssignManager(int id)
        {
            var employee = repository.GetEmployee(id);
            employee.ManagerID = null;
            TempData["OldManager"] = employee.OldManagerID;
            return View(employee);
        }

        [HttpPost]
        public ActionResult AssignManager(Employees_Model model)
        {
            var oldmanager = (int)TempData["OldManager"];
            TempData.Keep();
            if (model.ManagerID == oldmanager)
            {
                ModelState.AddModelError("ManagerID", "Already Manager of this Employee.");
                return View();
            }
            else if(model.ManagerID == model.EmployeeID)
            {
                ModelState.AddModelError("ManagerID", "An Employee can't be a Manager of himself.");
                return View();
            }
            else if (repository.ValidEmployee(model.ManagerID))
            {
                repository.AssignManager(model);
                return RedirectToAction("GetEmployees");
            }
            else
            {
                ModelState.AddModelError("ManagerID", "Invalid manager ID. Please provide a valid manager ID.");
                return View();
            }

        }

        public ActionResult Details(int id)  
        {
            var employee = repository.GetEmployee(id);
            return View(employee);
        }

        public ActionResult Edit(int id)   
        {
            var employee = repository.GetEmployee(id);
            TempData["model"] = employee;
            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit(Employees_Model model)   
        {
            var oldmodel = TempData["model"] as Employees_Model;
            TempData.Keep();
            if (model.FirstName == oldmodel.FirstName && model.LastName == oldmodel.LastName && model.EmailAddress == oldmodel.EmailAddress)
            {
                ModelState.AddModelError("", "No changes detected.");
            }
            else if(ModelState.IsValidField("FirstName") && ModelState.IsValidField("EmailAddress"))
            {
                repository.UpdateEmployee(model.EmployeeID, model);
                return RedirectToAction("Details", new { id = model.EmployeeID });
            }
            return View(model);
        }

        public ActionResult EditPassword(int id)   
        {
            var result = repository.GetEmployee(id);
            TempData["Email"] = result.EmailAddress;
            return View(result);
        }

        [HttpPost]
        public ActionResult EditPassword(Employees_Model model)   
        {
            var Email = TempData.Peek("Email");
            if (ModelState.IsValidField("Password") && repository.ValidPassword(model))
            {
                TempData["ID"] = model.EmployeeID;
                return RedirectToAction("UpdatePassword","Account",new { Email } );
            }
            else
            {
                ModelState.AddModelError("Password","Wrong Password.");
            }
            return View(model);
        }

        public ActionResult TerminateEmployee(int id)
        {
            var employee = repository.GetEmployee(id);
            ViewBag.UserStoriesCount = repository.UserStoriesCount(id);
            TempData["Role"] = "Admin";
            TempData["Path"] = "Admin";
            return View(employee);
        }

        public ActionResult ConfirmTerminate(int id)
        {
            if (repository.IsManager(id))
            {
                ViewBag.message = false;
                var employee = repository.GetEmployee(id);
                ViewBag.UserStoriesCount = repository.UserStoriesCount(id);
                return View("TerminateEmployee",employee);
            }
            else
            {
                repository.DeleteEmpUserStories(id);
                repository.DeleteEmployee(id);
            }
            return RedirectToAction("GetEmployees");
        }
    }
}