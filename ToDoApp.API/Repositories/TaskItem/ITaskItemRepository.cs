using ToDoApp.API.Models.Entities;

namespace ToDoApp.API.Repositories.TaskItems
{
    public interface ITaskItemRepository
    {
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task AddAsync(TaskItem task);
        void Update(TaskItem task);
        void Delete(TaskItem task);
        Task<bool> SaveChangesAsync();
        Task CreateAsync(TaskItem task);
        IQueryable<TaskItem> GetAllAsQueryable();
    }
}

