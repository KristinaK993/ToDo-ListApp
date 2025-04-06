using Bogus;
using Microsoft.EntityFrameworkCore;
using ToDoApp.API.Models.Entities;

namespace ToDoApp.API.Data
{
    public class SeedData
    {
        public static async Task Seed(AppDbContext context)
        {
            // Seed Users
            if (!context.Users.Any())
            {
                var users = new Faker<User>()
                    .RuleFor(u => u.Username, f => f.Internet.UserName())
                    .RuleFor(u => u.PasswordHash, f => "placeholder")
                    .RuleFor(u => u.Role, f => "User")
                    .Generate(10);

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }

            // Seed Categories
            if (!context.Categories.Any())
            {
                var categories = new Faker<Category>()
                    .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
                    .Generate(5);

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // Seed TaskItems
            if (!context.Tasks.Any())
            {
                var categoryIds = await context.Categories.Select(c => c.Id).ToListAsync();
                var userIds = await context.Users.Select(u => u.Id).ToListAsync(); //  get UserId:n

                var tasks = new Faker<TaskItem>()
                    .RuleFor(t => t.Title, f => f.Lorem.Sentence(3))
                    .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
                    .RuleFor(t => t.IsDone, f => f.Random.Bool())
                    .RuleFor(t => t.CategoryId, f => f.PickRandom(categoryIds))
                    .RuleFor(t => t.UserId, f => f.PickRandom(userIds)) //  add UserId
                    .Generate(15);

                await context.Tasks.AddRangeAsync(tasks);
                await context.SaveChangesAsync();
            }
        }
    }
}
