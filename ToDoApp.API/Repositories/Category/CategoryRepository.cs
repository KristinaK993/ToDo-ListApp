using Microsoft.EntityFrameworkCore;
using ToDoApp.API.Data;
using ToDoApp.API.Models.Entities;
using ToDoApp.API.Repositories.CategoryRepo;

namespace ToDoApp.API.Repositories
{
    public class CategoryRepository : ICategoryRepository //ärver från IcategoryRepo
    {
        private readonly AppDbContext _context; //databaskoppling

        public CategoryRepository(AppDbContext context) //konstruktor
        {
            _context = context; //DI får inte databascontext
        }

        public async Task<List<Category>> GetAllAsync() //hämtar alla kategorier från databasen och ret som en lista
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id) //hämtar spec kategori baserat på ID
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id); //null om den inte hittas
        }

        public async Task AddAsync(Category category) //lägger till ny kategori i context
        {
            await _context.Categories.AddAsync(category);
        }

        public void Update(Category category) //uppdatera kategori
        {
            _context.Categories.Update(category);
        }

        public void Delete(Category category) //radera kategori
        {
            _context.Categories.Remove(category);
        }

        public async Task<bool> SaveChangesAsync() //spara till databasen
        {
            return await _context.SaveChangesAsync() > 0; //returnera true om minst 1 rad ändrats i databasen
        }
    }
}
