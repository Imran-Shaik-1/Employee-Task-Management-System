using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBOperations.DBOps;
using DBOperations.Models;

namespace Task_Management_System.Controllers
{
    public class UserStoriesController : Controller
    {
        UserStoriesRepository repository = new UserStoriesRepository();

        public ActionResult GetAllUserStories()  // Admin Dashboard
        {
            var result = repository.GetAllUserStories();
            TempData["path"] = "GetAllUserStories";
            return View(result);
        }

        public ActionResult GetUserStorydetails(int id)  // Admin views Userstory from dashboard
        {
            var result = repository.GetUserStory(id);
            TempData["UserId"] = result.AssignedEmployeeID;
            ViewBag.path = TempData["path"];
            TempData.Keep();
            ViewBag.TasksCount = repository.CountTasks(id);
            return View(result);
        }

        public ActionResult GetEmpUserStories(int Emp_id)  // Manager views/creates USs.
        {
            var result = repository.GetMyUserStories(Emp_id);
            ViewBag.empid = Emp_id;
            ViewBag.flag = TempData["flag"];
            TempData["ManagerID"] = repository.GetManager(Emp_id);
            ViewBag.managerid = TempData["ManagerID"];
            TempData.Keep();
            return View(result);
        }

        public ActionResult GetEmpUserStorydetails(int id)  // Manager view/edit particular US.
        {
            var ManagerID = (int)TempData["ManagerID"];
            TempData["Team"] = repository.GetMyEmp(ManagerID);
            var Team = TempData["Team"] as IEnumerable<Employees_Model>;
            ViewBag.team = new SelectList(Team, "EmployeeID", "FirstName");
            ViewBag.list = new List<string>() { "High", "Medium", "Low" };
            var result = repository.GetUserStory(id);
            TempData["UserId"] = result.AssignedEmployeeID;
            ViewBag.TasksCount = repository.CountTasks(id);
            TempData["path"] = "GetEmpUserStorydetails";
            TempData.Keep();
            return View(result);
        }

        [HttpPost]
        public ActionResult GetEmpUserStorydetails(UserStories_Model model)  // post
        {
            int empid = (int)TempData["UserId"];
            if (ModelState.IsValid)
            {
                repository.UpdateUserStory(model);
                return RedirectToAction("GetMyEmployees", "Employee", new { id = (int)TempData["ManagerID"] });
            }
            ViewBag.list = new List<string>() { "High", "Medium", "Low" };
            var Team = TempData["Team"] as IEnumerable<Employees_Model>;
            ViewBag.team = new SelectList(Team, "EmployeeID", "FirstName");
            model.AssignedEmployeeID = empid;
            TempData["UserId"] = empid;
            TempData.Keep();
            return View(model);
        }

        public ActionResult GetMyUserStories(int Emp_id)  // Emp or Admin can see US
        {
            ViewBag.path = TempData["Path"];
            ViewBag.EmpId = Emp_id;
            TempData.Keep();
            var result = repository.GetMyUserStories(Emp_id);
            return View(result);
        }

        public ActionResult GetMyUserStorydetails(int id)  // Emp can update status
        {
            var list = new List<string>() { "New", "In Progress","Completed" };
            ViewBag.list = list;
            var result = repository.GetUserStory(id);
            ViewBag.TasksCount = repository.CountTasks(id);
            ViewBag.DueDate = result.DueDate;
            return View(result);
        }

        [HttpPost]
        public ActionResult GetMyUserStorydetails(UserStories_Model model)  //post
        {
            if (model.Status != null)
            {
                repository.UpdateUserStoryStatus(model);
                return RedirectToAction("GetMyUserStorydetails", new { id = model.UserStoryID });
            }
            ModelState.AddModelError("Status", "Status is Required.");
            var list = new List<string>() { "New", "In Progress", "Completed" };
            ViewBag.list = list;
            ViewBag.DueDate = model.DueDate;
            return View(model);
        }

        public ActionResult AddUserStory(int empid ,int managerid)
        {
            var list = new List<string>() { "High", "Medium", "Low" };
            ViewBag.list = list;
            var model = new UserStories_Model()
            {
                AssignedEmployeeID = empid,
                AssignedManagerID = managerid,
                DueDate = DateTime.Now.AddDays(1)
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddUserStory(UserStories_Model model)
        {
            if (ModelState.IsValid) {
                repository.AddUserStory(model);
                return RedirectToAction("GetEmpUserStories", "UserStories", new { Emp_id = model.AssignedEmployeeID });
            }
            var list = new List<string>() { "High", "Medium", "Low" };
            ViewBag.list = list;
            return View(model);
        }

        public ActionResult DeleteUserStory(int id)
        {
            var userStory = repository.GetUserStory(id);
            TempData["UserId"] = userStory.AssignedEmployeeID;
            ViewBag.TasksCount = repository.CountTasks(id);
            return View(userStory);
        }

        public ActionResult ConfirmDelete(int id)
        {
            int empid = (int)TempData["UserId"];
            repository.DeleteUserStoryTasks(id);
            repository.DeleteUserStory(id);
            return RedirectToAction("GetEmpUserStories", "UserStories", new { Emp_id = empid });

        }
    }
}
