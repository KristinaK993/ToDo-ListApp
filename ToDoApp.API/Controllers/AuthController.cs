using Microsoft.AspNetCore.Mvc;
using ToDoApp.API.Models.DTOs;
using ToDoApp.API.Services;


namespace ToDoApp.API.Controllers
{
    [Route("api/[controller]")]  
    [ApiController]
    public class AuthController : ControllerBase // ärver från controllerbase
    {
        //Dependency Injection (inloggning/reg logik)
        private readonly IUserService _userService; 

        public AuthController(IUserService userService)  //konstruktor som tar emot Iuserservice (DI)
        {
            _userService = userService;
        }

        [HttpPost("register")]  //POSTendpoint
        public async Task<IActionResult> Register(UserRegisterDto dto) // används för att registrera användare
        {
            var result = await _userService.RegisterUserAsync(dto); //anrop tjänst reg. skickar DTO, usern + pass
            if (!result) return BadRequest("Username already exists"); //felmeddelande om användaren redan finns
            return Ok("User registered successfully"); //retur OK om reg. lyckades
        }

        [HttpPost("login")] //postEndpoint
        public async Task<IActionResult> Login(UserLoginDto dto) //login
        {
            var token = await _userService.LoginUserAsync(dto); //försöker logga in och skapa en token
            if (token == null) return Unauthorized("Invalid credentials"); //ifall lösenord var fel
            return Ok(new { token }); //inloggning lyckas = ok
        }
    }
}
