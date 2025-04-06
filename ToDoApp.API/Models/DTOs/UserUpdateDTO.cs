namespace ToDoApp.API.Models.DTOs
{
    public class UserUpdateDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }
}
