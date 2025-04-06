using Microsoft.EntityFrameworkCore;
using ToDoApp.API.Models.Entities;

namespace ToDoApp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; } 
        public DbSet<Category> Categories { get; set; }  
    }
}
