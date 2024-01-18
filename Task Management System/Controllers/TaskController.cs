using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBOperations.DBOps;
using DBOperations.Models;

namespace Task_Management_System.Controllers
{
    public class TaskController : Controller
    {
        TaskRepository repository = new TaskRepository();

        public ActionResult AddTask(int userstoryid)
        {
            TempData["userstoryid"] = userstoryid;
            var model = new Tasks_Model()
            {
                AssignedUserStoryID = userstoryid,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddTask(Tasks_Model model)
        {
            int userstoryid = (int)TempData["userstoryid"];
            if (ModelState.IsValid)
            {
                repository.AddTask(model);
                return RedirectToAction("GetMyUserStorydetails", "UserStories", new { id = userstoryid });
            }
            TempData.Keep();
            return View(model);
        }

        public ActionResult GetTasks(int userstoryid)
        {
            var model = repository.GetTasks(userstoryid);
            if (model != null)
            {
                ViewBag.id = model[0].AssignedUserStoryID;
            }
            return View(model);
        }

        public ActionResult GetEmpTasks(int userstoryid)
        {
            ViewBag.flag = TempData["UserId"];
            TempData.Keep();
            var model = repository.GetTasks(userstoryid);
            if (model != null)
            {
                ViewBag.id = model[0].AssignedUserStoryID;
            }
            return View(model);
        }

        public ActionResult ViewEmpTasks(int userstoryid)
        {
            ViewBag.flag = TempData["UserId"];
            TempData.Keep();
            var model = repository.GetTasks(userstoryid);
            if (model != null)
            {
                ViewBag.id = model[0].AssignedUserStoryID;
            }
            return View(model);
        }

        public ActionResult UpdateTask(int taskid)
        {
            var list = new List<string>() { "New", "In Progress", "Completed" };
            ViewBag.list = list;
            var model = repository.GetTask(taskid);
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateTask(Tasks_Model model)
        {
            if (model.Status == null)
            {
                ModelState.AddModelError("Status", "Status is Required.");
            }
            else if (ModelState.IsValid)
            {
                repository.UpdateTask(model);
                return RedirectToAction("GetTasks", new { userstoryid = model.AssignedUserStoryID });
            }
            var list = new List<string>() { "New", "In Progress", "Completed" };
            ViewBag.list = list;
            return View(model);
        }

        public ActionResult DeleteTask(int taskid)
        {
            var model = repository.GetTask(taskid);
            TempData["UserStoryId"] = model.AssignedUserStoryID;
            return View(model);
        }

        public ActionResult ConfirmDelete(int taskid)
        {
            int UserStoryId = (int)TempData["UserStoryId"];
            repository.DeleteTask(taskid);
            return RedirectToAction("GetTasks", new { userstoryid = UserStoryId });
        }
    }
}