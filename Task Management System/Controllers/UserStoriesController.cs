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

        public ActionResult GetAllUserStories()
        {
            var result = repository.GetAllUserStories();
            return View(result);
        }

        public ActionResult GetEmpUserStories(int Emp_id)
        {
            var result = repository.GetMyUserStories(Emp_id);
            ViewBag.empid = Emp_id;
            ViewBag.managerid = repository.GetManager(Emp_id);
            return View(result);
        }

        public ActionResult ViewEmpUserStories(int Emp_id)
        {
            var result = repository.GetMyUserStories(Emp_id);
            ViewBag.EmpId = Emp_id;
            return View(result);
        }

        public ActionResult ViewEmpUserStorydetails(int id)
        {
            var result = repository.GetUserStory(id);
            TempData["UserId"] = result.AssignedEmployeeID;
            ViewBag.TasksCount = repository.CountTasks(id);
            return View(result);
        }
        public ActionResult GetEmpUserStorydetails(int id)
        {
            var list = new List<string>() { "High", "Medium", "Low" };
            ViewBag.list = list;
            var result = repository.GetUserStory(id);
            TempData["UserId"] = result.AssignedEmployeeID;
            ViewBag.TasksCount = repository.CountTasks(id);
            return View(result);
        }

        [HttpPost]
        public ActionResult GetEmpUserStorydetails(UserStories_Model model)
        {
            int empid = (int)TempData["UserId"];
            if (ModelState.IsValid)
            {
                repository.UpdateUserStory(model);
                return RedirectToAction("GetEmpUserStories", "UserStories", new { Emp_id = model.AssignedEmployeeID });
            }
            var list = new List<string>() { "High", "Medium", "Low" };
            ViewBag.list = list;
            model.AssignedEmployeeID = empid;
            TempData["UserId"] = empid;
            return View(model);
        }

        public ActionResult GetMyUserStories(int Emp_id)
        {
            var result = repository.GetMyUserStories(Emp_id);
            return View(result);
        }

        public ActionResult GetMyUserStorydetails(int id)
        {
            var list = new List<string>() { "New", "In Progress","Completed" };
            ViewBag.list = list;
            var result = repository.GetUserStory(id);
            ViewBag.TasksCount = repository.CountTasks(id);
            ViewBag.DueDate = result.DueDate;
            return View(result);
        }

        [HttpPost]
        public ActionResult GetMyUserStorydetails(UserStories_Model model)
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

        public ActionResult GetUserStory(int Userstory_id)
        {
            var result = repository.GetUserStory(Userstory_id);
            return View(result);
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

        public ActionResult GetUserStorydetails(int id)
        {
            var result = repository.GetUserStory(id);
            TempData["UserId"] = result.AssignedEmployeeID;
            TempData["flag"] = "true";
            ViewBag.TasksCount = repository.CountTasks(id);
            return View(result);
        }

        [HttpPost]
        public ActionResult GetUserStorydetails(UserStories_Model model)
        {
            int empid = (int)TempData["UserId"];
            if (ModelState.IsValid)
            {
                repository.UpdateUserStory(model);
                return RedirectToAction("GetEmpUserStories", "UserStories", new { Emp_id = model.AssignedEmployeeID });
            }
            model.AssignedEmployeeID = empid;
            TempData["UserId"] = empid;
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
