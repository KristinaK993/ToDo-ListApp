namespace ToDoApp.API.Models.DTOs.Tasks
{
    public class TodoTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }

        public string CategoryName { get; set; } = string.Empty;
    }
}
// DTO för att skapa en ny task via API
// Innehåller bara de fält som klienten ska skicka in