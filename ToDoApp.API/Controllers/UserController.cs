using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.API.Models.Entities;
using ToDoApp.API.Repositories;

namespace ToDoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo; //Repo för att hantera användardata

        public UserController(IUserRepository repo) //DI för av UserRepository
        {
            _repo = repo;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _repo.GetAllAsync(); //hämtar alla användare från databas
            return Ok(users); //returnera users i en lista
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id) //hämtar en spec användare by ID
        {
            var user = await _repo.GetByIdAsync(id); //hämta användare baserat på ID
            if (user == null)
                return NotFound(); //returnera 404 om inte hittad

            return Ok(user); //returnera användaren
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult> Create(User user) //skapar User
        {
            await _repo.AddUserAsync(user); //lägger till user 
            await _repo.SaveChangesAsync(); //sparar user i databasen
            //201 + länk till ny GET by id
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, User updatedUser) //Updatera USER
        {
            var user = await _repo.GetByIdAsync(id); //hitta user by ID
            if (user == null)
                return NotFound(); //om inte hittad 

            //uppdatera egenskaper
            user.Username = updatedUser.Username;
            user.PasswordHash = updatedUser.PasswordHash;
            user.Role = updatedUser.Role;

            _repo.Update(user); //uppdateras
            await _repo.SaveChangesAsync(); //spara till databasen

            return NoContent(); //204, inget innehåll returneras
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) //ta bort USER by ID
        {
            var user = await _repo.GetByIdAsync(id); //hitta by ID 
            if (user == null) 
                return NotFound(); //om inte hittad

            _repo.Delete(user); //här tas den bort 
            await _repo.SaveChangesAsync();//sparas till databasen

            return NoContent(); // 204, inget innehåll
        }
    }
}
