using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.Models;
using System.Transactions;
using Task = ProjectManagementAPI.Models.Task;

namespace ProjectManagementAPI
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}
