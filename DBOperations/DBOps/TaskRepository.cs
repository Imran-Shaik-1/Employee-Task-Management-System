using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBOperations.Models;

namespace DBOperations.DBOps
{
    public class TaskRepository
    {
        public void AddTask(Tasks_Model model)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                Tasks task = new Tasks()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Status = "New",
                    AssignedUserStoryID = model.AssignedUserStoryID,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };

                context.Tasks.Add(task);
                context.SaveChanges();
            }
        }

        public List<Tasks_Model> GetTasks(int userstoryid)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var result = context.Tasks.
                    Where(x => x.AssignedUserStoryID == userstoryid).
                    Select(x => new Tasks_Model()
                    {
                        TaskID = x.TaskID,
                        Title = x.Title,
                        Description = x.Description,
                        Status = x.Status,
                        CreatedOn = x.CreatedOn,
                        ModifiedOn = x.ModifiedOn,
                        AssignedUserStoryID = x.AssignedUserStoryID
                    }).ToList();
                return result;
            }
        }

        public Tasks_Model GetTask(int id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var result = context.Tasks
                    .Where(x => x.TaskID == id)
                    .Select(x => new Tasks_Model()
                    {
                        TaskID = x.TaskID,
                        Title = x.Title,
                        Description = x.Description,
                        Status = x.Status,
                        CreatedOn = x.CreatedOn,
                        ModifiedOn = x.ModifiedOn,
                        AssignedUserStoryID = x.AssignedUserStoryID
                    }).FirstOrDefault();

                return result;
            }
        }

        //public int CountTasks(int userstory_id)
        //{
        //    using (var context = new EmployeeManagementDBEntities())
        //    {
        //        var count = context.Tasks
        //            .Where(x => x.AssignedUserStoryID == userstory_id).Count();
        //        return count;
        //    }
        //}

        public void UpdateTask(Tasks_Model model)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var task = context.Tasks.FirstOrDefault(x => x.TaskID == model.TaskID);
                
                task.Title = model.Title;
                task.Description = model.Description;
                task.Status = model.Status;
                task.ModifiedOn = DateTime.Now;

                context.SaveChanges();
            }
        }

        public void DeleteTask(int id)
        {
            using (var context = new EmployeeManagementDBEntities())
            {
                var task = new Tasks()
                {
                    TaskID = id
                };

                context.Entry(task).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
