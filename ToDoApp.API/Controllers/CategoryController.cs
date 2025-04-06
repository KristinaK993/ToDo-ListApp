using Microsoft.AspNetCore.Mvc;
using ToDoApp.API.Models.Entities;
using ToDoApp.API.Repositories.CategoryRepo;
using System.Threading.Tasks;


namespace ToDoApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repo;

        public CategoryController(ICategoryRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _repo.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            await _repo.AddAsync(category);
            await _repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Category updatedCategory)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return NotFound();

            category.Name = updatedCategory.Name;
            await _repo.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return NotFound();

            _repo.Delete(category);
            await _repo.SaveChangesAsync();

            return NoContent();
        }
    }
}
