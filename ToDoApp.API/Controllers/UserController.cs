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
        private readonly IUserRepository _repo;

        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _repo.GetAllAsync();
            return Ok(users);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult> Create(User user)
        {
            await _repo.AddUserAsync(user);
            await _repo.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, User updatedUser)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            user.Username = updatedUser.Username;
            user.PasswordHash = updatedUser.PasswordHash;
            user.Role = updatedUser.Role;

            _repo.Update(user);
            await _repo.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            _repo.Delete(user);
            await _repo.SaveChangesAsync();

            return NoContent();
        }
    }
}
