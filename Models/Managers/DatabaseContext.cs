using Microsoft.EntityFrameworkCore;
using PitonProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PitonProject.Models.Managers
{
    public class DatabaseContext : DbContext
    {
        private const string connectionString = @"Data Source=.; Initial Catalog=PitonDB; Integrated Security=True;";
        public DbSet<User> Users { get; set; }
        public DbSet<LogType> LogTypes { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<TaskClass> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }

    }
}
