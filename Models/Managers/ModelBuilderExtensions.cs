using Microsoft.EntityFrameworkCore;
using PitonProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PitonProject.Models.Managers
{
    public static class ModelBuilderExtensions
    {

        public static void Seed(this ModelBuilder modelBuilder)
        {
            External external = new External();
            string[] logTypeValues = new string[] { "info", "error" };
            string[] taskTypeValues = new string[] { "day", "week", "month" };

            #region user seeding 
            modelBuilder.Entity<User>().HasData(
              new User
              {
                  ID = 1,
                  UserName = "admin",
                  Password = external.GenerateSha256("admin")
              }
            );
            #endregion

            #region logType seeding 
            modelBuilder.Entity<LogType>().HasData(
                           new LogType
                           {
                               ID = 1,
                               LogTypeText = logTypeValues[0]
                           }, 
                           new LogType
                           {
                               ID = 2,
                               LogTypeText = logTypeValues[1]
                           }
                       );
            #endregion

            #region taskType seeding 
            modelBuilder.Entity<TaskType>().HasData(
                           new TaskType
                           {
                               ID = 1,
                               TaskTypeText = taskTypeValues[0]
                           },
                           new TaskType
                           {
                               ID = 2,
                               TaskTypeText = taskTypeValues[1]
                           },
                           new TaskType
                           {
                               ID = 3,
                               TaskTypeText = taskTypeValues[2]
                           }
                       );
            #endregion






        }
    }
}
