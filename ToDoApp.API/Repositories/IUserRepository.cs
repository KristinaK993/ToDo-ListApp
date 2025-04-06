using ToDoApp.API.Models.Entities;


namespace ToDoApp.API.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task AddUserAsync(User user);
       
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        void Update(User user);
        void Delete(User user);
        Task<bool> SaveChangesAsync();
    }
}
