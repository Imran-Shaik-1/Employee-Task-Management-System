using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBOperations.Models;

namespace DBOperations.DBOps
{
    public class EmployeeRepository
    {
        public List<Employees_Model> GetEmployees()
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var result = context.Employees.
                    Select(x => new Employees_Model()
                    {
                        EmployeeID = x.EmployeeID,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        EmailAddress = x.EmailAddress,
                        IsManager = x.IsManager,
                        ManagerID = x.ManagerID
                    }).ToList();
                return result;
            }
        }

        public List<Employees_Model> GetMyTeam(int id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var result = context.Employees.Where(x => x.ManagerID == id).
                    Select(x => new Employees_Model()
                    {
                        EmployeeID = x.EmployeeID,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        EmailAddress = x.EmailAddress,
                        IsManager = x.IsManager,
                        ManagerID = x.ManagerID
                    }).ToList();
                return result;
            }
        }

        public Employees_Model GetEmployee(int id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var result = context.Employees.Where(x => x.EmployeeID == id).
                    Select(x => new Employees_Model()
                    {
                        EmployeeID = x.EmployeeID,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        EmailAddress = x.EmailAddress,
                        OldManagerID = x.ManagerID,
                        IsManager = x.IsManager,
                        ManagerID = x.ManagerID
                    }).FirstOrDefault();
                return result;
            }
        }

        public void UpdateEmployee(int id, Employees_Model model)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var employee = context.Employees.FirstOrDefault(x => x.EmployeeID == id);

                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.EmailAddress = model.EmailAddress;

                context.SaveChanges();
            }
        }

        public bool ValidPassword(Employees_Model model)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var employee = context.Employees.FirstOrDefault(x => x.EmployeeID == model.EmployeeID);
                return (employee.Password == model.Password);
            }
        }

        public int UserStoriesCount(int Empid)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var count = context.UserStories
                    .Where(x => x.AssignedEmployeeID == Empid).Count();
                return count;
            }
        }

        public void UpdatePassword(Employees_Model model)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var employee = context.Employees.FirstOrDefault(x => x.EmployeeID == model.EmployeeID);

                employee.Password = model.Password;

                context.SaveChanges();
            }
        }

        public bool ValidEmployee(int? Employee_id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var maxEmployeeID = context.Employees.Max(e => e.EmployeeID);
                return Employee_id <= maxEmployeeID;
            }
        }

        public void DeleteEmployee(int id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var model = context.Employees.FirstOrDefault(x => x.EmployeeID == id);
                var manager_id = model.ManagerID;

                context.Employees.Remove(model);
                context.SaveChanges();

                if (!context.Employees.Any(x => x.ManagerID == manager_id))
                {
                    var manager = context.Employees.FirstOrDefault(x => x.EmployeeID == manager_id);
                    manager.IsManager = false;
                    context.SaveChanges();
                }
            }
        }

        public void DeleteEmpUserStories(int id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var result = context.UserStories.Where(x => x.AssignedEmployeeID == id).ToList();

                int n = result.Count();
                for(int i = 0; i < n; i++)
                {
                    DeleteUserStoryTasks(result[i].UserStoryID);
                }

                context.UserStories.RemoveRange(result);
                context.SaveChanges();

            }
        }

        public void DeleteUserStoryTasks(int id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var result = context.Tasks.Where(x => x.AssignedUserStoryID == id).ToList();
                context.Tasks.RemoveRange(result);
                context.SaveChanges();
            }
        }



        public bool IsManager(int id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                bool result = context.Employees.Any(x => x.ManagerID == id);
                return result;
            }
       }

        public void AssignManager(Employees_Model model)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var employee = context.Employees.FirstOrDefault(x => x.EmployeeID == model.EmployeeID);
                employee.ManagerID = model.ManagerID;

                if (model.ManagerID != null)
                {
                    var manager = context.Employees.FirstOrDefault(x => x.EmployeeID == model.ManagerID);
                    manager.IsManager = true;
                }
                context.SaveChanges();
                if (!context.Employees.Any(x => x.ManagerID == model.OldManagerID))
                {
                    employee = context.Employees.FirstOrDefault(x => x.EmployeeID == model.OldManagerID);
                    employee.IsManager = false;
                }

                context.SaveChanges();
            }
        }
       
    }
}
