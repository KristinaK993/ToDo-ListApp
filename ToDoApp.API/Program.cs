
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToDoApp.API.Data;
using ToDoApp.API.Repositories;
using ToDoApp.API.Repositories.CategoryRepo;
using ToDoApp.API.Repositories.TaskItems;
using ToDoApp.API.Services;
using ToDoApp.API.Validators;


namespace ToDoApp.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); //initierar appen 

            // H�r registreras olika tj�nster som API beh�ver
            builder.Services.AddControllers() //api kan hantera requests via controllers
                        .AddJsonOptions(options =>
                        {
                            // Bevarar objektreferenser f�r att undvika o�ndliga JSON-loopar vid serialisering
                            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                        }); //hur API ska hantera Json svar
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ToDoApp API",
                    Version = "v1"
                });
                //  f�r JWT support
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
            });
            //l�ser databaskopplingen
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUserService, UserService>(); //service f�r auth
            builder.Services.AddScoped<IUserRepository, UserRepository>(); //repo f�r User
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); //repo f�r kategori
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());//AutoMapper
            builder.Services.AddFluentValidationAutoValidation(); //fluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<UserRegisterDTOValidator>();//l�gg till alla validatorer
            builder.Services.AddHttpClient<IRandomJokeService, RandomJokeService>(); //extern API
            builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();//repo f�r task 

            //JWT-konfiguration
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters //valideringregler
            {
                ValidateIssuer = false, //vem som skapade
                ValidateAudience = false, //till vem den skapades
                ValidateIssuerSigningKey = true, //kontrollera att signering �r korrekt
                IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) //h�r anges vilken nyckel
            };
        });

            builder.Services.AddAuthorization(); //l�gger till rollhantering

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())  //bogusSeedData
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await SeedData.Seed(context); //fyller databasen med data
            }

            //starta
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
