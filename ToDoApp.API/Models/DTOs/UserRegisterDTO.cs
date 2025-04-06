namespace ToDoApp.API.Models.DTOs
{
    public class UserRegisterDto : UserLoginDto
    {
        public string Role { get; set; } = "User";
    }
}
