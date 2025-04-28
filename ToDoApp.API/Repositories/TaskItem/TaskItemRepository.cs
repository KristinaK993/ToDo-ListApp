using Microsoft.EntityFrameworkCore;
using ToDoApp.API.Data;
using ToDoApp.API.Models.Entities;
using ToDoApp.API.Repositories.TaskItems;


namespace ToDoApp.API.Repositories.TaskItems
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly AppDbContext _context;

        public TaskItemRepository(AppDbContext context)
        {
            _context = context; //tar in databascontext via Dependency injection
        }

        public async Task<List<TaskItem>> GetAllAsync() //returnerar alla tasks även tillhörande kategori
        {
            return await _context.Tasks
                .Include(t => t.Category)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id) //hämta tasks med spec ID
        {
            return await _context.Tasks
                .Include(t => t.Category) //kategori
                .FirstOrDefaultAsync(t => t.Id == id); //returnera task el null
        }

        public void Update(TaskItem task) //uppdatera bef task
        {
            _context.Tasks.Update(task); //markerar att den är ändrad i content
        }

        public void Delete(TaskItem task) //radera task 
        {
            _context.Tasks.Remove(task); //markerar task för bort tagning
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0; //returnerar true om minst 1 sak sparades
        }

        public async Task CreateAsync(TaskItem task) //skapa 
        {
            await _context.Tasks.AddAsync(task); //lägg till i context 
            await _context.SaveChangesAsync(); //spara 
        }

        public IQueryable<TaskItem> GetAllAsQueryable() // ret allt som kan filtrera,sortera,paginera
        {
            return _context.Tasks
                .Include(t => t.Category)
                .AsQueryable();
        }
    }
}
