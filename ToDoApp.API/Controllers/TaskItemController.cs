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
    [Authorize]  //för endast inloggade användare
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskItemRepository _repo;
        private readonly IRandomJokeService _joke;

        public TaskItemController(ITaskItemRepository repo, IRandomJokeService joke)
        {
            _repo = repo;
            _joke = joke;
        }


        [HttpGet("Joke")]
        public async Task<IActionResult> GetRandomJoke()
        {
            var advice = await _joke.GetRandomJokeAsync();
            return Ok(_joke);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] bool? isDone,
            [FromQuery] string? sort,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _repo.GetAllAsQueryable();
            if (isDone.HasValue)
                query = query.Where(t => t.IsDone == isDone.Value);

            if (sort == "asc")
                query = query.OrderBy(t => t.Title);
            else if (sort == "desc")
                query = query.OrderByDescending(t => t.Title);

            var result = await query.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItem task)
        {
            await _repo.CreateAsync(task);
            await _repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem updatedTask)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return NotFound();

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.DueDate = updatedTask.DueDate;
            task.CategoryId = updatedTask.CategoryId;

            _repo.Update(task);
            await _repo.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return NotFound();

            _repo.Delete(task);
            await _repo.SaveChangesAsync();

            return NoContent();
        }
    }
}

