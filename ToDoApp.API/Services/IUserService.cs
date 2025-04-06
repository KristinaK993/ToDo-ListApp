using ToDoApp.API.Models.DTOs;
using ToDoApp.API.Models.Entities;

namespace ToDoApp.API.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserRegisterDto dto);
        Task<string?> LoginUserAsync(UserLoginDto dto);
        Task<List<UserDTO>> GetAllUsersAsync();

    }
}
