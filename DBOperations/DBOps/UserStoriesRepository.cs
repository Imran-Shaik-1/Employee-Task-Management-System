using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBOperations.Models;

namespace DBOperations.DBOps
{
    public class UserStoriesRepository
    {
        public void AddUserStory(UserStories_Model model)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                UserStories userStory = new UserStories()
                {
                    Title = model.Title,
                    Description = model.Description,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    AssignedEmployeeID = model.AssignedEmployeeID,
                    DueDate = model.DueDate,
                    StoryPoints = model.StoryPoints,
                    Priority = model.Priority,
                    Status = "New"
                };

                context.UserStories.Add(userStory);
                context.SaveChanges();
            }
        }

        public List<UserStories_Model> GetAllUserStories()
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var result = context.UserStories.
                    Select(x => new UserStories_Model()
                    {
                        UserStoryID = x.UserStoryID,
                        Title = x.Title,
                        Description = x.Description,
                        CreatedOn = x.CreatedOn,
                        AssignedEmployeeID = x.AssignedEmployeeID,
                        DueDate = x.DueDate,
                        ModifiedOn = x.ModifiedOn,
                        Status = x.Status,
                        StoryPoints = x.StoryPoints,
                        Priority = x.Priority
                    }).ToList();
                return result;
            }
        }

        public List<UserStories_Model> GetMyUserStories(int emp_id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var result = context.UserStories.
                    Where(x => x.AssignedEmployeeID == emp_id).
                    Select(x => new UserStories_Model()
                    {
                        UserStoryID = x.UserStoryID,
                        Title = x.Title,
                        Description = x.Description,
                        CreatedOn = x.CreatedOn,
                        DueDate = x.DueDate,
                        ModifiedOn = x.ModifiedOn,
                        Status = x.Status,
                        StoryPoints = x.StoryPoints,
                        Priority = x.Priority,
                        AssignedEmployeeID = x.AssignedEmployeeID
                    }).ToList();
                return result;
            }
        }

        public List<Employees_Model> GetMyEmp(int id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var result = context.Employees.Where(x => x.ManagerID == id).
                    Select(x => new Employees_Model()
                    {
                        EmployeeID = x.EmployeeID,
                        FirstName = x.EmployeeID + " - " + x.FirstName
                    }).ToList();
                return result;
            }
        }

        public UserStories_Model GetUserStory(int userstory_id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var result = context.UserStories
                    .Where(x => x.UserStoryID == userstory_id)
                    .Select(x => new UserStories_Model()
                    {
                        UserStoryID = x.UserStoryID,
                        Title = x.Title,
                        Description = x.Description,
                        CreatedOn = x.CreatedOn,
                        DueDate = x.DueDate,
                        ModifiedOn = x.ModifiedOn,
                        Status = x.Status,
                        StoryPoints = x.StoryPoints,
                        Priority = x.Priority,
                        AssignedEmployeeID = x.AssignedEmployeeID,
                        AssignedManagerID = x.Employees.EmployeeID
                    }).FirstOrDefault();
                return result;
            }
        }

        public int CountTasks(int userstory_id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var count = context.Tasks
                    .Where(x => x.AssignedUserStoryID == userstory_id).Count();
                return count;
            }
        }

        public int? GetManager(int empid)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var result = context.Employees
                    .FirstOrDefault(x => x.EmployeeID == empid);
                return result.ManagerID;
            }
        }

        public void UpdateUserStory(UserStories_Model model)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var UserStory = context.UserStories.FirstOrDefault(x => x.UserStoryID == model.UserStoryID);
                UserStory.Title = model.Title;
                UserStory.Description = model.Description;
                UserStory.AssignedEmployeeID = model.AssignedEmployeeID;
                UserStory.DueDate = model.DueDate;
                UserStory.ModifiedOn = DateTime.Now;
                UserStory.StoryPoints = model.StoryPoints;
                UserStory.Priority = model.Priority;

                context.SaveChanges();
            }
        }

        public void UpdateUserStoryStatus(UserStories_Model model)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var UserStory = context.UserStories.FirstOrDefault(x => x.UserStoryID == model.UserStoryID);
                
                UserStory.Status = model.Status;

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

        public void DeleteUserStory(int id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {

                var UserStory = new UserStories()
                {
                    UserStoryID = id
                };

                context.Entry(UserStory).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
