using Bogus;
using Microsoft.EntityFrameworkCore;
using ToDoApp.API.Models.Entities;

namespace ToDoApp.API.Data
{
    public class SeedData //kollar om databasen är tom, isåfall fyller den med test data 
    {
        public static async Task Seed(AppDbContext context) //metod körs för att fylla databasen
        {
            // Seed Users
            if (!context.Users.Any()) //kontrollerar om det redan finns användare
            {
                var users = new Faker<User>()
                    .RuleFor(u => u.Username, f => f.Internet.UserName())
                    .RuleFor(u => u.PasswordHash, f => "placeholder")
                    .RuleFor(u => u.Role, f => "User")
                    .Generate(10); //skapar 10 användare

                await context.Users.AddRangeAsync(users); //lägger till users i databasen
                await context.SaveChangesAsync(); //sparas
            }

            // Seed Categories
            if (!context.Categories.Any()) //skapar 5 kategorier om det inte redan finns
            {
                var categories = new Faker<Category>()
                    .RuleFor(category => category.Name, fake => fake.Commerce.Categories(1)[0]) //olika kategorier ex books, electronics
                    .Generate(5);

                await context.Categories.AddRangeAsync(categories); //category list läggs till i minnet(context)
                await context.SaveChangesAsync(); //sparas till databsen
            }

            // Seed TaskItems
            if (!context.Tasks.Any()) //kontrollera om Tasks-tabellen redan har data 
            {
                //categoryID för att koppla varje task till category
                //userID för att varje task tillhör en användare
                var categoryIds = await context.Categories.Select(category => category.Id).ToListAsync();
                var userIds = await context.Users.Select(u => u.Id).ToListAsync(); //  get UserId

                var tasks = new Faker<TaskItem>()//skapa 15 st fakeTasks
                    .RuleFor(task => task.Title, fake => fake.Lorem.Sentence(3)) //slumpmässig titel
                    .RuleFor(task => task.Description, fake => fake.Lorem.Paragraph())//slumpmässig beskrivning
                    .RuleFor(task => task.IsDone, fake => fake.Random.Bool()) //slumpmässig Klar/inte
                    .RuleFor(task => task.CategoryId, fake => fake.PickRandom(categoryIds)) //koppla till kategori
                    .RuleFor(task => task.UserId, fake => fake.PickRandom(userIds)) //  koppla till användare
                    .Generate(15);

                await context.Tasks.AddRangeAsync(tasks); //lägger till i context
                await context.SaveChangesAsync(); //sparas i databasen
            }
        }
    }
}
