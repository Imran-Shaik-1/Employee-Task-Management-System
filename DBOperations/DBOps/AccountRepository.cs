using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBOperations.Models;
using System.Net;
using System.Net.Mail;

namespace DBOperations.DBOps
{
    public class AccountRepository
    {
        public bool LoginValid(Employees_Model model)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                bool isValid = context.Employees.Any(x => x.EmailAddress == model.EmailAddress && x.Password == model.Password);
                return isValid;
            }
        }

        public Employees_Model GetEmployee(string Email)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var result = context.Employees.Where(x => x.EmailAddress == Email).
                    Select(x => new Employees_Model()
                    {
                        EmployeeID = x.EmployeeID,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        EmailAddress = x.EmailAddress,
                        ManagerID = x.ManagerID
                    }).FirstOrDefault();
                return result;
            }
        }

        public void updatePassword(Employees_Model model)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var employee = context.Employees.FirstOrDefault(x => x.EmailAddress == model.EmailAddress);
                employee.Password = model.Password;
                context.SaveChanges();
            }
        }

        public void SignUp(Employees_Model model)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                Employees employee = new Employees()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    Password = model.Password
                };
                context.Employees.Add(employee);
                context.SaveChanges();
            }
        }

        public bool isnewEmail(string EmailAddress)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                if (context.Employees.Any(x => x.EmailAddress == EmailAddress))
                {
                    return false;
                }
                return true;
            }
        }

        public void SendEmail(Employees_Model model)
        {
            string SenderEmail = "taskhubapplication@gmail.com";
            string SenderName = "TaskHub";
            string SenderPassword = "titxnpnkqucsgqyb";
            string RecipientEmail = model.EmailAddress;
            string RecipientName = model.FirstName == null? "User" : model.FirstName;
            string Subject = "OTP for Email Verification";
            int OTP = GenerateOTP();
            string Body = "Dear " + RecipientName + ",\n Welcome to TaskHub. The OTP to verify your Email is " + OTP;

            var fromAddress = new MailAddress(SenderEmail, SenderName);
            var toAddress = new MailAddress(RecipientEmail, RecipientName);
       
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, SenderPassword)
            };

            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = Subject,
                Body = Body
            };

            smtp.Send(message);

        }

        public static int generatedOTP;

        public static int GenerateOTP()
        {
            Random random = new Random();
            generatedOTP =  random.Next(100000, 999999);
            return generatedOTP;
        }

        public bool VerifyOTP(int inputOTP)
        {
            return inputOTP == generatedOTP;
        }
    }
}