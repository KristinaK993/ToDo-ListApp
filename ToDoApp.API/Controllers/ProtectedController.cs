using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        [Authorize] // Endast inloggade användare får åtkomst
        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            return Ok(" Du är inloggad!");
        }

        [Authorize(Roles = "Admin")] // Endast Admin
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok(" Du är Admin och har åtkomst till detta endpoint!");
        }
    }
}
