using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.API.Models.Entities;
using ToDoApp.API.Repositories.TaskItems;
using ToDoApp.API.Services;



namespace ToDoApp.API.Repositories.TaskItems
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  //kräver att man är inloggad
    public class TaskItemController : ControllerBase //ärver från controllerbase
    {
        private readonly ITaskItemRepository _repo; //repo för databasoperationer
        private readonly IRandomJokeService _joke; // service som hämtar randomJoke

        public TaskItemController(ITaskItemRepository repo, IRandomJokeService joke) //konstruktor med DI
        {
            _repo = repo;
            _joke = joke;
        }


        [HttpGet("Joke")]
        public async Task<IActionResult> GetRandomJoke()
        {
            var joke = await _joke.GetRandomJokeAsync();  //hämtar skämt
            return Ok(joke); //returnerar ok (skämt)
        }


        [HttpGet]
        //sortering/filtrering/paginering
        public async Task<IActionResult> GetAll(
            [FromQuery] bool? isDone, //tar emot parameter från URL true or false
            [FromQuery] string? sort, //tar emot sorteringsmetod
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10) //sida1, 10upg
        {
            var query = _repo.GetAllAsQueryable(); //hämtar tasks som IqueryAble från databasen
            if (isDone.HasValue)
                query = query.Where(t => t.IsDone == isDone.Value);

            if (sort == "asc") //sorterar listan baserat på titel asc eller desc
                query = query.OrderBy(t => t.Title);
            else if (sort == "desc")
                query = query.OrderByDescending(t => t.Title);

            var result = await query.ToListAsync(); //här hämtas alla uppgifter
            return Ok(result); //visas ok + resultat
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) //hämtar en spec task med ID
        {
            var task = await _repo.GetByIdAsync(id); //hämtar task med angivet ID
            if (task == null) return NotFound(); //ifall task inte finns
            return Ok(task); //returnera 201 + task
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItem task) //skapa ny task
        {
            await _repo.CreateAsync(task); //lägger till task i databas
            await _repo.SaveChangesAsync(); //sparar i databasen
            //returnerar 201 + länk till nyUppgift via GET 
            return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem updatedTask) //uppdatera task med viss ID
        {
            var task = await _repo.GetByIdAsync(id); //hämta upg från databas
            if (task == null) return NotFound(); //404 om inte hittad

            //Uppdatera egenskaper
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.DueDate = updatedTask.DueDate;
            task.CategoryId = updatedTask.CategoryId;

            _repo.Update(task); //markerar objekt som uppdaterat
            await _repo.SaveChangesAsync(); //sparas

            return NoContent(); //204, inget innehåll
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) //radera task by ID
        {
            var task = await _repo.GetByIdAsync(id); //Hitta task by ID
            if (task == null) return NotFound(); //404 om ej hittad

            _repo.Delete(task); //ta bort task från context
            await _repo.SaveChangesAsync(); //sparar ändringar i databasen

            return NoContent(); //delet lyckas,inget innehåll
        }
    }
}

