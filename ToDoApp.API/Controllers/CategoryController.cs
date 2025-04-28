using Microsoft.AspNetCore.Mvc;
using ToDoApp.API.Models.Entities;
using ToDoApp.API.Repositories.CategoryRepo;
using System.Threading.Tasks;


namespace ToDoApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase //ärver från controllerBase
    {
        private readonly ICategoryRepository _repo; // Dependency Injection av kategori-repo

        public CategoryController(ICategoryRepository repo) //konstruktor som tar in Repo via DI
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() //hämtar alla kategorier
        {
            var categories = await _repo.GetAllAsync(); //hämtar alla kat. från databasen via repository
            return Ok(categories); //reurnerar ok + datan
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) //hämtar kategori med spec ID
        {
            var category = await _repo.GetByIdAsync(id); //hämtar från databasen
            if (category == null) return NotFound(); //returnerar 404 om det inte finns
            return Ok(category); //ok om det hittas + datan
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category) //skapar ny kategori
        {
            await _repo.AddAsync(category); //lägger till i databasen
            await _repo.SaveChangesAsync(); //sparar
            //return 201 created, länk till GETendpoint + den nya kategorin
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Category updatedCategory) //updaterar en befintlig kategori
        {
            var category = await _repo.GetByIdAsync(id); //hämtar den
            if (category == null) return NotFound(); //om den inte finns

            category.Name = updatedCategory.Name; //updaterar namnet
            await _repo.SaveChangesAsync(); //sparar

            return NoContent(); //201 svar utan innehåll
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) //radera kategori
        {
            var category = await _repo.GetByIdAsync(id); //hämtar by ID
            if (category == null) return NotFound(); //om den inte finns

            _repo.Delete(category); //raderar
            await _repo.SaveChangesAsync(); //sparar

            return NoContent(); //201 svar utan innehåll
        }
    }
}
