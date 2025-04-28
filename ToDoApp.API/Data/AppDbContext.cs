using Microsoft.EntityFrameworkCore;
using ToDoApp.API.Models.Entities;

namespace ToDoApp.API.Data
{
    public class AppDbContext : DbContext //appDb ärver från Db 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) //skickar konfiguration via "options" när AppDbC skapas
            : base(options) { } //skickar det vidare till DbContext-bas 

        //tabeller i databasen
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; } 
        public DbSet<Category> Categories { get; set; }  
    }
}
