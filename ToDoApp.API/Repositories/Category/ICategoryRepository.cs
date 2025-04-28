using ToDoApp.API.Models.Entities;

namespace ToDoApp.API.Repositories.CategoryRepo
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync(); //hämtar alla kategorier från databasen
        Task<Category?> GetByIdAsync(int id); //hämtar kategorier by ID 
        Task AddAsync(Category category); //Lägger till kategorier
        void Update(Category category); //uppdaterar
        void Delete(Category category); //raderar
        Task<bool> SaveChangesAsync(); //sparar
    }
}
