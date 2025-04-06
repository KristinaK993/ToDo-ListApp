using ToDoApp.API.Models.Entities;

namespace ToDoApp.API.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public ICollection<TaskItem> TaskItems { get; set; }

    }
}
