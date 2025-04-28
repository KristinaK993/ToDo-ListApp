using ToDoApp.API.Data;
using ToDoApp.API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace ToDoApp.API.Repositories
{

    public class UserRepository : IUserRepository //implementera interface
    {
        private readonly AppDbContext _context; //databas

        public UserRepository(AppDbContext context) //konstruktor som tar in AppDbContext via DI
        {
            _context = context; //tilldelar context till klassens privata fält
        }

        public async Task<User?> GetByUsernameAsync(string username) //hämtar användare baserat på anvNamn
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username); //ret första Anv som matchar
        }

        public async Task AddUserAsync(User user) //lägger till ny användare 
        {
            await _context.Users.AddAsync(user); //lägger till i context 
        }

        public async Task<bool> SaveChangesAsync() //sparar alla ändringar i databasen
        {
            return await _context.SaveChangesAsync() > 0; //kör alla ändringar och sparar om minst 1 ändring gjorts
        }
        public async Task<List<User>> GetAllAsync() //hämtar alla användare från databasen
        {
            return await _context.Users.ToListAsync(); //returnera en lista på alla användare
        }

        public async Task<User?> GetByIdAsync(int id) //hämta användare by ID
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);//returnera första som matchar med ID
        }

        public void Update(User user) //markera användare som ändrad i context
        {
            _context.Users.Update(user); //säger till EF att detta har ändrats
        }

        public void Delete(User user) //markera användare som ska tass bort 
        {
            _context.Users.Remove(user); //spara borttagning
        }
       


    }
}
