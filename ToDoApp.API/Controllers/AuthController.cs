using Microsoft.AspNetCore.Mvc;
using ToDoApp.API.Models.DTOs;
using ToDoApp.API.Services;


namespace ToDoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var result = await _userService.RegisterUserAsync(dto);
            if (!result) return BadRequest("Username already exists");
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var token = await _userService.LoginUserAsync(dto);
            if (token == null) return Unauthorized("Invalid credentials");
            return Ok(new { token });
        }
    }
}
