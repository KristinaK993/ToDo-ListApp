using ToDoApp.API.Models.Entities;

namespace ToDoApp.API.Repositories.TaskItems
{
    public interface ITaskItemRepository
    {
        Task<List<TaskItem>> GetAllAsync();  //hämtar alla tasks
        Task<TaskItem?> GetByIdAsync(int id); //hämtar task by ID 
        Task CreateAsync(TaskItem task); //lägg till OCH spara task (korta operationer)
        void Update(TaskItem task); //uppdaterar
        void Delete(TaskItem task); //raderar
        Task<bool> SaveChangesAsync(); //sparar
        IQueryable<TaskItem> GetAllAsQueryable();  //lägger till alla tasks som IqueryAble för att kunna filtreras,sort,pag
    }
}

