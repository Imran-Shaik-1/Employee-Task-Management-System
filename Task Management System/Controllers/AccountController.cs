using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DBOperations; 
using DBOperations.Models;
using DBOperations.DBOps;

namespace Task_Management_System.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        AccountRepository repository = new AccountRepository();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Employees_Model model)
        {
            if (repository.LoginValid(model))
            {
                var userDetails = repository.GetEmployee(model.EmailAddress);
                Session["UserDetails"] = userDetails;
                FormsAuthentication.SetAuthCookie(model.EmailAddress, false);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid Email or Password");
            return View();
        }

        public ActionResult forgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult forgotPassword(Employees_Model model)
        {
            if (model.EmailAddress == null)
            {
                return View();
            }
            bool isnewEmail = repository.isnewEmail(model.EmailAddress);
            TempData["Email"] = model.EmailAddress;
            if (isnewEmail)
            {
                ViewBag.Message = "Account with this Email does not exist. Please create a new Account";
            }
            else
            {
                ViewBag.flag = "true";
                repository.SendEmail(model);
            }
            return View();
        }

        public ActionResult VerifyOTP(Employees_Model model)
        {
            model.EmailAddress = (string)TempData["Email"];
            if (repository.VerifyOTP(model.OTP))
            {
                return RedirectToAction("updatePassword",new { Email = model.EmailAddress });
            }
            ViewBag.flag = "true";
            TempData["Email"] = model.EmailAddress;
            TempData["ID"] = model.EmployeeID;
            ModelState.AddModelError("OTP", "Incorrect OTP.");
            return View("forgotPassword",model);
        }

        public ActionResult UpdatePassword(string Email)
        {
            TempData["Email"] = Email;
            ViewBag.ID = TempData.Peek("ID");
            TempData.Keep();
            return View();
        }

        [HttpPost]
        public ActionResult UpdatePassword(Employees_Model model)
        {
            if (ModelState.IsValidField("Password") && ModelState.IsValidField("ConfirmPassword"))
            {
                model.EmailAddress = (String)TempData["Email"];
                repository.updatePassword(model);

                if(TempData["ID"] == null)    return RedirectToAction("Login");
                return RedirectToAction("Details", "Employee",new { id = TempData["ID"]});
            }
            ViewBag.ID = TempData.Peek("ID");
            TempData.Keep();
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Employees_Model model)
        {
            if (ModelState.IsValid)
            {
                if (repository.isnewEmail(model.EmailAddress))
                {
                    repository.SendEmail(model);
                    TempData["usermodel"] = model;
                    return RedirectToAction("ConfirmOTP");
                }
                else
                {
                    ModelState.AddModelError("EmailAddress","Account with this Email already exist.");
                    ViewBag.Message = "Please Login to continue.";
                }
            }
            return View();
        }

        public ActionResult ConfirmOTP()
        {
            var model = TempData["usermodel"] as Employees_Model;
            TempData["usermodel"] = model;
            ViewBag.Email = model.EmailAddress;
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmOTP(Employees_Model tempmodel)
        {
            var model = TempData["usermodel"] as Employees_Model;
            if (repository.VerifyOTP(tempmodel.OTP))
            {
                repository.SignUp(model);
                return RedirectToAction("Login");
            }
            ModelState.AddModelError("OTP","Invalid OTP");
            TempData["usermodel"] = model;
            ViewBag.Email = model.EmailAddress;
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}