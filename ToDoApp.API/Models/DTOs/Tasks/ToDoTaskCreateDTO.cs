﻿namespace ToDoApp.API.Models.DTOs.Tasks
{
    public class TodoTaskCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }

        public int UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
