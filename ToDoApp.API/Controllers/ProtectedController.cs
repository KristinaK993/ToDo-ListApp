using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProtectedController : ControllerBase  //ärver från controllerbase (api utan vyer)
    {
        [Authorize] // kräver användare inloggad
        [HttpGet("secret")]
        public IActionResult GetSecret() //actionmetod för att returnera
        {
            return Ok(" Du är inloggad!"); //return att man blivit inloggad
        }

        [Authorize(Roles = "Admin")] // Endast Admin
        [HttpGet("admin-only")]
        public IActionResult AdminOnly() //ationmetod för att returnera
        {
            return Ok(" Du är Admin och har åtkomst till detta endpoint!"); //return att man blivit inloggad som admin
        }
    }
}
